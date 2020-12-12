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
  class ActionMap
  {
    /*
   	 <Context ContextName="PLANE">
     ... 
     </Context>
     */

    // Attributes
    public string ActionMapName { get; private set; }

    public int NumberOfActions { get; private set; } = 0;

    public List<ActionEntry> Actions { get; private set; } = new List<ActionEntry>( );

    public static List<ActionMap> ReadXML( XmlReader reader )
    {
      List<ActionMap> actionMaps = new List<ActionMap>();
      ActionMap aMap=null;
      reader.ReadToFollowing( "Context" ); // next element
      do {
        switch ( reader.NodeType ) {
          case XmlNodeType.Element:
            if ( reader.Name == "Context" ) {
              aMap = new ActionMap( );
              aMap.ActionMapName = reader.GetAttribute( "ContextName" );
              Console.WriteLine( $"\tFound the element: Context as {aMap.ActionMapName}" );

              aMap.Actions.AddRange( ActionEntry.ReadXML( reader ) ); // get all actions of this Context
            }
            break;

          case XmlNodeType.EndElement:
            ; // DEBUG stop
            break;

          default: break;
        }
        // check the outcome
        if ( aMap != null ) {
          aMap.NumberOfActions = aMap.Actions.Count;
          actionMaps.Add( aMap );
          aMap = null;
        }
        if ( ( reader.NodeType == XmlNodeType.EndElement ) && ( reader.Name == "Device" ) ) {
          return actionMaps;
        }
        if ( !reader.ReadToNextSibling( "Context" ) ) {
          ;
        }
      } while ( true );
    }

    /// <summary>
    /// Create an ActionItemList from the contained Device data
    /// </summary>
    /// <returns></returns>
    public void GetForLayout( string devName, string devGuid, string inputType, ActionItemList alist )
    {
      foreach ( var act in this.Actions ) {
        act.GetForLayout( devName, devGuid, inputType, ActionMapName, alist );
      }
    }

    public void AsRTF( RTFformatter rtf )
    {
      rtf.RBold = true; rtf.RColor = RTFformatter.ERColor.ERC_Blue;
      string ctGen = $"CONTEXT_{ActionMapName}"; // Tx needs CONTEXT_ before..
      string ct = MSFS.LangPak.Instance.LangItem(ctGen );
      if ( ct == ctGen ) ct = ActionMapName;  // did not change - seems there is no translation
      rtf.WriteLnUC( ct );
      rtf.RBold = false; rtf.RColor = RTFformatter.ERColor.ERC_Black;

      foreach ( var action in Actions ) {
        action.AsRTF( rtf );
      }
      rtf.WriteLn( );



    }

  }
}
