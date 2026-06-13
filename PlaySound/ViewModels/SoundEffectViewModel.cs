using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Input;
using PlaySound.Contracts;

namespace PlaySound.ViewModels;

public sealed partial class SoundEffectViewModel : ObservableObject
{
    public ISoundEffect Effect { get; set; }

    public RelayCommand RemoveCommand { get; set; }

    [RelayCommand]
    public void Play()
    {
        Effect.Play();
    }

    [RelayCommand]
    public void Pause()
    {
        Effect.Pause();
    }

    [RelayCommand]
    public void Restart()
    {
        Effect.Restart();
    }
}
