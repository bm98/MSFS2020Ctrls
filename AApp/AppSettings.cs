using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace MSFS2020Ctrls
{
  sealed class AppSettings : ApplicationSettingsBase
  {
    // Singleton
    private static readonly Lazy<AppSettings> m_lazy = new Lazy<AppSettings>( () => new AppSettings( ) );
    public static AppSettings Instance { get => m_lazy.Value; }

    private AppSettings()
    {
      if ( this.FirstRun ) {
        // migrate the settings to the new version if the app runs the first time
        try {
          this.Upgrade( );
        }
        catch { }
        this.FirstRun = false;
        this.Save( );
      }
    }


    #region Setting Properties

    // manages Upgrade
    [UserScopedSetting( )]
    [DefaultSettingValue( "True" )]
    public bool FirstRun
    {
      get { return (bool)this["FirstRun"]; }
      set { this["FirstRun"] = value; }
    }


    // Control bound settings
    [UserScopedSetting( )]
    [DefaultSettingValue( "1000, 900" )]
    public Size FormSize
    {
      get { return (Size)this["FormSize"]; }
      set { this["FormSize"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "10, 10" )]
    public Point FormLocation
    {
      get { return (Point)this["FormLocation"]; }
      set { this["FormLocation"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "en-US" )] // just a default
    public string MyLanguageChoice {
      get { return (string)this["MyLanguageChoice"]; }
      set { this["MyLanguageChoice"] = value; }
    }

    //**** Form Layout

    [UserScopedSetting( )]
    [DefaultSettingValue( "1000, 765" )]
    public Size FormLayoutSize
    {
      get { return (Size)this["FormLayoutSize"]; }
      set { this["FormLayoutSize"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "10, 10" )]
    public Point FormLayoutLocation
    {
      get { return (Point)this["FormLayoutLocation"]; }
      set { this["FormLayoutLocation"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "16" )]
    public int LayoutFontSize
    {
      get { return (int)this["LayoutFontSize"]; }
      set { this["LayoutFontSize"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,0,0,139|255,255,255,255" )]
    public string GroupColor_00
    {
      get { return (string)this["GroupColor_00"]; }
      set { this["GroupColor_00"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,255,140,00|255,255,255,255" )]
    public string GroupColor_01
    {
      get { return (string)this["GroupColor_01"]; }
      set { this["GroupColor_01"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,138,43,226|255,255,255,255" )]
    public string GroupColor_02
    {
      get { return (string)this["GroupColor_02"]; }
      set { this["GroupColor_02"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,220,20,60|255,255,255,255" )]
    public string GroupColor_03
    {
      get { return (string)this["GroupColor_03"]; }
      set { this["GroupColor_03"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,184,134,11|255,255,255,255" )]
    public string GroupColor_04
    {
      get { return (string)this["GroupColor_04"]; }
      set { this["GroupColor_04"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,46,139,87|255,255,255,255" )]
    public string GroupColor_05
    {
      get { return (string)this["GroupColor_05"]; }
      set { this["GroupColor_05"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,128,128,0|255,255,255,255" )]
    public string GroupColor_06
    {
      get { return (string)this["GroupColor_06"]; }
      set { this["GroupColor_06"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,47,79,79|255,255,255,255" )]
    public string GroupColor_07
    {
      get { return (string)this["GroupColor_07"]; }
      set { this["GroupColor_07"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,255,0,0|255,255,255,255" )]
    public string GroupColor_08
    {
      get { return (string)this["GroupColor_08"]; }
      set { this["GroupColor_08"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,255,215,0|255,255,255,255" )]
    public string GroupColor_09
    {
      get { return (string)this["GroupColor_09"]; }
      set { this["GroupColor_09"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,0,0,0|255,255,255,255" )]
    public string GroupColor_10
    {
      get { return (string)this["GroupColor_10"]; }
      set { this["GroupColor_10"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,128,0,128|255,255,255,255" )]
    public string GroupColor_11
    {
      get { return (string)this["GroupColor_11"]; }
      set { this["GroupColor_11"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "255,255,20,147|255,255,255,255" )]
    public string GroupColor_12
    {
      get { return (string)this["GroupColor_12"]; }
      set { this["GroupColor_12"] = value; }
    }


    #endregion


  }
}
