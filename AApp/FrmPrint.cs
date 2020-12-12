using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSFS2020Ctrls
{
  public partial class FrmPrint : Form
  {
    public FrmPrint( )
    {
      InitializeComponent( );
    }

    /// <summary>
    /// Get, Set the RTF text 
    /// </summary>
    public string RTF { get => RTB.Rtf; set => RTB.Rtf = value; }

    private void ctxCopy_Click( object sender, EventArgs e )
    {
      RTB.Copy( );
    }

    private void ctxSelectAll_Click( object sender, EventArgs e )
    {
      RTB.SelectAll( );
    }

    private void ctxSaveAs_Click( object sender, EventArgs e )
    {
      SFD.FileName = "FSFS2020-Layout.rtf";
      if ( SFD.ShowDialog(this)== DialogResult.OK ) {
        File.WriteAllText( SFD.FileName, RTB.Rtf );
      }
    }


  }
}
