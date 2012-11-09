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

namespace JawiAuto
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
    /// Open file programmically.
    /// </summary>
    /// <param name="fileName"></param>
    /// <seealso cref="http://social.msdn.microsoft.com/Forums/en-US/windowsaccessibilityandautomation/thread/061f9ef1-2e34-4176-b3f2-530ef3468047">UIAutomation: Missing MenuBar Items</seealso>
    public void OpenFile(string fileName)
    {
      System.Diagnostics.Debug.WriteLine("opening "+fileName+"...");
      //OpenMenu(FlexisignMenu.File);
      
      //looking file at menu bar in application
      PropertyCondition idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "MenuBar");
      AutomationElement menuBar = window.FindFirst(TreeScope.Children,idCondition);
      
      PropertyCondition nameCondition = new PropertyCondition(AutomationElement.NameProperty,"File");
      AutomationElement fileMenu = menuBar.FindFirst(TreeScope.Descendants,nameCondition);
      if(fileMenu == null)
      {
        System.Diagnostics.Debug.WriteLine("file menu not found");
        return;
      }
      
      ExpandCollapsePattern fileECPat = fileMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
      fileECPat.Expand();
      
      idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "Item 51832");
      AutomationElement openMenuItem = fileMenu.FindFirst(TreeScope.Descendants, idCondition);
      if(fileMenu == null)
      {
        System.Diagnostics.Debug.WriteLine("open menu not found");
        return;
      }
      
      InvokePattern openInvPat = openMenuItem.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
      openInvPat.Invoke();
      Thread.Sleep(2000);
      
      
      //get open dialog
      AutomationExtensions.OpenDialogEnter(window, "Open",fileName);

      //enter Font Substitution dialog try 3 times if fail
      for(int i=0;i<3;i++)
      {
        if(AutomationExtensions.OpenDialogEnter(window, "Font Substitution"))
          break;
      }
      
      //export to PLT
      nameCondition = new PropertyCondition(AutomationElement.NameProperty,"File");
      fileMenu = menuBar.FindFirst(TreeScope.Descendants,nameCondition);
      if(fileMenu == null)
      {
        System.Diagnostics.Debug.WriteLine("file menu not found");
        return;
      }
      
      fileECPat = fileMenu.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
      fileECPat.Expand();
      
      string id = "Item 51744";
      idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, id);
      AutomationElement menuItem = fileMenu.FindFirst(TreeScope.Descendants, idCondition);
      if(menuItem == null)
      {
        System.Diagnostics.Debug.WriteLine(id+" not found");
        return;
      }
      
      InvokePattern pressMenu = menuItem.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
      pressMenu.Invoke();
      Thread.Sleep(2000);
      
      
      string dialogName = "Export";
      nameCondition = new PropertyCondition(AutomationElement.NameProperty, dialogName);
      AutomationElement dialog = window.FindFirst(TreeScope.Children, nameCondition);
      if(dialog == null)
      {
        System.Diagnostics.Debug.WriteLine(dialogName+" dialog not found");
        return;
      }
      
      System.Diagnostics.Debug.WriteLine("launching "+dialogName+" Dialog...");
      Thread.Sleep(1000);
      
      //select export type as HPGL (*.PLT)"
      idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "1136");
      AutomationElement exportComboBox = dialog.FindFirst(TreeScope.Children,idCondition);
      if(exportComboBox == null)
      {
        System.Diagnostics.Debug.WriteLine("export combobox not found");
        return;
      }
      
      System.Diagnostics.Debug.WriteLine("found export combobox");
      SetSelectedComboBoxItem(exportComboBox,"HPGL (*.PLT)");     
      
      //press first button
      PropertyCondition typeCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
      AutomationElementCollection buttons = dialog.FindAll(TreeScope.Children,typeCondition);
      foreach(AutomationElement e in buttons)
      {
        InvokePattern button = e.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
        if(button == null)
        {
          System.Diagnostics.Debug.WriteLine("not a valid button press");
          return;
        }

        System.Diagnostics.Debug.WriteLine("Click on button "+e.Current.AutomationId);
        button.Invoke();
        Thread.Sleep(1000);
        break;
      }
    }
    public static void SetSelectedComboBoxItem(AutomationElement comboBox, string item)
    {
        AutomationPattern automationPatternFromElement = GetSpecifiedPattern(comboBox, "ExpandCollapsePatternIdentifiers.Pattern");
        ExpandCollapsePattern expandCollapsePattern = comboBox.GetCurrentPattern(automationPatternFromElement) as ExpandCollapsePattern;

        expandCollapsePattern.Expand();
        expandCollapsePattern.Collapse();

        AutomationElement listItem = comboBox.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, item));
        automationPatternFromElement = GetSpecifiedPattern(listItem, "SelectionItemPatternIdentifiers.Pattern");

        SelectionItemPattern selectionItemPattern = listItem.GetCurrentPattern(automationPatternFromElement) as SelectionItemPattern;
        selectionItemPattern.Select();
    }

    private static AutomationPattern GetSpecifiedPattern(AutomationElement element, string patternName)
    {
        AutomationPattern[] supportedPattern = element.GetSupportedPatterns();
        foreach (AutomationPattern pattern in supportedPattern)
        {
            if (pattern.ProgrammaticName == patternName)
                return pattern;
        }

        return null;
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
      Thread.Sleep(100);
      
      idCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "Item 57665");
      AutomationElement closeMenuItem = fileMenu.FindFirst(TreeScope.Descendants, idCondition);
      if(fileMenu == null) System.Diagnostics.Debug.WriteLine("close menu not found");
      InvokePattern closeInvPat = closeMenuItem.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
      closeInvPat.Invoke();
    }
    
    //FAIL on Flexisign
    public void OpenMenu(string menu)
    {
      ExpandCollapsePattern pattern = FindMenu(menu);
    }
    private ExpandCollapsePattern FindMenu(string menu)
    {
      AutomationElement menuElement = window.FindFirst(
        TreeScope.Descendants,
        new PropertyCondition(AutomationElement.NameProperty,menu));
      ExpandCollapsePattern pattern = menuElement.GetCurrentPattern(ExpandCollapsePattern.Pattern)
        as ExpandCollapsePattern;
      return pattern;
    }
    /// <summary>
    /// Generic method to push a button control
    /// Throws exception if button control is not enabled, or buttonName is null.
    /// </summary>
    /// <remarks>
    /// TODO: Move to helper class.
    /// </remarks>
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
  }
}
