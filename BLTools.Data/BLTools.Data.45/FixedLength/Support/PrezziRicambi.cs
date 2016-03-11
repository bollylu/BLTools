using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StockRicDatafiles {
  public class PrezziRicambi : DataFileFixedLength {

    public PrezziRicambi(string physicalName) : base(physicalName, 200, typeof(PrezziRicambiRecord)) {
    }

    public new PrezziRicambiRecord Select() {
      string TempData = this._read();
      if ( TempData != "" ) {
        return new PrezziRicambiRecord(TempData);
      } else {
        return null;
      }
    }
    public PrezziRicambiRecord Select( int recordNumber ) {
      string TempData = this._read(recordNumber);
      if ( TempData != "" ) {
        return new PrezziRicambiRecord(TempData);
      } else {
        return null;
      }
    }

    
  }

  public class PrezziRicambiRecord : DataRecord {

    #region Public properties
    [DataField(2, 10)]
    public long PartNo { get; private set; }

    [DataField(StartPos = 12, Length = 20)]
    public string DescNl { get; private set; }

    [DataField(StartPos = 32, Length = 20)]
    public string DescFr { get; private set; }

    [DataField(StartPos = 58, Length = 1)]
    public string Caliva { get; private set; }

    [DataField(StartPos = 59, Length = 1)]
    public string CodSconto { get; private set; }

    [DataField(StartPos = 61, Length = 1)]
    public string MacroFamiglia { get; private set; }

    [DataField(StartPos = 62, Length = 4)]
    public string Famiglia { get; private set; }

    [DataField(StartPos = 72, Length = 3)]
    public string TypoModello { get; private set; }

    [DataField(StartPos = 80, Length = 10)]
    public string Preli { get; private set; }

    [DataField(StartPos = 90, Length = 10)]
    public string PreliVa { get; private set; }

    [DataField(StartPos = 100, Length = 10)]
    public string SuperseedingPart { get; private set; }

    [DataField(StartPos = 110, Length = 10)]
    public string SuperseedingPartPrice { get; private set; }
    #endregion Public properties

    #region Constructor(s)
    public PrezziRicambiRecord( string rawData )
      : base(rawData) {
    }
    #endregion Constructor(s)

    #region Public methods
    
    #endregion Public methods

  }
}
