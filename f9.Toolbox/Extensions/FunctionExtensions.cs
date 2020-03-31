using System;
using System.Diagnostics;
using System.Reflection;

namespace f9.Toolbox.Extensions
{
  public static class FunctionExtensions
  {
    private static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private static readonly Stopwatch m_Stopwatch = new Stopwatch();

    public static TimeSpan WarningTime { get; } = TimeSpan.FromMilliseconds(400);

    public static T Profile<T>(this Func<T> function)
    {
      m_Stopwatch.Restart();
      var result = function();
      m_Stopwatch.Stop();
      if (m_Stopwatch.Elapsed >= WarningTime)
      {
        m_Log.Warn("The function " + function.Method.Name + " took " + m_Stopwatch.ElapsedMilliseconds + " ms.");
      }

      return result;
    }

    
  }
}
