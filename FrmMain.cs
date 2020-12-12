using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSFS2020Ctrls.Layout;
using MSFS2020Ctrls.MSFS;

namespace MSFS2020Ctrls
{
  public partial class FrmMain : Form
  {

    /// <summary>
    /// Checks if a rectangle is visible on any screen
    /// </summary>
    /// <param name="formRect"></param>
    /// <returns>True if visible</returns>
    private static bool IsOnScreen( Rectangle formRect )
    {
      Screen[] screens = Screen.AllScreens;
      foreach ( Screen screen in screens ) {
        if ( screen.WorkingArea.Contains( formRect ) ) {
          return true;
        }
      }
      return false;
    }

    public FrmMain( )
    {
      InitializeComponent( );

      var lang = LangPak.GetLanguagePacks();
      foreach ( var l in lang ) {
        cbxLang.Items.Add( l );
      }
      if ( cbxLang.Items.Count > 0 )
        cbxLang.SelectedIndex = 0;

      foreach(var item in cbxLang.Items ) {
        if (item.ToString()== AppSettings.Instance.MyLanguageChoice ) {
          cbxLang.SelectedItem = item;
        }
      }

      btShowForm.Enabled = false;
      btPrint.Enabled = false;

      // some applic initialization 
      // Assign Size property - check if on screen, else use defaults
      if ( IsOnScreen( new Rectangle( AppSettings.Instance.FormLocation, AppSettings.Instance.FormSize ) ) ) {
        // this.Size = AppSettings.Instance.FormSize;
        this.Location = AppSettings.Instance.FormLocation;
      }

      string version = Application.ProductVersion;  // get the version information
      // BETA VERSION; TODO -  comment out if not longer
      //lblTitle.Text += " - V " + version.Substring( 0, version.IndexOf( ".", version.IndexOf( "." ) + 1 ) ); // PRODUCTION
      this.Text += " - V " + version + " beta"; // BETA

      btLoad.Focus( );

    }

    private void FmrMain_FormClosing( object sender, FormClosingEventArgs e )
    {
      // don't record minimized, maximized forms
      if ( this.WindowState == FormWindowState.Normal ) {
        AppSettings.Instance.FormSize = this.Size;
        AppSettings.Instance.FormLocation = this.Location;
      }
      AppSettings.Instance.MyLanguageChoice = cbxLang.SelectedItem.ToString();

      AppSettings.Instance.Save( );
    }


    private List<DeviceEntry>m_devices = new List<DeviceEntry>();

    private void btLoad_Click( object sender, EventArgs e )
    {
      DeviceEntry.DeviceReset( ); // reset Joystick enumeration
      clbDevices.Items.Clear( );

      // load all..
      var x = MsFiles.GetControlProfiles();
      m_devices = new List<DeviceEntry>( );
      RTB.Text = $"Collected MSFS2020 Game Control Data:\n";
      foreach ( var f in x ) {
        var dev = Controller.FromXmlFile( f );
        m_devices.Add( dev );
        clbDevices.Items.Add( dev );
        RTB.Text += $"\n{dev.DeviceProfileName}   - \n";
        RTB.Text += $"  {dev.DeviceName}\n";
        RTB.Text += $"  {dev.NumberOfActionMaps} Map(s); {dev.NumberOfActions} Actions\n";
      }
      RTB.Text += $"--- \n";
      RTB.Text += $"{m_devices.Count} Device Configuration Files\n";
      int sum = m_devices.Sum(d=> d.NumberOfActions);
      RTB.Text += $"{sum} Action Entries\n";

      btShowForm.Enabled = true;
      btPrint.Enabled = true;

    }

    private void btShowForm_Click( object sender, EventArgs e )
    {
      string fName = (cbxLang.SelectedItem as LangPak.LangPakEntry).LangFile;
      MSFS.LangPak.Instance.LoadLanguage( fName );

      ActionItemList alist = new ActionItemList();
      foreach ( var dev in clbDevices.CheckedItems ) {
        var cDev = dev as DeviceEntry;
        cDev.GetForLayout( alist );
      }
      if ( alist.Count < 1 ) return;

      var LAYOUT = new FormLayout { ActionList = alist };
      LAYOUT.ShowDialog( this );
      LAYOUT = null; // get rid and create a new one next time..
    }

    private void btPrint_Click( object sender, EventArgs e )
    {
      string fName = (cbxLang.SelectedItem as LangPak.LangPakEntry).LangFile;
      MSFS.LangPak.Instance.LoadLanguage( fName );
      var rtf = new Support.RTFformatter();
      rtf.SetTab( 200 ); rtf.SetTab( 6000 ); rtf.SetTab( 8000 ); rtf.SetTab( 10000 );
      //rtf.WriteTab( "1" ); rtf.WriteTab( "2" ); rtf.WriteTab( "3" ); rtf.WriteTab( "4" ); rtf.WriteLn( ); // TAB preview
      foreach ( var dev in clbDevices.CheckedItems ) {
        var cDev = dev as DeviceEntry;
        cDev.AsRTF( rtf );
      }
      var PRINT = new FrmPrint{ RTF= rtf.RTFtext };
      PRINT.ShowDialog( this );
      PRINT = null;
    }

  }
}
