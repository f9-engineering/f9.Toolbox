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

    public static T GetRandomItem<T>(this IEnumerable<T> list)
    {
      var list1 = list.ToList();
      var rand = new Random();
      return list1.ElementAt(rand.Next(list1.Count));
    }

    /// <summary>
    /// Indicates whether the specified array is null or has a length of zero.
    /// https://stackoverflow.com/questions/8560106/isnullorempty-equivalent-for-array-c-sharp
    /// </summary>
    /// <param name="array">The array to test.</param>
    /// <returns>true if the array parameter is null or has a length of zero; otherwise, false.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> array)
    {
      return array == null || !array.Any();
    }
  }
}
