using Microsoft.UI.Xaml;
using PlaySound.Contracts;

namespace PlaySound.Services;

public sealed partial class WindowManager : IWindowManager
{
    public IReadOnlyList<Window> Windows => _windows;
    private readonly List<Window> _windows = [];

    public MainWindow MainWindow => _mainWindow;
    private MainWindow _mainWindow;

    public T CreateWindow<T>() where T : Window, new()
    {
        T window = new();
        _windows.Add(window);

        if (window is MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }    

        return window;
    }
}
