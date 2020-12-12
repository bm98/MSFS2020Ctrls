using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace MSFS2020Ctrls.MSFS
{
  /// <summary>
  /// Base Class for the ActionEntries
  /// </summary>
  class PrimSecActionBase
  {
    /*
			<Primary>
				<KEY Information="Ctrl">162</KEY>
				<KEY Information="Num -">109</KEY>
			</Primary>
			<Secondary>
				<KEY Information="Ctrl">162</KEY>
				<KEY Information="Alt">164</KEY>
				<KEY Information="Down">40</KEY>
			</Secondary>

     */

    public List<KeyItem> KeyCodes { get; private set; } = new List<KeyItem>( );



    /*
    <KEY Information="Ctrl">162</KEY>
    ...

             
      WinKeyCodes 
 */

    /*
        Joy:  jsN_+ "x", "rotx", "y", "roty", "z", "rotz", "slider1", "slider2", "buttonN", "hatN_up", "hatN_left", ..
        mouse: mo1_+ "maxis_x", "maxis_y", "mwheel_up", "mouseN", ..
        kbd:   kb1_+ "np_2", "1", "f14", "period", ..
          mod:   "lalt+", "ralt+", "lshift+", "rshift+", "lctrl+", "rctrl+" e.g. "kb1_lshift+lctrl+1"
 */

    // using SC codes except for the keyboard where the WinKeyCode is delivered (not all have a name and the code is used at the end)
    private string TranslateKey( )
    {
      var ret = "";
      List<Keys> keys = new List<Keys>();
      // collect the codes from the list
      foreach ( var kc in KeyCodes ) {
        string ctrl = kc.Control.Trim().ToLowerInvariant();
        string code = kc.KeyCode.Trim();
        Keys codeKey =(Keys)int.Parse(code);

        string p1="", p2="", p3="";
        var e = ctrl.Split();
        if ( e.Length > 0 ) p1 = e[0].Trim( );
        if ( e.Length > 1 ) p2 = e[1].Trim( );
        if ( e.Length > 2 ) p3 = e[2].Trim( );

        // JOY
        if ( ctrl.StartsWith( "joystick" ) ) {
          // "Joystick Button 2", "Joystick R-Axis Z ", "Joystick L-Axis Z ", "Joystick Slider X ", "Joystick Pov Up"
          ret += "";
          switch ( p2 ) {
            case "button": ret += $"button{p3}"; break;
            case "r-axis": ret += $"rot{p3}"; break;
            case "l-axis": ret += $"{p3}"; break;
            case "slider": string num= (p3=="Z") ? "3": (p3=="Y") ? "2":"1"; ret += $"slider{num}"; break;// TODO verify X,Y,Z to number
            case "pov": ret += $"hat1_{p3}"; break;// POV could be more than one ??
            default: ret += "TX-ERROR"; break;
          }

        }
        // MOUSE
        else if ( ctrl.StartsWith( "mouse" ) ) {
          // "Mouse 5", "Mouse Wheel Down"
          if ( p2 == "wheel" )
            ret += $"mwheel_{p3}"; // wheel
          else
            ret += $"mouse{p2}"; // button
        }
        else if ( ctrl.StartsWith( "left-click" ) ) {
          // "Left-Click"
          ret += "mouse1"; // usually...
        }
        else if ( ctrl.StartsWith( "mid-click" ) ) {
          // "Mid-Click"
          ret += "mouse3"; // usually...
        }
        else if ( ctrl.StartsWith( "right-click" ) ) {
          // "Right-Click"
          ret += "mouse2"; // usually...
        }
        else if ( ctrl.StartsWith( "axis" ) ) {
          //  "Axis Y+", 
          ret += "maxis_" + p2.Substring( 0, 1 ); // ignore +- qualifier
        }
        // XPAD
        // KEYBOARD 
        else {
          if ( codeKey == Keys.H ) {
            ;
          }
          keys.Add( codeKey );
        }
      }
      if ( keys.Count > 0 ) { 
        if ( ret.Length > 0 ) {
          ;// stop
        }
        ret = Support.KeyboardCls.FromWinKeyboardCmd( keys, true, false );
        Console.WriteLine( ret );
      }
      return ret;
    }


    /// <summary>
    /// Returns an SC code for easier implementation
    /// </summary>
    public string SCCode => TranslateKey( );

  }

  /// <summary>
  /// A Primary Action Entry
  /// </summary>
  class ActionEntryPrimary : PrimSecActionBase
  {
    // use base

    public static PrimSecActionBase ReadXML( XmlReader reader )
    {
      // must arrive on Prim node
      ActionEntryPrimary actionItem = null;
      switch ( reader.NodeType ) {
        case XmlNodeType.Element:
          if ( reader.Name == "Primary" ) {
            Console.WriteLine( $"\t\t\tFound the element: Primary" );
            actionItem = new ActionEntryPrimary( );
            actionItem.KeyCodes.AddRange( KeyItem.ReadXML( reader ) );
          }
          break;

        case XmlNodeType.EndElement:
          ; // DEBUG stop
          break;

        default: break;
      }
      return actionItem;
    }

  }

  /// <summary>
  /// A Secondary Action Entry
  /// </summary>
  class ActionEntrySecondary : PrimSecActionBase
  {
    // use base

    public static PrimSecActionBase ReadXML( XmlReader reader )
    {
      // must arrive on Sec node
      ActionEntrySecondary actionItem = null;
      switch ( reader.NodeType ) {
        case XmlNodeType.Element:
          if ( reader.Name == "Secondary" ) {
            Console.WriteLine( $"\t\t\tFound the element: Secondary" );
            actionItem = new ActionEntrySecondary( );
            actionItem.KeyCodes.AddRange( KeyItem.ReadXML( reader ) );
          }
          break;

        case XmlNodeType.EndElement:
          ; // DEBUG stop
          break;

        default: break;
      }
      return actionItem;
    }



  }
}
