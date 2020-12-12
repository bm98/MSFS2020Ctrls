using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS2020Ctrls.Layout
{
  /// <summary>
  /// One Action Item for the Layout process
  /// </summary>
  class ActionItem
  {
    /// <summary>
    /// The Text Shown in the Map
    /// </summary>
    public string KeyText { get; set; }
    /// <summary>
    /// The Text Shown in the Map
    /// </summary>
    public string DispText { get; set; }
    /// <summary>
    /// The action map this item belongs to
    /// </summary>
    public string ActionMap { get; set; } = ""; // TODO may be set a color for this one later

    // Input Device Refs

    /// <summary>
    /// The Device Name
    /// </summary>
    public string DeviceName { get; set; } = "";
    /// <summary>
    /// Device Product GUID in {B351044F-0000-0000-0000-504944564944} notation
    /// </summary>
    public string DeviceProdGuid { get; set; } = "";
    /// <summary>
    /// K1, M1, Jn, G1 (keyb, mouse, joystick jsN number 1.., gamepad)
    /// </summary>
    public string InputType { get; set; } = "";
    /// <summary>
    /// Command Input Ref - match required to find the display location 
    /// buttonN, hatN_up,_right,_down,_left, [rot]xyz, sliderN (CryInput notification)
    /// </summary>
    public string ControlInput { get; set; } = "";

    /// <summary>
    /// Returnd the PID VID part of the device GUID in lowercase
    ///  or the plain ProductGuid
    /// </summary>
    public string DevicePidVid
    {
      get {
        if ( DeviceProdGuid.Length > 9 ) {
          return DeviceProdGuid.Substring( 1, 8 ).ToLowerInvariant( );
        }
        return DeviceProdGuid;
      }
    }
    /// <summary>
    /// The Type Letter of the Input Type
    ///  J1 returns J
    /// </summary>
    public string InputTypeLetter { get => InputType.Substring( 0, 1 ); }
    /// <summary>
    /// The Type number of the Input Type
    /// J1 returns 1
    /// </summary>
    public short InputTypeNumber { get => short.Parse( InputType.Substring( 1 ) ); } // cannot fail else we have a program error...

    /// <summary>
    /// Returns the Modifier for this item
    /// i.e. only modifiers
    /// </summary>
    public string Modifier
    {
      get {
        // input can be:  {modifier+}Input
        if ( !ControlInput.Contains( "+" ) ) return ""; // no modifier

        string[] e = ControlInput.Split( new char[] { '+' } );
        string mod = "";
        for ( int i = 0; i < e.Length - 1; i++ ) {
          mod += MapProps.ModS( e[i] );
        }
        return "(" + mod + ")";
      }
    }

    /// <summary>
    /// Returns the Main Control for this item
    /// i.e. no modifiers
    /// </summary>
    public string MainControl
    {
      get {
        // input can be:  {modifier+}Input
        if ( !ControlInput.Contains( "+" ) ) return ControlInput; // no modifier

        string[] e = ControlInput.Split( new char[] { '+' } );
        return e[e.Length - 1]; // last item
      }
    }

    /// <summary>
    /// Returns the DispText with Modding added
    /// </summary>
    public string ModdedDispText
    {
      get {
        return Modifier+DispText;
      }
    }


  }
}
