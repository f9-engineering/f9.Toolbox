using System.Reflection;
using log4net;
using log4net.Appender;

namespace f9.Toolbox.Helpers
{
  public static class Log4NetHelper
  {
    public static void FlushBuffers()
    {
      foreach (var appender in LogManager.GetRepository(Assembly.GetEntryAssembly()).GetAppenders())
      {
        var buffered = appender as BufferingAppenderSkeleton;
        buffered?.Flush();
      }
    }
  }
}
