/*
 * Created by SharpDevelop.
 * User: Yeangshing.Then
 * Date: 11/4/2012
 * Time: 10:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;

using NUnit;
using NUnit.Framework;

namespace JawiAuto
{
  /// <summary>
  /// Description of FlexisignTests.
  /// </summary>
  [TestFixture]
  public class FlexisignTests
  {
    [Test]
    public void LaunchApplicationTest()
    {
      Flexisign target = new Flexisign();
      Process[] result = Process.GetProcessesByName("app");
      int notExpected = 0;
      int actual = result.Length;
      System.Diagnostics.Debug.WriteLine("Found in TaskManager: "+actual);
      Assert.AreNotEqual(notExpected,actual);
      target.Dispose();
    }
    [Test]
    public void OpenFileTest()
    {
      Flexisign target = new Flexisign();
      target.OpenFile(@"D:\JawiName\abas bin din.FS");
      //target.CloseFile();
      //Thread.Sleep(2000);
    }
    [Test]
    public void ExitTest()
    {
      Flexisign target = new Flexisign();
      target.Exit();
    }
    
    
    [Test]
    public void AutomateExport()
    {
      Flexisign target = new Flexisign();
      DirectoryInfo directoryInfo = new DirectoryInfo(@"D:\JawiName");
      int counter = 0;
      foreach(FileInfo fileInfo in directoryInfo.GetFiles())
      {
        counter++;
        if(!fileInfo.Extension.ToLower().Contains("fs")) continue;
        
        System.Diagnostics.Debug.WriteLine("opening "+fileInfo.FullName+"...");
        
        //        try
        //        {
        target.OpenFile(fileInfo.FullName);//target.ExportPLT(fileInfo.Name);
        target.CloseFile();
        //        }
        //        catch(Exception ex)
        //        {
        //          System.Diagnostics.Debug.WriteLine(ex);
        //          continue;
        //        }
        
        //dispose memory even wrapped exception also will increase memory consumption.
        if(counter%500==0) GC.Collect();//key
        Thread.Sleep(2000);
      }
    }
    
    /// <summary>
    /// Check file name different between two folders without compare extension.
    /// </summary>
    [Test]
    public void CheckDiff()
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(@"D:\JawiExport");
      DirectoryInfo directoryInfo2 = new DirectoryInfo(@"D:\JawiSvg");
      
      List<string> files1 = new List<string>();
      List<string> files2 = new List<string>();
      List<string> diffFiles = new List<string>();
      FileInfo[] fileInfos1 = directoryInfo1.GetFiles();
      FileInfo[] fileInfos2 = directoryInfo2.GetFiles();
      
      //do nothing since all exported
      if(fileInfos1.Length<=fileInfos2.Length) return;
      
      int expected = fileInfos1.Length-fileInfos2.Length;
      
      foreach(FileInfo info in fileInfos1)
        files1.Add(info.Name.Replace(info.Extension,string.Empty));
      files1.Sort(new NameComparer());
      
      foreach(FileInfo info in fileInfos2)
        files2.Add(info.Name.Replace(info.Extension,string.Empty));
      files2.Sort(new NameComparer());
      
      int j = files2.Count-1;
      for(int i=files1.Count-1;i>=0;i--)
      {
        if(j>-1)
        {
          if(files1[i].CompareTo(files2[j]) > 0)
          {
            diffFiles.Add(files1[i]);
            files1.RemoveAt(i);
          }
          else
          {
            j--;
            
            //if found different move to another folder
            string source = @"D:\JawiExport\"+files1[i]+".plt";
            string destination = @"D:\JawiPlt\"+files1[i]+".plt";
            File.Move(source,destination);
          }
        }
        else
        {
          diffFiles.Add(files1[i]);
          
          //if found different move to another folder
          string source = @"D:\JawiExport\"+files1[i]+".plt";
          string destination = @"D:\JawiPlt\"+files1[i]+".plt";
          File.Move(source,destination);
        }
      }
      
      //show different files
      foreach(string file in diffFiles)
      {
        System.Diagnostics.Debug.WriteLine(file);
        //string source = file+".FS";
        //string destination = source.Replace("JawiDone","JawiName");
        //File.Move(source,destination);
      }
      
      
      int actual = diffFiles.Count;
      System.Diagnostics.Debug.WriteLine("diff files: "+actual);
      Assert.AreEqual(expected,actual);
    }
    public class NameComparer: IComparer<string>
    {
      public int Compare(string x, string y)
      {
        return x.CompareTo(y);
      }
    }
  }
}