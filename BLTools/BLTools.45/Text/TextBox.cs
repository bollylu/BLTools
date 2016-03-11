using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace BLTools.Text {
  public static class TextBox {
    public enum StringAlignmentEnum {
      Left,
      Center,
      Right
    }

    public static string BuildDynamicIBM(string sourceString, int margin = 0, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = '·') {
      return BuildDynamic(sourceString, margin, alignment, filler, "╒═╕│╛═╘│");
    }
    public static string BuildDynamic(string sourceString, int margin = 0, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = ' ', string border = "") {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (margin < 0) {
        margin = 0;
      }
      #endregion Validate parameters

      string CompletedBorder = string.Format("{0}+-+|+-+|", border).Left(8);

      char TopLeft = CompletedBorder[0];
      char TopBar = CompletedBorder[1];
      char TopRight = CompletedBorder[2];
      char LeftBar = CompletedBorder[7];
      char RightBar = CompletedBorder[3];
      char BottomLeft = CompletedBorder[6];
      char BottomBar = CompletedBorder[5];
      char BottomRight = CompletedBorder[4];

      StringBuilder RetVal = new StringBuilder();

      // Find larger string
      int MaxLength = 0;
      foreach (string StringItem in sourceString.Replace("\r\n", "\n").Split('\n')) {
        if (StringItem.Length > MaxLength) {
          MaxLength = StringItem.Length;
        }
      }

      RetVal.AppendLine(string.Format("{0}{1}{2}", TopLeft, new string(TopBar, MaxLength + (margin * 2) + 2), TopRight));

      foreach (string StringItem in sourceString.Replace("\r\n", "\n").Split('\n')) {
        int LeftPadding = 0;
        int RightPadding = 0;

        switch (alignment) {
          case StringAlignmentEnum.Left:
            RightPadding = MaxLength - StringItem.Length;
            break;
          case StringAlignmentEnum.Right:
            LeftPadding = MaxLength - StringItem.Length;
            break;
          case StringAlignmentEnum.Center:
            LeftPadding = Convert.ToInt32(Math.Floor((MaxLength - StringItem.Length) / 2d));
            RightPadding = MaxLength - StringItem.Length - LeftPadding;
            break;
        }
        RetVal.AppendLine(string.Format("{0}{1}{2} {3} {4}{5}{6}", LeftBar,
                                                                   new string(filler, margin),
                                                                   new string(filler, LeftPadding),
                                                                   StringItem,
                                                                   new string(filler, RightPadding),
                                                                   new string(filler, margin),
                                                                   RightBar));
      }
      RetVal.Append(string.Format("{0}{1}{2}", BottomLeft, new string(BottomBar, MaxLength + (margin * 2) + 2), BottomRight));

      return RetVal.ToString();
    }

    public static string BuildFixedWidth(string sourceString, int width = 80, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = ' ', string border = "") {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (width < 0) {
        width = 0;
      }
      #endregion Validate parameters

      string CompletedBorder = string.Format("{0}+-+|+-+|", border).Left(8);

      char TopLeft = CompletedBorder[0];
      char TopBar = CompletedBorder[1];
      char TopRight = CompletedBorder[2];
      char LeftBar = CompletedBorder[7];
      char RightBar = CompletedBorder[3];
      char BottomLeft = CompletedBorder[6];
      char BottomBar = CompletedBorder[5];
      char BottomRight = CompletedBorder[4];

      StringBuilder RetVal = new StringBuilder();

      RetVal.AppendLine(string.Format("{0}{1}{2}", TopLeft, new string(TopBar, width + 2), TopRight));

      string[] Lines = sourceString.Replace("\r\n", "\n").Split('\n');
      foreach (string StringItem in Lines) {
        int StartPtr = 0;
        while (StartPtr < StringItem.Length) {
          string WorkString = StringItem.Substring(StartPtr, Math.Min(StringItem.Length - StartPtr, width));
          StartPtr += WorkString.Length;

          int LeftPadding = 0;
          int RightPadding = 0;

          switch (alignment) {
            case StringAlignmentEnum.Left:
              RightPadding = Math.Max(width - WorkString.Length, 0);
              break;
            case StringAlignmentEnum.Right:
              LeftPadding = Math.Max(width - WorkString.Length, 0);
              break;
            case StringAlignmentEnum.Center:
              LeftPadding = Math.Max(Convert.ToInt32(Math.Floor((width - WorkString.Length) / 2d)), 0);
              RightPadding = Math.Max(width - WorkString.Length - LeftPadding, 0);
              break;
          }

          RetVal.AppendLine(string.Format("{0}{1} {2} {3}{4}", LeftBar, new string(filler, LeftPadding), WorkString, new string(filler, RightPadding), RightBar));
        }
      }
      RetVal.Append(string.Format("{0}{1}{2}", BottomLeft, new string(BottomBar, width + 2), BottomRight));
      return RetVal.ToString();
    }
    public static string BuildFixedWidthIBM(string sourceString, int width = 0, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = '·') {
      return BuildFixedWidth(sourceString, width, alignment, filler, "╒═╕│╛═╘│");
    }
  }
}
