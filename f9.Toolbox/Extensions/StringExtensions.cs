using System.IO;
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
  }
}
