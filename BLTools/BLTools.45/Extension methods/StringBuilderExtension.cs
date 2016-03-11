using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  public static class StringBuilderExtension {
    /// <summary>
    /// Removes n characters from the end of the StringBuilder
    /// </summary>
    /// <param name="source">The string builder</param>
    /// <param name="length">The amount of character(s) to remove</param>
    /// <returns></returns>
    public static StringBuilder Truncate(this StringBuilder source, int length) {
      if (source == null) {
        return null;
      }
      if (length <= 0) {
        return source;
      }
      if (length >= source.Length) {
        return source.Clear();
      }

      return source.Remove(source.Length - length, length);
    }

    /// <summary>
    /// Remove trailing spaces or tabs from StringBuilder
    /// </summary>
    /// <param name="source">The string builder</param>
    /// <returns></returns>
    public static StringBuilder Trim(this StringBuilder source) {
      return source.Trim(' ', '\t');
    }

    /// <summary>
    /// Remove leading spaces or tabs from StringBuilder
    /// </summary>
    /// <param name="source">The string builder</param>
    /// <returns></returns>
    public static StringBuilder TrimLeft(this StringBuilder source) {
      return source.TrimLeft(' ', '\t');
    }

    /// <summary>
    /// Remove leading spaces or tabs from StringBuilder
    /// </summary>
    /// <param name="source">The string builder</param>
    /// <returns></returns>
    public static StringBuilder TrimAll(this StringBuilder source) {
      return source.TrimAll(' ', '\t');
    }

    /// <summary>
    /// Remove trailing characters from StringBuilder
    /// </summary>
    /// <param name="source">The string builder</param>
    /// <param name="chars">The characters to remove</param>
    /// <returns></returns>
    public static StringBuilder Trim(this StringBuilder source, params char[] chars) {
      if (source == null) {
        return null;
      }
      if (source.Length == 0) {
        return source;
      }
      if (chars == null || chars.Length == 0) {
        return source;
      }

      while (chars.Contains(source[source.Length - 1])) {
        source = source.Remove(source.Length - 1, 1);
      }

      return source;
    }

    /// <summary>
    /// Remove leading characters from StringBuilder
    /// </summary>
    /// <param name="source">The string builder</param>
    /// <param name="chars">The characters to remove</param>
    /// <returns></returns>
    public static StringBuilder TrimLeft(this StringBuilder source, params char[] chars) {
      if (source == null) {
        return null;
      }
      if (source.Length == 0) {
        return source;
      }
      if (chars == null || chars.Length == 0) {
        return source;
      }

      while (chars.Contains(source[0])) {
        source = source.Remove(0, 1);
      }

      return source;
    }

    /// <summary>
    /// Remove leading and trailing characters from StringBuilder
    /// </summary>
    /// <param name="source">The string builder</param>
    /// <param name="chars">The characters to remove</param>
    /// <returns></returns>
    public static StringBuilder TrimAll(this StringBuilder source, params char[] chars) {
      if (source == null) {
        return null;
      }
      if (source.Length == 0) {
        return source;
      }
      if (chars == null || chars.Length == 0) {
        return source;
      }

      while (chars.Contains(source[source.Length - 1])) {
        source = source.Remove(source.Length - 1, 1);
      }

      while (chars.Contains(source[0])) {
        source = source.Remove(0, 1);
      }

      return source;
    }
  }
}
