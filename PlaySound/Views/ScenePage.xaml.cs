using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using PlaySound.ViewModels;
using Microsoft.UI.Xaml.Navigation;

namespace PlaySound.Views;

public sealed partial class ScenePage : Page
{
    public ScenePageViewModel ViewModel { get; } = Ioc.Default.GetRequiredService<ScenePageViewModel>();

    public ScenePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.OnNavigatedTo(e);
        base.OnNavigatedTo(e);
    }
}
