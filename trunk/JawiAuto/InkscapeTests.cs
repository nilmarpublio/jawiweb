/*
 * Created by SharpDevelop.
 * User: yeang-shing.then
 * Date: 10/23/2012
 * Time: 11:31 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using NUnit;
using NUnit.Framework;

namespace JawiAuto
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	[TestFixture]
	public class InkscapeTests
	{
		private void Delay()
		{
			Delay(1000);
		}
		private void Delay(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}
		
		[Test]
		public void LaunchApplicationTest()
		{
			Inkscape target = new Inkscape();
			Process[] result = Process.GetProcessesByName("Inkscape");
			int notExpected = 0;
			int actual = result.Length;
			System.Diagnostics.Debug.WriteLine("Found in TaskManager: "+actual);
			Assert.AreNotEqual(notExpected,actual);
			target.Dispose();
		}
		/// <summary>
		/// Fail.
		/// </summary>
		[Test]
		public void OpenMenuTest()
		{
			Inkscape target = new Inkscape();
			target.OpenMenu(InkscapeMenu.File);
			target.ExecuteMenuByName("Open");
		}
		[Test]
		public void OpenFileTest()
		{
			Inkscape target = new Inkscape();
			//target.OpenFile(@"G:\works\words\talib.svg");//.ToUpper());
			target.OpenFile(@"G:\works\Output\mat saad bin jusoh.svg");
		}
		[Test]
		public void SaveAsFileTest()
		{
			Inkscape target = new Inkscape();
			target.SaveAsFile(@"G:\works\words\talib.hpgl");
		}
		
		[Test]
		public void BreakApartTest()
		{
			Inkscape target = new Inkscape();
			target.OpenFile(@"G:\works\Output\abu bakar bin mohd hassan.svg");//mat saad bin jusoh.svg");
			target.BreakApart(@"D:\Export\abu bakar bin mohd hassan.svg");//mat saad bin jusoh.svg");
			target.Dispose();
		}
		[Test]
		public void SaveAsPLTTest()
		{
			string source = @"D:\Export\mat saad bin jusoh.svg";
			Inkscape target = new Inkscape();
			target.SaveAsPLT(source,source.Replace(".svg",".plt"));
			target.Dispose();
		}
		/// <summary>
		/// TODO: Automate UniConvertor export the svg file to plt.
		/// </summary>
		[Test]
		public void ExportToPLTTest()
		{
			string from = @"G:\works\Output\mat saad bin jusoh.svg.svg";
			string to = @"D:\Export\mat saad bin jusoh.svg.svg";
			
			Inkscape target = new Inkscape();
			target.OpenFile(from);
			target.BreakApart(to);
			target.SaveAsPLT(to,to.Replace(".svg",".plt"));
			target.Dispose();
		}
	}
}