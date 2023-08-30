using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SimpleTtsPrompt;

public partial class App : Application
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    public override void Initialize()
    {
        // Setting up logger
        var config = new NLog.Config.LoggingConfiguration();
#if DEBUG
        var target = new NLog.Targets.ConsoleTarget("log-console");
#else
        var target = new NLog.Targets.FileTarget("log-file") { FileName = "logs.log" };
#endif
        config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, target);
        NLog.LogManager.Configuration = config;
        // Avalonia UI stuff
        AvaloniaXamlLoader.Load(this);

        DataContext = this;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();
    }

    public void OpenPrompt()
    {
        // TODO: Implement dialog window for text prompt
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        if (desktop.Windows.Any(win => win is PromptWindow))
            return;

        var dialog = new PromptWindow();
        dialog.PromptEntered += (s, e) =>
        {
            Logger.Info(e.Prompt);
        };
        dialog.Show();
    }

    public void Exit()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.Shutdown();

        throw new NotImplementedException("Exit method wasn't implement for this platform");
    }
}