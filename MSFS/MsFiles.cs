using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSFS2020Ctrls.MSFS
{
  class MsFiles
  {

    public const string m_xmlDir ="";

    private static IList<string> FindControlFiles( )
    {
      var ret = new List<string>();

      var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      // C:\Users\bm\AppData\Local\Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\SystemAppData\wgs
      var FSdir = Path.Combine(appData, @"Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\SystemAppData\wgs");
      // here it is : 000901F2A6ECEA8C_00000000000000000000000069F80140 
      // but this may be different on other systems ???

      // we look for file with names like: 270BFBB03CEF45F687FFD50BC34BC384
      var files = Directory.EnumerateFiles( FSdir, "*.",  SearchOption.AllDirectories );
      foreach (var f in files ) {
        /*
            <?xml version="1.0" encoding="UTF-8"?>
            <Version Num="1409"/>
            <FriendlyName>Saitek Pro Flight X-55 Rhino Stick profile</FriendlyName>
            <Device DeviceName="Saitek Pro Flight X-55 Rhino Stick" GUID="{6F187BC0-5D28-11E7-8006-444553540000}" ProductID="8725">         
        */
        using (var sr = new StreamReader( f ) ) {
          try {
            string buffer = sr.ReadLine();
            if ( buffer != $"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" ) continue; // nope
            // could be one .. 
            buffer = sr.ReadLine();
            if ( !buffer.StartsWith( "<Version Num=" ) ) continue; // nope
            // could be one .. 
            buffer = sr.ReadLine( );
            if ( !buffer.StartsWith( "<FriendlyName>" ) ) continue; // nope
            // could be one .. 
            buffer = sr.ReadLine( );
            if ( !buffer.StartsWith( "<Device" ) ) continue; // nope
            // now we think we have one...
            ret.Add( f );
          }
          catch {
            ; // nope, not even readable
          }
          sr.Close( );
        }
      }
      return ret;
    }


    public static IList<string> GetControlProfiles( )
    {
      return FindControlFiles( );
    }



  }
}
