using System;

namespace SimpleTtsPrompt;

public class PromptEnteredEventArgs : EventArgs
{
    public PromptEnteredEventArgs(string prompt)
    {
        Prompt = prompt;
    }
    public string Prompt { get; }
}