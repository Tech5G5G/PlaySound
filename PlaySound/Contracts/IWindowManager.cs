using Microsoft.UI.Xaml;

namespace PlaySound.Contracts;

public interface IWindowManager
{
    IReadOnlyList<Window> Windows { get; }

    MainWindow MainWindow { get; }

    T CreateWindow<T>() where T : Window, new();
}
