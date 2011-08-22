using HLGranite.Jawi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace HLGranite.Jawi.Test
{
    /// <summary>
    ///This is a test class for MuslimCalendarTest and is intended
    ///to contain all MuslimCalendarTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MuslimCalendarTest
    {
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        /// A test to determine input string is match pattern yyyy-MM-dd.
        ///</summary>
        [TestMethod()]
        public void IsTrueyyyyMMddFormatTest()
        {
            Regex regex = new Regex(@"[0-9]{4}[-][0-9]{2}[-][0-9]{2}");
            bool result = regex.IsMatch("2011-08-22");
            Assert.IsTrue(result);
        }
        /// <summary>
        /// A test to determine input string is match pattern yyyy-MM-dd.
        ///</summary>
        [TestMethod()]
        public void IsFalseyyyyMMddFormatTest()
        {
            Regex regex = new Regex(@"[0-9]{4}[-][0-9]{2}[-][0-9]{2}");
            bool result = regex.IsMatch("2011-0845");
            Assert.IsFalse(result);
        }
    }
}