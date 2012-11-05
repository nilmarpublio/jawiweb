/*
 * Created by SharpDevelop.
 * User: yeang-shing.then
 * Date: 10/23/2012
 * Time: 11:34 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;

namespace JawiAuto
{
	public enum InkscapeMenu
	{
		File,
		Edit,
	}
	/// <summary>
	/// Description of Inkscape.
	/// </summary>
	public class Inkscape: IDisposable
	{
		private Process process;
		private AutomationElement window;
		
		public Inkscape()
		{
//			ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files\Inkscape\inkscape.exe");
//			process = Process.Start(info);
//			process.WaitForInputIdle();
//			Delay(2000);
//
//			do
//			{
//				window = AutomationElement.RootElement.FindChildByProcessId(process.Id);
//			}
//			while(window == null);
//			System.Diagnostics.Debug.WriteLine("Found the first child by process id: "+process.ProcessName);
		}
		
		public void Dispose()
		{
			if(process != null)
			{
				System.Diagnostics.Debug.WriteLine("exit app");
				process.CloseMainWindow();
				process.Dispose();
			}
		}
		private ExpandCollapsePattern FindMenu(InkscapeMenu menu)
		{
			AutomationElement menuElement = window.FindFirst(
				TreeScope.Descendants,
				new PropertyCondition(AutomationElement.NameProperty,menu.ToString()));
			ExpandCollapsePattern pattern = menuElement.GetCurrentPattern(ExpandCollapsePattern.Pattern)
				as ExpandCollapsePattern;
			return pattern;
		}
		public void SaveAsFile(string fileName)
		{
			SendKeys.SendWait("^+s");
		}
		public void SaveAsHPGL(string fileName)
		{
			SaveAsFile(fileName);
			
			
//			for(int i=0;i<15;i++)
//			{
//				SendKeys.SendWait("{DOWN}");
//				Thread.Sleep(100);
//			}
		}
		public void OpenFile(string fileName)
		{
			System.Diagnostics.Debug.WriteLine("opening file "+fileName+"...");
			ProcessStartInfo info = new ProcessStartInfo(fileName);
			process = Process.Start(info);
			process.WaitForInputIdle();
			Thread.Sleep(2000);
			
			do
			{
				//TODO: handle if hang or passing 5 sec
				window = AutomationElement.RootElement.FindChildByProcessId(process.Id);
			}
			while(window == null);
			System.Diagnostics.Debug.WriteLine("Found the first child by process id: "+process.ProcessName);
		}
		public void BreakApart(string destination)
		{
			//select all
			System.Diagnostics.Debug.WriteLine("select all");
			SendKeys.SendWait("^a");
			Thread.Sleep(100);
			
			//object to path
//			System.Diagnostics.Debug.WriteLine("shift + ctrl + k");
//			SendKeys.SendWait("+^k");
//			Thread.Sleep(500);
			
			//success if the text object fill with color but the plt export contains double lines
			//fail if the text object not fill with color then have to use shfit + ctrl + c
			System.Diagnostics.Debug.WriteLine("ctrl + alt +  c");
			SendKeys.SendWait("^%c");
			Thread.Sleep(500);
			
			//save as
			System.Diagnostics.Debug.WriteLine("save as");
			SendKeys.SendWait("^+s");
			Thread.Sleep(2000);
			
			//get dialog
			PropertyCondition nameCondition = new PropertyCondition(AutomationElement.NameProperty, "Select file to save to");
			AutomationElement dialog = window.FindFirst(TreeScope.Children, nameCondition);
			if(dialog == null)
				System.Diagnostics.Debug.WriteLine("dialog not found");
			else
				System.Diagnostics.Debug.WriteLine("launching Dialog...");
			Thread.Sleep(1000);
			
			//set file name in textbox
			PropertyCondition idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "1148");
			AutomationElement edit = dialog.FindFirst(TreeScope.Children,idCondition);
			edit.SetFocus();
			edit.SetValue(destination);
			
			//press open button
			PropertyCondition typeCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
			AutomationElementCollection buttons = dialog.FindAll(TreeScope.Children,typeCondition);
			foreach(AutomationElement e in buttons)
			{
				InvokePattern openPress = e.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
				if(openPress == null)
					System.Diagnostics.Debug.WriteLine("not a valid button press");
				else
				{
					System.Diagnostics.Debug.WriteLine("Click on button "+e.Current.AutomationId);
					openPress.Invoke();
					Thread.Sleep(2000);
					break;
				}
			}
		}
		public void SaveAsPLT(string source, string target)
		{
			//TODO: remove circle id before save as plt
			
			string program = @"C:\Program Files\sK1 Project\UniConvertor-1.1.5\uniconvertor.cmd";
			//string program = "uniconvertor";
			string command = string.Format("\"{0}\" \"{1}\" \"{2}\"",program,source,target);
			System.Diagnostics.Debug.WriteLine(command);
			//Process.Start(command);
//			
			ProcessStartInfo console = new ProcessStartInfo();
			console.FileName = program;// "cmd";
			console.Arguments = command;
			Process.Start(console);
		}
		/// <summary>
		/// FAIL
		/// </summary>
		/// <param name="fileName"></param>
		/*public void OpenFile(string fileName)
		{
			System.Diagnostics.Debug.WriteLine("opening file "+fileName+"...");
			PropertyCondition typeCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane);
			AutomationElement allPaneArea = window.FindFirst(TreeScope.Children,typeCondition);
			Thread.Sleep(1000);
			
			int i = 0;
			AutomationElementCollection panes = allPaneArea.FindAll(TreeScope.Children, typeCondition);
			
			//the last one is menu bar
			AutomationElement menuBar = panes[panes.Count-1];
			AutomationElementCollection menus = menuBar.FindAll(TreeScope.Children,typeCondition);
			
			//the last one is File, Edit, View, Layer, Object, Path, ...
			AutomationElement fileMenu = menus[menus.Count-1];
			//ExpandCollapsePattern expandable = fileMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
			//expandable.Expand();
			//Thread.Sleep(1000);
			
			AutomationElementCollection fileMenus = fileMenu.FindAll(TreeScope.Descendants, typeCondition);
			foreach(AutomationElement menu in fileMenus)
			{
				++i;
				System.Diagnostics.Debug.Write(i.ToString()+" ");
				PrintElement(menu);
			}
		}
		 */
		private void PrintElement(AutomationElement e)
		{
			System.Diagnostics.Debug.WriteLine(e.Current.AutomationId+": "+e.Current.Name+" at "+e.Current.BoundingRectangle);
		}
		/// <summary>
		/// HACK: Open file.
		/// </summary>
		/// <param name="fileName"></param>
		/*public void OpenFile(string fileName)
		{
			//@see http://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.send.aspx
			SendKeys.SendWait("^o");
			Thread.Sleep(1000);
			
			AutomationElement comboBox = AutomationElement.FocusedElement;
			AutomationElement textBox = null;
			Thread.Sleep(100);
			if(comboBox != null)
			{
				System.Diagnostics.Debug.WriteLine("Found "+comboBox.Current.Name);
				comboBox.SetValue(fileName);
			}
			
			Thread.Sleep(1000);
			SendKeys.SendWait("{ENTER}");
			System.Diagnostics.Debug.WriteLine("ENTER");
		}
		 */
		public void OpenMenu(InkscapeMenu menu)
		{
			ExpandCollapsePattern pattern = FindMenu(menu);
		}
		public void ExecuteMenuByName(string menu)
		{
			AutomationElement menuElement = window.FindFirst(
				TreeScope.Descendants,
				new PropertyCondition(AutomationElement.NameProperty,menu));
			if(menuElement == null)return;
			
			InvokePattern invokePattern = menuElement.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
			if(invokePattern != null) invokePattern.Invoke();
		}
		private void Delay()
		{
			Delay(1000);
		}
		private void Delay(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}
	}
}