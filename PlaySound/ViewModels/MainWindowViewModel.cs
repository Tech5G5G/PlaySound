using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.Storage.Pickers;
using PlaySound.Contracts;
using PlaySound.Models;

namespace PlaySound.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    public partial Production Production { get; set; } = new();

    private readonly IWindowManager _manager;
    private readonly IPlaySoundNavigationService _navigation;

    public MainWindowViewModel(IWindowManager manager, IPlaySoundNavigationService navigation)
    {
        _manager = manager;
        _navigation = navigation;

        navigation.Navigate(PlaySoundPage.None);
    }

    [RelayCommand]
    public async Task OpenProductionAsync()
    {
        FileOpenPicker picker = new(_manager.MainWindow.AppWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.MusicLibrary,
            FileTypeChoices = { { "Production", [".prod"] } }
        };

        if (await picker.PickSingleFileAsync() is { Path: { } path } && File.Exists(path) &&
            (Production = await Production.ReadFromFileAsync(path)) is { Scenes.Count: not 0 })
        {
            _navigation.Navigate(PlaySoundPage.ScenePage, Production.Scenes.FirstOrDefault(), new EntranceNavigationTransitionInfo());
        }
    }

    [RelayCommand]
    public async Task SaveCurrentProduction()
    {
        // TODO: Save as if file doesn't exist
        if (File.Exists(Production.Path))
        {
            await Production.WriteToFileAsync();
        }
    }

    [RelayCommand]
    public async Task SaveProductionAsync()
    {
        FileSavePicker picker = new(_manager.MainWindow.AppWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            FileTypeChoices = { { "Production", [".prod"] } },
            SuggestedFileName = "My production"
        };

        if (await picker.PickSaveFileAsync() is { Path: { } path })
        {
            Production.Path = path;
            await Production.WriteToFileAsync();
        }
    }

    [RelayCommand]
    public async Task CreateSceneAsync()
    {
        TextBox box = new()
        {
            PlaceholderText = "Scene name"
        };

        ContentDialog dialog = new()
        {
            Title = "Create scene",
            Content = box,
            PrimaryButtonText = "Create",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = _manager.MainWindow.Content.XamlRoot
        };

        if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        {
            Production.Scenes.Add(new Scene(box.Text));
        }
    }
}
