using System;
using System.Collections.Generic;
using System.Linq;

namespace f9.Toolbox.Extensions
{
  public static class EnumerableExtensions
  {
    // Ex: collection.TakeLast(5);
    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
    {
      return source.Skip(Math.Max(0, source.Count() - count));
    }
  }
}
