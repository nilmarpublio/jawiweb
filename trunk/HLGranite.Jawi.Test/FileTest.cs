/*
 * Created by SharpDevelop.
 * User: yeang-shing.then
 * Date: 10/08/2012
 * Time: 15:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HLGranite.Jawi;
using NUnit.Framework;

namespace HLGranite.Jawi.Test
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	[TestFixture]
	public class FileTest
	{
		/// <summary>
		/// A test to make sure all svg file not in Inkscape format.
		/// </summary>
		[Test]
		public void FindContentTest()
		{
			bool expected = false;
			bool actual = false;
			
			string token = "inkscape";			
			string source = @"G:\projects\JawiWeb\JawiWPF\bin\Debug\words";
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(source);
            System.IO.FileInfo[] filesInfo = directoryInfo.GetFiles();
            var files = filesInfo.OrderBy(f => f.FullName);//sort alphabetically
            foreach(System.IO.FileInfo info in files)
            {
            	StreamReader reader = new StreamReader(info.FullName);
            	string content = reader.ReadToEnd();
            	if(content.Contains(token)) {
            		actual = true;
            		System.Diagnostics.Debug.WriteLine(info.FullName + " contains "+token);
            	}
            }
            
            Assert.AreEqual(expected,actual);
		}
	}
}