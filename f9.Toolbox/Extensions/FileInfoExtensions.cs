using System;
using System.IO;

namespace f9.Toolbox.Extensions
{
  public static class FileInfoExtensions
  {
    /// <summary>
    /// Gets the relative path.
    /// From : https://stackoverflow.com/a/703292/2939215
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="folder">The folder.</param>
    /// <returns>The relative path to the file</returns>
    public static string GetRelativePath(this FileInfo file, string folder)
    {
      Uri pathUri = new Uri(file.FullName);
      // Folders must end in a slash
      if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
      {
        folder += Path.DirectorySeparatorChar;
      }
      Uri folderUri = new Uri(folder);
      return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
    }
  }
}
