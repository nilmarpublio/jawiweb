/*
 * Created by SharpDevelop.
 * User: yeang-shing.then
 * Date: 11/16/2012
 * Time: 1:32 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;

namespace JawiPatch
{
	/// <summary>
  /// Description of SvgRepairTest.
  /// </summary>
  [TestFixture]
  public class SvgRepairTests
  {
    [Test]
    public void PatchLineTest()
    {
      SvgRepair repair = new SvgRepair("D:\\Output");
      string target = "sodipodi:nodetypes=\"ccccccccccccccccccccccccccccssscsssssccscccccssssscssssccsssscsssssccccscsccssccccccccssccccsscscsscscsscccscssccsccssssssccssssssssccc\"";
      string expected = "";
      string actual = repair.PatchLine(target);
      Assert.AreEqual(expected,actual);
      
      target = "sodipodi:linespacing=\"125%\"";
      actual = repair.PatchLine(target);
      Assert.AreEqual(expected,actual);
      
      target = "inkscape:connector-curvature=\"0\"";
      actual = repair.PatchLine(target);
      Assert.AreEqual(expected,actual);
    }
  }
}
