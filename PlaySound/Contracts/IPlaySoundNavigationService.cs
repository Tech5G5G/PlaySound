using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace PlaySound.Contracts;

public interface IPlaySoundNavigationService
{
    PlaySoundPage Page { get; }

    void Navigate(PlaySoundPage page, object parameter = null, NavigationTransitionInfo transition = null);
    void GoBack();

    void SetFrame(Frame frame);
}

public enum PlaySoundPage
{
    None,
    ScenePage
}
