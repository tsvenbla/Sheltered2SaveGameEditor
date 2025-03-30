using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace Sheltered2SaveGameEditor;

public sealed partial class MainWindow : Window
{
    public static Frame? RootFrameInstance { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        CustomizeTitleBar();
        RootFrameInstance = RootFrame;
    }

    private void CustomizeTitleBar()
    {
        // Obtain the AppWindow for the current Window
        nint hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        // Extend content into the title bar
        ExtendsContentIntoTitleBar = true;

        // Access the AppWindowTitleBar
        AppWindowTitleBar titleBar = appWindow.TitleBar;

        // Set the preferred height option to Tall
        titleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) =>
        throw new Exception($"Failed to load Page {e.SourcePageType.FullName}: {e.Exception}");
}