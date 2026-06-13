using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using PlaySound.Views;
using PlaySound.Contracts;

namespace PlaySound.Services;

public sealed class NavigationService : IPlaySoundNavigationService
{
    public PlaySoundPage Page
    {
        get => _frame.Content switch
        {
            ScenePage => PlaySoundPage.ScenePage,
            _ => PlaySoundPage.None
        };
    }

    private Frame _frame;

    public void Navigate(PlaySoundPage page, object parameter = null, NavigationTransitionInfo transition = null)
    {
        _frame.Navigate(
            page switch
            {
                PlaySoundPage.ScenePage => typeof(ScenePage),
                _ => typeof(BlankPage)
            },
            parameter,
            transition);
    }

    public void GoBack()
    {
        if (_frame.CanGoBack)
        {
            _frame.GoBack();
        }
    }

    public void SetFrame(Frame frame)
    {
        if (_frame is not null)
        {
            throw new InvalidOperationException($"Cannot set the frame of {GetType().FullName} more than once.");
        }

        _frame = frame;
    }
}
