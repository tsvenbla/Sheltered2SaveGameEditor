using Microsoft.UI.Xaml.Controls;
using Sheltered2SaveGameEditor;

public static class NavigationHelper
{
    public static bool Navigate<T>(object? parameter = null) where T : Page => MainWindow.RootFrameInstance?.Navigate(typeof(T), parameter) ?? false;

    public static void GoBack()
    {
        if (MainWindow.RootFrameInstance?.CanGoBack == true)
            MainWindow.RootFrameInstance.GoBack();
    }

    public static void GoForward()
    {
        if (MainWindow.RootFrameInstance?.CanGoForward == true)
            MainWindow.RootFrameInstance.GoForward();
    }
}