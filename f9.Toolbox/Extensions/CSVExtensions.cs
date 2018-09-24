using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace f9.Toolbox.Extensions
{

  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public sealed class CsvIgnoreAttribute : Attribute
  {
  }

  public static class CsvExtensions
  {
    public static string[][] GetTable<T>(this IEnumerable<T> objects)
    {
      var table = new List<string[]>();
      var properties = typeof(T).GetProperties().Where(p => p.CanRead && !Attribute.IsDefined(p, typeof(CsvIgnoreAttribute))).ToArray();

      table.Add(properties.Select(p => p.Name).ToArray());

      foreach (var o in objects)
      {
        table.Add(o.GetRow(properties));
      }

      return table.ToArray();
    }

    private static string[] GetRow<T>(this T target, IEnumerable<PropertyInfo> properties)
    {
      var values = new List<string>();

      foreach (var propertyInfo in properties)
      {
        if(propertyInfo.PropertyType == typeof(FileInfo))
        {
          var fileInfo = propertyInfo.GetValue(target, null) as FileInfo;
          if(fileInfo != null) values.Add(fileInfo.FullName);
        }
        else
        {
          values.Add(Convert.ToString(propertyInfo.GetValue(target, null), CultureInfo.InvariantCulture));
        }
      }

      return values.ToArray();
    }

    public static void ExportToCsv<T>(this IEnumerable<T> objects, FileInfo csvFile)
    {
      using (var textWriter = csvFile.CreateText())
      {
        if (objects.Any())
        {
          var table = objects.GetTable();
          foreach (var row in table)
          {
            textWriter.WriteLine(string.Join(", ", row));
          }
        }
      }
    }

    public static IEnumerable<T> ImportCsv<T>(this FileInfo csvFile)
    {
      return ImportCsv<T>(csvFile, ',', false);
    }

    public static IEnumerable<T> ImportCsv<T>(this FileInfo csvFile, Char separator)
    {
      return ImportCsv<T>(csvFile, separator, false);
    }

    public static IEnumerable<T> ImportCsv<T>(this FileInfo csvFile, Char separator, bool isUnknownColumnIgnored)
    {
      if (csvFile == null)
      {
        throw new ArgumentNullException(nameof(csvFile));
      }

      csvFile.Refresh();

      if (!csvFile.Exists)
      {
        return new List<T>();
      }

      using (var textReader = csvFile.OpenText())
      {
        return ImportCsv<T>(textReader, separator, isUnknownColumnIgnored);
      }
    }

    public static IEnumerable<T> ImportCsv<T>(this StreamReader csvStream, char separator)
    {
      return ImportCsv<T>(csvStream, separator, false);
    }

    public static IEnumerable<T> ImportCsv<T>(this StreamReader csvStream, char separator, bool isUnknownColumnIgnored)
    {
      if (csvStream == null) throw new ArgumentNullException(nameof(csvStream));

      var objects = new List<T>();

      var firstLine = csvStream.ReadLine();

      if (string.IsNullOrEmpty(firstLine)) return objects;

      var propertyNames = firstLine.Split(separator).Select(n => n.Trim()).ToArray();
      var properties = typeof(T).GetProperties().ToDictionary(p => p.Name, p => p);
      
      var csvLine = csvStream.ReadLine();
      while (!string.IsNullOrEmpty(csvLine))
      {
        var obj = Activator.CreateInstance<T>();
        var values = csvLine.Split(separator).Select(n => n.Trim()).ToArray();

        for (var i = 0; i < propertyNames.Length; i++)
        {

          PropertyInfo property;

          if (properties.TryGetValue(propertyNames[i], out property))
          {
            if (!string.IsNullOrEmpty(values[i]) && !Attribute.IsDefined(property, typeof(CsvIgnoreAttribute)))
            {
              try
              {
                if (property.PropertyType == typeof (FileInfo))
                {
                  property.SetValue(obj, new FileInfo(values[i]), null);
                }
                else
                {
                  var value = Convert.ChangeType(values[i], property.PropertyType, CultureInfo.InvariantCulture);
                  property.SetValue(obj, value, null);
                }
              }
              catch (Exception ex)
              {
                throw new InvalidOperationException(
                  "Cannot parse the string :'" + values[i] + "' for the property '" + propertyNames[i] + "'.", ex);
              }
            }
          }
          else if (!isUnknownColumnIgnored)
          {
            throw new Exception("Property '" + propertyNames[i] + "' does not exists.");
          }
        }
        objects.Add(obj);

        csvLine = csvStream.ReadLine();
      }

      return objects;
    }

    public static List<string[]> ReadCsvFile(this FileInfo csvFile, Char separator)
    {
      if (csvFile == null)
      {
        throw new ArgumentNullException(nameof(csvFile));
      }

      csvFile.Refresh();

      using (var textReader = csvFile.OpenText())
      {
        return textReader.ReadCsvFile(separator);
      }
    }

    public static List<string[]> ReadCsvFile(this StreamReader csvStream, char separator)
    {
      if (csvStream == null) throw new ArgumentNullException(nameof(csvStream));

      var csvTable = new List<string[]>();

      var csvLine = csvStream.ReadLine();
      while (!string.IsNullOrEmpty(csvLine))
      {
        csvTable.Add(csvLine.Split(separator).Select(n => n.Trim()).ToArray());
        csvLine = csvStream.ReadLine();
      }

      return csvTable;
    }
  }
}
