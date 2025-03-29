using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using Microsoft.UI.Xaml.Controls;
using Sheltered2SaveGameEditor.Pages;

namespace Sheltered2SaveGameEditor;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public sealed partial class App : Application
{
    /// <summary>
    /// Get the initial window created for this app.
    /// </summary>
    public static Window? StartupWindow { get; private set; }

    /// <summary>
    /// Initializes the singleton Application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
        UnhandledException += HandleExceptions;
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        StartupWindow = new MainWindow
        {
            ExtendsContentIntoTitleBar = true
        };

        StartupWindow.Activate();
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e) => throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

    /// <summary>
    /// Prevents the app from crashing when a exception gets thrown and notifies the user.
    /// </summary>
    /// <param name="sender">The app as an object.</param>
    /// <param name="e">Details about the exception.</param>
    private void HandleExceptions(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        e.Handled = true; //Don't crash the app.

        //Create the notification.
        AppNotification notification = new AppNotificationBuilder()
            .AddText("An exception was thrown.")
            .AddText($"Type: {e.Exception.GetType()}")
            .AddText($"Message: {e.Message}\r\n" +
                     $"HResult: {e.Exception.HResult}")
            .BuildNotification();

        //Show the notification
        AppNotificationManager.Default.Show(notification);
    }
}