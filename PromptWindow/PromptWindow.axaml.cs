using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SimpleTtsPrompt;

public partial class PromptWindow : Window
{
    public static readonly DirectProperty<PromptWindow, string?> PromptProperty =
    AvaloniaProperty.RegisterDirect<PromptWindow, string?>(
        nameof(Prompt),
        o => o.Prompt,
        (o, v) => o.Prompt = v);

    private static History<string> PreviousPrompts = new(20);
    private string? _Prompt;
    public string? Prompt
    {
        get { return _Prompt; }
        set { SetAndRaise(PromptProperty, ref _Prompt, value); }
    }

    public event EventHandler<PromptEnteredEventArgs>? PromptEntered;
    public PromptWindow()
    {
        InitializeComponent();
        DataContext = this;
    }
    public void GetPreviousPromptHistory()
    {
        PreviousPrompts.SelectPrevious();
        Prompt = PreviousPrompts.Selected?.Value;
    }
    public void GetNextPromptHistory()
    {
        PreviousPrompts.SelectNext();
        Prompt = PreviousPrompts.Selected?.Value;
    }
    public void Confirm()
    {
        if (string.IsNullOrEmpty(Prompt))
            return;

        PromptEntered?.Invoke(this, new PromptEnteredEventArgs(Prompt));
        if (Prompt != PreviousPrompts.Selected?.Value)
            PreviousPrompts.Add(Prompt);

        PreviousPrompts.Deselect();
        Prompt = string.Empty;
    }
    public void ConfirmAndClose()
    {
        Confirm();
        Close();
    }
}