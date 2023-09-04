using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Utilities;
using SimpleTtsPrompt.Tts.SpeechPlayer;
using SimpleTtsPrompt.Tts.SpeechSynthesizer;

namespace SimpleTtsPrompt.Tts;

public class TtsManager
{
    private ISpeechSynthesizer Synthesizer;
    private ISpeechPlayer Player;

    public TtsManager(ISpeechSynthesizer synthesizer, ISpeechPlayer player)
    {
        Synthesizer = synthesizer;
        Player = player;
    }

    public async Task NewRequest(string prompt)
    {
        string path = await Synthesizer.SynthesizeFileAsync(prompt);
        await Player.PlayAsync(path);
        File.Delete(path);
    }
}