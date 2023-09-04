using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleTtsPrompt.Tts.SpeechPlayer.Linux;

public class PulseAudioPlayer : ISpeechPlayer
{
    private readonly string? Sink;
    private readonly SemaphoreSlim ProcessSemaphore = new(1);
    public PulseAudioPlayer() { }
    public PulseAudioPlayer(string sink)
    {
        Sink = sink;
    }
    public async Task PlayAsync(string file)
    {
        await ProcessSemaphore.WaitAsync();
        try
        {
            // Setting process info
            ProcessStartInfo startInfo = new()
            {
                FileName = "/bin/paplay",
                Arguments = file
            };
            if (Sink != null)
                startInfo.Arguments = "-d " + Sink + " " + startInfo.Arguments;
            // Starting sound playback process
            if (Process.Start(startInfo) is not Process process)
                throw new NullReferenceException("Failed to start audio playback process");

            await process.WaitForExitAsync();
        }
        finally
        {
            ProcessSemaphore.Release();
        }
    }
}