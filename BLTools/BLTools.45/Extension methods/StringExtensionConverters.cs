using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace BLTools {

  /// <summary>
  /// Extensions for string
  /// </summary>
  public static partial class StringExtension {

    #region Converters
    /// <summary>
    /// Converts a string to a bool
    /// </summary>
    /// <param name="booleanString">A string representing a bool (0,1; false,true; no,yes; n,y)</param>
    /// <returns>A bool as represented by the string (default=false)</returns>
    public static bool ToBool(this string booleanString) {
      #region Validate parameters
      if ( booleanString == null ) {
        return false;
      }
      #endregion Validate parameters
      switch ( booleanString.Trim().ToLower() ) {
        case "0":
        case "false":
        case "no":
        case "n":
          return false;
        case "1":
        case "true":
        case "yes":
        case "y":
          return true;
        default:
          return false;
      }

    }
    /// <summary>
    /// Converts a string to a bool
    /// </summary>
    /// <param name="booleanString">The string to convert</param>
    /// <param name="trueValue">The string value representing true</param>
    /// <param name="falseValue">The string value representing false</param>
    /// <param name="isCaseSensitive">Do we test the values with case sensitivity (default=false)</param>
    /// <returns>A bool as represented by the string (default=false)</returns>
    public static bool ToBool(this string booleanString, string trueValue = "true", string falseValue = "false", bool isCaseSensitive = false) {
      #region Validate parameters
      if ( string.IsNullOrWhiteSpace(booleanString) ) {
        return false;
      }
      #endregion Validate parameters

      string ValueToCompare;
      string TrueValue;
      string FalseValue;

      if ( !isCaseSensitive ) {
        ValueToCompare = booleanString.ToLowerInvariant();
        TrueValue = trueValue.ToLowerInvariant();
        FalseValue = falseValue.ToLowerInvariant();
      } else {
        ValueToCompare = booleanString;
        TrueValue = trueValue;
        FalseValue = falseValue;
      }

      if ( ValueToCompare == TrueValue ) {
        return true;
      }

      if ( ValueToCompare == FalseValue ) {
        return false;
      }

      Trace.WriteLine($"Error: value to convert doesn't match any possible value : \"{TrueValue}\", \"{FalseValue}\", \"{ValueToCompare}\"");
      return false;

    }

    /// <summary>
    /// Convert a string to an array of bytes
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <returns>The array of bytes</returns>
    public static byte[] ToByteArray(this string sourceString) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      #endregion Validate parameters
      return sourceString.Select<char, byte>(c => (byte)c).ToArray();
    }

    /// <summary>
    /// Convert a SecureString to a normal string
    /// </summary>
    /// <param name="securePassword">The source SecureString</param>
    /// <returns>The string</returns>
    public static string ConvertToUnsecureString(this SecureString securePassword) {
      if ( securePassword == null ) {
        return null;
      }

      IntPtr UnmanagedString = IntPtr.Zero;
      try {
        UnmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
        return Marshal.PtrToStringUni(UnmanagedString);
      } finally {
        Marshal.ZeroFreeGlobalAllocUnicode(UnmanagedString);
      }
    }

    /// <summary>
    /// Converts a string to a SecureString
    /// </summary>
    /// <param name="unsecureString">The source string</param>
    /// <returns>The SecureString</returns>
    public static SecureString ConvertToSecureString(this string unsecureString) {
      if ( unsecureString == null ) {
        return null;
      }
      SecureString RetVal = new SecureString();
      foreach ( char CharItem in unsecureString ) {
        RetVal.AppendChar(CharItem);
      }
      RetVal.MakeReadOnly();
      return RetVal;
    }
    #endregion Converters

  }

}
