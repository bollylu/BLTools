using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data.FixedLength {
  /// <summary>
  /// Defines a fixed length record
  /// </summary>
  public interface IFixedLengthRecord {
    /// <summary>
    /// The length of the record
    /// </summary>
    int RecLen { get; }
    /// <summary>
    /// Whether or not the dates without values are left blank
    /// </summary>
    bool EmptyDatesValueBlank { get; set; }
    /// <summary>
    /// Convert the data properties to a raw data record
    /// </summary>
    /// <param name="encoding"></param>
    /// <returns></returns>
    byte[] ToRawData(Encoding encoding);
    /// <summary>
    /// Convert the raw data record to properties
    /// </summary>
    /// <param name="rawData"></param>
    void FromRawData(byte[] rawData);
  }
}
