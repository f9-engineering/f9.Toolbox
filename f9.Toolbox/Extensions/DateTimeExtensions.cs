using System;

namespace f9.Toolbox.Extensions
{
  public static class DateTimeExtensions
  {
    public static string ToReadableTimeStamp(this DateTime time)
    {
      return "" + time.Year.ToString().Substring(2) + time.Month.ToString("00") + time.Day.ToString("00") + time.Hour.ToString("00") + time.Minute.ToString("00") + time.Second.ToString("00");
    }
  }
}
