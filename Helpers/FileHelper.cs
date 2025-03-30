using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Sheltered2SaveGameEditor.Helpers;
internal static class FileHelper
{
    private const string ExpectedHeader = "<root>";
    private const string ExpectedFooter = "</root>";
    private const ulong MaxFileSize = 25 * 1024 * 1024; // 25 MB
    private const string BackupFileSuffix = "_backup_";
    private const string BackupDateFormat = "yyyyMMdd_HHmmss";

    /// <summary>
    /// Opens a file picker dialog to select a save file.
    /// </summary>
    internal static async Task<StorageFile?> PickFileAsync(CancellationToken cancellationToken = default)
    {
        FileOpenPicker picker = new()
        {
            FileTypeFilter = { ".dat" }
        };

        try
        {
            InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(App.StartupWindow));
            _ = await StorageFolder.GetFolderFromPathAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"..\LocalLow\Unicube\Sheltered2"));
            StorageFile? file = await picker.PickSingleFileAsync().AsTask(cancellationToken).ConfigureAwait(false);

            return file;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or COMException)
        {
            throw new InvalidOperationException($"An error occurred while initialize the file picker.", ex);
        }
    }

    /// <summary>
    /// Loads and decrypts the specified save file.
    /// </summary>
    internal static async Task<string> LoadAndDecryptSaveFileAsync(StorageFile file, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (!await IsValidSaveFileAsync(file, cancellationToken).ConfigureAwait(false))
        {
            throw new InvalidDataException($"The file '{file.Name}' is not a valid save file.");
        }

        try
        {
            byte[] decryptedData = await XorCipherHelper.LoadAndDecryptAsync(file.Path, cancellationToken).ConfigureAwait(false);
            string content = Encoding.UTF8.GetString(decryptedData);
            return content;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or COMException)
        {
            throw new IOException($"An error occurred while loading or decrypting '{file.Path}'.", ex);
        }
    }

    /// <summary>
    /// Encrypts and saves the specified content to a file.
    /// </summary>
    internal static async Task EncryptAndSaveSaveFileAsync(StorageFile file, string content, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        try
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            await XorCipherHelper.EncryptAndSaveAsync(file.Path, contentBytes, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or COMException)
        {
            throw new IOException($"An error occurred while encrypting or saving '{file.Path}'.", ex);
        }
    }

    /// <summary>
    /// Creates a backup copy of the specified file in the same directory.
    /// </summary>
    internal static async Task<StorageFile?> CreateBackupAsync(StorageFile file, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        try
        {
            StorageFolder folder = await file.GetParentAsync();
            string backupFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}{BackupFileSuffix}{DateTime.Now.ToString(BackupDateFormat, CultureInfo.InvariantCulture)}{Path.GetExtension(file.Name)}";
            StorageFile backupFile = await file.CopyAsync(folder, backupFileName, NameCollisionOption.GenerateUniqueName)
                             .AsTask(cancellationToken)
                             .ConfigureAwait(false);

            return backupFile;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or COMException)
        {
            return null;
        }
    }

    /// <summary>
    /// Validates whether the specified file is a properly formatted and valid save file.
    /// </summary>
    internal static async Task<bool> IsValidSaveFileAsync(StorageFile file, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        try
        {
            Windows.Storage.FileProperties.BasicProperties fileProperties = await file.GetBasicPropertiesAsync();
            if (fileProperties.Size > MaxFileSize)
                return false;

            byte[] encryptedData = await File.ReadAllBytesAsync(file.Path, cancellationToken).ConfigureAwait(false);
            if (encryptedData.Length == 0)
                return false;

            byte[] decryptedData = XorCipherHelper.Transform(encryptedData, cancellationToken);
            if (decryptedData.Length == 0)
                return false;

            string decryptedContent = Encoding.UTF8.GetString(decryptedData).Trim();
            return HasValidSignature(decryptedContent);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if the decrypted content has the expected XML header and footer.
    /// </summary>
    private static bool HasValidSignature(string decryptedContent) =>
        decryptedContent.StartsWith(ExpectedHeader, StringComparison.Ordinal) &&
        decryptedContent.EndsWith(ExpectedFooter, StringComparison.Ordinal);
}
