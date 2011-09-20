using HLGranite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FindLiandui.Test
{
    /// <summary>
    ///This is a test class for LianduisTest and is intended
    ///to contain all LianduisTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LianduisTest
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
        /// A test for SaveToFile.
        ///</summary>
        [TestMethod()]
        public void FullAttribuiteSaveToFileTest()
        {
            Lianduis target = new Lianduis();
            Liandui liandui = new Liandui();
            liandui.Prefix = 1;
            liandui.Type = HLGranite.Type.n;
            liandui.Value = "abcdefghij";
            target.Liandui.Add(liandui);

            string fileName = "Lianduis.xml";
            target.SaveToFile(fileName);
        }
        /// <summary>
        /// A test for SaveToFile.
        ///</summary>
        [TestMethod()]
        public void NullAttribuiteSaveToFileTest()
        {
            Lianduis target = new Lianduis();
            Liandui liandui = new Liandui();
            //liandui.Prefix = 1;
            //liandui.Type = HLGranite.Type.n;
            liandui.Value = "7435ewrew";
            target.Liandui.Add(liandui);

            string fileName = "Lianduis.xml";
            target.SaveToFile(fileName);
        }
        /// <summary>
        /// A test for loading from source file.
        /// </summary>
        [TestMethod()]
        public void LoadFromFileTest()
        {
            Lianduis target = Lianduis.LoadFromFile(@"D:\My Projects\JawiWeb\FindLiandui\bin\Debug\Lianduis.xml");
            Assert.IsTrue(target.Liandui.Count > 0);
        }
    }
}