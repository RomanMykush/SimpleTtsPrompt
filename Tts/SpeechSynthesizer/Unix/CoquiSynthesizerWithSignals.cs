using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleTtsPrompt.Tts.SpeechSynthesizer.Unix;

public class CoquiSynthesizerWithSignals : ISpeechSynthesizer
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    private readonly PosixSignalRegistration Sigusr2Registration;
    private readonly int SIGUSR1;
    private readonly int SIGUSR2;
    private readonly FileInfo InputFile;
    private readonly FileInfo OutputFile;
    private readonly Process CoquiTts;
    private readonly SemaphoreSlim SignalSemaphore = new(0, 1);
    private readonly SemaphoreSlim ProcessSemaphore = new(1, 1);
    public CoquiSynthesizerWithSignals(int sigusr1, int sigusr2)
    {
        InputFile = new FileInfo(".tmp/prompt.tmp");
        OutputFile = new FileInfo(".tmp/output.wav");
        // Setting process info
        ProcessStartInfo startInfo = new()
        {
            FileName = "/bin/python",
            Arguments = "Tts/SpeechSynthesizer/Unix/tts_daemon.py -p " + InputFile + " -o " + OutputFile
        };
        // Starting tts synthesizer child process
        if (Process.Start(startInfo) is not Process child)
            throw new NullReferenceException("Failed to start TTS child process");

        CoquiTts = child;
        SIGUSR1 = sigusr1;
        SIGUSR2 = sigusr2;
        // Register handler for SIGUSR2 signal
        // Child process can inform program process about successfully finished synthesizing
        Sigusr2Registration = PosixSignalRegistration.Create((PosixSignal)SIGUSR2, Sigusr2Handler);
        // Kill tts process if program exits
        AppDomain.CurrentDomain.ProcessExit += (s, e) => {
            CoquiTts.Kill();
        };
    }
    public async Task<string> SynthesizeFileAsync(string prompt)
    {
        await ProcessSemaphore.WaitAsync();
        try
        {
            // Ensure target directory exist
            if (!InputFile.Directory!.Exists)
                InputFile.Directory.Create();
            // Write input data to temp file
            File.WriteAllText(InputFile.FullName, prompt);
            // Send SIGUSR1 signal to Tts child process to start synthesis
            ProcessStartInfo startInfo = new("/bin/kill", "-s " + SIGUSR1 + " " + CoquiTts.Id);
            if (Process.Start(startInfo) is not Process signalProc)
                throw new NullReferenceException("Failed to send task start signal to Tts child proccess");
            await signalProc.WaitForExitAsync();
            // Wait for child process to finish
            await SignalSemaphore.WaitAsync();
            // Generate new file name
            string newName = RandomString.Generate(8) + ".wav";
            while (OutputFile.Directory!.GetFiles().Any(file => file.Name == newName))
                newName = RandomString.Generate(8) + ".wav";
            // Rename output file and return new name
            newName = OutputFile.Directory!.FullName + "/" + newName;
            File.Move(OutputFile.FullName, newName);
            return newName;
        }
        finally
        {
            ProcessSemaphore.Release();
        }
    }

    // Child process successfully finished synthesizing
    private void Sigusr2Handler(PosixSignalContext signalContext)
    {
        SignalSemaphore.Release();
        signalContext.Cancel = true;
    }
}