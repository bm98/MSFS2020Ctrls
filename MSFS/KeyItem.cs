using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MSFS2020Ctrls.MSFS
{
  class KeyItem
  {

    public string Control { get; private set; }
    public string  KeyCode { get; private set; }


    public static List<KeyItem> ReadXML( XmlReader reader )
    {
      List<KeyItem> keyItems = new List<KeyItem>();
      KeyItem keyItem=null;
      reader.ReadToFollowing( "KEY" ); // next element
      do {
        switch ( reader.NodeType ) {
          case XmlNodeType.Element:
            if ( reader.Name == "KEY" ) {
              Console.WriteLine( "Found the element: KEY" );
              keyItem = new KeyItem( );
              keyItem.Control = reader.GetAttribute( "Information" );
              keyItem.KeyCode = reader.ReadElementContentAsString( ); // moved to end
            }
            break;

          case XmlNodeType.EndElement:
            ; // DEBUG stop
            break;

          default: break;
        }
        // check the outcome
        if ( keyItem != null ) {
          keyItems.Add( keyItem );
          keyItem = null;
        }

        if ( (reader.NodeType== XmlNodeType.EndElement) && 
          (reader.Name=="Primary" || reader.Name == "Secondary" ) ) {
          return keyItems;
        }
      } while ( true );

    }

  }
}
