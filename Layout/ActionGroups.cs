using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS2020Ctrls.Layout
{
  /// <summary>
  /// defines the action grouping
  /// </summary>
  class ActionGroups
  {
    /// <summary>
    /// All actionmap groups
    /// </summary>
    public enum EGroup
    {
      Plane = 0,
      CockpitInteractions,
      ATC,
      Cameras,
      Drone,
      Slew,
      InGameUI,
      Modes,
      Menu,
      Multiplayer,
      Others,
      Debug,
    }

    private static Dictionary<EGroup, List<string>> m_actionDict;

    /// <summary>
    /// cTor
    /// </summary>
    static ActionGroups()
    {
      m_actionDict = new Dictionary<EGroup, List<string>>( );


      // Try to read the user file
      //      var lg = LayoutGroupsJson.FromJson( TheUser.LayoutJsonFileName( ) );
      LayoutGroupsJson lg = null;
      if ( lg != null ) {
        /*
        // read from the user file
        m_actionDict.Add( EGroup.SpaceFlight, lg.SpaceFlight );
        m_actionDict.Add( EGroup.SpaceTargeting, lg.SpaceTargeting );
        m_actionDict.Add( EGroup.SpaceMining, lg.SpaceMining );
        m_actionDict.Add( EGroup.SpaceWeapons, lg.SpaceWeapons );
        m_actionDict.Add( EGroup.SpaceDefensive, lg.SpaceDefensive );
        m_actionDict.Add( EGroup.Lights, lg.Lights );
        m_actionDict.Add( EGroup.Player, lg.Player );
        m_actionDict.Add( EGroup.EVA, lg.EVA );
        m_actionDict.Add( EGroup.Vehicle, lg.Vehicle );
        //m_actionDict.Add( EGroup.VehicleWeapons, lg.VehicleWeapons ); // removed in 3.10.0
        m_actionDict.Add( EGroup.Interaction, lg.Interaction );
        m_actionDict.Add( EGroup.Spectator, lg.Spectator );
        m_actionDict.Add( EGroup.Others, lg.Others );
        */
      }
      else {
        // Define which maps belongs to which group
        var x = new List<string>( ) { "PLANE" };
        m_actionDict.Add( EGroup.Plane, x );
        x = new List<string>( ) { "INGAME_UI" };
        m_actionDict.Add( EGroup.InGameUI, x );
        x = new List<string>( ) { "MULTIPLAYER" };
        m_actionDict.Add( EGroup.Multiplayer, x );
        x = new List<string>( ) { "MODES", "MODE_PAUSE" };
        m_actionDict.Add( EGroup.Modes, x );
        x = new List<string>( ) { "COCKPIT_INTERACTIONS", "COCKPIT_GLOBAL_CAMERA", "COCKPIT_CAMERA", "INSTRUMENTS_CAMERA", "COCKPIT_GLOBAL_CAMERA_TRANSLATE" };
        m_actionDict.Add( EGroup.CockpitInteractions, x );
        x = new List<string>( ) { "DRONE" };
        m_actionDict.Add( EGroup.Drone, x );
        x = new List<string>( ) { "SLEW" };
        m_actionDict.Add( EGroup.Slew, x );
        x = new List<string>( ) { "ATC" };
        m_actionDict.Add( EGroup.ATC, x );
        x = new List<string>( ) { "CAMERA", "EXTERNAL_CAMERA", "USER_CAMERA", "FIXED_CAMERA", "SMART_CAMERA", "FSX_CAMERA", "REPLAY" };
        m_actionDict.Add( EGroup.Cameras, x );
        x = new List<string>( ) { "GAME" };
        m_actionDict.Add( EGroup.Menu, x );
        x = new List<string>( ) { "DEBUG", "DEVMODE", "DEVMODECAMERA" };
        m_actionDict.Add( EGroup.Debug, x );
        x = new List<string>( ) { "RTC", "FAILURES", "MISC" };
        m_actionDict.Add( EGroup.Others, x );
      }
    }

    /// <summary>
    /// Return the names of the groups
    /// </summary>
    /// <returns></returns>
    public static List<string> ActionGroupNames()
    {
      return Enum.GetNames( typeof( EGroup ) ).ToList( );
    }

    /// <summary>
    /// Returns the list of actionmaps of a group
    /// </summary>
    /// <param name="eClass"></param>
    /// <returns></returns>
    public static List<string> ActionmapNames( EGroup eClass )
    {
      return m_actionDict[eClass];
    }

    /// <summary>
    /// Returns the group from the actionmap
    /// </summary>
    /// <param name="actionmap"></param>
    /// <returns></returns>
    public static EGroup MapNameToGroup( string actionmap )
    {
      foreach ( var kv in m_actionDict ) {
        if ( kv.Value.Contains( actionmap ) )
          return kv.Key;
      }
      return EGroup.Others;
    }

    /// <summary>
    /// Returns the group from the actiongroup name
    /// </summary>
    /// <param name="actiongroup"></param>
    /// <returns></returns>
    public static EGroup GroupNameToGroup( string actiongroup )
    {
      foreach ( var kv in m_actionDict ) {
        if ( kv.Key.ToString( ) == actiongroup )
          return kv.Key;
      }
      return EGroup.Others;
    }



  }
}
