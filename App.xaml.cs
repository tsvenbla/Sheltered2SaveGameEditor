using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Sheltered2SaveGameEditor.Helpers;
using System;
using System.Diagnostics;

namespace Sheltered2SaveGameEditor;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public sealed partial class App : Application
{
    /// <summary>
    /// Gets the initial window created for this app.
    /// </summary>
    internal static Window? StartupWindow { get; private set; }

    internal static AppDataHelper? CurrentSaveData { get; set; }

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
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // Create and activate the main window
        StartupWindow = new MainWindow();
        StartupWindow.Activate();
    }

    /// <summary>
    /// Prevents the app from crashing when an exception gets thrown and notifies the user.
    /// </summary>
    /// <param name="sender">The app as an object.</param>
    /// <param name="e">Details about the exception.</param>
    private void HandleExceptions(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        e.Handled = true; // Don't crash the app

        try
        {
            // Create a notification to inform the user
            AppNotification notification = new AppNotificationBuilder()
                .AddText("An unexpected error occurred")
                .AddText($"Type: {e.Exception.GetType()}")
                .AddText($"Message: {e.Message}")
                .BuildNotification();

            // Show the notification
            AppNotificationManager.Default.Show(notification);
        }
        catch (Exception ex)
        {
            // If notification fails, at least log the exception
            Debug.WriteLine($"Error handling exception: {ex}");
        }
    }
}
