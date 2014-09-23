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
    }
}
