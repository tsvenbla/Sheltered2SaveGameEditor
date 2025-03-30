using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Sheltered2SaveGameEditor.Pages;

/// <summary>
/// The home page displayed when the application starts.
/// </summary>
public sealed partial class HomePage : Page
{
    public HomePage() => InitializeComponent();

    private async void LoadFileButton_Click(object sender, RoutedEventArgs e)
    {
        LoadFileTextBlock.Text = "Selecting a file...";
        try
        {
            FileOpenPicker picker = new();

            InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(App.StartupWindow));
            string defaultFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"..\LocalLow\Unicube\Sheltered2");
            _ = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(defaultFolderPath);

            picker.FileTypeFilter.Add(".dat");

            Windows.Storage.StorageFile? file = await picker.PickSingleFileAsync();
            if (file is not null)
            {
                // Handle the selected file here
                // e.g., Load file content, parse data, navigate to another page, etc.
            }
            SaveFileButton.IsEnabled = true;
        }
        catch (Exception ex)
        {
            LoadFileTextBlock.Text = $"Error: {ex.Message}";
        }
    }

    private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
    {
        return;
    }
}