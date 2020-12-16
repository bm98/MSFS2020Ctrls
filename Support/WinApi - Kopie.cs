using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static MSFS2020Ctrls.Support.KeyboardCls;

namespace MSFS2020Ctrls.Support
{
  static class WinApiOLD
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
    public static extern uint MapVirtualKeyEx( VirtualKey uCode, VirtualKeyMapType uMapType, IntPtr dwhkl );



    public static string FSimCodeToVK( uint uCode )
    {
      string key = ( (System.Windows.Forms.Keys)uCode ).ToString( );
      if ( key.StartsWith( "Oem" ) ) {
        key = GetCharFromKey( uCode ); // try this ...
      }
      return key;
    }

      /// <summary>
      /// The above does not work great - cannot get Navigation buttons, returns Numpad as Navig. e.g. NumLock OFF (seems to be stuck with the OLD DOS PC Keyboard layout...)
      /// We only need the Alpha Keyboard to be translated by the MS routine..
      /// </summary>
      /// <param name="uCode">The ScanCode</param>
      /// <param name="uMapType"></param>
      /// <param name="dwhkl"></param>
      /// <returns></returns>
      public static string KbdScanCodeToVK( uint uCode )
    {
      switch ( uCode ) {
        // handle modifiers first
        case (uint)DXKey.LeftAlt: return "LAlt";
        case (uint)DXKey.RightAlt: return "RAlt";
        case (uint)DXKey.LeftShift: return "LShift";
        case (uint)DXKey.RightShift: return "RShift";
        case (uint)DXKey.LeftControl: return "LCtrl";
        case (uint)DXKey.RightControl: return "RCtrl";

        // all keys where the DX name does not match the SC name
        // Numpad
        case (uint)DXKey.NumberLock: return "N.Lck";
        case (uint)DXKey.Divide: return "NP /";
        case (uint)DXKey.Multiply: return "NP *";
        case (uint)DXKey.Subtract: return "NP -";
        case (uint)DXKey.Add: return "NP +";
        case (uint)DXKey.Decimal: return "NP .";
        case (uint)DXKey.NumberPadEnter: return "NP ←┘";
        case (uint)DXKey.NumberPad0: return "NP 0";
        case (uint)DXKey.NumberPad1: return "NP 1";
        case (uint)DXKey.NumberPad2: return "NP 2";
        case (uint)DXKey.NumberPad3: return "NP 3";
        case (uint)DXKey.NumberPad4: return "NP 4";
        case (uint)DXKey.NumberPad5: return "NP 5";
        case (uint)DXKey.NumberPad6: return "NP 6";
        case (uint)DXKey.NumberPad7: return "NP 7";
        case (uint)DXKey.NumberPad8: return "NP 8";
        case (uint)DXKey.NumberPad9: return "NP 9";
        // Digits
        case (uint)DXKey.D0: return "0";
        case (uint)DXKey.D1: return "1";
        case (uint)DXKey.D2: return "2";
        case (uint)DXKey.D3: return "3";
        case (uint)DXKey.D4: return "4";
        case (uint)DXKey.D5: return "5";
        case (uint)DXKey.D6: return "6";
        case (uint)DXKey.D7: return "7";
        case (uint)DXKey.D8: return "8";
        case (uint)DXKey.D9: return "9";
        // navigation
        case (uint)DXKey.Insert: return "Ins";
        case (uint)DXKey.Home: return "⸠◄9";
        case (uint)DXKey.Delete: return "Del";
        case (uint)DXKey.End: return "►⸡";
        case (uint)DXKey.PageUp: return "Pg▲";
        case (uint)DXKey.PageDown: return "Pg▼";
        case (uint)DXKey.PrintScreen: return "PrtScr";
        case (uint)DXKey.ScrollLock: return "ScrlLck";
        case (uint)DXKey.Pause: return "Pause";
        // Arrows
        case (uint)DXKey.Up: return "↑";
        case (uint)DXKey.Down: return "↓";
        case (uint)DXKey.Left: return "←";
        case (uint)DXKey.Right: return "→";
        // non letters
        case (uint)DXKey.Period: return ".";
        case (uint)DXKey.Minus: return "-";
        case (uint)DXKey.Equals: return "=";
        case (uint)DXKey.Return: return "←┘";
        /*
        case (uint)Key.Grave: return "^";
        case (uint)Key.Escape: return "→";
        case (uint)Key.Underline: key += "underline+"; break;
        case (uint)Key.Back: key += "backspace+"; break;
        case (uint)Key.Tab: key += "tab+"; break;
        case (uint)Key.LeftBracket: key += "lbracket+"; break;
        case (uint)Key.RightBracket: key += "rbracket+"; break;
        case (uint)Key.Capital: key += "capslock+"; break;
        case (uint)Key.Colon: key += "colon+"; break;
        case (uint)Key.Backslash: key += "backslash+"; break;
        case (uint)Key.Comma: key += "comma+"; break;
        case (uint)Key.Slash: key += "slash+"; break;
        case (uint)Key.Space: key += "space+"; break;
        case (uint)Key.Semicolon: key += "semicolon+"; break;
        case (uint)Key.Apostrophe: key += "apostrophe+"; break;
        */
        // all where the lowercase DX name matches the SC name
        default:
          uint vKeyCode = MapVirtualKeyEx( uCode, VirtualKeyMapType.MAPVK_VSC_TO_VK_EX, IntPtr.Zero );
          string key = ( (System.Windows.Forms.Keys)vKeyCode ).ToString( );
          if ( key.StartsWith( "Oem" ) ) {
            key = GetCharFromKey( vKeyCode ); // try this ...
          }
          return key;
      }
    }


    [DllImport( LibraryName, ExactSpelling = true )]
    public static extern uint ToAscii( uint uVirtKey, uint uScanCode, [MarshalAs( UnmanagedType.LPArray, SizeConst = 256 )] byte[] lpbKeyState, out uint lpwTransKey, uint uFlags );

    [DllImport( LibraryName, ExactSpelling = true )]
    public static extern uint ToUnicode( uint wVirtKey, uint wScanCode, [MarshalAs( UnmanagedType.LPArray, SizeConst = 256 )] byte[] lpbKeyState, out uint pwszBuff, int cchBuff, uint uFlags );

    [DllImport( LibraryName, ExactSpelling = true )]
    public static extern bool GetKeyboardState( [MarshalAs( UnmanagedType.LPArray, SizeConst = 256 )] out byte[] lpKeyState );

    [DllImport( LibraryName, ExactSpelling = true )]
    [return: MarshalAs( UnmanagedType.Bool )]
    static extern bool GetKeyboardState( byte[] lpKeyState );

    [DllImport( LibraryName, ExactSpelling = true )]
    public static extern bool GetKeyboardState( IntPtr lpKeyState );

    public static string GetCharFromKey( uint virtKeyCode )
    {
      byte[] keyBoardState = new byte[256];
      if ( GetKeyboardState( keyBoardState ) ) {
        if ( ToAscii( virtKeyCode, 0, keyBoardState, out uint outChar, 0 ) < 0 ) {
          // dead key i.e. not a pressed one
          if ( outChar <= 255 ) {
            return ( (char)outChar ).ToString( );
          }
          else {
            //return Microsoft.VisualBasic.Left(StrConv(ChrW(Out), vbUnicode), 1)
            //return Microsoft.VisualBasic.Left(StrConv(ChrW(Out), VbStrConv.None), 1)
            return ( (char)outChar ).ToString( );
          }

        }
        else {
          return ( (char)outChar ).ToString( );
        }

      }
      else {
        return "";
      }

    }


  }
}
