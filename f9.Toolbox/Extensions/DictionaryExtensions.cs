using System.Collections.Generic;

namespace f9.Toolbox.Extensions
{
  public static class DictionaryExtensions
  {
    public static void AddIfUndefined<TK, TV>(this Dictionary<TK, TV> dictionary, TK key, TV value)
    {
      if (dictionary.ContainsKey(key)) return;
      dictionary.Add(key, value);
    }
  }
}
