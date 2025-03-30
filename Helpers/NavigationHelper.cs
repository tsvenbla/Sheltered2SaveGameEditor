using Microsoft.UI.Xaml.Controls;
using Sheltered2SaveGameEditor.Pages;
using System;

namespace Sheltered2SaveGameEditor.Helpers;

/// <summary>
/// Provides helper methods for navigation throughout the application.
/// </summary>
public static class NavigationHelper
{
    /// <summary>
    /// Navigates to a page of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the page to navigate to.</typeparam>
    /// <param name="parameter">Optional parameter to pass to the destination page.</param>
    /// <returns>True if navigation was successful; otherwise, false.</returns>
    public static bool Navigate<T>(object? parameter = null) where T : Page =>
        MainWindow.RootFrameInstance?.Navigate(typeof(T), parameter) ?? false;

    /// <summary>
    /// Navigates to a page based on the NavigationViewItem's x:Name.
    /// </summary>
    /// <param name="navigationItemTag">The Tag of the NavigationViewItem.</param>
    /// <param name="parameter">Optional parameter to pass to the destination page.</param>
    /// <returns>True if navigation was successful; otherwise, false.</returns>
    public static bool NavigateByNavItemTag(string navigationItemTag, object? parameter = null)
    {
        Type? pageType = navigationItemTag switch
        {
            "Home" => typeof(HomePage),
            // Add mappings for other navigation items as their pages are created
            // "Characters" => typeof(CharactersPage),
            // "Pets" => typeof(PetsPage),
            // "Inventory" => typeof(InventoryPage),
            // "Crafting" => typeof(CraftingPage),
            // "Factions" => typeof(FactionsPage),
            // "Donate" => typeof(DonatePage),
            _ => null
        };

        return pageType != null && (MainWindow.RootFrameInstance?.Navigate(pageType, parameter) ?? false);
    }

    /// <summary>
    /// Navigates to the previous page in the navigation stack if possible.
    /// </summary>
    public static void GoBack()
    {
        if (MainWindow.RootFrameInstance?.CanGoBack == true)
            MainWindow.RootFrameInstance.GoBack();
    }

    /// <summary>
    /// Navigates to the next page in the navigation stack if possible.
    /// </summary>
    public static void GoForward()
    {
        if (MainWindow.RootFrameInstance?.CanGoForward == true)
            MainWindow.RootFrameInstance.GoForward();
    }
}