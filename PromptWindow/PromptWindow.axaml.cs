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
    public void Confirm()
    {
        if (string.IsNullOrEmpty(Prompt))
            return;

        PromptEntered?.Invoke(this, new PromptEnteredEventArgs(Prompt));
        Prompt = string.Empty;
    }
    public void ConfirmAndClose()
    {
        if (!string.IsNullOrEmpty(Prompt))
            PromptEntered?.Invoke(this, new PromptEnteredEventArgs(Prompt));
        Close();
    }
}