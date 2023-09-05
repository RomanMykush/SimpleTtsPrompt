using System;
using System.IO;
using System.Threading.Tasks;
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

    public async Task NewRequestAsync(string prompt)
    {
        string path = await Synthesizer.SynthesizeFileAsync(prompt);
        await Player.PlayAsync(path);
        File.Delete(path);
    }
}