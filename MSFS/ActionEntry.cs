using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using MSFS2020Ctrls.Layout;
using MSFS2020Ctrls.Support;

namespace MSFS2020Ctrls.MSFS
{
  class ActionEntry
  {
    /*
        <Action ActionName="KEY_BRAKES" Flag="2">     
        ..
        </Action>
     */

    // Attributes
    public string ActionName { get; private set; }
    public string Flag { get; private set; }


    public PrimSecActionBase PrimaryAction { get; private set; }
    public PrimSecActionBase SecondaryAction { get; private set; }

    public static List<ActionEntry> ReadXML( XmlReader reader )
    {
      List<ActionEntry> actions = new List<ActionEntry>();
      ActionEntry action=null;
      reader.ReadToFollowing( "Action" ); // next element
      do {
        switch ( reader.NodeType ) {
          case XmlNodeType.Element:
            if ( reader.Name == "Action" ) {
              action = new ActionEntry( );
              action.ActionName = reader.GetAttribute( "ActionName" );
              if ( action.ActionName== "CAMERA_USER_LOAD_2" ) {
                ;
              }
              Console.WriteLine( $"\t\tFound the element: Action as {action.ActionName}" );
              action.Flag = reader.GetAttribute( "Flag" );
              // need to get forward to the Prim
              if ( reader.ReadToDescendant( "Primary" ) ) {
                action.PrimaryAction = ActionEntryPrimary.ReadXML( reader );
                reader.Read( );

                if ( reader.Name == "Secondary" ) {
                  action.SecondaryAction = ActionEntrySecondary.ReadXML( reader );
                  reader.Read( );
                }
              }
              else {
                if ( reader.ReadToDescendant( "Secondary" ) ) {
                  action.SecondaryAction = ActionEntrySecondary.ReadXML( reader );
                  reader.Read( );
                }
              }
            }
            break;

          default: break;
        }
        // check the outcome
        if ( action != null ) {
          actions.Add( action );
          action = null;
        }
        if ( ( reader.NodeType == XmlNodeType.EndElement ) && ( reader.Name == "Context" ) ) {
          return actions;
        }
        if ( !reader.ReadToNextSibling( "Action" ) ) {
          ;
        }
      } while ( true );
    }


    /// <summary>
    /// Create an ActionItemList from the contained Device data
    /// </summary>
    /// <returns></returns>
    public void GetForLayout( string devName, string devGuid, string inputType, string aMap, ActionItemList alist )
    {
      if ( PrimaryAction != null ) {
        var ai = new ActionItem {
          ActionMap =aMap,
          DeviceName = devName,
          DeviceProdGuid = devGuid,
          InputType = inputType,
          ControlInput = PrimaryAction.SCCode,
          DispText = LangPak.Instance.LangItem(ActionName)
        };
        alist.Add( ai );
      }
      if ( SecondaryAction != null ) {
        var ai = new ActionItem {
          ActionMap =aMap,
          DeviceName = devName,
          DeviceProdGuid = devGuid,
          InputType = inputType,
          ControlInput = SecondaryAction.SCCode,
          DispText = LangPak.Instance.LangItem(ActionName)
        };
        alist.Add( ai );
      }
    }


    public void AsRTF( RTFformatter rtf )
    {
      rtf.RBold = true;
      rtf.WriteTabUC( LangPak.Instance.LangItem( ActionName ) );
      rtf.RBold = false;
      rtf.WriteTab( "" );
      if ( PrimaryAction != null ) {
        rtf.Write( PrimaryAction.SCCode );
      }
      rtf.WriteTab( "" );
      if ( SecondaryAction != null ) {
        rtf.WriteUC( SecondaryAction.SCCode );
      }
      rtf.WriteLn( );
    }

  }
}
