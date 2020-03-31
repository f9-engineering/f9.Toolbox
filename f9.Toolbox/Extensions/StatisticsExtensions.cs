using System;
using System.Collections.Generic;
using System.Linq;

namespace f9.Toolbox.Extensions
{
  public static class StatisticsExtensions
  {
    /// <summary>
    /// https://stackoverflow.com/a/6252351/2939215
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static double StandardDeviation(this IEnumerable<double> values)
    {
      var avg = values.Average();
      return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
    }
  }
}
