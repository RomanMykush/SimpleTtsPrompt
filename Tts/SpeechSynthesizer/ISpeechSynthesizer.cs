using System.Threading.Tasks;

namespace SimpleTtsPrompt.Tts.SpeechSynthesizer;

public interface ISpeechSynthesizer
{
    Task<string> SynthesizeFileAsync(string prompt);
}