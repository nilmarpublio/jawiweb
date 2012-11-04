/*
 * Created by SharpDevelop.
 * User: Yeangshing.Then
 * Date: 11/4/2012
 * Time: 11:57 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit;
using NUnit.Framework;

namespace JawiPatch.Test
{
  /// <summary>
  /// Description of SvgRepairTest.
  /// </summary>
  [TestFixture]
  public class SvgRepairTest
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
