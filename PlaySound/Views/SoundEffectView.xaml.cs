using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PlaySound.ViewModels;

namespace PlaySound.Views;

public sealed partial class SoundEffectView : UserControl
{
    public SoundEffectViewModel ViewModel
    {
        get => (SoundEffectViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static DependencyProperty ViewModelProperty { get; } =
        DependencyProperty.Register(nameof(ViewModel), typeof(SoundEffectViewModel), typeof(SoundEffectView), new(defaultValue: null));

    public SoundEffectView()
    {
        InitializeComponent();
    }

    private double GetSeconds(TimeSpan time)
    {
        return time.TotalSeconds;
    }
}
