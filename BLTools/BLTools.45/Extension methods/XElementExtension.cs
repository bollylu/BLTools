using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
using System.Globalization;

namespace BLTools {

  /// <summary>
  /// Extensions for XElement
  /// </summary>
  public static class XElementExtension {

    /// <summary>
    /// Provides additional debug information when true
    /// </summary>
    public static bool IsDebug = false;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Attributes
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Obtains the value of an typed attribute value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the attribute</param>
    /// <returns>The attribute value in the requested type or the specified default value</returns>
    public static T SafeReadAttribute<T>(this XElement xElement, string name) {
      return SafeReadAttribute<T>(xElement, name, default(T), CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Obtains the value of an typed attribute value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the attribute</param>
    /// <returns>The attribute value in the requested type or the specified default value</returns>
    public static T SafeReadAttribute<T>(this XElement xElement, XName name) {
      return SafeReadAttribute<T>(xElement, name, default(T), CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Obtains the value of an typed attribute value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the attribute</param>
    /// <param name="defaultValue">The default value (same type as the returned value)</param>
    /// <returns>The attribute value in the requested type or the specified default value</returns>
    public static T SafeReadAttribute<T>(this XElement xElement, string name, T defaultValue) {
      return SafeReadAttribute<T>(xElement, name, defaultValue, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Obtains the value of an typed attribute value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the attribute</param>
    /// <param name="defaultValue">The default value (same type as the returned value)</param>
    /// <returns>The attribute value in the requested type or the specified default value</returns>
    public static T SafeReadAttribute<T>(this XElement xElement, XName name, T defaultValue) {
      return SafeReadAttribute<T>(xElement, name, defaultValue, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Obtains the value of an typed attribute value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the attribute</param>
    /// <param name="defaultValue">The default value (same type as the returned value)</param>
    /// <param name="culture">The culture info used to convert the return value</param>
    /// <returns>The attribute value in the requested type or the specified default value</returns>
    public static T SafeReadAttribute<T>(this XElement xElement, string name, T defaultValue, CultureInfo culture) {
      #region Validate parameters
      if (name == null) {
        Trace.WriteLineIf(IsDebug, string.Format("Name is null : Unable to read attribute from\r\n{0}", xElement.ToString()));
        return defaultValue;
      }
      if (!xElement.HasAttributes) {
        Trace.WriteLineIf(IsDebug, string.Format("No attribute available : Unable to read attribute {0} from\r\n{1}", name, xElement.ToString()));
        return defaultValue;
      }
      if (xElement.Attribute(name) == null) {
        Trace.WriteLineIf(IsDebug, string.Format("Missing attribute {0} : Unable to read attribute from\r\n{1}", name, xElement.ToString()));
        return defaultValue;
      } 
      #endregion Validate parameters

      return BLConverter.BLConvert<T>(xElement.Attribute(name).Value, culture, defaultValue);

    }

    /// <summary>
    /// Obtains the value of an typed attribute value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the attribute</param>
    /// <param name="defaultValue">The default value (same type as the returned value)</param>
    /// <param name="culture">The culture info used to convert the return value</param>
    /// <returns>The attribute value in the requested type or the specified default value</returns>
    public static T SafeReadAttribute<T>(this XElement xElement, XName name, T defaultValue, CultureInfo culture) {
      #region Validate parameters
      if (name == null) {
        Trace.WriteLineIf(IsDebug, string.Format("Name is null : Unable to read attribute from\r\n{0}", xElement.ToString()));
        return defaultValue;
      }
      if (!xElement.HasAttributes) {
        Trace.WriteLineIf(IsDebug, string.Format("No attribute available : Unable to read attribute {0} from\r\n{1}", name, xElement.ToString()));
        return defaultValue;
      }
      if (xElement.Attribute(name) == null) {
        Trace.WriteLineIf(IsDebug, string.Format("Missing attribute {0} : Unable to read attribute from\r\n{1}", name, xElement.ToString()));
        return defaultValue;
      }
      #endregion Validate parameters

      return BLConverter.BLConvert<T>(xElement.Attribute(name).Value, culture, defaultValue);

    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Element
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Read an XElement from an XElement while handling error cases
    /// </summary>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the inner XElement to read</param>
    /// <returns>The requested inner XElement or an empty XElement named after the name parameter</returns>
    public static XElement SafeReadElement(this XElement xElement, string name) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(name)) {
        Trace.WriteLineIf(IsDebug, string.Format("Name is null or invalid : Unable to read element from\r\n{0}", xElement.ToString()));
        return new XElement(name);
      }
      if (!xElement.HasElements) {
        Trace.WriteLineIf(IsDebug, string.Format("No element available : Unable to read element {0} from\r\n{1}", name, xElement.ToString()));
        return new XElement(name);
      }
      #endregion Validate parameters

      XElement SafeElement;
      try {
        SafeElement = xElement.Element(name);
      } catch (Exception ex) {
        Trace.WriteLineIf(IsDebug, string.Format("Missing element {0} : {1}\r\n{2}", name, ex.Message, xElement.ToString()));
        return new XElement(name);
      }
      if (SafeElement == null) {
        return new XElement(name);
      }
      return SafeElement;
    }

    /// <summary>
    /// Read an XElement from an XElement while handling error cases
    /// </summary>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the inner XElement to read</param>
    /// <returns>The requested inner XElement or an empty XElement named after the name parameter</returns>
    public static XElement SafeReadElement(this XElement xElement, XName name) {
      #region Validate parameters
      if (name==null) {
        Trace.WriteLineIf(IsDebug, string.Format("Name is null or invalid : Unable to read element from\r\n{0}", xElement.ToString()));
        return new XElement(name);
      }
      if (!xElement.HasElements) {
        Trace.WriteLineIf(IsDebug, string.Format("No element available : Unable to read element {0} from\r\n{1}", name, xElement.ToString()));
        return new XElement(name);
      }
      #endregion Validate parameters

      XElement SafeElement;
      try {
        SafeElement = xElement.Element(name);
      } catch (Exception ex) {
        Trace.WriteLineIf(IsDebug, string.Format("Missing element {0} : {1}\r\n{2}", name.ToString(), ex.Message, xElement.ToString()));
        return new XElement(name);
      }
      if (SafeElement == null) {
        return new XElement(name);
      }
      return SafeElement;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Element as value
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Obtains the value of an inner XElement value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the inner XElemnt</param>
    /// <returns>The inner XElement value in the requested type or the specified default value</returns>
    public static T SafeReadElementValue<T>(this XElement xElement, string name) {
      return SafeReadElementValue<T>(xElement, name, default(T), CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Obtains the value of an inner XElement value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the inner XElemnt</param>
    /// <returns>The inner XElement value in the requested type or the specified default value</returns>
    public static T SafeReadElementValue<T>(this XElement xElement, XName name) {
      return SafeReadElementValue<T>(xElement, name, default(T), CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Obtains the value of an inner XElement value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the inner XElemnt</param>
    /// <param name="defaultValue">The default value (same type as the returned value)</param>
    /// <returns>The inner XElement value in the requested type or the specified default value</returns>
    public static T SafeReadElementValue<T>(this XElement xElement, string name, T defaultValue) {
      return SafeReadElementValue<T>(xElement, name, defaultValue, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Obtains the value of an inner XElement value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the inner XElemnt</param>
    /// <param name="defaultValue">The default value (same type as the returned value)</param>
    /// <returns>The inner XElement value in the requested type or the specified default value</returns>
    public static T SafeReadElementValue<T>(this XElement xElement, XName name, T defaultValue) {
      return SafeReadElementValue<T>(xElement, name, defaultValue, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Obtains the value of an inner XElement value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the inner XElemnt</param>
    /// <param name="defaultValue">The default value (same type as the returned value)</param>
    /// <param name="culture">The culture info used to convert the return value</param>
    /// <returns>The inner XElement value in the requested type or the specified default value</returns>
    public static T SafeReadElementValue<T>(this XElement xElement, string name, T defaultValue, CultureInfo culture) {
      #region Validate parameters
      if (name == null) {
        Trace.WriteLineIf(IsDebug, string.Format("Name is null : Unable to read element from\r\n{0}", xElement.ToString()));
        return defaultValue;
      }
      if (!xElement.HasElements) {
        Trace.WriteLineIf(IsDebug, string.Format("No element available : Unable to read element {0} from\r\n{1}", name, xElement.ToString()));
        return defaultValue;
      }
      if (xElement.Element(name) == null) {
        Trace.WriteLineIf(IsDebug, string.Format("Missing element {0} : Unable to read element from\r\n{1}", name, xElement.ToString()));
        return defaultValue;
      } 
      #endregion Validate parameters

      return BLConverter.BLConvert<T>(xElement.Element(name).Value, culture, defaultValue);
      
    }

    /// <summary>
    /// Obtains the value of an inner XElement value from an XElement with a default value in case of error.
    /// </summary>
    /// <typeparam name="T">Type of the returned value</typeparam>
    /// <param name="xElement">The source XElement</param>
    /// <param name="name">The name of the inner XElemnt</param>
    /// <param name="defaultValue">The default value (same type as the returned value)</param>
    /// <param name="culture">The culture info used to convert the return value</param>
    /// <returns>The inner XElement value in the requested type or the specified default value</returns>
    public static T SafeReadElementValue<T>(this XElement xElement, XName name, T defaultValue, CultureInfo culture) {
      #region Validate parameters
      if (name == null) {
        Trace.WriteLineIf(IsDebug, string.Format("Name is null : Unable to read element from\r\n{0}", xElement.ToString()));
        return defaultValue;
      }
      if (!xElement.HasElements) {
        Trace.WriteLineIf(IsDebug, string.Format("No element available : Unable to read element {0} from\r\n{1}", name, xElement.ToString()));
        return defaultValue;
      }
      if (xElement.Element(name) == null) {
        Trace.WriteLineIf(IsDebug, string.Format("Missing element {0} : Unable to read element from\r\n{1}", name, xElement.ToString()));
        return defaultValue;
      }
      #endregion Validate parameters

      return BLConverter.BLConvert<T>(xElement.Element(name).Value, culture, defaultValue);

    }
  }
}
