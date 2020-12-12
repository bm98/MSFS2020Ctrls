using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using MSFS2020Ctrls.Layout;
using MSFS2020Ctrls.Support;

namespace MSFS2020Ctrls.MSFS
{
  class DeviceEntry
  {
    // should enumerate Joysticks
    private static int DevEnum = 1;
    private static int NextEnum( )
    {
      return DevEnum++;
    }
    public static void DeviceReset( )
    {
      DevEnum = 1;
    }


    /*
     <Device DeviceName="Saitek Pro Flight X-55 Rhino Stick" GUID="{6F187BC0-5D28-11E7-8006-444553540000}" ProductID="8725">
     ... 
     </Device>


    joystick: Saitek Pro Flight X-55 Rhino Stick
	 - Product: {22150738-0000-0000-0000-504944564944} - Instance: {6f187bc0-5d28-11e7-8006-444553540000}

     */

    // Attributes
    public string DeviceName { get; private set; }
    public string DeviceProfileName { get; private set; }
    public string DeviceInstanceGuid { get; private set; }
    public string DeviceProductGuid { get; private set; } // how do we get this one ?? Registry ??
    public string DeviceProductID { get; private set; }

    public int NumberOfActionMaps { get; private set; } = 0;
    public int NumberOfActions { get; private set; } = 0;

    public override string ToString( )
    {
      return $"{DeviceProfileName} ({DeviceName})";
    }

    private int m_joyEnum = 0;
    /// <summary>
    ///  // K1, M1, Jn, G1 (keyb, mouse, joystick jsN number 1.., gamepad)
    /// </summary>
    public string InputType {
      get {
        if ( DeviceName.ToLowerInvariant( ) == "keyboard" ) return "K1";
        if ( DeviceName.ToLowerInvariant( ) == "mouse" ) return "M1";
        if ( DeviceInstanceGuid.Length > 0 ) {
          if ( m_joyEnum == 0 ) m_joyEnum = NextEnum( ); // first call assigns the new Number (murks but works...)
          return $"J{m_joyEnum}";
        }
        else return "X0"; // potentially something not known so far...
      }
    }

    // Content
    public List<ActionMap> ActionMaps { get; private set; } = new List<ActionMap>( );



    public static DeviceEntry ReadXML( XmlReader reader )
    {
      DeviceEntry device = null;

      device = new DeviceEntry( );
      // <FriendlyName>Mouse profile</FriendlyName>
      reader.Read( );
      reader.ReadToFollowing( "FriendlyName" );
      device.DeviceProfileName = reader.ReadElementContentAsString( );

      if ( reader.Name != "Device" )
        reader.ReadToFollowing( "Device" );

      switch ( reader.NodeType ) {
        case XmlNodeType.Element:
          if ( reader.Name == "Device" ) {
            // synched with content
            device.DeviceName = reader.GetAttribute( "DeviceName" );
            Console.WriteLine( $"Found the element: Device as {device.DeviceName}" );
            // we have to subst such GUIDs to be compatible with the original Layout code FS one gets us 0 guids for mouse and kbd
            if ( device.DeviceName.ToLowerInvariant( ) == "keyboard" ) {
              device.DeviceInstanceGuid = "{00000000-0000-0000-0000-000000000000}";
            }
            else if ( device.DeviceName.ToLowerInvariant( ) == "mouse" ) {
              device.DeviceInstanceGuid = "{10001000-0000-0000-0000-000000000000}";
            }
            else {
              device.DeviceInstanceGuid = reader.GetAttribute( "GUID" );
            }
            // create a dummy one, only the PIDVID part is used the rest not... (more murks..)
            device.DeviceProductGuid = $"{{{Support.USBGuid.GetPidVidFromInstance( device.DeviceInstanceGuid )}-0000-0000-0000-000000000000}}";
            device.DeviceProductID = reader.GetAttribute( "ProductID" );

            device.ActionMaps.AddRange( ActionMap.ReadXML( reader ) );
            device.NumberOfActionMaps = device.ActionMaps.Count;
            device.NumberOfActions += device.ActionMaps.Sum( x => x.NumberOfActions );
            return device; // should be finished here
          }
          break;

      }
      return null;
    }


    /// <summary>
    /// Create an ActionItemList from the contained Device data
    /// </summary>
    /// <returns></returns>
    public void GetForLayout( ActionItemList alist )
    {
      foreach ( var aMap in this.ActionMaps ) {
        aMap.GetForLayout( this.DeviceName, this.DeviceProductGuid, this.InputType, alist );
      }
      return;
    }


    public void AsRTF( RTFformatter rtf )
    {
      rtf.RBold = true; rtf.RColor = RTFformatter.ERColor.ERC_DarkGreen;
      rtf.FontSize( 15 );
      rtf.WriteLn( DeviceProfileName );
      rtf.FontSize( 10 );
      rtf.WriteLn( $"  Device: {DeviceName}" );
      rtf.RBold = false; rtf.RColor = RTFformatter.ERColor.ERC_Black;
      rtf.WriteLn( );

      foreach ( var map in ActionMaps ) {
        map.AsRTF( rtf );
      }
      rtf.WriteLn( );



    }

  }
}
