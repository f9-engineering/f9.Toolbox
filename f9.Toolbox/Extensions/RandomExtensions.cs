using System;

namespace f9.Toolbox.Extensions
{
  public static class RandomExtensions
  {
    // https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
    public static void Shuffle<T>(this Random rng, T[] array)
    {
      var n = array.Length;
      while (n > 1)
      {
        var k = rng.Next(n--);
        var temp = array[n];
        array[n] = array[k];
        array[k] = temp;
      }
    }

  }
}
