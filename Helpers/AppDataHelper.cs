using Sheltered2SaveGameEditor.Models;
using System.Collections.Generic;
using Windows.Storage;

namespace Sheltered2SaveGameEditor.Helpers;
internal class AppDataHelper
{
    public StorageFile? SourceFile { get; set; }
    public string DecryptedContent { get; set; } = string.Empty;
    public IReadOnlyList<Character> Characters { get; set; } = [];
    public bool IsLoaded => SourceFile != null && !string.IsNullOrEmpty(DecryptedContent);
}