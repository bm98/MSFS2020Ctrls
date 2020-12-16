using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSFS2020Ctrls.Support
{
  // Support to import KLE Files and convert them to our format

  class KLE_File
  {
    /*
     KLE of the en-US file
  [
    ["\u001B\n\u001B\n\n\n\n\n\n\n\nEsc",{x:1},"\n\n\n\n\n\n\n\n\nF1","\n\n\n\n\n\n\n\n\nF2","\n\n\n\n\n\n\n\n\nF3","\n\n\n\n\n\n\n\n\nF4",{x:0.5},"\n\n\n\n\n\n\n\n\nF5","\n\n\n\n\n\n\n\n\nF6","\n\n\n\n\n\n\n\n\nF7","\n\n\n\n\n\n\n\n\nF8",{x:0.5},"\n\n\n\n\n\n\n\n\nF9","\n\n\n\n\n\n\n\n\nF10","\n\n\n\n\n\n\n\n\nF11","\n\n\n\n\n\n\n\n\nF12",{x:0.25},"\n\n\n\n\n\n\n\n\nPrtSc","\n\n\n\n\n\n\n\n\nScroll Lock","\n\n\n\n\n\n\n\n\nPause"],
    [{y:0.5},"~\n`","!\n1","@\n2","#\n3","$\n4","%\n5","^\n6","&\n7","*\n8","(\n9",")\n0","_\n-","+\n=",{w:2},"\b\n\b\n\n\n\n\n\n\n\nBackspace",{x:0.25},"\n\n\n\n\n\n\n\n\nInsert","\n\n\n\n\n\n\n\n\nHome","\n\n\n\n\n\n\n\n\nPage Up",{x:0.25},"\n\n\n\n\n\n\n\n\nNum Lock","/\n/","*\n*","-\n-"],
    [{w:1.5},"\t\n\t\n\n\n\n\n\n\n\nTab","Q\nq","W\nw","E\ne","R\nr","T\nt","Y\ny","U\nu","I\ni","O\no","P\np","{\n[","}\n]",{x:0.25,w:1.25,h:2,w2:1.5,h2:1,x2:-0.25},"\r\n\r\n\n\n\n\n\n\n\nEnter",{x:0.25},"\n\n\n\n\n\n\n\n\nDelete","\n\n\n\n\n\n\n\n\nEnd","\n\n\n\n\n\n\n\n\nPage Down",{x:0.25},"\n7","\n8","\n9",{h:2},"+\n+"],
    [{w:1.75},"\n\n\n\n\n\n\n\n\nCaps Lock","A\na","S\ns","D\nd","F\nf","G\ng","H\nh","J\nj","K\nk","L\nl",":\n;","\"\n'","|\n\\",{x:4.75},"\n4","\n5","\n6"],
    [{w:1.25},"\n\n\n\n\n\n\n\n\nShift","|\n\\","Z\nz","X\nx","C\nc","V\nv","B\nb","N\nn","M\nm","<\n,",">\n.","?\n/",{w:2.75},"\n\n\n\n\n\n\n\n\nShift",{x:1.25},"\n\n\n\n\n\n\n\n\n↑",{x:1.25},"\n1","\n2","\n3",{h:2},"\r\n\r\n\n\n\n\n\n\n\nEnter"],
    [{w:1.25},"\n\n\n\n\n\n\n\n\nCtrl",{w:1.25},"\n\n\n\n\n\n\n\n\nWin",{w:1.25},"\n\n\n\n\n\n\n\n\nAlt",{w:6.25}," \n ",{w:1.25},"\n\n\n\n\n\n\n\n\nAltGr",{w:1.25},"\n\n\n\n\n\n\n\n\nWin",{w:1.25},"\n\n\n\n\n\n\n\n\nMenu",{w:1.25},"\n\n\n\n\n\n\n\n\nCtrl",{x:0.25},"\n\n\n\n\n\n\n\n\n←","\n\n\n\n\n\n\n\n\n↓","\n\n\n\n\n\n\n\n\n→",{x:0.25,w:2},"\n0","\n."]
  ]
    */
    // this kills "{xyz},"  and "\uxxxx" sequences

    private static string killNumFields = @"(\\u.{4})|(\{\b.+?\},)";
    private static string killPreChar = @"("")(.{1}\\n)";
    private static string killNl = @"(\\)([btnr])";
    private static string lCaseLetters = @"""([a-z]{1})""";
    private static string separation = @"(""\s*,\s*"")";
    /// <summary>
    /// Clean the KLE from unused content
    /// </summary>
    /// <param name="kle"></param>
    /// <returns></returns>
    private static string CleanKLE( string kle )
    {
      // importing a Javascipt or TS for this purpose seems overdone.. handy method though
      var result = kle.Replace( "\n", "" ).Replace( "\r", "" ); // CR LFs
      result = result.Replace( "[[", "" ).Replace( "],[", "," ).Replace( "]]", "" ); // non text brackets
      result = Regex.Replace( result, killNumFields, new MatchEvaluator(RegKill) );
      result = Regex.Replace( result, killPreChar, new MatchEvaluator( RegDQuote ) ); 
      result = result.Replace(@"\""\n", "" ); // handle the escaped quote "\"\nC" 
      result = Regex.Replace( result, killNl, new MatchEvaluator( RegKill ) );
      result = Regex.Replace( result, lCaseLetters, new MatchEvaluator( RegUCase ) );
      result = Regex.Replace( result, separation, new MatchEvaluator( RegSeparation ) );
      result = result.Substring( 1 , result.Length-2); 
      // one quoted string
      return result;
    }

    private static string RegKill( Match match ) { return ""; }
    private static string RegDQuote( Match match ) { return "\""; }
    private static string RegUCase( Match match ) { return match.Value.ToUpperInvariant( ); }
    private static string RegSeparation( Match match ) { return "‡"; } // new separator (comma may be part of the display letters)

    public List<string> GetKeyLabels( string filename )
    {
      if ( !File.Exists( filename ) ) return null;

      string s = File.ReadAllText(filename);
      s = CleanKLE( s );
      string [] e = s.Split(new char[]{ '‡' } );
      int nChar = e.Length; // should be 105 
      if ( nChar != 105 ) {
        ; // Debug stop
      }

      var ret = e.ToList();  // now we should have our Keylabel list...

      return ret;
    }




  }
}
