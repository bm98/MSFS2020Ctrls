using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MSFS2020Ctrls.Support
{
  /// <summary>
  /// A class to read JSON formatted KbdLayouts from the layouts/kbd folder
  /// </summary>
  [DataContract]
  class KbdLayoutFile
  {
    /// <summary>
    /// A list of Keyboard Layout IDs 
    ///  see http://kbdlayout.info/
    /// </summary>
    [DataMember( Name = "KLID", IsRequired = true )]
    public List<string> MapKLIDs { get; set; } = new List<string>( );

    /// <summary>
    /// A description - not used in the application
    /// </summary>
    [DataMember( Name = "Description" )]
    public string MapDescription { get; set; }

    /// <summary>
    /// A comment - not used in the application
    /// </summary>
    [DataMember( Name = "Comment" )]
    public string MapComment { get; set; }

    /// <summary>
    /// The key label map 
    /// fixed order is assumed here:
    /// 
    /// Esc  F F F F  F F F F  F F F F 
    /// K  1 2 3 4 5 6 7 8 9 0 K K Back
    /// Tab  L L L L L L L L L L K K
    /// Caps  L L L L L L L L L K K K Ret
    /// LSh  D L L L L L L L K K K  R-Shift
    /// LCt L-Win L-Alt Space R-Alt R-Win App RCt
    /// - mid section -
    /// Print Screen ScLock Pause
    /// Ins Home Pup
    /// Del End  Pdown
    ///     Up
    /// Le  Do  Ri
    /// - num section -
    /// NLck Div Mult Sub
    ///  7    8    9   
    ///  4    5    6   +
    ///  1    2    3
    ///    0       .  Ent
    /// </summary>
    [DataMember( Name = "Map" )]
    public List<string> MapKeys { get; set; } = new List<string>( );


    // NON JSON

    /// <summary>
    /// Writes this object as JSON to the open filestream
    /// </summary>
    /// <param name="filestream">The target stream</param>
    public void ToJson( Stream filestream )
    {
      try {
        var jsonSerializer = new DataContractJsonSerializer( typeof( KbdLayoutFile ) );
        jsonSerializer.WriteObject( filestream, this );
      }
#pragma warning disable CS0168 // Variable is declared but never used
      catch ( Exception e ) {
        ; // Debug stop
#pragma warning restore CS0168 // Variable is declared but never used
      }
    }


    /// <summary>
    /// Writes this object as JSON file with filename
    /// </summary>
    /// <param name="filename">The target filename</param>
    public void ToJson( string filename )
    {
      string fName = filename;
      if ( string.IsNullOrEmpty( Path.GetDirectoryName( filename ) ) ) {
        fName = Path.Combine( @".\layouts\kbd", filename ); // if path is not defined use the default path
      }

      using ( var sw = File.OpenWrite( fName ) ) {
        ToJson( sw );
      }
    }



    /// <summary>
    /// Reads from the open stream one KbdLayoutFile entry
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A KbdLayoutFile obj or null for errors</returns>
    public static KbdLayoutFile FromJson( Stream jStream )
    {
      try {
        var jsonSerializer = new DataContractJsonSerializer( typeof( KbdLayoutFile ) );
        object objResponse = jsonSerializer.ReadObject( jStream );
        var jsonResults = objResponse as KbdLayoutFile;
        return jsonResults;
      }
#pragma warning disable CS0168 // Variable is declared but never used
      catch ( Exception e ) {
#pragma warning restore CS0168 // Variable is declared but never used
        return null;
      }
    }

    /// <summary>
    /// Reads from a file one KbdLayoutFile entry
    /// </summary>
    /// <param name="jFilename">The Json Filename</param>
    /// <returns>A KbdLayoutFile obj or null for errors</returns>
    public static KbdLayoutFile FromJson( string jFilename )
    {
      KbdLayoutFile c = null;
      string fn = jFilename;
      if ( !File.Exists( jFilename ) ) {
        fn = Path.Combine( @".\layouts\kbd", fn ); // if it is not found use the default path
      }
      if ( File.Exists( fn ) ) {
        using ( var ts = File.OpenRead( fn ) ) {
          c = FromJson( ts );
        }
      }
      return c;
    }




  }
}
