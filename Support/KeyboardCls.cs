using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MSFS2020Ctrls.Support
{
  /// <summary>
  /// Handles one Keyboard device as DXInput device
  /// In addition provide some static tools to handle KBD props here in one place
  /// </summary>
  public class KeyboardCls 
  {
    // http://kbdlayout.info/


    public static string MyLayoutName( )
    {
      // Gets the current input language.
      InputLanguage myCurrentLanguage = InputLanguage.CurrentInputLanguage;

      if ( myCurrentLanguage != null )
        return myCurrentLanguage.LayoutName;
      else
        return "";
    }

    #region Static Items

    public const string DeviceClass = "keyboard";  // the device name used throughout this app
    public const string DeviceID = "kb1_";
    static public int RegisteredDevices = 0;  // devices add here once they are created (though will not decrement as they are not deleted)
    public const string DevNameCIG = "Keyboard"; // just a name...
    public const string DevGUIDCIG = "{00000000-0000-0000-0000-000000000000}"; // - Fixed for Keyboards, we dont differentiate

    /// <summary>
    /// Format the various parts to a valid ctrl entry
    /// </summary>
    /// <param name="input">The input by the user</param>
    /// <param name="modifiers">Modifiers to be applied</param>
    /// <returns></returns>
    static public string MakeCtrl( string input, string modifiers )
    {
      return DeviceID + modifiers + input;
    }


    #endregion




  }
}
