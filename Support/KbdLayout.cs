using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSFS2020Ctrls.Support.InputTransform;

namespace MSFS2020Ctrls.Support
{
  /// <summary>
  /// Keyboard layouts
  /// </summary>
  public class KbdLayout
  {
    // string array of keys expected as follows 
    // (Ref sequence of the KLE JSON File - in fact not JSON but a Java/Typescript object...)
    //
    //--------------------------
    // Esc  F F F F  F F F F  F F F F Print Screen ScLock Pause
    // K  1 2 3 4 5 6 7 8 9 0 K K Back Ins Home Pup NLck Div Mult Sub
    // Tab  L L L L L L L L L L K K Ret Del End  Pdown 7 8 9 +
    // Caps  L L L L L L L L L K K K 4 5 6  
    // LSh  D L L L L L L L K K K  R-Shift Up 1 2 3 Ent
    // LCt L-Win L-Alt Space R-Alt R-Win App RCt Le  Do  Ri 0 . 
    //--------------------------
    /// <summary>
    /// This list must follow the order above to capture KLE Input properly
    /// </summary>
    private static List<DxKey> m_keyList = new List<DxKey>()
    {
      DxKey.Escape,DxKey.F1,DxKey.F2,DxKey.F3,DxKey.F4,DxKey.F5,DxKey.F6,DxKey.F7,DxKey.F8,DxKey.F9,DxKey.F10,DxKey.F11,DxKey.F12,
        DxKey.PrintScreen,DxKey.ScrollLock,DxKey.Pause,

      DxKey.Grave,DxKey.D1,DxKey.D2,DxKey.D3,DxKey.D4,DxKey.D5,DxKey.D6,DxKey.D7,DxKey.D8,DxKey.D9,DxKey.D0,DxKey.Minus,DxKey.Equals,DxKey.Back,
        DxKey.Insert,DxKey.Home,DxKey.PageUp,
        DxKey.NumberLock,DxKey.Divide,DxKey.Multiply,DxKey.Subtract,

      DxKey.Tab,DxKey.Q,DxKey.W,DxKey.E,DxKey.R,DxKey.T,DxKey.Y,DxKey.U,DxKey.I,DxKey.O, DxKey.P,DxKey.LeftBracket,DxKey.RightBracket,DxKey.Return,
        DxKey.Delete,DxKey.End,DxKey.PageDown,
        DxKey.NumberPad7,DxKey.NumberPad8,DxKey.NumberPad9,DxKey.Add,

      DxKey.Capital,DxKey.A,DxKey.S,DxKey.D,DxKey.F,DxKey.G,DxKey.H,DxKey.J,DxKey.K,DxKey.L,DxKey.Semicolon,DxKey.Apostrophe,DxKey.Backslash,
      DxKey.NumberPad4,DxKey.NumberPad5,DxKey.NumberPad6,

      DxKey.LeftShift,DxKey.Oem102,DxKey.Z,DxKey.X,DxKey.C,DxKey.V,DxKey.B,DxKey.N,DxKey.M,DxKey.Comma,DxKey.Period,DxKey.Slash,DxKey.RightShift,
        DxKey.Up,
        DxKey.NumberPad1,DxKey.NumberPad2,DxKey.NumberPad3,DxKey.NumberPadEnter,

      DxKey.LeftControl,DxKey.LeftWindowsKey,DxKey.LeftAlt,DxKey.Space,DxKey.RightAlt,DxKey.RightWindowsKey,DxKey.Applications,DxKey.RightControl,
        DxKey.Left,DxKey.Down,DxKey.Right,
        DxKey.NumberPad0,DxKey.Decimal
    };

    private static List<string> m_defaultMap = new List<string>(){
      "Esc", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "PrtSc", "Scroll Lock", "Pause", 
      "`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "Backspace", "Insert", "Home", "Page Up", "Num Lock", "/", "*", "-", 
      "Tab", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "[", "]", "Enter", "Delete", "End", "Page Down", "7", "8", "9", "+", 
      "Caps Lock", "A", "S", "D", "F", "G", "H", "J", "K", "L", ";", "'", "\\\\", "4", "5", "6", 
      "Shift", "\\\\", "Z", "X", "C", "V", "B", "N", "M", ",", ".", "/", "Shift", "↑", "1", "2", "3", "Enter", 
      "Ctrl", "Win", "Alt", " ", "AltGr", "Win", "Menu", "Ctrl", "←", "↓", "→", "0", "."
    };



    //  CLASS

    // Singleton
    private static readonly Lazy<KbdLayout> m_lazy = new Lazy<KbdLayout>( () => new KbdLayout( ) );
    public static KbdLayout Instance { get => m_lazy.Value; }



    private SortedList<string, KbdLayoutFile> m_layoutList = new SortedList<string, KbdLayoutFile>();
    private List<string> m_Layout = null;

    public KbdLayout( )
    {
      GetLayouts( );

      m_Layout = m_defaultMap; // default
    }

    /// <summary>
    /// Load all kbd layouts 
    /// </summary>
    private void GetLayouts( )
    {
      string path = Path.Combine("layouts", "kbd");
      var files = Directory.EnumerateFiles(path, "*.json", SearchOption.TopDirectoryOnly);
      if ( files == null ) return;

      // for all kbd files found
      foreach ( var f in files ) {
        var kf = KbdLayoutFile.FromJson(f);
        if ( kf == null ) continue;

        // add for all lang IDs
        foreach ( var klid in kf.MapKLIDs ) {
          m_layoutList.Add( klid, kf );
        }
      }
    }

    /// <summary>
    /// Returns true if we found it
    /// </summary>
    /// <param name="KLID">A Keyboar Lang ID</param>
    public bool KbdLayoutExists( string KLID )
    {
      return m_layoutList.ContainsKey( KLID );
    }
    /// <summary>
    /// Set the layout to use
    ///   if not found defaults to en_US
    /// </summary>
    /// <param name="KLID">A Keyboar Lang ID</param>
    public void SetKbdLayout( string KLID )
    {
      if ( m_layoutList.ContainsKey( KLID ) ) {
        m_Layout = m_layoutList[KLID].MapKeys;
      }
      else {
        m_Layout = m_defaultMap;
      }
    }

    /// <summary>
    /// Get a letter for a DxKey that should be the one on the keyboard
    ///  given a properly set layout..
    /// </summary>
    /// <param name="key">A DxKey code</param>
    /// <returns>A letter or string (keylabel)</returns>
    public string LabelForDxKey( DxKey key )
    {
      if ( m_Layout == null ) return "NO KBD";

      try {
        return m_Layout[m_keyList.IndexOf( key )];
      }
      catch { }
      return "DEAD KEY";
    }


  }
}
