using System;
using System.Runtime.InteropServices;
using SimpleTtsPrompt.Tts.SpeechSynthesizer.Unix;

namespace SimpleTtsPrompt.Tts.SpeechSynthesizer;

public static class SpeechSynthesizerFactory
{
    public static ISpeechSynthesizer Create()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return new CoquiSynthesizerWithSignals(10, 12);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return new CoquiSynthesizerWithSignals(30, 31);

        throw new NotImplementedException();
    }
}