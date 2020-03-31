using System.IO;
using System.Text;
using log4net;

namespace f9.Toolbox.Extensions
{
  public static class StringExtensions
  {
    private static readonly ILog m_Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetValidPathName(this string sequenceName)
    {
      var validName = sequenceName;
      var invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

      foreach (var c in invalid)
      {
        validName = validName.Replace(c.ToString(), "");
      }

      if (validName != sequenceName)
      {
        m_Log.Warn("The sequence name " + sequenceName + " is not valid and has be changed to " + validName);
      }

      return validName;
    }

    /// <summary>
    /// From https://stackoverflow.com/questions/1522884/remove-all-non-ascii-characters-from-string/14145356
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToCleanAscii(this string s)
    {
      if (s == null) return null;

      var stringBuilder = new StringBuilder(s.Length);
      foreach (var c in s)
      {
        if (c > 127) // you probably don't want 127 either
          continue;
        if (c < 32)  // I bet you don't want control characters 
          continue;
        if (c == ',')
          continue;
        if (c == '"')
          continue;
        stringBuilder.Append(c);
      }
      return stringBuilder.ToString();
    }
  }
}
