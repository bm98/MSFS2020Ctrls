using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MSFS2020Ctrls.MSFS
{
  sealed class LangPak
  {
    // Singleton
    private static readonly Lazy<LangPak> m_lazy = new Lazy<LangPak>( () => new LangPak( ) );
    public static LangPak Instance { get => m_lazy.Value; }
      

    /// <summary>
    /// One Lang Pak Entry...
    /// </summary>
    public class LangPakEntry
    {
      public string LangName;
      public string LangFile;

      public override string ToString( )
      {
        return LangName;
      }
    }

    /// <summary>
    /// Returns the MSFS Package Installation path
    /// </summary>
    /// <returns></returns>
    public static string GetPackageBasePath( )
    {
      string retPath = "";
      try {
        using ( var rk = Registry.CurrentUser ) {
          using ( var rFS = rk.OpenSubKey( @"SOFTWARE\Microsoft\FlightSimulator" ) ) {
            string instPath = (string)rFS.GetValue("InstallPackagePath", "");
            if ( Directory.Exists( instPath ) ) {
              return instPath;
            }
          }
        }
      }
      catch { }

      return retPath;
    }


    /// <summary>
    /// Returns a list of found language packs in the install folder
    /// 
    /// </summary>
    /// <returns>A list with pak files or an empty list on errors</returns>
    public static IList<LangPakEntry> GetLanguagePacks( )
    {
      var ret = new List<LangPakEntry>();

      string pkPath = GetPackageBasePath();
      if ( string.IsNullOrEmpty( pkPath ) ) return ret; // nope..

      pkPath = Path.Combine( pkPath, @"Official\OneStore\fs-base" );
      if ( !Directory.Exists( pkPath ) ) return ret; // nope

      var packs = Directory.EnumerateFiles(pkPath, "*.LocPak", SearchOption.TopDirectoryOnly);
      if ( packs == null ) return ret; // nope

      foreach ( var f in packs ) {
        var entry = new LangPakEntry();
        entry.LangName = GetPakLanguage( f );
        entry.LangFile = f;
        ret.Add( entry );
      }

      return ret;
    }


    /// <summary>
    /// Returns a language code for a given file
    /// </summary>
    /// <param name="pakFile"></param>
    /// <returns></returns>
    public static string GetPakLanguage( string pakFile )
    {
      string lang = Path.GetFileNameWithoutExtension(pakFile);
      // de-DE, en-US etc. 
      return lang;
    }

    // **** CLASS

    private Dictionary<string,string> m_dict = new Dictionary<string, string>();

    /// <summary>
    /// Load a Language File to mem
    /// </summary>
    /// <param name="langFile">The Lang File</param>
    public void LoadLanguage( string langFile )
    {
      m_dict.Clear( );
      if ( !File.Exists( langFile ) ) return;

      using ( var sr = new StreamReader( langFile ) ) {
        while ( !sr.EndOfStream ) {
          string line = sr.ReadLine().Trim();
          if ( line.StartsWith( $"\"INPUT." ) && ( !line.Contains( $"_DESC\"" ) ) ) {
            string[] e = line.Split(new char[]{ ':' } );
            //       "INPUT.KEY_FUELSYSTEM_TRIGGER_ON": "TRIGGER ON",
            string key = e[0].Replace($"\"", "").Replace("INPUT.", "").Trim();
            string cont = e[1].Replace($"\"", "").Replace($",", "").Trim();
            if ( !m_dict.ContainsKey( key ) )
              m_dict.Add( key, cont );
          }
        }
      }
    }

    /// <summary>
    /// Returns an item for a key
    /// </summary>
    /// <param name="key">The key string</param>
    /// <returns>The content string; if not found the key</returns>
    public string LangItem( string key )
    {
      string ret=key;
      if ( m_dict.ContainsKey( key ) )
        ret = m_dict[key];

      return ret;
    }

  }
}
