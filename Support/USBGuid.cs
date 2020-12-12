using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS2020Ctrls.Support
{
  /// <summary>
  /// Support looking up the PID & VID of an USB device
  /// </summary>
  class USBGuid
  {
    private const string DI_Path = @"HKEY_CURRENT_USER\System\CurrentControlSet\Control\MediaProperties\PrivateProperties\DirectInput";

    /// <summary>
    /// Takes an instance GUID of an USB HID device and tries to find the product USB VID_PID
    /// </summary>
    /// <param name="instanceGuid">An instance GUID to find</param>
    /// <returns>pidvid as ppppvvvv  string</returns>
    public static string GetPidVidFromInstance( string instanceGuid )
    {
      var iGuid = instanceGuid.Replace("{","").Replace("}","").ToLowerInvariant(); // just in case... to match the Guid.ToString() format

      // HKEY_CURRENT_USER\System\CurrentControlSet\Control\MediaProperties\PrivateProperties\DirectInput
      // VID_xxxx&PID_cccc\Calibration\N (0..)
      // GUID as binary data => Instance GUID

      // (string)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\81bfc699-f883-50c7-b674-2483b6baae23", "InstallLocation", null );

      string retPidVid = iGuid.Substring(0, 8); // default reply is the first 8 chars of the input GUID
      // we bail out on any exception
      try {
        // Create a RegistryKey, which will access the HKEY_USERS
        // key in the registry of this machine.
        using ( var rk = Registry.CurrentUser ) {
          using ( var rDI = rk.OpenSubKey( @"System\CurrentControlSet\Control\MediaProperties\PrivateProperties\DirectInput" ) ) {
            // Retrieve all the subkeys for the specified key.
            string [] names = rDI?.GetSubKeyNames();
            if ( names == null ) return retPidVid; // ERROR does not find anything here...

            // we should have all VIDPIDs used in DirectInput
            foreach ( var vpid in names ) {
              if ( !vpid.StartsWith( "VID_" ) ) continue; // not a VID_ entry
              // dive down
              using ( var rCal = rDI.OpenSubKey( $"{vpid}\\Calibration" ) ) {
                string[] calEntries = rCal?.GetSubKeyNames();
                if ( calEntries == null ) continue; // no cal entries
                // there should be a number of entries here 0,1,... else we skip
                foreach ( var calEntry in calEntries ) {
                  using ( var node = rCal.OpenSubKey( calEntry ) ) {
                    // now we should find the GUID value as binary data
                    byte[] guidBin = (byte[])node.GetValue("GUID", new byte[]{ } ); // gets an empty array if nothing else is found
                    if ( guidBin.Length == 16 ) {
                      var foundGuid = new Guid(guidBin);
                      if ( foundGuid.ToString( ) == iGuid ) {
                        // vpid is now VID_xxxx&PID_cccc
                        // should return PIDVID as lowercase....
                        retPidVid = ( vpid.Substring( 13, 4 ) + vpid.Substring( 4, 4 ) ).ToLowerInvariant( );
                        return retPidVid;
                      }
                    }
                  }
                }

              }
            }
          }
        }
      }
      catch ( Exception e ) {
        Console.WriteLine( "USBGuid: " + e.ToString( ) );
      }
      return retPidVid; // empty one
    }


  }
}
