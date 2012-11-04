/*
 * Created by SharpDevelop.
 * User: Yeangshing.Then
 * Date: 11/4/2012
 * Time: 10:16 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Fs2Svg
{
  public enum FlexisignMenu
  {
    File,
    Edit,
  }
  /// <summary>
  /// Description of Flexisign.
  /// </summary>
  public class Flexisign: IDisposable
  {
    private Process process;
    private AutomationElement window;
    
    public Flexisign()
    {
      ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files\FlexiSIGN-PRO 8.1v1\Program\App.exe");
      process = Process.Start(info);
      process.WaitForInputIdle();
      Thread.Sleep(2000);
      
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <seealso cref="http://social.msdn.microsoft.com/Forums/en-US/windowsaccessibilityandautomation/thread/061f9ef1-2e34-4176-b3f2-530ef3468047">UIAutomation: Missing MenuBar Items</seealso>
    public void OpenFile(string fileName)
    {
      //      try
      //      {
      System.Diagnostics.Debug.WriteLine("opening "+fileName+"...");
      //OpenMenu(FlexisignMenu.File);
      
      PropertyCondition idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "MenuBar");
      AutomationElement menuBar = window.FindFirst(TreeScope.Children,idCondition);
      
      PropertyCondition nameCondition = new PropertyCondition(AutomationElement.NameProperty,"File");
      AutomationElement fileMenu = menuBar.FindFirst(TreeScope.Descendants,nameCondition);
      if(fileMenu == null) System.Diagnostics.Debug.WriteLine("file menu not found");
      ExpandCollapsePattern fileECPat = fileMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
      fileECPat.Expand();
      
      idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "Item 51832");
      AutomationElement openMenuItem = fileMenu.FindFirst(TreeScope.Descendants, idCondition);
      if(fileMenu == null) System.Diagnostics.Debug.WriteLine("open menu not found");
      InvokePattern openInvPat = openMenuItem.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
      openInvPat.Invoke();
      Thread.Sleep(2000);
      
      //get open dialog
      nameCondition = new PropertyCondition(AutomationElement.NameProperty, "Open");
      AutomationElement openDialog = window.FindFirst(TreeScope.Children, nameCondition);
      if(openDialog == null)
        System.Diagnostics.Debug.WriteLine("open dialog not found");
      else
        System.Diagnostics.Debug.WriteLine("launching Open Dialog...");
      Thread.Sleep(1000);
      
      //set file name in textbox
      idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "1148");
      AutomationElement edit = openDialog.FindFirst(TreeScope.Children,idCondition);
      edit.SetFocus();
      edit.SetValue(fileName);
      
      //press open button
      PropertyCondition typeCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
      AutomationElementCollection buttons = openDialog.FindAll(TreeScope.Children,typeCondition);
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

      //fail
      //idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "1");
      //AutomationElement openButton = openDialog.FindFirst(TreeScope.Children, idCondition);
      //if(openButton == null) System.Diagnostics.Debug.WriteLine("open button not found");      
      //PushButton(openButton);
      
      //check whether prompt other dialog
      //"Font Substitution"
//      typeCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window);
//      AutomationElementCollection dialogs = window.FindAll(TreeScope.Children,typeCondition);
//      foreach(AutomationElement e in dialogs)
//      {
//        System.Diagnostics.Debug.WriteLine(e.Current.Name);
//      }
      
      nameCondition = new PropertyCondition(AutomationElement.NameProperty, "Font Substitution");
      AutomationElement dialog = window.FindFirst(TreeScope.Children, nameCondition);
      if(dialog == null)
        System.Diagnostics.Debug.WriteLine("font dialog not found");
      else
        System.Diagnostics.Debug.WriteLine("open font dialog...");
      typeCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
      buttons = dialog.FindAll(TreeScope.Children,typeCondition);
      foreach(AutomationElement e in buttons)
      {
        InvokePattern openPress = e.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
        if(openPress == null)
          System.Diagnostics.Debug.WriteLine("not a valid button press");
        else
        {
          System.Diagnostics.Debug.WriteLine("Click on button "+e.Current.AutomationId);
          openPress.Invoke();
          Thread.Sleep(100);
          break;
        }
      }
      
      
      //      }
      //      catch(Exception ex)
      //      {
      //        System.Diagnostics.Debug.WriteLine(ex);
      //      }
    }
    
    /// <summary>
    /// Generic method to push a button control
    /// Throws exception if button control is not enabled, or buttonName is null
    // </summary>
    private static void PushButton(AutomationElement buttonName)
    {
      try
      {
        InvokePattern pushButton = (InvokePattern)buttonName.GetCurrentPattern(InvokePattern.Pattern);
        pushButton.Invoke();
      }
      catch (ElementNotEnabledException)
      {
        throw new ElementNotEnabledException("Button is not enabled.");
      }
      catch (InvalidOperationException)
      {
        throw new InvalidOperationException("Button not found.");
      }
    }
    public void CloseFile()
    {      
      System.Diagnostics.Debug.WriteLine("closing file...");
      PropertyCondition idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "MenuBar");
      AutomationElement menuBar = window.FindFirst(TreeScope.Children,idCondition);
      
      PropertyCondition nameCondition = new PropertyCondition(AutomationElement.NameProperty,"File");
      AutomationElement fileMenu = menuBar.FindFirst(TreeScope.Descendants,nameCondition);
      if(fileMenu == null) System.Diagnostics.Debug.WriteLine("file menu not found");
      ExpandCollapsePattern fileECPat = fileMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
      fileECPat.Expand();
      
      idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "Item 57602");
      AutomationElement menuItem = fileMenu.FindFirst(TreeScope.Descendants, idCondition);
      if(menuItem == null) System.Diagnostics.Debug.WriteLine("close file menu not found");
      InvokePattern menuClick = menuItem.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
      menuClick.Invoke();
    }
    public void Exit()
    {
      System.Diagnostics.Debug.WriteLine("closing...");
      PropertyCondition idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "MenuBar");
      AutomationElement menuBar = window.FindFirst(TreeScope.Children,idCondition);
      
      PropertyCondition nameCondition = new PropertyCondition(AutomationElement.NameProperty,"File");
      AutomationElement fileMenu = menuBar.FindFirst(TreeScope.Descendants,nameCondition);
      if(fileMenu == null) System.Diagnostics.Debug.WriteLine("file menu not found");
      ExpandCollapsePattern fileECPat = fileMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
      fileECPat.Expand();
      
      idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "Item 57665");
      AutomationElement closeMenuItem = fileMenu.FindFirst(TreeScope.Descendants, idCondition);
      if(fileMenu == null) System.Diagnostics.Debug.WriteLine("close menu not found");
      InvokePattern closeInvPat = closeMenuItem.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
      closeInvPat.Invoke();
    }
    public void OpenMenu(FlexisignMenu menu)
    {
      ExpandCollapsePattern pattern = FindMenu(menu);
    }
    private ExpandCollapsePattern FindMenu(FlexisignMenu menu)
    {
      AutomationElement menuElement = window.FindFirst(
        TreeScope.Descendants,
        new PropertyCondition(AutomationElement.NameProperty,menu.ToString()));
      ExpandCollapsePattern pattern = menuElement.GetCurrentPattern(ExpandCollapsePattern.Pattern)
        as ExpandCollapsePattern;
      return pattern;
    }
  }
}
