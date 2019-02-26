using System;

namespace f9.Toolbox.Extensions
{
  public static class IntExtensions
  {
    /// <summary>
    /// To the letter: taken from : https://stackoverflow.com/questions/181596/how-to-convert-a-column-number-eg-127-into-an-excel-column-eg-aa
    /// </summary>
    /// <param name="number">The number (starting at 0).</param>
    /// <returns>Excel Style column name</returns>
    public static string ToLetters(this int number)
    {
      var dividend = number + 1;
      var columnName = string.Empty;

      while (dividend > 0)
      {
        var modulo = (dividend - 1) % 26;
        columnName = Convert.ToChar('A' + modulo) + columnName;
        dividend = (dividend - modulo) / 26;
      }

      return columnName;
    }
  }
}
