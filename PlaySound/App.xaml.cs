using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using PlaySound.Services;
using PlaySound.Contracts;
using PlaySound.ViewModels;
using Microsoft.UI.Dispatching;

namespace PlaySound;

public sealed partial class App : Application
{
    public static new App Current => (App)Application.Current;

    public App()
    {
        InitializeComponent();
        DispatcherQueue.GetForCurrentThread().ShutdownStarting += OnShutdownStarting;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .AddSingleton<IWindowManager, WindowManager>()
            .AddSingleton<IPlaySoundNavigationService, NavigationService>()

            .AddTransient<ScenePageViewModel>()
            .AddTransient<MainWindowViewModel>()
            .AddTransient<SoundEffectViewModel>()

            .BuildServiceProvider());

        Ioc.Default.GetRequiredService<IWindowManager>().CreateWindow<MainWindow>().Activate();
    }

    private void OnShutdownStarting(DispatcherQueue sender, DispatcherQueueShutdownStartingEventArgs e)
    {
        var deferral = e.GetDeferral();

        try
        {
            // TODO: Dispose services + SFX.
        }
        finally
        {
            deferral.Complete();
        }
    }
}
