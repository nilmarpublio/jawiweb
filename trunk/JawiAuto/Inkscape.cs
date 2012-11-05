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
			ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files\Inkscape\inkscape.exe");
			process = Process.Start(info);//@"C:\Program Files\Inkscape\inkscape.exe");
			process.WaitForInputIdle();
			Delay(2000);
			
			do
			{
				window = AutomationElement.RootElement.FindChildByProcessId(process.Id);
			}
			while(window == null);
			System.Diagnostics.Debug.WriteLine("Found the first child by process id: "+process.ProcessName);
		}
		
		public void Dispose()
		{
			process.CloseMainWindow();
			process.Dispose();
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
			//@see http://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.send.aspx
			SendKeys.SendWait("^o");
			Thread.Sleep(1000);
			
			AutomationElement comboBox = AutomationElement.FocusedElement;
			//System.Diagnostics.Debug.WriteLine("Popup "+dialog.Current.Name);			
			//AutomationElement dialog = null;
			//AutomationElement comboBox = null;
			AutomationElement textBox = null;
			
			//PropertyCondition findDialog = new PropertyCondition(AutomationElement.NameProperty, "Select file to open");
			//dialog = window.FindFirst(TreeScope.Descendants,findDialog);
			Thread.Sleep(100);
//			if(dialog != null)
//			{
//				System.Diagnostics.Debug.WriteLine("Found "+dialog.Current.Name);
//				
//				//PropertyCondition findComboBox = new PropertyCondition(AutomationElement.AutomationIdProperty, "1148");
//				//comboBox = window.FindFirst(TreeScope.Children,findComboBox);
//				//Thread.Sleep(100);
//			}	
			
			if(comboBox != null)
			{
				System.Diagnostics.Debug.WriteLine("Found "+comboBox.Current.Name);
				comboBox.SetValue(fileName);
			}
			
			Thread.Sleep(1000);
			SendKeys.SendWait("{ENTER}");
			System.Diagnostics.Debug.WriteLine("ENTER");
		}
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