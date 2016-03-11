using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BLTools.Web.ASP {
  public static class ExtensionMethods {

    public static void HideControl(this WebControl control) {
      control.Style["Display"] = "none";
    }

    public static void ShowControl(this WebControl control) {
      control.Style["Display"] = "normal";
    }

    public static void SetControlVisible(this WebControl control, bool isVisible) {
      if (isVisible) {
        control.ShowControl();
      } else {
        control.HideControl();
      }
    }
  }
}
