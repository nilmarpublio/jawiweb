/*
 * Created by SharpDevelop.
 * User: Yeangshing.Then
 * Date: 10/1/2012
 * Time: 10:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace JawiPatch
{
  /// <summary>
  /// Repair svg file to basic format it can be preview under explorer.
  /// </summary>
  public class SvgRepair
  {
    private string directory;
    public SvgRepair()
    {
      this.directory = AppDomain.CurrentDomain.BaseDirectory;
    }
    public SvgRepair(string directory)
    {
      this.directory = directory;
    }
    public string PatchLine(string original)
    {
      string output = original;
      output = original.Replace("connector-curvature=\"0\"","");
      
      string cell = "a-zA-Z0-9%-.";
      Match match = Regex.Match(original, string.Format("inkscape:[{0}]+=\"[{0}]+\"",cell));
      if(match.Success)
      {
        string toReplace = match.Groups[0].Value;
        output = original.Replace(toReplace,"");
      }
      
      match = Regex.Match(original,string.Format("sodipodi:[{0}]+=\"[{0}]+\"",cell));
      if(match.Success)
      {
        string toReplace = match.Groups[0].Value;
        output = original.Replace(toReplace,"");
      }
      
      return output;
    }
    public void Patch()
    {
      Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      AppSettingsSection appSection = config.AppSettings;
      
      DateTime lastPatch = DateTime.MinValue;
      if(appSection.Settings["LastSvgPatch"].Value.Length>0)
        lastPatch = Convert.ToDateTime(appSection.Settings["LastSvgPatch"].Value);//ConfigurationManager.AppSettings["LastSvgPatch"].ToString());
      System.Diagnostics.Debug.WriteLine(lastPatch);
      
      DirectoryInfo directoryInfo = new DirectoryInfo(this.directory);
      FileInfo[] fileInfos = directoryInfo.GetFiles().OrderByDescending(f => f.LastWriteTime).ToArray();      
      
      foreach(FileInfo fileInfo in fileInfos)
      {        
        //only taking care of svg extension
        if(fileInfo.Extension.ToLower() != ".svg") continue;
        
        //stop process at last patch file
        if(fileInfo.LastWriteTime.CompareTo(lastPatch) <= 0) break;
        
        System.Diagnostics.Debug.WriteLine(fileInfo.Name);
        Console.WriteLine(fileInfo.Name);
        
        //store file content into memory
        DateTime[] dates = new DateTime[] { fileInfo.CreationTime, fileInfo.LastAccessTime, fileInfo.LastWriteTime};
        StreamReader reader = new StreamReader(fileInfo.FullName);
        string content = reader.ReadToEnd();
        reader.Close();        
        
        //string newFileName = @"D:\Output\"+fileInfo.Name;//TODO: change destination directory
        //FileInfo newFileInfo = fileInfo.CopyTo(newFileName,true);
        TextWriter writer = fileInfo.CreateText();
        writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
        writer.WriteLine("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">");
        writer.WriteLine("<svg viewBox=\"0 0 600 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:svg=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">");
        
        bool startClone = false;
        string[] lines = content.Split(new char[]{'\n'});
        //while((line = reader.ReadLine()) != null)
        foreach(string line in lines)
        {
          if(line.Contains("path")) startClone = true;
          if(line.Contains("<g")) startClone = true;          
          if(startClone)
          {
            string newLine = PatchLine(line);
            if(newLine.Trim().Length>0) writer.WriteLine(newLine);
          }
          
          //only start clone after next line
          if(line.Contains("<svg>")) startClone = true;
        }
        writer.Flush();
        writer.Close();
        
        //fileInfo.Attributes = tempInfo.Attributes;
        fileInfo.CreationTime = dates[0];
        fileInfo.LastAccessTime = dates[1];
        fileInfo.LastWriteTime = dates[2];
      }//end loops      
      
      //set the last access file as the latest patch date
      lastPatch = fileInfos[0].LastWriteTime;
      
      //ConfigurationManager.AppSettings["LastPath"] = lastPatch;
      //ConfigurationManager.AppSettings.Set("LastSvgPatch",lastPatch.ToString());
      appSection.Settings.Remove("LastSvgPatch");
      appSection.Settings.Add("LastSvgPatch",lastPatch.ToString());//TODO: convert to UTC time ToUniversalTime()
      config.Save(ConfigurationSaveMode.Modified);
      Console.WriteLine("done");
    }
  }
}