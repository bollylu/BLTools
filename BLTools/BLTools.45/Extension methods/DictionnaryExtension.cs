using System.Collections;
using System.Globalization;

namespace BLTools {
  public static class DictionaryExtension {

    /// <summary>
    /// Get a value (converted to the right type) from a dictionary with a default value in case of missing key
    /// </summary>
    /// <typeparam name="K">Type type of the key</typeparam>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="source">The source dictionary</param>
    /// <param name="key">The key to search for</param>
    /// <param name="defaultValue">The typed value to return in case of missing key</param>
    /// <returns></returns>
    public static T SafeGetValue<K, T>(this IDictionary source, K key, T defaultValue) {

      if ( key == null ) {
        return defaultValue;
      }

      if ( source.Count == 0 ) {
        return defaultValue;
      }

      if ( source.Contains(key) ) {
        return BLConverter.BLConvert<T>(source[key], CultureInfo.CurrentCulture, defaultValue);
      }

      return defaultValue;
    }
  }
}
