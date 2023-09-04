using System;
using System.Runtime.InteropServices;
using SimpleTtsPrompt.Tts.SpeechPlayer.Linux;

namespace SimpleTtsPrompt.Tts.SpeechPlayer;

public static class SpeechPlayerFactory
{
    public static ISpeechPlayer Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return new PulseAudioPlayer();

        throw new NotImplementedException();
    }
    public static ISpeechPlayer CreateWithSink(string sink)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return new PulseAudioPlayer(sink);

        throw new NotImplementedException();
    }
}