//-----------------------------------------------------------------------
// <copyright file="XElementExtensions.cs" company="Transurb Technirail">
//   Copyright © Transurb Technirail.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
// <author>Laurent De Plaen</author>
// <date>02/03/2012</date>
// <project>RailNet.Model</project>
// <web>http://www.transurb.com</web>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace f9.Toolbox.Extensions
{
  
  public static class XElementExtensions
  {

    private static readonly log4net.ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static bool IsPropertyNotFoundWarningDisabled { get; set; }

    /// <summary>
    /// Restores the value of a property or a member from the storage. The value is retrieved from the storage and affected to the property or the member. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element">The element.</param>
    /// <param name="parameter">The parameter.</param>
    public static void RestoreValue<T>(this XElement element, Expression<Func<T>> parameter)
    {
      var value = GetValue(element, parameter);
      AssignValue(parameter, value);
    }

    private static void AssignValue<T>(Expression<Func<T>> parameter, T value)
    {
      var body = (MemberExpression) parameter.Body;

      object target;
      if(body.Expression is ConstantExpression)
      {
        target = (body.Expression as ConstantExpression).Value;
      }
      else
      {
        throw new Exception("The type of the expression is not supported :" + body.Expression.GetType().FullName);
      }

      // Affectation du membre ou de la propriété
      if (body.Member is FieldInfo)
      {
        var fieldInfo = (FieldInfo)body.Member;
        fieldInfo.SetValue(target, value);
      }
      else if (body.Member is PropertyInfo)
      {
        var propertyInfo = (PropertyInfo)body.Member;
        propertyInfo.SetValue(target, value, null);
      }
      else
      {
        throw new Exception("This type is not handled : " + body.Member.GetType().FullName);
      }
    }

    /// <summary>
    /// Restores the value of a property or a member from the storage. The value is retrieved from the storage and affected to the property or the member.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element">The element.</param>
    /// <param name="parameter">The parameter.</param>
    /// <returns>True if it succeeds</returns>
    public static bool TryRestoreValue<T>(this XElement element, Expression<Func<T>> parameter)
    {
      try
      {
        T value;

        if (TryGetValue(element, parameter, out value))
        {
          AssignValue(parameter, value);
          return true;
        }
      }
      catch (Exception ex)
      {
        m_Logger.Debug(ex);
      }
      return false;
    }

    /// <summary>
    /// Gets the value of a property or a member that was saved in the storage. The property or the member is not affected by the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element">The element.</param>
    /// <param name="parameter">The parameter.</param>
    /// <returns>The requested value</returns>
    public static T GetValue<T>(this XElement element, Expression<Func<T>> parameter)
    {
      var body = (MemberExpression) parameter.Body;
      return GetValue<T>(element, body.Member.Name);
    }

    /// <summary>
    /// Gets the value of a property or a member that was saved in the storage. The property or the member is not affected by the value.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <returns>
    /// The requested value
    /// </returns>
    public static object GetValue(this XElement element, Type type, string name)
    {
      var attribute = element.Attribute(name);
      if (attribute == null)
      {
        //m_Logger.Warn(string.Format(@"The value {0} cannot be found", body.Member.Name));
        //return default(T);
        throw new PropertyNotFoundException(name, element.GetAbsoluteXPath());
      }

      object value;
      if (type == typeof(Guid))
      {
        value = new Guid(attribute.Value);
      }
      else if (type.IsEnum)
      {
        value = Enum.Parse(type, attribute.Value);
      }
      else if (type == typeof(TimeSpan))
      {
        value = TimeSpan.FromMilliseconds(int.Parse(attribute.Value));
      }
      else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        value = Convert.ChangeType(attribute.Value, Nullable.GetUnderlyingType(type), CultureInfo.InvariantCulture);
      }
      else
      {
        value = Convert.ChangeType(attribute.Value, type, CultureInfo.InvariantCulture);
      }

      return value;
    }

    /// <summary>
    /// Gets the value of a property or a member that was saved in the storage. The property or the member is not affected by the value.
    /// </summary>
    /// <typeparam name="T">The type of the return value</typeparam>
    /// <param name="element">The element.</param>
    /// <param name="name">The name.</param>
    /// <returns>The requested value</returns>
    public static T GetValue<T>(this XElement element, string name)
    {
      return (T)GetValue(element, typeof (T), name);
    }

    /// <summary>
    /// Tries get the value of a property or a member that was saved in the storage. The property or the member is not affected by the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element">The element.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="value">The value.</param>
    /// <returns>
    /// True if it succeeds.
    /// </returns>
    public static bool TryGetValue<T>(this XElement element, Expression<Func<T>> parameter, out T value)
    {
      try
      {
        value = GetValue(element, parameter);
        return true;
      }
      catch (PropertyNotFoundException ex)
      {
        if (!IsPropertyNotFoundWarningDisabled)
        {
          var body = (MemberExpression)parameter.Body;
          m_Logger.Warn("Argument " + body.Member.Name + " : " + ex.Message);
        } ;
      }
      catch (Exception ex)
      {
        var body = (MemberExpression) parameter.Body;
        m_Logger.Debug("Argument " + body.Member.Name + " : " + ex.Message);
      }

      value = default(T);
      return false;
    }

    /// <summary>
    /// Tries get the value of a property or a member that was saved in the storage. The property or the member is not affected by the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element">The element.</param>
    /// <param name="argument">The argument.</param>
    /// <param name="value">The value.</param>
    /// <returns>
    /// True if it succeeds.
    /// </returns>
    public static bool TryGetValue<T>(this XElement element, string argument, out T value)
    {
      try
      {
        value = GetValue<T>(element, argument);
        return true;
      }
      catch (PropertyNotFoundException ex)
      {
        if (!IsPropertyNotFoundWarningDisabled)
        {
          m_Logger.Warn("Argument " + argument + " : " + ex.Message);
        };
      }
      catch (Exception ex)
      {
        m_Logger.Debug("Argument " + argument + " : " + ex.Message);
      }

      value = default(T);
      return false;
    }

    public static double GetDouble(this XElement element, string name)
    {
      var attribute = element.Attribute(name);
      if (attribute == null)
      {
        throw new PropertyNotFoundException(name, element.GetAbsoluteXPath());
      }

      return double.Parse(attribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    public static int GetInt32(this XElement element, string name)
    {
      var attribute = element.Attribute(name);
      if (attribute == null)
      {
        throw new PropertyNotFoundException(name, element.GetAbsoluteXPath());
      }

      return int.Parse(attribute.Value);
    }

    public static long GetInt64(this XElement element, string name)
    {
      var attribute = element.Attribute(name);
      if (attribute == null)
      {
        throw new PropertyNotFoundException(name, element.GetAbsoluteXPath());
      }

      return long.Parse(attribute.Value);
    }

    public static Guid GetGuid(this XElement element, string name)
    {
      var attribute = element.Attribute(name);
      if (attribute == null)
      {
        throw new PropertyNotFoundException(name, element.GetAbsoluteXPath());
      }

      return new Guid(attribute.Value);
    }

    public static bool GetBoolean(this XElement element, string name)
    {
      var attribute = element.Attribute(name);
      if (attribute == null)
      {
        throw new PropertyNotFoundException(name, element.GetAbsoluteXPath());
      }

      return bool.Parse(attribute.Value);
    }

    public static TEnum GetEnumValue<TEnum>(this XElement element, string name)
    {
      var attribute = element.Attribute(name);
      if (attribute == null)
      {
        throw new PropertyNotFoundException(name, element.GetAbsoluteXPath());
      }

      return (TEnum) Enum.Parse(typeof(TEnum), attribute.Value);
    }

    /// <summary>
    /// Removes the attribute recursively.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    public static void RemoveAttributeRecursively(this XElement element, string attributeName)
    {
      var attr = element.Attribute(attributeName);
      attr?.Remove();
      foreach (var el in element.Elements()) el.RemoveAttributeRecursively(attributeName);
    }

    public static string GetString(this XElement element, string name)
    {
      var attribute = element.Attribute(name);
      if (attribute == null)
      {
        throw new PropertyNotFoundException(name, element.GetAbsoluteXPath());
      }

      return attribute.Value;
    }

    public static void StoreValue<T>(this XElement element, Expression<Func<T>> parameter)
    {
      var body = ((MemberExpression) parameter.Body);
      var expression = body.Expression as ConstantExpression;

      if (body.Member is FieldInfo)
      {
        var fieldInfo = body.Member as FieldInfo;
        var value = fieldInfo.GetValue(expression.Value);
        if (value == null && typeof(T) == typeof(string))
        {
          value = string.Empty;
        }

        StoreValue(element, body.Member.Name, value);
      }
      else
      {
        var propertyInfo = body.Member as PropertyInfo;
        var value = propertyInfo.GetValue(expression.Value, null);
        if (value == null && typeof(T) == typeof(string))
        {
          value = string.Empty;
        }

        StoreValue(element, body.Member.Name, value);
      }
    }

    public static void StoreValue(this XElement element, string name, object value)
    {
      object adaptedValue;

      if (value is double doubleValue)
      {
        var stringDoubleValue = doubleValue.ToString("r", CultureInfo.InvariantCulture);
        if (stringDoubleValue.Length > 10)
        {
          stringDoubleValue = stringDoubleValue.Substring(0, 10);
        }
        adaptedValue = stringDoubleValue;
      }
      else if (value is float floatValue)
      {
        var stringFloatValue = floatValue.ToString("r", CultureInfo.InvariantCulture);
        if (stringFloatValue.Length > 7)
        {
          stringFloatValue = stringFloatValue.Substring(0, 7);
        }
        adaptedValue = stringFloatValue;
      }
      else if (value is TimeSpan span)
      {
        adaptedValue = (int)span.TotalMilliseconds;
      }
      else
      {
        adaptedValue = value;
      }

      var attribute = element.Attribute(name);

      if (attribute == null)
      {
        element.Add(new XAttribute(name, adaptedValue));
      }
      else
      {
        attribute.Value = adaptedValue.ToString();
      }
    }


    /// <summary>
    /// Get the absolute XPath to a given XElement
    /// (e.g. "/people/person[6]/name[1]/last[1]").
    /// </summary>
    public static string GetAbsoluteXPath(this XElement element)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      Func<XElement, string> relativeXPath = e =>
      {
        var index = e.IndexPosition();
        var name = e.Name.LocalName;

        // If the element is the root, no index is required
        return (index == -1) ? "/" + name : $"/{name}[{index.ToString()}]";
      };

      var ancestors = element.Ancestors().Select(e => relativeXPath(e));

      return string.Concat(ancestors.Reverse().ToArray()) + relativeXPath(element);
    }

    /// <summary>
    /// Get the index of the given XElement relative to its
    /// siblings with identical names. If the given element is
    /// the root, -1 is returned.
    /// </summary>
    /// <param name="element">
    /// The element to get the index of.
    /// </param>
    public static int IndexPosition(this XElement element)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      if (element.Parent == null)
      {
        return -1;
      }

      var i = 1; // Indexes for nodes start at 1, not 0

      foreach (var sibling in element.Parent.Elements(element.Name))
      {
        if (sibling == element)
        {
          return i;
        }

        i++;
      }

      throw new InvalidOperationException("Element has been removed from its parent.");
    }

    public static XElement Normalize(this XElement element)
    {
      var attributes = element.Attributes().OrderBy(a => a.Name.ToString()).ToArray();
      var children = element.Elements().Select(e => e.Normalize()).OrderBy(e => e.Name.ToString()).ToArray();
      element.RemoveAll();

      element.Add(attributes);
      element.Add(children);

      return element;
    }
  }

  public class PropertyNotFoundException : Exception
  {
    public PropertyNotFoundException(string propertyName, string parentName) : base("The value " + propertyName + " cannot be found in element '" + parentName + "'.")
    {
    }
  }


}