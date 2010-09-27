using System;
using HLGranite.WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace JawiWPF.Test
{
    /// <summary>
    /// This is a test class for DipHelperTest and is intended
    /// to contain all DipHelperTest Unit Tests
    /// </summary>
    [TestClass()]
    public class DipHelperTest
    {
        public DipHelperTest()
        {
        }
        /// <summary>
        /// Test divide by 8 double value for inch units.
        /// </summary>
        [TestMethod()]
        public void InchLoopingTest()
        {
            double[] expecteds = new double[] { 0.0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1.0 };
            double[] actuals = new double[8];
            int max = 8;
            for (int i = 0; i < max; i++)
            {
                decimal m = 0.125m;// Convert.ToDecimal(1 / max);
                System.Diagnostics.Debug.WriteLine(m);
                actuals[i] = Convert.ToDouble(i * m);
                System.Diagnostics.Debug.WriteLine(actuals[i]);
            }
            for (int i = 0; i < Math.Min(expecteds.Length, actuals.Length); i++)
                Assert.AreEqual(expecteds[i], actuals[i]);
        }
    }
}