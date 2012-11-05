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
      DirectoryInfo directoryInfo = new DirectoryInfo(@"D:\JawiName");
      int counter = 0;
      foreach(FileInfo fileInfo in directoryInfo.GetFiles())
      {      
        ++counter;        
        if(counter == 5) break;
        
        target.OpenFile(fileInfo.FullName);
        target.CloseFile();
      }      
    }
    [Test]
    public void ExitTest()
    {
      Flexisign target = new Flexisign();
      target.Exit();
    }
  }
}
