using System;
using System.IO;

namespace f9.Toolbox.Helpers
{
  public static class FileInfoHelper
  {
    private static string m_ExecutableDirectory;
    public static string ExecutableDirectory => m_ExecutableDirectory ?? (m_ExecutableDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

    public static FileInfo GetFileInfoRelativeToExecutable(string relativePath)
    {
      return new FileInfo(Path.Combine(ExecutableDirectory, relativePath));
    }

    public static int LineCount(this FileInfo file)
    {
      var lineCount = 0;
      using (var reader = file.OpenText())
      {
        while (reader.ReadLine() != null)
        {
          lineCount++;
        }
      }
      return lineCount;
    }

    public static bool IsLocked(this FileInfo file)
    {
      FileStream stream = null;

      try
      {
        stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
      }
      catch (IOException)
      {
        //the file is unavailable because it is:
        //still being written to
        //or being processed by another thread
        //or does not exist (has already been processed)
        return true;
      }
      catch(UnauthorizedAccessException)
      {
        return true;
      }
      finally
      {
        stream?.Close();
      }

      //file is not locked
      return false;
    }
  }
}
