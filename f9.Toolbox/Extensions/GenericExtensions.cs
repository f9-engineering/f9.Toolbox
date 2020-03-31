using System.Linq;

namespace f9.Toolbox.Extensions
{
  public static class GenericExtensions
  {
    /// <summary>
    /// Copies the properties to.
    /// https://stackoverflow.com/a/28814556/2939215
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TU">The type of the u.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="dest">The dest.</param>
    public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
    {
      var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
      var destProps = typeof(TU).GetProperties().Where(x => x.CanWrite).ToList();

      foreach (var sourceProp in sourceProps)
      {
        var propertyInfo = destProps.FirstOrDefault(p => p.Name == sourceProp.Name);
        if (propertyInfo != null)
        {
          propertyInfo.SetValue(dest, sourceProp.GetValue(source, null), null);
        }
      }

    }
  }
}
