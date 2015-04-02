using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NisanWPF.BusinessLogic.Test
{
    [TestFixture]
    public class miscTest
    {
        public miscTest()
        {
        }

        [Test]
        public void DateTimeNowTest()
        {
            DateTime today = DateTime.Today;
            DateTime now = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("Today: " + today);
            System.Diagnostics.Debug.WriteLine("Now: " + now);
            Assert.IsTrue(now > today);
        }
        [Test]
        public void ColorTest()
        {
            char[] romans = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            Assert.AreEqual(26, romans.Length);
            //int incremental = 0;
            //foreach (char roman in romans)
            //{
            //    int i = Convert.ToInt32(roman);// (int)Char.GetNumericValue(roman);
            //    incremental = i *i;
            //    String hex = incremental.ToString("X6");
            //    System.Diagnostics.Debug.WriteLine(roman.ToString() + ": " + hex);
            //}

            //int[] rhb = new int[3] { 0, 0, 0 };
            //for (int i = 0; i < romans.Length; i++)
            //{
            //    rhb[i % 3] += 128;
            //    System.Diagnostics.Debug.WriteLine(rhb[0].ToString() + " " + rhb[1].ToString() + " " + rhb[2].ToString());
            //}

            int[] codes = new int[4] { 0, 64, 128, 255 };
            foreach (int i in codes)
            {
                foreach (int j in codes)
                {
                    foreach (int k in codes)
                    {
                        System.Diagnostics.Debug.WriteLine(i.ToString() + " " + j.ToString() + " " + k.ToString());
                    }
                }
            }


            Assert.IsTrue(true);
        }
    }
}