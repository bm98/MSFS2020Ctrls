using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MSFS2020Ctrls.MSFS
{
  class Controller
  {

    // create a stream from a string
    private static Stream StreamFromString( string s )
    {
      var stream = new MemoryStream( );
      var writer = new StreamWriter( stream );
      writer.Write( s );
      writer.Flush( );
      stream.Position = 0;
      return stream;
    }

    public static DeviceEntry FromXmlStream( Stream inStream )
    {
      XmlReader reader = XmlReader.Create(inStream,
        new XmlReaderSettings( ){
          ConformanceLevel = ConformanceLevel.Fragment,
          IgnoreComments = true,
          IgnoreWhitespace = true,
          CloseInput = true,
          ValidationType = ValidationType.None
        });

      // Deserialize the data and read it from the instance.
      try {
        DeviceEntry device = DeviceEntry.ReadXML( reader );
        return device;
      }
      catch ( Exception e ) {
        Console.WriteLine( $"FromXmlStream: Ex: {e}" );
      }
      return null;
    }

    public static DeviceEntry FromXmlFile( string filename )
    {
      if ( !File.Exists( filename ) ) return null;

      DeviceEntry device;
      using ( var ts = new FileStream( filename, FileMode.Open, FileAccess.Read ) ) {
        device = FromXmlStream( ts );
      }

      return device;
    }


  }
}
