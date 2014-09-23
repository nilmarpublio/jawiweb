using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NisanWPF.BusinessLogic;
using NUnit.Framework;

namespace NisanWPF.BusinessLogic.Test
{
    [TestFixture]
    public class nisanTest
    {
        public nisanTest()
        {
        }

        [Test]
        public void LoadFromFileTest()
        {
            nisan nisan;
            nisan.LoadFromFile("nisan.xml", out nisan);
            System.Diagnostics.Debug.WriteLine(nisan.Items.Count);
            Assert.IsTrue(nisan.Items.Count > 0);
        }

        [Test]
        public void SaveFileTest()
        {
            nisan nisan;
            nisan.LoadFromFile("nisan.xml", out nisan);
            int before = nisan.Items.Count;
            System.Diagnostics.Debug.WriteLine("Before: " + before);

            nisanOrder order = new nisanOrder();
            order.date = "2014-09-18";
            order.item = "2' Batu Batik(L)";
            order.soldto = "ADI";
            order.name = "Ali bin Test";
            order.price = 250;

            nisan.Items.Add(order);
            nisan.SaveToFile("nisan.xml");

            nisan nisan2;
            nisan.LoadFromFile("nisan.xml", out nisan2);
            int after = nisan2.Items.Count;
            System.Diagnostics.Debug.WriteLine("After: " + after);
            Assert.AreEqual(1, after - before);
        }

        [Test]
        public void InitializeOrderTest()
        {
            nisan nisan;
            nisan.LoadFromFile("nisan.xml", out nisan);
            nisan.Initialize(nisan);
            System.Diagnostics.Debug.WriteLine("All: " + nisan.Items.Count + " Order: " + nisan.Orders.Count);
            Assert.IsTrue(nisan.Orders.Count > 0);
            Assert.IsTrue(nisan.Items.Count >= nisan.Orders.Count);
        }
    }
}
