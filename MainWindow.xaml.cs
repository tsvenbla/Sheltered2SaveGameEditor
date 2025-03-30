using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Sheltered2SaveGameEditor.Helpers;
using System;

namespace Sheltered2SaveGameEditor;

public sealed partial class MainWindow : Window
{
    public static Frame? RootFrameInstance { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        CustomizeTitleBar();

        // Set up navigation
        RootFrameInstance = RootFrame;
        _ = RootFrameInstance.Navigate(typeof(Pages.HomePage));

        // Assign event handler for navigation view
        NavigationViewControl.SelectionChanged += OnNavigationViewSelectionChanged;
        NavigationViewControl.BackRequested += (s, e) => NavigationHelper.GoBack();
        NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[0];
    }

    private void OnNavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item && item.Tag is string pageTag)
        {
            _ = NavigationHelper.NavigateByNavItemTag(pageTag);
        }
    }

    private void CustomizeTitleBar()
    {
        // Obtain the AppWindow for the current Window
        nint hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        ExtendsContentIntoTitleBar = true;
        AppWindowTitleBar titleBar = appWindow.TitleBar;
        titleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        Title = Application.Current.Resources["AppTitle"] as string;
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) =>
        throw new Exception($"Failed to load Page {e.SourcePageType.FullName}: {e.Exception}");
}