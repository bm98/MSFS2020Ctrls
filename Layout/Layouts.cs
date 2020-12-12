using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS2020Ctrls.Layout
{
  class Layouts : List<DeviceLayout>
  {
    /// <summary>
    /// cTor: Collect all Layouts in the distribution folder
    /// </summary>
    public Layouts()
    {
      this.Clear( );
      if ( !Directory.Exists(@".\layouts" ) ) return;

      var jsons = Directory.EnumerateFiles( @".\layouts", "*.json", SearchOption.TopDirectoryOnly );
      foreach ( var f in jsons ) {
        var devLayout = new DeviceLayout {
          Filename = f,
          DeviceController = ControllerJson.FromJson( f )
        };
        if ( devLayout.DeviceController != null ) {
          this.Add( devLayout );
        }
      }
    }

    /// <summary>
    /// Collect all known devices (key is GUID), value is Name
    /// </summary>
    /// <returns>Dict. of all known devices</returns>
    public List<Device.DeviceDescriptor> Devices()
    {
      var ret = new List<Device.DeviceDescriptor>( );
      foreach ( var dlayout in this ) {
        var devs = dlayout.DeviceController.Devices( );
        foreach ( var dev in devs ) {
          if ( !ret.Exists( x => x.DevGuid == dev.DevGuid ) ) {
            ret.Add( dev );
          }
        }
      }
      return ret;
    }


  }
}
