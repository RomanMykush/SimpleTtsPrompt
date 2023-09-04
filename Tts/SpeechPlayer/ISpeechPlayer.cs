using System.Threading.Tasks;

namespace SimpleTtsPrompt.Tts.SpeechPlayer;

public interface ISpeechPlayer
{
    Task PlayAsync(string file);
}