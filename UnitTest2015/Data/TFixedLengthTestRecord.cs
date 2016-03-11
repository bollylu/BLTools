using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.Data;
using BLTools.Data.FixedLength;

namespace UnitTest2015 {
  public class TFixedLengthTestRecord : TFixedLengthRecord {

    #region Data fields
    [TDataField(StartPos = 0, Length = 3)]
    public string SupplierCode { get; set; }

    [TDataField(StartPos = 3, Length = 15)]
    public string Name { get; set; }

    [TDataField(StartPos = 18, Length = 50)]
    public string Customer { get; set; }

    [TDataField(StartPos = 68, Length = 6)]
    public long OrderNumber { get; set; }

    [TDataField(StartPos = 74, Length = 4)]
    public int Quantity { get; set; }

    [TDataField(StartPos = 78, Length = 4, IsSigned = true)]
    public int NegativeQuantity { get; set; }

    [TDataField(StartPos = 82, Length = 7, DecimalDigits = 2, DecimalIsVirtual = false, DecimalSeparator = '.')]
    public float FloatCost { get; set; }

    [TDataField(StartPos = 89, Length = 7, DecimalDigits = 2, DecimalIsVirtual = true)]
    public float FloatPrice { get; set; }

    [TDataField(StartPos = 96, Length = 7, DecimalDigits = 2, DecimalIsVirtual = false, DecimalSeparator = '.')]
    public double DoubleCost { get; set; }

    [TDataField(StartPos = 103, Length = 7, DecimalDigits = 2, DecimalIsVirtual = true)]
    public double DoublePrice { get; set; }

    [TDataField(StartPos = 110, Length = 7, DecimalDigits = 2, DecimalIsVirtual = false, DecimalSeparator = '.')]
    public decimal DecimalCost { get; set; }

    [TDataField(StartPos = 117, Length = 7, DecimalDigits = 2, DecimalIsVirtual = true)]
    public decimal DecimalPrice { get; set; }

    [TDataField(StartPos = 124, Length = 1, BoolFormat = EBoolFormat.YOrN)]
    public bool IsGoodRecord { get; set; }

    [TDataField(StartPos = 125, Length = 1, BoolFormat = EBoolFormat.TOrF)]
    public bool IsAuthentic { get; set; }

    [TDataField(StartPos = 126, Length = 4, BoolFormat = EBoolFormat.Custom, TrueValue = "Good", FalseValue = "Bad ")]
    public bool GoodOrBad { get; set; }

    [TDataField(StartPos = 130, Length = 8, DateTimeFormat = EDateTimeFormat.DateOnly)]
    public DateTime OrderDate { get; set; }

    [TDataField(StartPos = 138, Length = 8, DateTimeFormat = EDateTimeFormat.Custom, DateTimeFormatCustom = "ddMMyyyy")]
    public DateTime DeliveryDate { get; set; }

    [TDataField(StartPos = 146, Length = 2)]
    public string CRLF { get; private set; }
    #endregion Data fields

    public TFixedLengthTestRecord() {
      CRLF = "\r\n";
    }
  }
}
