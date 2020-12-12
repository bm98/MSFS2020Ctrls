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
    public enum DXKey
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

    #region Static Items

    public const string DeviceClass = "keyboard";  // the device name used throughout this app
    public const string DeviceID = "kb1_";
    static public int RegisteredDevices = 0;  // devices add here once they are created (though will not decrement as they are not deleted)
    public const string DevNameCIG = "Keyboard"; // just a name...
    public const string DevGUIDCIG = "{00000000-0000-0000-0000-000000000000}"; // - Fixed for Keyboards, we dont differentiate

    public const string ClearMods = "escape";

    // See also SC keybinding_localization.xml

    // space, tab, semicolon, apostrophe, insert, left, right, up, down, home, pgup, pgdown, end, backspace
    // lbracket, rbracket,  np_0, np_1.., np_period, np_divide f1.., equal, minus, slash, comma, enter, backslash, equals, 
    // capslock
    // Modifiers: lalt, ralt, lctrl, rctrl (e.g. ralt+l, lshift+lctrl+1, lalt+lctrl+1)

    /// <summary>
    /// Translate the DX Keypressed list into SC keycode string
    /// </summary>
    /// <param name="pressedKeys">The list of pressed DX keys</param>
    /// <returns>The SC keycode string</returns>
    public static string FromDXKeyboardCmd( List<DXKey> pressedKeys, bool modAndKey, bool keyOnly )
    {
      string altMod = "";
      string shiftMod = "";
      string ctrlMod = "";
      string key = "";

      foreach ( DXKey k in pressedKeys ) {
        switch ( (int)k ) {
          // handle modifiers first
          case (int)DXKey.LeftAlt: altMod += "lalt+"; break;
          case (int)DXKey.RightAlt: altMod += "ralt+"; break;
          case (int)DXKey.LeftShift: shiftMod += "lshift+"; break;
          case (int)DXKey.RightShift: shiftMod += "rshift+"; break;
          case (int)DXKey.LeftControl: ctrlMod += "lctrl+"; break;
          case (int)DXKey.RightControl: ctrlMod += "rctrl+"; break;

          // function keys first - modifier ??
          case (int)DXKey.F1: key += "f1+"; break;
          case (int)DXKey.F2: key += "f2+"; break;
          case (int)DXKey.F3: key += "f3+"; break;
          case (int)DXKey.F4: key += "f4+"; break;
          case (int)DXKey.F5: key += "f5+"; break;
          case (int)DXKey.F6: key += "f6+"; break;
          case (int)DXKey.F7: key += "f7+"; break;
          case (int)DXKey.F8: key += "f8+"; break;
          case (int)DXKey.F9: key += "f9+"; break;
          case (int)DXKey.F10: key += "f10+"; break;
          case (int)DXKey.F11: key += "f11+"; break;
          case (int)DXKey.F12: key += "f12+"; break;
          case (int)DXKey.F13: key += "f13+"; break;
          case (int)DXKey.F14: key += "f14+"; break;
          case (int)DXKey.F15: key += "f15+"; break;

          // all keys where the DX name does not match the SC name
          // Numpad
          case (int)DXKey.NumberLock: key += "numlock+"; break;
          case (int)DXKey.Divide: key += "np_divide+"; break;
          case (int)DXKey.Multiply: key += "np_multiply+"; break;
          case (int)DXKey.Subtract: key += "np_subtract+"; break;
          case (int)DXKey.Add: key += "np_add+"; break;
          case (int)DXKey.Decimal: key += "np_period+"; break;
          case (int)DXKey.NumberPadEnter: key += "np_enter+"; break;
          case (int)DXKey.NumberPad0: key += "np_0+"; break;
          case (int)DXKey.NumberPad1: key += "np_1+"; break;
          case (int)DXKey.NumberPad2: key += "np_2+"; break;
          case (int)DXKey.NumberPad3: key += "np_3+"; break;
          case (int)DXKey.NumberPad4: key += "np_4+"; break;
          case (int)DXKey.NumberPad5: key += "np_5+"; break;
          case (int)DXKey.NumberPad6: key += "np_6+"; break;
          case (int)DXKey.NumberPad7: key += "np_7+"; break;
          case (int)DXKey.NumberPad8: key += "np_8+"; break;
          case (int)DXKey.NumberPad9: key += "np_9+"; break;
          // Digits
          case (int)DXKey.D0: key += "0+"; break;
          case (int)DXKey.D1: key += "1+"; break;
          case (int)DXKey.D2: key += "2+"; break;
          case (int)DXKey.D3: key += "3+"; break;
          case (int)DXKey.D4: key += "4+"; break;
          case (int)DXKey.D5: key += "5+"; break;
          case (int)DXKey.D6: key += "6+"; break;
          case (int)DXKey.D7: key += "7+"; break;
          case (int)DXKey.D8: key += "8+"; break;
          case (int)DXKey.D9: key += "9+"; break;
          // navigation
          case (int)DXKey.Insert: key += "insert+"; break;
          case (int)DXKey.Home: key += "home+"; break;
          case (int)DXKey.Delete: key += "delete+"; break;
          case (int)DXKey.End: key += "end+"; break;
          case (int)DXKey.PageUp: key += "pgup+"; break;
          case (int)DXKey.PageDown: key += "pgdown+"; break;
          case (int)DXKey.PrintScreen: key += "print+"; break;
          case (int)DXKey.ScrollLock: key += "scrolllock+"; break;
          case (int)DXKey.Pause: key += "pause+"; break;
          // Arrows
          case (int)DXKey.Up: key += "up+"; break;
          case (int)DXKey.Down: key += "down+"; break;
          case (int)DXKey.Left: key += "left+"; break;
          case (int)DXKey.Right: key += "right+"; break;
          // non letters
          case (int)DXKey.Escape: key += "escape+"; break;
          case (int)DXKey.Minus: key += "minus+"; break;
          case (int)DXKey.Equals: key += "equals+"; break;
          case (int)DXKey.Grave: key += ""; break; // "grave+"; break; // reserved for Console 
          case (int)DXKey.Underline: key += "underline+"; break;
          case (int)DXKey.Back: key += "backspace+"; break;
          case (int)DXKey.Tab: key += "tab+"; break;
          case (int)DXKey.LeftBracket: key += "lbracket+"; break;
          case (int)DXKey.RightBracket: key += "rbracket+"; break;
          case (int)DXKey.Return: key += "enter+"; break;
          case (int)DXKey.Capital: key += "capslock+"; break;
          case (int)DXKey.Colon: key += "colon+"; break;
          case (int)DXKey.Backslash: key += "backslash+"; break;
          case (int)DXKey.Comma: key += "comma+"; break;
          case (int)DXKey.Period: key += "period+"; break;
          case (int)DXKey.Slash: key += "slash+"; break;
          case (int)DXKey.Space: key += "space+"; break;
          case (int)DXKey.Semicolon: key += "semicolon+"; break;
          case (int)DXKey.Apostrophe: key += "apostrophe+"; break;

          // all where the lowercase DX name matches the SC name
          default:
            if ( ( (int)k >= (int)DXKey.Q ) && ( (int)k <= (int)DXKey.P ) )
              key += k.ToString( ).ToLowerInvariant( ) + "+"; // ranges are based on the enum values...

            else if ( ( (int)k >= (int)DXKey.A ) && ( (int)k <= (int)DXKey.L ) )
              key += k.ToString( ).ToLowerInvariant( ) + "+"; // ranges are based on the enum values...

            else if ( ( (int)k >= (int)DXKey.Z ) && ( (int)k <= (int)DXKey.M ) )
              key += k.ToString( ).ToLowerInvariant( ) + "+"; // ranges are based on the enum values...

            else { } // no other ones handled
            break;
        }

      }//for
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
        if ( !key.Contains( ClearMods ) ) key = altMod + shiftMod + ctrlMod;
      }

      return key.TrimEnd( new char[] { '+' } );  // return killing the last +
    }

    /// <summary>
    /// Translate the Win Keypressed list into SC keycode string
    /// </summary>
    /// <param name="pressedKeys">The list of pressed Windows (virtual) keys</param>
    /// <returns>The SC keycode string</returns>
    public static string FromWinKeyboardCmd( List<Keys> pressedKeys, bool modAndKey, bool keyOnly )
    {
      string altMod = "";
      string shiftMod = "";
      string ctrlMod = "";
      string key = "";

      foreach ( Keys k in pressedKeys ) {
        switch ( (int)k ) {
          // handle modifiers first
          case (int)Keys.LMenu: altMod += "lalt+"; break;
          case (int)Keys.RMenu: altMod += "ralt+"; break;
          case (int)Keys.LShiftKey: shiftMod += "lshift+"; break;
          case (int)Keys.RShiftKey: shiftMod += "rshift+"; break;
          case (int)Keys.LControlKey: ctrlMod += "lctrl+"; break;
          case (int)Keys.RControlKey: ctrlMod += "rctrl+"; break;

          // function keys first - modifier ??
          case (int)Keys.F1: key += "f1+"; break;
          case (int)Keys.F2: key += "f2+"; break;
          case (int)Keys.F3: key += "f3+"; break;
          case (int)Keys.F4: key += "f4+"; break;
          case (int)Keys.F5: key += "f5+"; break;
          case (int)Keys.F6: key += "f6+"; break;
          case (int)Keys.F7: key += "f7+"; break;
          case (int)Keys.F8: key += "f8+"; break;
          case (int)Keys.F9: key += "f9+"; break;
          case (int)Keys.F10: key += "f10+"; break;
          case (int)Keys.F11: key += "f11+"; break;
          case (int)Keys.F12: key += "f12+"; break;
          case (int)Keys.F13: key += "f13+"; break;
          case (int)Keys.F14: key += "f14+"; break;
          case (int)Keys.F15: key += "f15+"; break;

          // all keys where the Keys name does not match the SC name
          // Numpad
          case (int)Keys.NumLock: key += "numlock+"; break;
          case (int)Keys.Divide: key += "np_divide+"; break;
          case (int)Keys.Multiply: key += "np_multiply+"; break;
          case (int)Keys.Subtract: key += "np_subtract+"; break;
          case (int)Keys.Add: key += "np_add+"; break;
          case (int)Keys.Decimal: key += "np_period+"; break;
          //case (int)Keys.NumPadEnter: key += "np_enter+"; break;
          case (int)Keys.NumPad0: key += "np_0+"; break;
          case (int)Keys.NumPad1: key += "np_1+"; break;
          case (int)Keys.NumPad2: key += "np_2+"; break;
          case (int)Keys.NumPad3: key += "np_3+"; break;
          case (int)Keys.NumPad4: key += "np_4+"; break;
          case (int)Keys.NumPad5: key += "np_5+"; break;
          case (int)Keys.NumPad6: key += "np_6+"; break;
          case (int)Keys.NumPad7: key += "np_7+"; break;
          case (int)Keys.NumPad8: key += "np_8+"; break;
          case (int)Keys.NumPad9: key += "np_9+"; break;
          // Digits
          case (int)Keys.D0: key += "0+"; break;
          case (int)Keys.D1: key += "1+"; break;
          case (int)Keys.D2: key += "2+"; break;
          case (int)Keys.D3: key += "3+"; break;
          case (int)Keys.D4: key += "4+"; break;
          case (int)Keys.D5: key += "5+"; break;
          case (int)Keys.D6: key += "6+"; break;
          case (int)Keys.D7: key += "7+"; break;
          case (int)Keys.D8: key += "8+"; break;
          case (int)Keys.D9: key += "9+"; break;
          // navigation
          case (int)Keys.Insert: key += "insert+"; break;
          case (int)Keys.Home: key += "home+"; break;
          case (int)Keys.Delete: key += "delete+"; break;
          case (int)Keys.End: key += "end+"; break;
          case (int)Keys.PageUp: key += "pgup+"; break;
          case (int)Keys.PageDown: key += "pgdown+"; break;
          case (int)Keys.PrintScreen: key += "print+"; break;
          case (int)Keys.Scroll: key += "scrolllock+"; break;
          case (int)Keys.Pause: key += "pause+"; break;
          // Arrows
          case (int)Keys.Up: key += "up+"; break;
          case (int)Keys.Down: key += "down+"; break;
          case (int)Keys.Left: key += "left+"; break;
          case (int)Keys.Right: key += "right+"; break;
          // non letters
          case (int)Keys.Escape: key += "escape+"; break;
          case (int)Keys.OemMinus: key += "minus+"; break;
          //case (int)Keys.equals: key += "equals+"; break;
          //case (int)Keys.grave: key += ""; break; // "grave+"; break; // reserved for Console 
          //case (int)Keys.underline: key += "underline+"; break;
          case (int)Keys.Back: key += "backspace+"; break;
          case (int)Keys.Tab: key += "tab+"; break;
          case (int)Keys.OemOpenBrackets: key += "lbracket+"; break;
          case (int)Keys.OemCloseBrackets: key += "rbracket+"; break;
          case (int)Keys.Return: key += "enter+"; break;
          case (int)Keys.CapsLock: key += "capslock+"; break;
          //case (int)Keys.: key += "colon+"; break;
          case (int)Keys.OemBackslash: key += "backslash+"; break;
          //case (int)Keys.comma: key += "comma+"; break;
          case (int)Keys.OemPeriod: key += "period+"; break;
          //case (int)Keys.slash: key += "slash+"; break;
          case (int)Keys.Space: key += "space+"; break;
          case (int)Keys.OemSemicolon: key += "semicolon+"; break;
          case (int)Keys.OemQuotes: key += "apostrophe+"; break;

          // all where the lowercase DX name matches the SC name
          default:
            if ( ( (int)k >= (int)Keys.A ) && ( (int)k <= (int)Keys.Z ) )
              key += k.ToString( ).ToLowerInvariant( ) + "+"; // ranges are based on the enum values...
            else { } // no other ones handled
            break;
        }

      }//for
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
        if ( !key.Contains( ClearMods ) ) key = altMod + shiftMod + ctrlMod;
      }

      return key.TrimEnd( new char[] { '+' } );  // return killing the last +
    }


    /// <summary>
    /// Converts from SC command to DX command
    /// </summary>
    /// <param name="scKey">A single SC game keyname</param>
    /// <returns>The DX Code of this key</returns>
    static public DXKey FromSCKeyboardCmd( string scKey )
    {
      switch ( scKey ) {
        // handle modifiers first
        case "lalt":  return DXKey.LeftAlt;
        case "ralt": return DXKey.RightAlt;
        case "lshift": return DXKey.LeftShift;
        case "rshift": return DXKey.RightShift;
        case "lctrl":return DXKey.LeftControl;
        case "rctrl": return DXKey.RightControl;

        // function keys first 
        case "f1": return DXKey.F1;
        case "f2": return DXKey.F2;
        case "f3": return DXKey.F3;
        case "f4": return DXKey.F4;
        case "f5": return DXKey.F5;
        case "f6": return DXKey.F6;
        case "f7": return DXKey.F7;
        case "f8": return DXKey.F8;
        case "f9": return DXKey.F9;
        case "f10": return DXKey.F10;
        case "f11": return DXKey.F11;
        case "f12": return DXKey.F12;
        case "f13": return DXKey.F13;
        case "f14": return DXKey.F14;
        case "f15": return DXKey.F15;

        // all keys where the DX name does not match the SC name
        // Numpad
        case "numlock": return DXKey.NumberLock;
        case "np_divide": return DXKey.Divide;
        case "np_multiply": return DXKey.Multiply;
        case "np_subtract": return DXKey.Subtract;
        case "np_add": return DXKey.Add;
        case "np_period": return DXKey.Decimal;
        case "np_enter": return DXKey.NumberPadEnter;
        case "np_0": return DXKey.NumberPad0;
        case "np_1": return DXKey.NumberPad1;
        case "np_2": return DXKey.NumberPad2;
        case "np_3": return DXKey.NumberPad3;
        case "np_4": return DXKey.NumberPad4;
        case "np_5": return DXKey.NumberPad5;
        case "np_6": return DXKey.NumberPad6;
        case "np_7": return DXKey.NumberPad7;
        case "np_8": return DXKey.NumberPad8;
        case "np_9": return DXKey.NumberPad9;
        // Digits
        case "0": return DXKey.D0;
        case "1": return DXKey.D1;
        case "2": return DXKey.D2;
        case "3": return DXKey.D3;
        case "4": return DXKey.D4;
        case "5": return DXKey.D5;
        case "6": return DXKey.D6;
        case "7": return DXKey.D7;
        case "8": return DXKey.D8;
        case "9": return DXKey.D9;
        // navigation
        case "insert": return DXKey.Insert;
        case "home": return DXKey.Home;
        case "delete": return DXKey.Delete;
        case "end": return DXKey.End;
        case "pgup": return DXKey.PageUp;
        case "pgdown": return DXKey.PageDown;
        case "print": return DXKey.PrintScreen;
        case "scrolllock": return DXKey.ScrollLock;
        case "pause": return DXKey.Pause;
        // Arrows
        case "up": return DXKey.Up;
        case "down": return DXKey.Down;
        case "left": return DXKey.Left;
        case "right": return DXKey.Right;
        // non letters
        case "escape": return DXKey.Escape;
        case "minus": return DXKey.Minus;
        case "equals": return DXKey.Equals;
        case "grave": return DXKey.Grave;
        case "underline": return DXKey.Underline;
        case "backspace": return DXKey.Back;
        case "tab": return DXKey.Tab;
        case "lbracket": return DXKey.LeftBracket;
        case "rbracket": return DXKey.RightBracket;
        case "enter": return DXKey.Return;
        case "capslock": return DXKey.Capital;
        case "colon": return DXKey.Colon;
        case "backslash": return DXKey.Backslash;
        case "comma": return DXKey.Comma;
        case "period": return DXKey.Period;
        case "slash": return DXKey.Slash;
        case "space": return DXKey.Space;
        case "semicolon": return DXKey.Semicolon;
        case "apostrophe": return DXKey.Apostrophe;

        // all where the lowercase DX name matches the SC name
        default:
          if ( string.IsNullOrEmpty( scKey ) ) return DXKey.Unknown;

          string letter = scKey.ToUpperInvariant( );
          if (Enum.TryParse( letter, out DXKey dxKey ) ) {
            return dxKey;
          }
          else {
            return DXKey.Unknown;
          }
      }

    }

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
