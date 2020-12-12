﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MSFS2020Ctrls.Layout
{
  /// <summary>
  /// Reads a DeviceFile from a file or stream
  /// </summary>
  class ControllerJson
  {

    /// <summary>
    /// Reads from a file one Controller entry
    /// </summary>
    /// <param name="jFilename">The Json Filename</param>
    /// <returns>A Controller obj or null for errors</returns>
    public static DeviceFile FromJson( string jFilename )
    {
      DeviceFile c = null;
      string fn = jFilename;
      if ( !File.Exists( jFilename ) ) {
        fn = Path.Combine( @".\layouts", fn ); // if it is not found use the default path
      }
      if ( File.Exists( fn ) ) {
        using ( var ts = File.OpenRead( fn ) ) {
          c = FromJson( ts );
        }
      }
      return c;
    }


    /// <summary>
    /// Reads from the open stream one Controller entry
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A Controller obj or null for errors</returns>
    public static DeviceFile FromJson( Stream jStream )
    {
      try {
        var jsonSerializer = new DataContractJsonSerializer( typeof( DeviceFile ) );
        object objResponse = jsonSerializer.ReadObject( jStream );
        var jsonResults = objResponse as DeviceFile;
        return jsonResults;
      }
#pragma warning disable CS0168 // Variable is declared but never used
      catch ( Exception e ) {
#pragma warning restore CS0168 // Variable is declared but never used
        return null;
      }
    }


  }
}
