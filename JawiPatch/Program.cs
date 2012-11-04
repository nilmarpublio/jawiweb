using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace JawiPatch
{
  class Program
  {
    static void Main(string[] args)
    {
      //Console.WriteLine("Start rename sticker item according to male or female item.");
      //BreakStickerItem();
      //Console.WriteLine("Complete");

      //20100917tys
      //CountWord();

      //20121025
      /*int count = 0;
      DateTime start = DateTime.Now;
      WriteLine("Start: "+start);
      try
      {
        RenameFile(@"D:\jawiname", @"D:\JawiName3",out count);
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex);
        Console.Read();
      }
      DateTime end = DateTime.Now;
      TimeSpan spent = end.Subtract(start);
      WriteLine("End: "+end);
      WriteLine("spent: "+spent+" for "+count+" files");
       */
      
      //20121104
      SvgRepair repair = new SvgRepair(@"G:\works\Output");
      repair.Patch();
      
      Console.Read();
    }
    
    private static void WriteLine(string line)
    {
      System.Diagnostics.Debug.WriteLine(line);
      Console.WriteLine(line);
    }
    private static string Rename(string fileName,string extension)
    {
      //remove extension and get the file name only.
      fileName = fileName.Replace(extension,"");
      
      string newFileName = fileName.ToLower();
      
      Dictionary<string,string> dictionary = new Dictionary<string, string>();
      dictionary.Add("b.","bin");
      dictionary.Add("binti","bt");
      dictionary.Add("haji","hj");
      dictionary.Add("hajah","hjh");
      dictionary.Add("hajjah","hjh");
      foreach(KeyValuePair<string,string> pair in dictionary)
      {
        newFileName = newFileName.Replace(pair.Key,pair.Value);
      }
      
      newFileName = newFileName.Replace(".","");
      
      newFileName += extension;
      return newFileName;
    }
    /// <summary>
    /// Rename all files under a source folder to a new destination.
    /// </summary>
    /// <param name="source">Source folder.</param>
    /// <param name="destination">Destination folder.</param>
    private static void RenameFile(string source, string destination, out int count)
    {
      count = 0;
      DirectoryInfo directoryInfo = new DirectoryInfo(source);
      FileInfo[] fileInfos = directoryInfo.GetFiles();
      foreach(FileInfo fileInfo in fileInfos)
      {
        ++count;
        string newFileName = Rename(fileInfo.Name,fileInfo.Extension);
        fileInfo.CopyTo(destination+Path.DirectorySeparatorChar+newFileName,true);
        WriteLine("converting '"+fileInfo.Name+"' to '"+newFileName+"'");
      }
    }
    /// <summary>
    /// Print out word and count in console.
    /// </summary>
    private static void CountWord()
    {
      WordScales wordScales = new WordScales();
      wordScales.Option = WordScalesOption.Word;

      string content = string.Empty;
      DirectoryInfo directoryInfo = new DirectoryInfo(@"E:\jawiname");
      FileInfo[] files = directoryInfo.GetFiles();
      foreach (FileInfo file in files)
        content += file.Name.ToLower() + " ";
      wordScales.Count(content);
      foreach (KeyValuePair<string, int> item in wordScales.Result)
        Console.WriteLine("{0}\t{1}", item.Key, item.Value);
    }
    /// <summary>
    /// Rename sticker item according to male or female item if has.
    /// </summary>
    /// <remarks>
    /// since 2010-09-07.
    /// </remarks>
    private static void BreakStickerItem()
    {
      DataSet dataSet = new DataSet();
      dataSet.ReadXml("nisan.xml");
      if (dataSet.Tables.Count == 0) return;

      DataTable table = dataSet.Tables[0];
      foreach (DataRow row in table.Rows)
      {
        if (row["item"].ToString() == "1½' Sticker")
        {
          if (row["name"].ToString().Contains(" bin "))
            row["item"] = "1½' Sticker(L)";
          if (row["name"].ToString().Contains(" bt "))
            row["item"] = "1½' Sticker(P)";
        }
        if (row["item"].ToString() == "2' Sticker")
        {
          if (row["name"].ToString().Contains(" bin "))
            row["item"] = "2' Sticker(L)";
          if (row["name"].ToString().Contains(" bt "))
            row["item"] = "2' Sticker(P)";
        }
        if (row["item"].ToString() == "2½' Sticker")
        {
          if (row["name"].ToString().Contains(" bin "))
            row["item"] = "2½' Sticker(L)";
          if (row["name"].ToString().Contains(" bt "))
            row["item"] = "2½' Sticker(P)";
        }
      }
      table.AcceptChanges();
      table.WriteXml("nisan2.xml");
    }
  }
}