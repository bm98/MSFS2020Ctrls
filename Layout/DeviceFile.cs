using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MSFS2020Ctrls.Layout
{
  // The Json file for a controller

  /*
   {
      "MapName" : "T.16000M Joystick (right)",
      "MapImage" : "T16000M.jpg",
      "InputDevices" :[
      { 
        "InputType": "J1",
        "FontFamily": "Tahoma",
        "DeviceName": "T16000M",
        "DeviceProdGuid": ["{B10A044F-0000-0000-0000-504944564944}"],
         "Controls": [
               { "Input": "B1", "Type": "Digital", "X": 2044, "Y": 604, "Width": 642, "Height": 108, "Cmt": "Primary trigger" },
               ...
        ]
      }
      ]
    }

  */

  /// <summary>
  /// The Device Mapping File
  /// </summary>
  [DataContract]
  class DeviceFile
  {
    [DataMember( Name = "MapName", IsRequired = true )]
    public string MapName { get; set; }

    [DataMember( Name = "MapImage", IsRequired = true )]
    public string MapImage { get; set; } // The map Image

    [DataMember( Name = "InputDevices" )]
    public List<Device> InputDevices { get; set; } = new List<Device>( );

    // NON JSON

    /// <summary>
    /// Returns a list of all PidVid found in the DeviceFile
    /// </summary>
    public List<string> SupportedPidVid {
      get {
        List<string> ret = new List<string>();
        foreach ( var id in InputDevices ) {
          ret.AddRange( id.DevicePIDVID );
        }
        return ret;
      }
    }

    /// <summary>
    /// Create all possible ShapeItems for this Mapping File
    /// </summary>
    public void CreateShapes( )
    {
      for ( int i = 0; i < InputDevices.Count; i++ ) {
        InputDevices[i].CreateShapes( );
      }
    }

    /// <summary>
    /// Create all possible KeyLabels when the controls requires it
    /// </summary>
    /// <param name="dList">A Display List</param>
    public void LoadKeyLabels( DisplayList dList )
    {
      for ( int i = 0; i < InputDevices.Count; i++ ) {
        InputDevices[i].LoadKeyLabels( dList );
      }
    }

    /// <summary>
    /// Find a Control entry with the given product guid and input command
    /// </summary>
    /// <param name="pidVid">the Device Prduct PID VID string in lowecase</param>
    /// <param name="input">the Main input command without modifiers</param>
    /// <param name="firstInstance">If true it looks for the InputTypeNumber 1 (InputType="x1") else for the next</param>
    /// <returns>The found Control or Null</returns>
    public Control FindItem( string pidVid, string input, bool firstInstance )
    {
      if ( string.IsNullOrEmpty( pidVid ) ) return null;
      if ( string.IsNullOrEmpty( input ) ) return null;

      for ( int i = 0; i < InputDevices.Count; i++ ) {
        if ( InputDevices[i].DevicePIDVID.Contains( pidVid ) ) { // can have multiple PID VIDs for a device (alternates)
          // returns if we are asked for the first instance and it is the first one (default)
          // Use numbers only when there are more than one device with the same GUID in a file !!! (or 0)
          if ( firstInstance && ( InputDevices[i].InputTypeNumber <= 1 ) ) {
            return InputDevices[i].FindItem( input );
          }
          else if ( !firstInstance && ( InputDevices[i].InputTypeNumber > 1 || InputDevices[i].InputTypeNumber == 0 ) ) {
            return InputDevices[i].FindItem( input ); // not first and dev2+ or dev0.. - return any other (more than 2 not supported)
          }
        }
      }
      return null;
    }

    /// <summary>
    /// Get all devices with Name (only first GUID is returned)
    /// </summary>
    /// <returns></returns>
    public List<Device.DeviceDescriptor> Devices( )
    {
      var ret = new List<Device.DeviceDescriptor>( );
      for ( int i = 0; i < InputDevices.Count; i++ ) {
        if ( !ret.Exists( x => x.DevGuid == InputDevices[i].DevDescriptor.DevGuid ) )
          ret.Add( InputDevices[i].DevDescriptor );
      }
      return ret;
    }
  }

  /// <summary>
  /// One Game Device 
  /// </summary>
  [DataContract]
  class Device
  {
    [DataMember( Name = "InputType", IsRequired = true )]
    public string InputType { get; set; } // J[n], G[n], K[n], M[n], X

    [DataMember( Name = "FontFamily", IsRequired = false )]
    public string FontFamily { get; set; } // any valid FontFamily

    [DataMember( Name = "DeviceName", IsRequired = true )]
    public string DeviceName { get; set; } // The device name

    [DataMember( Name = "DeviceProdGuid", IsRequired = true )]
    private List<string> DeviceProdGuid_ { get; set; } = new List<string>( ); // The device product GUIDs as read by DirectInput

    [DataMember( Name = "Controls" )]
    public List<Control> Controls { get; set; } = new List<Control>( );// The list of Controls supported (see below)

    // non Json

    public bool IsInputType_Joystick => InputTypeLetter == "J";
    public bool IsInputType_Gamepad => InputTypeLetter == "G";
    public bool IsInputType_Mouse => InputTypeLetter == "M";
    public bool IsInputType_Kbd => InputTypeLetter == "K";
    public bool IsInputType_Kbd_Keys => InputTypeLetter == "X";


    private Font m_deviceFont = null;
    /// <summary>
    /// Get the base font for this device
    /// </summary>
    public Font DeviceFont {
      get {
        if ( !string.IsNullOrEmpty( FontFamily ) ) {
          return m_deviceFont;
        }
        return MapProps.MapFont; // no specified - get the default one
      }
    }

    /// <summary>
    /// Describes a known device
    /// Used for the Layout Debug Mode
    /// </summary>
    public class DeviceDescriptor
    {
      public string DevGuid { get; set; } = "";
      public string DevName { get; set; } = "";
      public override string ToString( )
      {
        return DevName;
      }
    }

    public DeviceDescriptor DevDescriptor {
      get {
        return new DeviceDescriptor( ) { DevGuid = DeviceProdGuid_[0].ToLowerInvariant( ), DevName = DeviceName };
      }
    }


    public string InputTypeLetter { get => InputType.Substring( 0, 1 ); }
    public int InputTypeNumber {
      get {
        if ( InputType.Length > 1 ) {
          if ( int.TryParse( InputType.Substring( 1 ), out int num ) ) {
            return num;
          }
        }
        return 0; //default
      }
    }

    /// <summary>
    /// returns the PID VID part of the GUID (seems how this is composed in Win)
    /// </summary>
    public List<string> DevicePIDVID {
      get {
        var ret = new List<string>( );
        foreach ( string s in DeviceProdGuid_ ) {
          if ( s.Length >= 9 ) {
            string pv = s.Substring( 1, 8 ).ToLowerInvariant( ); // this is "{12345678-0000-0000 etc}
            ret.Add( pv );
          }
        }
        return ret;
      }
    }


    // this one tracks the returned KbdItems - must be reset when Shapes are newly created
    private int m_kbdItemTracker = 0;
    /// <summary>
    /// Find a Control entry with the given input command
    /// For Keyboards with drawn Keys there is not an entry for every possible key - so return just the next one
    /// </summary>
    /// <param name="input">the Item (device property)</param>
    /// <returns>The found Control or Null</returns>
    public Control FindItem( string input )
    {
      // we may find it already here
      for ( int i = 0; i < Controls.Count; i++ ) {
        if ( input == Controls[i].Input ) {
          return Controls[i];
        }
      }
      // if not - and Keyboard with drawn Keys - assign a new one and tag it
      if ( this.IsInputType_Kbd_Keys ) {
        if ( Controls.Count > m_kbdItemTracker ) {
          int item = m_kbdItemTracker++;
          Controls[item].Input = input; // mark to reuse
          return Controls[item];
        }
      }
      return null;
    }


    /// <summary>
    /// Create all possible ShapeItems for this Device
    /// </summary>
    public void CreateShapes( )
    {
      m_kbdItemTracker = 0; // reset 
      if ( m_deviceFont != null ) m_deviceFont.Dispose( );
      if ( !string.IsNullOrEmpty( FontFamily ) ) {
        m_deviceFont = new Font( FontFamily, MapProps.FontSize ); // create actual Font here
      }
      for ( int i = 0; i < Controls.Count; i++ ) {
        Controls[i].CreateShapes( DeviceFont, this.IsInputType_Kbd_Keys ); // symbols only for X type maps (keyboard with Symbols)
      }
    }
    /// <summary>
    /// Create all possible KeyLabels when the control requires it
    /// </summary>
    /// <param name="dList"></param>
    public void LoadKeyLabels( DisplayList dList )
    {
      for ( int i = 0; i < Controls.Count; i++ ) {
        Controls[i].LoadKeyLabels( dList, this.IsInputType_Kbd_Keys ); // symbols only for X type maps (keyboard with Symbols)
      }
    }

  }

  /// <summary>
  /// One Device Input (command)
  /// </summary>
  [DataContract]
  class Control
  {
    [DataMember( Name = "Input", IsRequired = true )]
    public string Input { get; set; } = ""; // buttonN, hatN_up,_right,_down,_left, [rot]xyz, sliderN (CryInput notification)

    [DataMember( Name = "Type" )]
    public string Type { get; set; } = "";  // "" or Analog or Digital or Key

    [DataMember( Name = "X", IsRequired = true )]
    public int X { get; set; } = 0;         // X label pos (left=0)

    [DataMember( Name = "Y", IsRequired = true )]
    public int Y { get; set; } = 0;         // Y label pos (top=0)

    [DataMember( Name = "Width", IsRequired = true )]
    public int Width { get; set; } = 600;   // Label field width

    [DataMember( Name = "Height" )]
    public int Height { get; set; } = 54;   // Label field height

    [DataMember( Name = "Cmt" )]
    public string Cmt { get; set; } = "";   // Comment


    // non Json

    /// <summary>
    /// A queue with all available text fields
    ///  Take one and Insert the Display text and then add it to the DisplayList
    ///  If exhausted - well bad luck..
    /// </summary>
    public Queue<ShapeItem> ShapeItems = null;

    // Base layout values to get enough fields and still have readable text
    private const float c_baseFontSize = 16F;

    /// <summary>
    /// Create all possible ShapeItems for this Control
    /// </summary>
    public void CreateShapes( Font deviceFontRef, bool useSymbol )
    {
      // define the number of Actionlabels to fit into the Control rectangle

      // this is a bit messy...
      // have to allocate a number of Rectangles to draw into but the layout rects are very different in size..
      this.ShapeItems = new Queue<ShapeItem>( ); // get rid of previous ones
      // create a reference font 
      int baseHeight = MapProps.MapFont.Height;
      int baseWidth = MapProps.MapFont.Height * 12; // Lets see if this is a good Width or needs adjustment

      // live values from base
      int nCols = Width / baseWidth;
      if ( nCols == 0 ) {
        nCols = 1; // at least one column..
      }
      baseWidth = (int)Math.Floor( (double)Width / nCols ); // fill rectangle
      int nLines = Height / baseHeight;
      if ( nLines == 0 ) {
        nLines = 1; // at least one line..
      }
      baseHeight = (int)Math.Floor( (double)Height / nLines ); // fill rectangle

      // create as many shapes to hold action labels for this control
      bool symbol = useSymbol;
      // create one to draw the whole key
      if ( this.Type == "Key" && useSymbol ) {
        // Type Keyboard with keys drawn
        var sh = new ShapeKey {
          X = this.X,
          Y = this.Y,
          Width = this.Width,
          Height = this.Height/2,
          IsSymbolShape = true,
          TextFontRef = MapProps.KbdSymbolFont,
        };
        ShapeItems.Enqueue( sh ); // will only be drawn when used
      }

      for ( int l = 0; l < nLines; l++ ) {
        for ( int c = 0; c < nCols; c++ ) {

          if ( this.Type == "KeyS" ) {
            // Type Keyboard with keys drawn
            var sh = new ShapeKey {
              X = X + c * baseWidth,
              Y = Y + l * baseHeight,
              Width = baseWidth,
              Height = baseHeight,
              IsSymbolShape = false,
              TextFontRef = deviceFontRef
            };
            symbol = false; // only once
            ShapeItems.Enqueue( sh );
          }
          else {
            // other types - create labels only
            var sh = new ShapeItem {
              X = X + c * baseWidth,
              Y = Y + l * baseHeight,
              Width = baseWidth,
              Height = baseHeight,
              TextFontRef = deviceFontRef
            };
            ShapeItems.Enqueue( sh );
          }
        }
      }
    }

    /// <summary>
    /// Load KeyLabels if required by the control and mode (not if using symbols though)
    /// </summary>
    /// <param name="dList">A display list to add them</param>
    /// <param name="useSymbol">Wether or not usin Key Symbols rather than a drawn map</param>
    public void LoadKeyLabels( DisplayList dList, bool useSymbol )
    {
      if ( this.Type == "Key" && !useSymbol ) {
        // Type Keyboard with keys drawn
        var sh = new ShapeKey {
          X = this.X,
          Y = this.Y,
          Width = this.Width,
          Height = this.Height/2,
          IsLabelShape = true,
          DispText="DUMMY",
          SCGameKey = this.Input,
        };
        dList.Add( sh );
      }

    }
  }
}
