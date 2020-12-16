using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSFS2020Ctrls.Support
{
  /// <summary>
  /// A variety of Input conversions and transformations
  /// See Input PPTX
  /// also see http://kbdlayout.info/
  /// </summary>
  public class InputTransform
  {

    #region Locale Information
    /// <summary>
    /// Gets the active input locale identifier.
    /// </summary>
    public static string KbdLayoutID {
      get {
        InputLanguage myCurrentLanguage = InputLanguage.CurrentInputLanguage;
        if ( myCurrentLanguage != null )
          return $"{myCurrentLanguage.Culture.KeyboardLayoutId:X8}";
        else
          return "00000000";
      }
    }

    /// <summary>
    /// Gets the culture name in the format languagecode2-country/regioncode2.
    /// </summary>
    public static string LocaleString {
      get {
        {
          InputLanguage myCurrentLanguage = InputLanguage.CurrentInputLanguage;
          if ( myCurrentLanguage != null )
            return myCurrentLanguage.Culture.Name;
          else
            return "??-??";
        }
      }
    }

    #endregion

    #region KeyNames, Layouts (handles localized names)

    /// <summary>
    /// Returns the literal for a Win Virtual Key cleaned from OemXY types
    /// </summary>
    /// <param name="key">A VirtualKey code</param>
    /// <returns>A literal</returns>
    public static string VirtualKeyname( Keys key )
    {
      string keyLit = VirtualKeyLiteral( key );
      if ( keyLit.StartsWith( "Oem" ) ) {
        // Handle ugly named virtual keys - convert to something readable
        return GetCharFromVirtualKey( key ); // try this ...
      }
      return keyLit;
    }


    /// <summary>
    /// Returns the literal for a Win Virtual Key cleaned from OemXY types
    /// </summary>
    /// <param name="key">A VirtualKey code</param>
    /// <returns>A literal</returns>
    public static string VirtualKeyname( DxKey key )
    {
      var vKey = VirtualKeyFromDxKey(key);
      return VirtualKeyname( vKey );
    }

    #endregion

    #region Win Virtual Keys (handles Win Virtual Keys and literals)

    // Win Virtual Keys are defined in System.Windows.Forms.Keys

    /// <summary>
    /// Return a Key name from a Virtual Key
    /// </summary>
    /// <param name="virtKeyCode">The Virtual Key</param>
    /// <returns>A key name</returns>
    public static string GetCharFromVirtualKey( Keys virtKeyCode )
    {
      byte[] keyBoardState = new byte[256];
      if ( WinApi.GetKeyboardState( keyBoardState ) ) {
        WinApi.ToAscii( (uint)virtKeyCode, 0, keyBoardState, out uint outChar, 0 );
        return ( (char)outChar ).ToString( );
      }
      else {
        return "";
      }
    }

    /// <summary>
    /// Returns the literal for a Win Virtual Key
    /// </summary>
    /// <param name="keys">A VirtualKey code</param>
    /// <returns>A literal</returns>
    public static string VirtualKeyLiteral( Keys keys )
    {
      try {
        return Enum.GetName( typeof( Keys ), keys );
      }
      catch { }
      return "VirtKey UNKNOWN";
    }

    /// <summary>
    /// Returns a virtual key from a scancode
    /// </summary>
    /// <param name="scancode">A scancode</param>
    /// <returns>The virtual key via API mapping</returns>
    public static Keys VirtualKeyFromScanCode( uint scancode )
    {
      var vKeyCode = WinApi.MapVirtualKeyExEx( scancode );
      if ( Enum.IsDefined( typeof( Keys ), vKeyCode ) )
        return vKeyCode;
      else
        return Keys.None;
    }

    /// <summary>
    /// Returns a virtual Key from a DxKey
    /// </summary>
    /// <param name="dXKey">A DxKey</param>
    /// <returns>The virtual key via API mapping</returns>
    public static Keys VirtualKeyFromDxKey( DxKey dXKey )
    {
      uint scancode = ScanCodeFromDxKey(dXKey);
      Keys vKeyCode =VirtualKeyFromScanCode(scancode);
      if ( Enum.IsDefined( typeof( Keys ), vKeyCode ) )
        return vKeyCode;
      else
        return Keys.None;
    }

    #endregion

    #region CryEngine (handles from/to CryEngine Input literals)

    // See also SC keybinding_localization.xml

    // space, tab, semicolon, apostrophe, insert, left, right, up, down, home, pgup, pgdown, end, backspace
    // lbracket, rbracket,  np_0, np_1.., np_period, np_divide f1.., equal, minus, slash, comma, enter, backslash, equals, 
    // capslock
    // Modifiers: lalt, ralt, lctrl, rctrl (e.g. ralt+l, lshift+lctrl+1, lalt+lctrl+1)

    /// <summary>
    /// Map DXkey codes to CryEngine literals
    /// Cry Engine uses DXinput with their own literal representation
    /// </summary>
    private static SortedList<DxKey,string> DX2CRYmap = new SortedList<DxKey,string>(){
      { DxKey.Unknown, "" },
      { DxKey.Escape, "escape" },
      { DxKey.D1, "1"}, { DxKey.D2, "2"}, { DxKey.D3, "3"}, { DxKey.D4, "4"}, { DxKey.D5, "5"},
      { DxKey.D6, "6"}, { DxKey.D7, "7"}, { DxKey.D8, "8"}, { DxKey.D9, "9"}, { DxKey.D0, "0"},
      { DxKey.Minus, "minus" }, { DxKey.Equals, "equals" }, { DxKey.Back, "backspace" }, { DxKey.Tab, "tab" },
      { DxKey.Q, "q" }, { DxKey.W, "w" }, { DxKey.E, "e" }, { DxKey.R, "r" }, { DxKey.T, "t" }, { DxKey.Y, "y" },
      { DxKey.U, "u" }, { DxKey.I, "i" }, { DxKey.O, "o" }, { DxKey.P, "p" },
      { DxKey.LeftBracket, "lbracket" }, { DxKey.RightBracket,  "rbracket" }, { DxKey.Return,  "enter"}, { DxKey.LeftControl,  "lctrl" },
      { DxKey.A, "a" }, { DxKey.S, "s" }, { DxKey.D, "d" }, { DxKey.F, "f" }, { DxKey.G, "g" }, { DxKey.H, "h" },
      { DxKey.J, "j" }, { DxKey.K, "k" }, { DxKey.L, "l" },
      { DxKey.Semicolon, "semicolon"}, { DxKey.Apostrophe, "apostrophe"}, { DxKey.Grave, "grave"}, { DxKey.LeftShift, "lshift"}, { DxKey.Backslash, "backslash" },
      { DxKey.Z, "z" }, { DxKey.X, "x" }, { DxKey.C, "c" }, { DxKey.V, "v" }, { DxKey.B, "b" }, { DxKey.N, "n" }, { DxKey.M, "m" },
      { DxKey.Comma, "comma" }, { DxKey.Period, "period" }, { DxKey.Slash, "slash" }, { DxKey.RightShift, "rshift" },
      { DxKey.Multiply, "np_multiply" }, { DxKey.LeftAlt, "lalt" }, { DxKey.Space, "space" }, { DxKey.Capital, "capslock" },
      { DxKey.F1, "f1" }, { DxKey.F2, "f2" }, { DxKey.F3, "f3" }, { DxKey.F4, "f4" }, { DxKey.F5, "f5" },
      { DxKey.F6, "f6" }, { DxKey.F7, "f7" }, { DxKey.F8, "f8" }, { DxKey.F9, "f9" }, { DxKey.F10, "f10" },
      { DxKey.NumberLock, "numlock" }, { DxKey.ScrollLock, "scrolllock" },
      { DxKey.NumberPad7, "np_7" }, { DxKey.NumberPad8, "np_8" }, { DxKey.NumberPad9, "np_9" }, { DxKey.Subtract, "np_subtract" },
      { DxKey.NumberPad4, "np_4" }, { DxKey.NumberPad5, "np_5" }, { DxKey.NumberPad6, "np_6" }, { DxKey.Add, "np_add" },
      { DxKey.NumberPad1, "np_1" }, { DxKey.NumberPad2, "np_2" }, { DxKey.NumberPad3, "np_3" }, { DxKey.NumberPad0, "np_0" },
      { DxKey.Decimal, "np_period" }, { DxKey.Oem102, "oem102" },
      { DxKey.F11, "f11" }, { DxKey.F12, "f12" }, { DxKey.F13, "f13" }, { DxKey.F14, "f14" }, { DxKey.F15, "f15" },
      { DxKey.Colon, "colon" }, { DxKey.Underline, "underline" },
      { DxKey.NumberPadEnter, "np_enter"}, { DxKey.RightControl, "rctrl" },
      { DxKey.Divide, "np_divide" }, { DxKey.PrintScreen, "print" }, { DxKey.RightAlt, "ralt" }, { DxKey.Pause, "pause" },
      { DxKey.Home, "home" }, { DxKey.Up, "up" }, { DxKey.PageUp, "pgup" }, { DxKey.Left, "left" }, { DxKey.Right, "right" },
      { DxKey.End, "end" }, { DxKey.Down, "down" }, { DxKey.PageDown, "pgdown" }, { DxKey.Insert, "insert" }, { DxKey.Delete, "delete" },
    };

    /// <summary>
    /// Returns the Cry Literal of a DXKey code
    /// </summary>
    /// <param name="dXKey">The DXKey code</param>
    /// <returns>A Cry Literal or and empty string if not known</returns>
    public static string CryLiteralFromDxKey( DxKey dXKey )
    {
      try {
        return DX2CRYmap[dXKey];
      }
      catch { }
      return ""; // no keymap return or not found..
    }

    /// <summary>
    /// Returns the DXKey code for a Cry Literal
    /// </summary>
    /// <param name="cryLiteral">The Cry Literal</param>
    /// <returns>The DXKey code (can be .Unknown if not found)</returns>
    public static DxKey DxKeyFromCryLiteral( string cryLiteral )
    {
      if ( string.IsNullOrWhiteSpace( cryLiteral ) ) return DxKey.Unknown;

      int ret = DX2CRYmap.Values.IndexOf(cryLiteral);
      if ( ret >= 0 ) {
        return DX2CRYmap.Keys.ElementAt( ret );
      }
      else
        return DxKey.Unknown;
    }

    /// <summary>
    /// Translate the DX Keypressed list into a Cry string
    /// </summary>
    /// <param name="pressedKeys">The list of pressed DX keys</param>
    /// <param name="modAndKey">if true return Modifiers and Key literals, else only the modifier literal</param>
    /// <param name="keyOnly">if true return only the Key literal (modAndKey must be true)</param>
    /// <param name="CryClearMods">Spcl code to kill Modifiers right away</param>
    /// <returns>The Cry literal string</returns>
    public static string CryCommandFromDxKeyList( List<DxKey> pressedKeys, bool modAndKey, bool keyOnly, string CryClearMods = "escape" )
    {
      string altMod = "";
      string shiftMod = "";
      string ctrlMod = "";
      string key = "";

      foreach ( DxKey k in pressedKeys ) {
        switch ( (int)k ) {
          // handle modifiers first
          case (int)DxKey.LeftAlt: altMod += CryLiteralFromDxKey( k ) + "+"; break;
          case (int)DxKey.RightAlt: altMod += CryLiteralFromDxKey( k ) + "+"; break;
          case (int)DxKey.LeftShift: shiftMod += CryLiteralFromDxKey( k ) + "+"; break;
          case (int)DxKey.RightShift: shiftMod += CryLiteralFromDxKey( k ) + "+"; break;
          case (int)DxKey.LeftControl: ctrlMod += CryLiteralFromDxKey( k ) + "+"; break;
          case (int)DxKey.RightControl: ctrlMod += CryLiteralFromDxKey( k ) + "+"; break;
          default:
            // the rest
            key += CryLiteralFromDxKey( k ) + "+";
            break;
        }
      }
      // 
      if ( modAndKey ) {
        if ( keyOnly ) {
          ; // key only
        }
        else {
          key = altMod + shiftMod + ctrlMod + key;
        }
      }
      else {
        // mods only if not killed
        if ( !key.Contains( CryClearMods ) ) key = altMod + shiftMod + ctrlMod;
      }

      return key.TrimEnd( new char[] { '+' } );  // return killing the last +
    }

    /// <summary>
    /// Translate the Win Virtual Key pressed list into Cry string
    /// </summary>
    /// <param name="pressedKeys">The list of pressed Virtual keys</param>
    /// <param name="modAndKey">if true return Modifiers and Key literals, else only the modifier literal</param>
    /// <param name="keyOnly">if true return only the Key literal (modAndKey must be true)</param>
    /// <param name="CryClearMods">Spcl code to kill Modifiers right away</param>
    /// <returns>The Cry literal string</returns>
    public static string CryCommandFromVirtualKeyList( List<Keys> pressedKeys, bool modAndKey, bool keyOnly, string CryClearMods = "escape" )
    {
      var dxKeyList = new List<DxKey>();
      foreach ( Keys k in pressedKeys ) {
        dxKeyList.Add( DxKeyFromVirtualKey( k ) );
      }
      return CryCommandFromDxKeyList( dxKeyList, modAndKey, keyOnly, CryClearMods );
    }

    #endregion


    #region DXKey Section (handles Direct Input items)


    /// <summary>
    /// Converts a Win Virtual Key to a Direct Input (DXKey) code
    /// </summary>
    /// <param name="key">A Win Virtual Key</param>
    /// <returns>The DXKey code</returns>
    public static DxKey DxKeyFromVirtualKey( Keys key )
    {
      uint scancode = WinApi.MapVirtualKeyExEx( key );
      return DxKeyFromScanCode( scancode );
    }


    /// <summary>
    /// Converts a Win Scan Code to a Direct Input (DXKey) code
    /// </summary>
    /// <param name="scancode">A Win Keyboard Scancode</param>
    /// <returns>The DXKey code</returns>
    public static DxKey DxKeyFromScanCode( uint scancode )
    {
      uint sc = scancode;
      if ( scancode == 0xe11d ) return DxKey.Pause; // ?? no algo

      // E0xx SC codes get  (xx | 80) & FF        i.e.Low byte( xx ) with High Bit set
      if ( scancode > 0xff )
        sc = ( scancode | 0x80 ) & 0xff;
      else
        sc = scancode & 0xff;

      if ( Enum.IsDefined( typeof( DxKey ), (DxKey)sc ) )
        return (DxKey)sc;
      else
        return DxKey.Unknown;
    }

    /// <summary>
    /// Converts a Direct Input (DXKey) code to a Win Scan Code 
    /// </summary>
    /// <param name="dxKeyCode">A DXKey code</param>
    /// <returns>The Win Keyboard Scancode</returns>
    public static uint ScanCodeFromDxKey( DxKey dxKeyCode )
    {
      if ( dxKeyCode == DxKey.Pause ) return 0xe11d; // ?? no algo

      // Reverse the E0xx SC codes get  (xx | 80) & FF        i.e. strip high bit and | with e0000
      if ( ( (int)dxKeyCode & 0x80 ) > 0 ) // high bit set
        return ( ( (uint)dxKeyCode & 0x7f ) | 0xe000 ) & 0xffff;
      else
        return (uint)dxKeyCode;
    }

    /// <summary>
    /// Returns a literal for the given DXKey
    /// </summary>
    /// <param name="dXKey"></param>
    /// <returns></returns>
    public string DxKeyLiteral( DxKey dXKey )
    {
      try {
        return Enum.GetName( typeof( DxKey ), dXKey );
      }
      catch { }
      return "DXKey UNKNOWN";
    }

    //
    // COPIED FROM DXSHARP  (to not need to include the package)
    //

    //
    // Summary:
    //     Keyboard device constants, defined in Dinput.h, represent offsets within a keyboard
    //     device's data packet, a 256-byte array.
    //
    // Remarks:
    //     The following alternate names are available: Alternate name Regular name Note
    //     DIK_BACKSPACE SharpDX.DirectInput.Key.Back BACKSPACE DIK_CAPSLOCK SharpDX.DirectInput.Key.Capital
    //     CAPS LOCK DIK_CIRCUMFLEX SharpDX.DirectInput.Key.PreviousTrack On Japanese keyboard
    //     DIK_DOWNARROW SharpDX.DirectInput.Key.Down On arrow keypad DIK_LALT SharpDX.DirectInput.Key.LeftAlt
    //     Left ALT DIK_LEFTARROW SharpDX.DirectInput.Key.Left On arrow keypad DIK_NUMPADMINUS
    //     DIK__SUBTRACT MINUS SIGN (-) on numeric keypad DIK_NUMPADPERIOD SharpDX.DirectInput.Key.Decimal
    //     PERIOD (decimal point) on numeric keypad DIK_NUMPADPLUS SharpDX.DirectInput.Key.Add
    //     PLUS SIGN (+) on numeric keypad DIK_NUMPADSLASH DIK__DIVIDE Forward slash (/)
    //     on numeric keypad DIK_NUMPADSTAR SharpDX.DirectInput.Key.Multiply Asterisk (*)
    //     on numeric keypad DIK_PGDN SharpDX.DirectInput.Key.PageDown On arrow keypad DIK_PGUP
    //     SharpDX.DirectInput.Key.PageUp On arrow keypad DIK_RALT SharpDX.DirectInput.Key.RightAlt
    //     Right ALT DIK_RIGHTARROW SharpDX.DirectInput.Key.Right On arrow keypad DIK_UPARROW
    //     SharpDX.DirectInput.Key.Up On arrow keypad For information about Japanese keyboards,
    //     see DirectInput and Japanese Keyboards. The data at a given offset is associated
    //     with a keyboard key. Typically, these values are used in the dwOfs member of
    //     the SharpDX.DirectInput.ObjectData, DIOBJECTDATAFORMAT or SharpDX.DirectInput.DeviceObjectInstance
    //     structures, or as indices when accessing data within the array using array notation.
    public enum DxKey // Dx rather than DX from the original
    {
      Unknown = 0,
      Escape = 1,
      D1 = 2,
      D2 = 3,
      D3 = 4,
      D4 = 5,
      D5 = 6,
      D6 = 7,
      D7 = 8,
      D8 = 9,
      D9 = 10,
      D0 = 11,
      Minus = 12,
      Equals = 13,
      Back = 14,
      Tab = 15,
      Q = 16,
      W = 17,
      E = 18,
      R = 19,
      T = 20,
      Y = 21,
      U = 22,
      I = 23,
      O = 24,
      P = 25,
      LeftBracket = 26,
      RightBracket = 27,
      Return = 28,
      LeftControl = 29,
      A = 30,
      S = 31,
      D = 32,
      F = 33,
      G = 34,
      H = 35,
      J = 36,
      K = 37,
      L = 38,
      Semicolon = 39,
      Apostrophe = 40,
      Grave = 41,
      LeftShift = 42,
      Backslash = 43,
      Z = 44,
      X = 45,
      C = 46,
      V = 47,
      B = 48,
      N = 49,
      M = 50,
      Comma = 51,
      Period = 52,
      Slash = 53,
      RightShift = 54,
      Multiply = 55,
      LeftAlt = 56,
      Space = 57,
      Capital = 58,
      F1 = 59,
      F2 = 60,
      F3 = 61,
      F4 = 62,
      F5 = 63,
      F6 = 64,
      F7 = 65,
      F8 = 66,
      F9 = 67,
      F10 = 68,
      NumberLock = 69,
      ScrollLock = 70,
      NumberPad7 = 71,
      NumberPad8 = 72,
      NumberPad9 = 73,
      Subtract = 74,
      NumberPad4 = 75,
      NumberPad5 = 76,
      NumberPad6 = 77,
      Add = 78,
      NumberPad1 = 79,
      NumberPad2 = 80,
      NumberPad3 = 81,
      NumberPad0 = 82,
      Decimal = 83,
      Oem102 = 86,
      F11 = 87,
      F12 = 88,
      F13 = 100,
      F14 = 101,
      F15 = 102,
      Kana = 112,
      AbntC1 = 115,
      Convert = 121,
      NoConvert = 123,
      Yen = 125,
      AbntC2 = 126,
      NumberPadEquals = 141,
      PreviousTrack = 144,
      AT = 145,
      Colon = 146,
      Underline = 147,
      Kanji = 148,
      Stop = 149,
      AX = 150,
      Unlabeled = 151,
      NextTrack = 153,
      NumberPadEnter = 156,
      RightControl = 157,
      Mute = 160,
      Calculator = 161,
      PlayPause = 162,
      MediaStop = 164,
      VolumeDown = 174,
      VolumeUp = 176,
      WebHome = 178,
      NumberPadComma = 179,
      Divide = 181,
      PrintScreen = 183,
      RightAlt = 184,
      Pause = 197,
      Home = 199,
      Up = 200,
      PageUp = 201,
      Left = 203,
      Right = 205,
      End = 207,
      Down = 208,
      PageDown = 209,
      Insert = 210,
      Delete = 211,
      LeftWindowsKey = 219,
      RightWindowsKey = 220,
      Applications = 221,
      Power = 222,
      Sleep = 223,
      Wake = 227,
      WebSearch = 229,
      WebFavorites = 230,
      WebRefresh = 231,
      WebStop = 232,
      WebForward = 233,
      WebBack = 234,
      MyComputer = 235,
      Mail = 236,
      MediaSelect = 237
    }

    #endregion

    public InputTransform( )
    {
      // sanity check 
      if ( DxKeyFromCryLiteral( "delete" ) != DxKey.Delete ) // check last element of the mapping table
        Console.WriteLine( "\n\n****** ERROR IN InputTransform - DX2CRYmap does not match\n\n" );


    }

  }
}
#region WIN API Section

/// <summary>
/// Some underlying conversions and WinAPI interfaces
/// </summary>
static class WinApi
{
  public const string LibraryName = "user32";

  internal static class Properties
  {
#if !ANSI
    public const CharSet BuildCharSet = CharSet.Unicode;
#else
        public const CharSet BuildCharSet = CharSet.Ansi;
#endif
  }

  /// <summary>
  /// The set of valid MapTypes used in MapVirtualKey
  /// </summary>
  public enum VirtualKeyMapType
  {

    /// <summary>
    /// The uCode parameter is a virtual-key code and is translated into a scan code. 
    /// If it is a virtual-key code that does not distinguish between left- and right-hand keys, 
    /// the left-hand scan code is returned. 
    /// If there is no translation, the function returns 0. 
    /// </summary>
    MAPVK_VK_TO_VSC = 0x0,

    /// <summary>
    /// The uCode parameter is a scan code and is translated into a virtual-key code that does 
    /// not distinguish between left- and right-hand keys. 
    /// If there is no translation, the function returns 0. 
    /// </summary>
    MAPVK_VSC_TO_VK = 0x1,

    /// <summary>
    /// The uCode parameter is a virtual-key code and is translated into an unshifted character 
    /// value in the low order word of the return value. Dead keys (diacritics) are indicated 
    /// by setting the top bit of the return value. 
    /// If there is no translation, the function returns 0. 
    /// </summary>
    MAPVK_VK_TO_CHAR = 0x2,

    /// <summary>
    /// The uCode parameter is a scan code and is translated into a virtual-key code that 
    /// distinguishes between left- and right-hand keys. 
    /// If there is no translation, the function returns 0. 
    /// </summary>
    MAPVK_VSC_TO_VK_EX = 0x3,

    /// <summary>
    /// The uCode parameter is a virtual-key code and is translated into a scan code. 
    /// If it is a virtual-key code that does not distinguish between left- and right-hand keys, 
    /// the left-hand scan code is returned. If the scan code is an extended scan code, 
    /// the high byte of the uCode value can contain either 0xe0 or 0xe1 to specify the extended scan code. 
    /// If there is no translation, the function returns 0. 
    /// </summary>
    MAPVK_VK_TO_VSC_EX = 0x4,
  }

  /// <summary>
  /// Translates (maps) a virtual-key code into a scan code or character value, 
  /// or translates a scan code into a virtual-key code. 
  /// The function translates the codes using the input language and an input locale identifier.
  /// 
  /// NOTE: DX Keycodes are VSC codes (Scan Codes)
  /// </summary>
  /// <param name="uCode">Scan code for a key. 
  /// Starting with Windows Vista, the high byte of the uCode value can contain 
  /// either 0xe0 or 0xe1 to specify the extended scan code.
  /// </param>
  /// <param name="uMapType">MAPVK_VSC_TO_VK, MAPVK_VSC_TO_VK_EX</param>
  /// <param name="dwhkl">nput locale identifier to use for translating the specified code.</param>
  /// <returns>Either a scan code, a virtual-key code, or a character value, 
  /// depending on the value of uCode and uMapType. 
  /// If there is no translation, the return value is zero.
  /// </returns>
  [DllImport( LibraryName, CharSet = Properties.BuildCharSet )]
  public static extern uint MapVirtualKeyEx( uint uCode, VirtualKeyMapType uMapType, IntPtr dwhkl );

  /// <summary>
  /// Translates (maps) a virtual-key code into a scan code or character value, 
  /// or translates a scan code into a virtual-key code. 
  /// The function translates the codes using the input language and an input locale identifier.
  /// 
  /// NOTE: DX Keycodes are VSC codes
  /// </summary>
  /// <param name="uCode">The virtual-key code for a key. 
  /// Starting with Windows Vista, the high byte of the uCode value can contain 
  /// either 0xe0 or 0xe1 to specify the extended scan code.
  /// </param>
  /// <param name="uMapType">MAPVK_VK_TO_VSC, MAPVK_VK_TO_CHAR, MAPVK_VK_TO_VSC_EX </param>
  /// <param name="dwhkl">nput locale identifier to use for translating the specified code.</param>
  /// <returns>Either a scan code, a virtual-key code, or a character value, 
  /// depending on the value of uCode and uMapType. 
  /// If there is no translation, the return value is zero.
  /// </returns>
  [DllImport( LibraryName, CharSet = Properties.BuildCharSet )]
  public static extern uint MapVirtualKeyEx( Keys uCode, VirtualKeyMapType uMapType, IntPtr dwhkl );

  [DllImport( LibraryName, ExactSpelling = true )]
  public static extern uint ToAscii( uint uVirtKey, uint uScanCode, [MarshalAs( UnmanagedType.LPArray, SizeConst = 256 )] byte[] lpbKeyState, out uint lpwTransKey, uint uFlags );

  [DllImport( LibraryName, ExactSpelling = true )]
  public static extern uint ToUnicode( uint wVirtKey, uint wScanCode, [MarshalAs( UnmanagedType.LPArray, SizeConst = 256 )] byte[] lpbKeyState, out uint pwszBuff, int cchBuff, uint uFlags );

  /*
  [DllImport( LibraryName, ExactSpelling = true )]
  public static extern bool GetKeyboardState( [MarshalAs( UnmanagedType.LPArray, SizeConst = 256 )] out byte[] lpKeyState );
  */
  [DllImport( LibraryName, ExactSpelling = true )]
  [return: MarshalAs( UnmanagedType.Bool )]
  public static extern bool GetKeyboardState( byte[] lpKeyState );

  [DllImport( LibraryName, ExactSpelling = true )]
  public static extern bool GetKeyboardState( IntPtr lpKeyState );

  [DllImport( LibraryName, ExactSpelling = true )]
  public static extern IntPtr GetKeyboardLayout( uint idThread );

  /*
   * Substitute the crappy MapVirtualKeyEx which does only half the job.. as VK_ labels don't separate the Numlocked state
   * as it will not return Ext scancodes for navigation keys (and may be more...)
   */
  private static SortedList<Keys,uint> VC2SCmap  = new SortedList<Keys, uint>()
  {
    // Modifiers (else we get bit mods from Windows)
    { Keys.LShiftKey, 0x2a}, { Keys.RShiftKey, 0x36},
    { Keys.LControlKey, 0x1D}, { Keys.RControlKey, 0xe01d },
    { Keys.LMenu, 0x38}, { Keys.RMenu, 0xe038 },
    // Numpad
    { Keys.Multiply, 0x37}, { Keys.NumLock, 0x45},
    { Keys.NumPad7, 0x47}, { Keys.NumPad8, 0x48}, { Keys.NumPad9, 0x49}, { Keys.Subtract, 0x4a},
    { Keys.NumPad4, 0x4b}, { Keys.NumPad5, 0x4c}, { Keys.NumPad6, 0x4d}, { Keys.Add, 0x4e},
    { Keys.NumPad1, 0x4f}, { Keys.NumPad2, 0x50}, { Keys.NumPad3, 0x51}, { Keys.NumPad0, 0x52}, { Keys.Decimal, 0x53},
    // others found that don't match well
    { Keys.PrintScreen, 0x54}, 
    // extended codes and navigation keys
    { Keys.Return, 0xe01c }, { Keys.Divide, 0xe035 },
    { Keys.Home, 0xe047 }, { Keys.Up, 0xe048 }, { Keys.PageUp, 0xe049 },
    { Keys.Left, 0xe04b }, { Keys.Right, 0xe04d }, { Keys.End, 0xe04f }, { Keys.Down, 0xe050 },
    { Keys.PageDown, 0xe051 }, { Keys.Insert, 0xe052 }, { Keys.Delete, 0xe053 }, { Keys.Pause, 0xe11d }
  };

  /// <summary>
  /// returns a Virtual key from a scancode 
  ///  Assumes the NumLock on i.e. numberpad is active
  /// </summary>
  /// <param name="scancode">A scancode</param>
  /// <returns>A virtual key</returns>
  public static Keys MapVirtualKeyExEx( uint scancode )
  {
    // to virtual key
    if ( VC2SCmap.ContainsValue( scancode ) ) {
      return VC2SCmap.Keys.ElementAt( VC2SCmap.Values.IndexOf( scancode ) );
    }
    else {
      return (Keys)MapVirtualKeyEx( scancode, VirtualKeyMapType.MAPVK_VSC_TO_VK_EX, GetKeyboardLayout( 0 ) );
    }
  }

  /// <summary>
  /// Returns a scancode for a virtual key
  ///  Assumes the Numlock on i.e. returns numpad scancodes (not navigation ones)
  /// </summary>
  /// <param name="key">A virtual key</param>
  /// <returns>The scancode</returns>
  public static uint MapVirtualKeyExEx( Keys key )
  {
    // to scancode
    if ( VC2SCmap.ContainsKey( key ) ) {
      return VC2SCmap[key];
    }
    else {
      return MapVirtualKeyEx( key, VirtualKeyMapType.MAPVK_VK_TO_VSC_EX, GetKeyboardLayout( 0 ) );
    }
  }


  #endregion
}
