using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Sheltered2SaveGameEditor.Helpers;

/// <summary>
/// Implements XOR cipher operations for encrypting and decrypting files.
/// </summary>
internal sealed class XorCipherHelper
{
    private static readonly ImmutableArray<byte> XorKey = ImmutableArray.Create<byte>(
        0xAC, 0x73, 0xFE, 0xF2, 0xAA, 0xBA, 0x6D, 0xAB,
        0x30, 0x3A, 0x8B, 0xA7, 0xDE, 0x0D, 0x15, 0x21, 0x4A
    );

    /// <summary>
    /// Loads and decrypts a file using XOR cipher.
    /// </summary>
    internal static async Task<byte[]> LoadAndDecryptAsync(string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file '{filePath}' was not found.", filePath);

        try
        {
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken).ConfigureAwait(false);
            return Transform(fileBytes, cancellationToken);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"An error occurred while reading or decrypting '{filePath}'.", ex);
        }
    }

    /// <summary>
    /// Encrypts and saves data to a file using XOR cipher.
    /// </summary>
    internal static async Task EncryptAndSaveAsync(string filePath, byte[] content, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        ArgumentNullException.ThrowIfNull(content);

        try
        {
            byte[] encryptedBytes = Transform(content, cancellationToken);
            await File.WriteAllBytesAsync(filePath, encryptedBytes, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
        {
            throw new InvalidOperationException($"An error occurred while encrypting or saving '{filePath}'.", ex);
        }
    }

    /// <summary>
    /// Performs an XOR transformation on the data.
    /// </summary>
    internal static byte[] Transform(byte[] input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.Length == 0)
            return [];

        byte[] output = new byte[input.Length];
        ReadOnlySpan<byte> keySpan = XorKey.AsSpan();
        int keyLength = keySpan.Length;

        for (int i = 0; i < input.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            output[i] = (byte)(input[i] ^ keySpan[i % keyLength]);
        }

        return output;
    }
}