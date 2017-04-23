using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  public static class ToMemoryStreamExtension {

    #region --- Gets MemoryStream from string --------------------------------------------
    /// <summary>
    /// Gets a MemoryStream from a string (encoding UTF8)
    /// </summary>
    /// <param name="source">The source string</param>
    /// <returns>The requested MemoryStream</returns>
    public static MemoryStream ToStream(this string source) {
      #region === Validate parameters ===
      if ( string.IsNullOrEmpty(source) ) {
        return new MemoryStream();
      }
      #endregion === Validate parameters ===
      return source.ToStream(Encoding.UTF8);
    }

    /// <summary>
    /// Gets a MemoryStream from a string
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="encoding">The encoding for the stream creation</param>
    /// <returns>The requested MemoryStream</returns>
    public static MemoryStream ToStream(this string source, Encoding encoding) {
      #region === Validate parameters ===
      if ( string.IsNullOrEmpty(source) ) {
        return new MemoryStream();
      }
      #endregion === Validate parameters ===
      
      return encoding.GetBytes(source).ToStream();
    }

    /// <summary>
    /// Gets a MemoryStream from a string asynchronously (encoding UTF8)
    /// </summary>
    /// <param name="source">The source string</param>
    /// <returns>The requested MemoryStream</returns>
    public static Task<MemoryStream> ToStreamAsync(this string source) {
      #region === Validate parameters ===
      if ( string.IsNullOrEmpty(source) ) {
        return Task.FromResult<MemoryStream>(new MemoryStream());
      }
      #endregion === Validate parameters ===
      return source.ToStreamAsync(Encoding.UTF8);
    }

    /// <summary>
    /// Gets a MemoryStream from a string asynchronously
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="encoding">The encoding for the stream creation</param>
    /// <returns>The requested MemoryStream</returns>
    public static async Task<MemoryStream> ToStreamAsync(this string source, Encoding encoding) {
      #region === Validate parameters ===
      if ( string.IsNullOrEmpty(source) ) {
        return await Task.FromResult<MemoryStream>(new MemoryStream());
      }
      #endregion === Validate parameters ===
      
      return await encoding.GetBytes(source).ToStreamAsync();
    }
    #endregion --- Gets MemoryStream from string --------------------------------------------

    #region --- Gets MemoryStream from IEnumerable<byte> --------------------------------------------

    /// <summary>
    /// Gets a MemoryStream from a byte[]
    /// </summary>
    /// <param name="sourceBytes">The array of bytes</param>
    /// <returns>The requested MemoryStream</returns>
    public static MemoryStream ToStream(this IEnumerable<byte> sourceBytes) {
      #region === Validate parameters ===
      if ( sourceBytes == null || sourceBytes.Count() == 0 ) {
        return new MemoryStream();
      }
      #endregion === Validate parameters ===
      MemoryStream RetVal;
      if ( sourceBytes is byte[] ) {
        RetVal = new MemoryStream((byte[])sourceBytes);
      } else {
        RetVal = new MemoryStream(sourceBytes.ToArray());
      }
      RetVal.Seek(0, SeekOrigin.Begin);
      return RetVal;
    }

    /// <summary>
    /// Gets a MemoryStream from a byte[] asynchronously
    /// </summary>
    /// <param name="sourceBytes">The array of byte</param>
    /// <returns>The requested MemoryStream</returns>
    public static async Task<MemoryStream> ToStreamAsync(this IEnumerable<byte> sourceBytes) {
      #region === Validate parameters ===
      if ( sourceBytes == null || sourceBytes.Count() == 0 ) {
        return await Task.FromResult<MemoryStream>(new MemoryStream());
      }
      #endregion === Validate parameters ===
      MemoryStream RetVal = new MemoryStream();
      if ( sourceBytes is byte[] ) {
        await RetVal.WriteAsync((byte[])sourceBytes, 0, ( (byte[])sourceBytes ).Count());
      } else {
        await RetVal.WriteAsync(sourceBytes.ToArray(), 0, sourceBytes.Count());
      }
      await RetVal.FlushAsync();
      RetVal.Seek(0, SeekOrigin.Begin);
      return RetVal;
    }
    #endregion --- Gets MemoryStream from IEnumerable<byte> --------------------------------------------

  }
}
