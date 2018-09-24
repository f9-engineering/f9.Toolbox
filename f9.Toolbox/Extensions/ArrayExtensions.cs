using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace f9.Toolbox.Extensions
{
  public static class ArrayExtensions
  {
    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
      var result = new T[length];
      Array.Copy(data, index, result, 0, length);
      return result;
    }

    /// <summary>
    /// Serializes the specified array of two dimensions.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <returns></returns>
    public static string Serialize(this int[][] array)
    {
      return string.Join(",", array.Select(a => string.Join(" ", a.Select(e => e.ToString(CultureInfo.InvariantCulture)).ToArray())).ToArray());
    }

    /// <summary>
    /// Serializes the specified array of two dimensions.
    /// The compression replace zero by empty string
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="precision">The precision.</param>
    /// <param name="isCompressionEnabled">if set to <c>true</c> [is compression enabled].</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">The paramater Precision is out of range, should be between 0 and 3.</exception>
    public static string Serialize(this float[,] array, int precision = 0, bool isCompressionEnabled = false)
    {
      var builder = new StringBuilder();

      var lastiIndex = array.GetLength(0) - 1;
      var lastjIndex = array.GetLength(1) - 1;

      for (var i = 0; i < array.GetLength(0); i++)
      {
        
        for (var j = 0; j < array.GetLength(1); j++)
        {
          if (!isCompressionEnabled || array[i, j] != 0)
          {
            if (precision == 0)
              builder.Append(array[i, j].ToString(CultureInfo.InvariantCulture));
            else if (precision == 1)
              builder.Append(array[i, j].ToString("0.#", CultureInfo.InvariantCulture));
            else if (precision == 2)
              builder.Append(array[i, j].ToString("0.##", CultureInfo.InvariantCulture));
            else if (precision == 3)
              builder.Append(array[i, j].ToString("0.###", CultureInfo.InvariantCulture));
            else
              throw new InvalidOperationException("The precision is out of range.");
          }
          

          if (j != lastjIndex) builder.Append(",");
        }
        if (i != lastiIndex) builder.Append(";");
      }

      return builder.ToString();
    }

    /// <summary>
    /// Serializes the specified array of two dimensions.
    /// The compression replace zero by empty string
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="isCompressionEnabled">if set to <c>true</c> [is compression enabled].</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">The paramater Precision is out of range, should be between 0 and 3.</exception>
    public static string Serialize(this int[,] array, bool isCompressionEnabled = false)
    {
      var builder = new StringBuilder();

      var lastiIndex = array.GetLength(0) - 1;
      var lastjIndex = array.GetLength(1) - 1;

      for (var i = 0; i < array.GetLength(0); i++)
      {
        for (var j = 0; j < array.GetLength(1); j++)
        {
          if (!isCompressionEnabled || array[i, j] != 0)
          {
            builder.Append(array[i, j].ToString(CultureInfo.InvariantCulture));
          }
          if (j != lastjIndex) builder.Append(",");
        }
        if (i != lastiIndex) builder.Append(";");
      }

      return builder.ToString();
    }

    /// <summary>
    /// Deserialize2s the D array (Warning only returning integers).
    /// </summary>
    /// <param name="serializedArray">The serialized array.</param>
    /// <returns></returns>
    public static int[][] Deserialize2DArray(this string serializedArray)
    {
      var rows = serializedArray.Split(',');
      var array = new int[rows.Length][];
      for (var i = 0; i < rows.Length; i++)
      {
        array[i] = rows[i].Split(' ').Select(int.Parse).ToArray();
      }
      return array;
    }

    public static T[] ToOneDimension<T>(this T[,] array)
    {
      int width = array.GetLength(0);
      int height = array.GetLength(1);
      var ret = new T[width * height];

      for (var i = 0; i < width; i++)
      {
        for (var j = 0; j < height; j++)
        {
          ret[i* height + j] = array[i, j];
        }
      }

      return ret;
    }

    public static T[] ToOneDimension<T>(this T[][] array)
    {
      var height = array.Length;
      var width = array[0].Length;
      var ret = new T[height * width];

      for (var i = 0; i < height; i++)
      {
        for (var j = 0; j < width; j++)
        {
          ret[i * width + j] = array[i][j];
        }
      }

      return ret;
    }

    public static string ToSpaceSeparatedValues(this float[,] array)
    {
      return string.Join(" ", array.ToOneDimension().Select(v => v.ToString(CultureInfo.InvariantCulture)).ToArray());
    }
  }
}
