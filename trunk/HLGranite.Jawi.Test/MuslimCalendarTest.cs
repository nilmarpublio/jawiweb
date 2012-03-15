using HLGranite.Jawi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using System.Data;

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
        /// <summary>
        /// A test for conversion from Gregorian date to Muslim date.
        /// </summary>
        [TestMethod()]
        public void GetDateTest()
        {
            MuslimCalendar calendar = new MuslimCalendar(ReadXml("muslimcal.xml"));

            //2012-01-18 is 1433-02-24
            calendar.GetDate(new DateTime(2011, 11, 27));
            DateTime expected = new DateTime(1433, 1, 1);
            DateTime actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2011, 12, 26));
            expected = new DateTime(1433, 2, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 1, 24));
            expected = new DateTime(1433, 3, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 2, 23));
            expected = new DateTime(1433, 4, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 3, 24));
            expected = new DateTime(1433, 5, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 4, 22));
            expected = new DateTime(1433, 6, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 5, 22));
            expected = new DateTime(1433, 7, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 6, 21));
            expected = new DateTime(1433, 8, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 7, 20));
            expected = new DateTime(1433, 9, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 8, 19));
            expected = new DateTime(1433, 10, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 9, 17));
            expected = new DateTime(1433, 11, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            calendar.GetDate(new DateTime(2012, 10, 17));
            expected = new DateTime(1433, 12, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            //2012-01-18 is 1433-02-24
            calendar.GetDate(new DateTime(2012, 1, 18));
            expected = new DateTime(1433, 2, 24);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            //2011-10-30 is 1432-11-03
            calendar.GetDate(new DateTime(2011, 10, 30));
            expected = new DateTime(1432, 12, 3);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);

            //2011-11-27 is 1433-01-01
            calendar.GetDate(new DateTime(2011, 11, 27));
            expected = new DateTime(1433, 1, 1);
            actual = calendar.Value;
            Assert.AreEqual(expected, actual);
        }
        private DataTable ReadXml(string fileName)
        {
            DataTable table = new DataTable();
            DataSet dataset = new DataSet();

            try
            {
                if (System.IO.File.Exists(fileName))
                    dataset.ReadXml(fileName);
                if (dataset.Tables.Count > 0)
                    table = dataset.Tables[0].Copy();

                return table;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return table;
            }
            finally { dataset.Dispose(); }
        }
    }
}