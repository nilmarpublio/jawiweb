UI Automation
---------------
1. UISpy - download from [Microsoft Windows SDK](http://blogs.msdn.com/b/windowssdk/archive/2008/02/18/where-is-uispy-exe.aspx)
2. [UI Automation Verify](http://uiautomationverify.codeplex.com/)
3. CodeProject - [Automate your UI using Microsoft Automation Framework](http://www.codeproject.com/Articles/141842/Automate-your-UI-using-Microsoft-Automation-Framew) with CalculatorTest at 2011
4. [Automate Draw Spirographs with Paint.Net](http://blog.functionalfun.net/2009/06/introduction-to-ui-automation-with.html)
5. http://www.scip.be/index.php?Page=ArticlesNET20
6. http://www.testingmentor.com/imtesty/tag/test-automation/


[UniConvertor](http://sk1project.org/)
--------------
cmd > uniconvertor "bidah bt mat.svg" "bidah bt mat.plt"


Issues
-----
JawiAuto.FlexisignTests.AutomateExport: System.InvalidOperationException: Operation cannot be performed.

at MS.Internal.AutomationProxies.WindowsMenu.MenuItem.System.Windows.Automation.Provider.IExpandCollapseProvider.Expand()
at System.Runtime.InteropServices.Marshal.ThrowExceptionForHRInternal(Int32 errorCode, IntPtr errorInfo)
at MS.Internal.Automation.UiaCoreApi.CheckError(Int32 hr)
at System.Windows.Automation.ExpandCollapsePattern.Expand()
at JawiAuto.Flexisign.OpenFile(String fileName)
at JawiAuto.FlexisignTests.AutomateExport()
