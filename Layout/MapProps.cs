using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSFS2020Ctrls.Layout.ActionGroups;

namespace MSFS2020Ctrls.Layout
{
  /// <summary>
  /// Maintains the colormap for the Layout
  /// </summary>
  class MapProps
  {
    public static string ModShoulderLeft = "←";//═
    public static string ModShoulderRight = "→";
    public static string ModDpadDown = "↓";
    public static string ModTrigLeft = "◄";

    public static string ModAltLeft = "<A";
    public static string ModAltRight = "<A";
    public static string ModCtrlLeft = "<C";
    public static string ModCtrlRight = ">C";
    public static string ModShiftLeft = "<S";
    public static string ModShiftRight = ">S";

    /// <summary>
    /// Returns a Layout Modifier string (char) for a given modifier
    /// </summary>
    /// <param name="modifier">The modifier string</param>
    /// <returns></returns>
    public static string ModS( string modifier )
    {
      if ( modifier == "shoulderl" ) {
        return ModShoulderLeft;
      }
      else if ( modifier == "shoulderr" ) {
        return ModShoulderRight;
      }
      else if ( modifier == "lalt" ) {
        return ModAltLeft;
      }
      else if ( modifier == "ralt" ) {
        return ModAltRight;
      }
      else if ( modifier == "lshift" ) {
        return ModShiftLeft;
      }
      else if ( modifier == "rshift" ) {
        return ModShiftRight;
      }
      else if ( modifier == "lctrl" ) {
        return ModCtrlLeft;
      }
      else if ( modifier == "rctrl" ) {
        return ModCtrlRight;
      }
      else if ( modifier == "dpad_down" ) {
        return ModDpadDown;
      }
      else if ( modifier == "triggerl_btn" ) {
        return ModTrigLeft;
      }

      else {
        return "⸮";
      }
    }

    /// <summary>
    /// Save Color Settings for the Layout
    /// </summary>
    public struct ColorPair
    {
      public Color ForeColor;
      public Color BackColor;

      public ColorPair( Color fColor, Color bColor )
      {
        ForeColor = fColor;
        BackColor = bColor;
      }
    }

    // Text 
    private const string c_fontFamily = "Tahoma";
    private static int m_fontSize = 16; // maintained as int to support the TrackBar Value property)
    private static Font m_font = new Font( c_fontFamily, m_fontSize ); // real fontsize will be scaled to float but not used outside

    // Keyboard Layout Text 
    private const string c_kbdFontFamily = "Gill Sans Nova Cond";
    private static Font m_kbdFont = new Font( c_kbdFontFamily, m_fontSize, FontStyle.Bold ); // real fontsize will be scaled to float but not used outside

    // Keyboard Label Text 
    private const string c_kbdLabelFontFamily = "Verdana";
    private static Font m_kbdLabelFont = new Font( c_kbdLabelFontFamily, 24 , FontStyle.Bold);
    private static Brush m_kbdLabelBrush = Brushes.BlueViolet;

    // Keyboard Symbols
    private const string c_kbdSymbolFontFamily = "Tahoma";
    private const int c_kbdSymbolFontDecrement = 5; // kbd Symbol is so many points smaller than main font
    private static Font m_kbdSymbolFont = new Font( c_kbdSymbolFontFamily, m_fontSize - c_kbdSymbolFontDecrement );
    private static Brush m_kbdSymbolBrush = Brushes.DimGray;
    private static Pen m_kbdSymbolPen = Pens.Gray;


    // all known actionmaps with it's classification
    private static Dictionary<EGroup, ColorPair> m_amColors;

    /// <summary>
    /// cTor: Load Colors from AppSettings
    /// </summary>
    static MapProps()
    {
      AppSettings.Instance.Reload( );

      FontSize = AppSettings.Instance.LayoutFontSize; // also creates the MapFont property

      m_amColors = new Dictionary<EGroup, ColorPair> {
        { EGroup.Plane,     ConvertFromString(AppSettings.Instance.GroupColor_00) },
        { EGroup.CockpitInteractions,  ConvertFromString(AppSettings.Instance.GroupColor_01) },
        { EGroup.InGameUI,  ConvertFromString(AppSettings.Instance.GroupColor_02) },
        { EGroup.Modes,    ConvertFromString(AppSettings.Instance.GroupColor_03) },
        { EGroup.Multiplayer,     ConvertFromString(AppSettings.Instance.GroupColor_04) },
        { EGroup.Slew,          ConvertFromString(AppSettings.Instance.GroupColor_05) },
        { EGroup.ATC,             ConvertFromString(AppSettings.Instance.GroupColor_06) },
        { EGroup.Cameras,         ConvertFromString(AppSettings.Instance.GroupColor_07) },
       // { EGroup.VehicleWeapons,  ConvertFromString(AppSettings.Instance.GroupColor_08) }, // removed in 3.10.0
        { EGroup.Drone,          ConvertFromString(AppSettings.Instance.GroupColor_09) },
        { EGroup.Menu,     ConvertFromString(AppSettings.Instance.GroupColor_10) },
        { EGroup.Debug,       ConvertFromString(AppSettings.Instance.GroupColor_11) },
        { EGroup.Others,          ConvertFromString(AppSettings.Instance.GroupColor_12) }
      };
    }

    /// <summary>
    /// Save color map settings for the Layout
    /// </summary>
    public static void SaveToSettings()
    {
      AppSettings.Instance.LayoutFontSize = m_fontSize;

      AppSettings.Instance.GroupColor_00 = ConvertToString( m_amColors[EGroup.Plane] );
      AppSettings.Instance.GroupColor_01 = ConvertToString( m_amColors[EGroup.CockpitInteractions] );
      AppSettings.Instance.GroupColor_02 = ConvertToString( m_amColors[EGroup.InGameUI] );
      AppSettings.Instance.GroupColor_03 = ConvertToString( m_amColors[EGroup.Modes] );
      AppSettings.Instance.GroupColor_04 = ConvertToString( m_amColors[EGroup.Multiplayer] );
      AppSettings.Instance.GroupColor_05 = ConvertToString( m_amColors[EGroup.Slew] );
      AppSettings.Instance.GroupColor_06 = ConvertToString( m_amColors[EGroup.ATC] );
      AppSettings.Instance.GroupColor_07 = ConvertToString( m_amColors[EGroup.Cameras] );
      // AppSettings.Instance.GroupColor_08 = ConvertToString( m_amColors[EGroup.VehicleWeapons] );// removed in 3.10.0
      AppSettings.Instance.GroupColor_09 = ConvertToString( m_amColors[EGroup.Drone] );
      AppSettings.Instance.GroupColor_10 = ConvertToString( m_amColors[EGroup.Menu] );
      AppSettings.Instance.GroupColor_11 = ConvertToString( m_amColors[EGroup.Debug] );
      AppSettings.Instance.GroupColor_12 = ConvertToString( m_amColors[EGroup.Others] );

      AppSettings.Instance.Save( );
    }

    /// <summary>
    /// Cheap serializing...
    /// Converts from a ColorPair to a string
    /// </summary>
    /// <param name="colPair">A ColorPair</param>
    /// <returns>A serialized string</returns>
    private static string ConvertToString( ColorPair colPair )
    {
      string f = TypeDescriptor.GetConverter( typeof( Color ) ).ConvertToInvariantString( colPair.ForeColor );
      string b = TypeDescriptor.GetConverter( typeof( Color ) ).ConvertToInvariantString( colPair.BackColor );
      return $"{f}|{b}";
    }
    /// <summary>
    /// Cheap deserializing...
    /// Converts from a string to a ColorPair
    /// </summary>
    /// <param name="colPairS">A serialized string</param>
    /// <returns>A ColorPair</returns>
    private static ColorPair ConvertFromString( string colPairS )
    {
      string[] e = colPairS.Split( new char[] { '|' } );
      if ( e.Length == 2 ) {
        var f = (Color)TypeDescriptor.GetConverter( typeof( Color ) ).ConvertFromInvariantString( e[0] );
        var b = (Color)TypeDescriptor.GetConverter( typeof( Color ) ).ConvertFromInvariantString( e[1] );
        return new ColorPair( f, b );
      }
      else {
        return new ColorPair( Color.Pink, Color.Transparent );
      }
    }

    // Handle Layout Font
    /// <summary>
    /// Returns the Display Font for the layout
    /// </summary>
    public static Font MapFont { get => m_font; }

    /// <summary>
    /// FontSize property
    /// creates a new MapFont and KbdSymbolFont property to use
    /// </summary>
    public static int FontSize
    {
      get => m_fontSize;
      set {
        m_fontSize = value;
        m_font = new Font( m_font.FontFamily, m_fontSize );
        m_kbdSymbolFont = new Font( m_kbdSymbolFont.FontFamily, m_fontSize - c_kbdSymbolFontDecrement );

  }
}

    // Handle Layout Colors

    public static void SetMapColor( EGroup eGroup, ColorPair colorPair )
    {
      m_amColors[eGroup] = colorPair;
    }

    public static void SetMapColor( EGroup eGroup, Color fcolor, Color bcolor )
    {
      m_amColors[eGroup] = new ColorPair( fcolor, bcolor );
    }

    public static void SetMapForeColor( EGroup eGroup, Color color )
    {
      var copy = m_amColors[eGroup];
      copy.ForeColor = color;
      m_amColors[eGroup] = copy;
    }

    public static void SetMapBackColor( EGroup eGroup, Color color )
    {
      var copy = m_amColors[eGroup];
      copy.BackColor = color;
      m_amColors[eGroup] = copy;
    }

    public static ColorPair MapColor( string actionmap )
    {
      var acls = ActionGroups.MapNameToGroup( actionmap );
      return m_amColors[acls];
    }


    /// <summary>
    /// Returns the text color for an actionmap
    /// </summary>
    /// <param name="actionmap"></param>
    public static Color MapForeColor( string actionmap )
    {
      var acls = ActionGroups.MapNameToGroup( actionmap );
      return m_amColors[acls].ForeColor;
    }

    public static Color MapBackColor( string actionmap )
    {
      var acls = ActionGroups.MapNameToGroup( actionmap );
      return m_amColors[acls].BackColor;
    }


    public static ColorPair GroupColor( EGroup eGroup )
    {
      return m_amColors[eGroup];
    }

    public static Color GroupForeColor( EGroup eGroup )
    {
      return m_amColors[eGroup].ForeColor;
    }

    public static Color GroupBackColor( EGroup eGroup )
    {
      return m_amColors[eGroup].BackColor;
    }

    // Keyboard Text
    public static Font KbdFont { get => m_kbdFont; }

    // Keyboard Symbols 
    public static Font KbdSymbolFont { get => m_kbdSymbolFont; }
    public static Brush KbdSymbolBrush { get => m_kbdSymbolBrush; }
    public static Pen KbdSymbolPen { get => m_kbdSymbolPen; }

    // Keyboard Labels
    public static Font KbdLabelFont { get => m_kbdLabelFont; }
    public static Brush KbdLabelBrush { get => m_kbdLabelBrush; }

  }
}
