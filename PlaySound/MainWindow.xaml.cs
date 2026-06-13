using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using CommunityToolkit.Mvvm.DependencyInjection;
using WinUIEx;
using PlaySound.Models;
using PlaySound.Views;
using PlaySound.Contracts;
using PlaySound.ViewModels;

namespace PlaySound;

public sealed partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; }

    private readonly WindowManager _manager;

    public MainWindow()
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);

        _manager = WindowManager.Get(this);
        _manager.MinWidth = 622;
        _manager.MinHeight = 400;
        _manager.PersistenceId = nameof(MainWindow);

        Ioc.Default.GetRequiredService<IPlaySoundNavigationService>().SetFrame(ContentFrame);
        ViewModel = Ioc.Default.GetRequiredService<MainWindowViewModel>();
    }

    private void OnSceneChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.FirstOrDefault() is Scene scene)
        {
            // Pause previous sound effects.
            if (e.RemovedItems.FirstOrDefault() is Scene previousScene)
            {
                foreach (var sfx in previousScene.Effects)
                {
                    sfx.Pause();
                }
            }

            ContentFrame.Navigate(typeof(ScenePage), scene, new EntranceNavigationTransitionInfo());
        }
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (e.Parameter is Scene scene)
        {
            SceneList.SelectedItem = scene;
        }
    }
}
