using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Threading;
using Windows.Storage;
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
        LoadFileButton.IsEnabled = false;
        LoadFileTextBlock.Text = "Selecting a file...";

        try
        {
            StorageFile? file = await Helpers.FileHelper.PickFileAsync(CancellationToken.None);
            if (file is null)
            {
                LoadFileTextBlock.Text = "No file selected.";
                LoadFileButton.IsEnabled = true;
                return;
            }

            LoadFileTextBlock.Text = $"Loading {file.Name}...";
            string decryptedContent = await Helpers.FileHelper.LoadAndDecryptSaveFileAsync(file);
        }
        catch (Exception ex) when (ex is FileNotFoundException or InvalidDataException)
        {
            LoadFileTextBlock.Text = $"An error occurred while loading the file: {ex.Message}";
        }
        finally
        {
            LoadFileButton.IsEnabled = true;
        }
    }

    private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
    {
        return;
    }
}