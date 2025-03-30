using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Sheltered2SaveGameEditor.Pages;
using System;
using System.Linq;

namespace Sheltered2SaveGameEditor;

public sealed partial class MainWindow : Window
{
    public static Frame? RootFrameInstance { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        CustomizeTitleBar();

        RootFrame.Navigated += OnNavigated;
        RootFrame.NavigationFailed += OnNavigationFailed;

        // Navigate initially to Home
        NavigateToPage("Home");
    }

    private void OnNavigationViewBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (RootFrame.CanGoBack)
            RootFrame.GoBack();
    }

    private void OnNavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            // Navigate to Settings page if you have one.
            return;
        }

        if (args.SelectedItem is NavigationViewItem item && item.Tag is string pageTag)
        {
            NavigateToPage(pageTag);
        }
    }

    private void NavigateToPage(string pageTag)
    {
        Type? pageType = GetPageTypeFromTag(pageTag);
        if (pageType is not null && RootFrame.CurrentSourcePageType != pageType)
        {
            _ = RootFrame.Navigate(pageType);
        }
    }

    public static Type? GetPageTypeFromTag(string? tag) =>
        tag switch
        {
            "Home" => typeof(HomePage),
            "Characters" => typeof(CharactersPage),
            "Pets" => typeof(PetsPage),
            "Inventory" => typeof(InventoryPage),
            "Crafting" => typeof(CraftingPage),
            "Factions" => typeof(FactionsPage),
            "Donate" => typeof(DonatePage),
            _ => null
        };

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        NavigationViewControl.IsBackEnabled = RootFrame.CanGoBack;

        // Find and select the corresponding menu item
        NavigationViewItem? selectedItem = NavigationViewControl.MenuItems
            .Concat(NavigationViewControl.FooterMenuItems)
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => GetPageTypeFromTag(item.Tag as string) == e.SourcePageType);

        if (selectedItem is not null && !selectedItem.Equals(NavigationViewControl.SelectedItem))
        {
            NavigationViewControl.SelectedItem = selectedItem;
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