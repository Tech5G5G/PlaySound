using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage.Pickers;
using PlaySound.Contracts;
using PlaySound.Media;
using PlaySound.Models;
using System.Collections.ObjectModel;

namespace PlaySound.ViewModels;

public sealed partial class ScenePageViewModel(IWindowManager manager, IServiceProvider provider) : ObservableObject
{
    [ObservableProperty]
    public partial Scene Scene { get; set; }

    public ObservableCollection<SoundEffectViewModel> Effects { get; } = [];

    private readonly IWindowManager _manager = manager;
    private readonly IServiceProvider _provider = provider;

    [RelayCommand]
    public async Task AddSoundEffectAsync()
    {
        FileOpenPicker picker = new(_manager.MainWindow.AppWindow.Id)
        {
            SuggestedStartLocation = PickerLocationId.MusicLibrary,
            FileTypeFilter =
            {
                ".3g2", ".3gp", ".3gp2", ".3gpp",
                ".asf", ".wma", ".wmv",
                ".aac", ".adts",
                ".avi",
                ".mp3",
                ".m4a", ".m4v", ".mov", ".mp4",
                ".sami", ".smi",
                ".wav"
            }
        };

        if (await picker.PickSingleFileAsync() is { Path: { } path } && File.Exists(path))
        {
            var sfx = SoundEffectFactory.CreateSoundEffect(path);
            sfx.Name = Path.GetFileNameWithoutExtension(path);

            Scene.Effects.Add(sfx);
            AddSoundEffect(sfx);
        }
    }

    public void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is Scene scene)
        {
            Scene = scene;

            foreach (var sfx in scene.Effects)
            {
                AddSoundEffect(sfx);
            }
        }
    }

    private void AddSoundEffect(ISoundEffect sfx)
    {
        var model = _provider.GetRequiredService<SoundEffectViewModel>();

        model.RemoveCommand = new(() =>
        {
            Scene.Effects.Remove(model.Effect);
            Effects.Remove(model);
        });

        model.Effect = sfx;
        Effects.Add(model);
    }
}
