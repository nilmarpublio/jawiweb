using HLGranite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FindLiandui.Test
{
    /// <summary>
    /// This is a test class for LianduisTest and is intended
    /// to contain all LianduisTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LianduisTest
    {
        /// <summary>
        /// A test for SaveToFile.
        /// </summary>
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
        /// </summary>
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
            Lianduis target = Lianduis.LoadFromFile(@"G:\My Projects\JawiWeb\FindLiandui\bin\Debug\Lianduis.xml");
            Assert.IsTrue(target.Liandui.Count > 0);
        }
        /// <summary>
        /// A test to find out which is redundant entry in datasheet collection.
        /// </summary>
        [TestMethod()]
        public void DuplicateEntryTest()
        {
            Lianduis target = Lianduis.LoadFromFile(@"G:\My Projects\JawiWeb\FindLiandui\bin\Debug\Lianduis.xml");
            var grouping = target.Liandui.GroupBy(f => f.Value);
            var morethanone = grouping.Where(f => f.Count() > 1);
            foreach (var item in morethanone)
                System.Diagnostics.Debug.WriteLine("{0}:{1}", item.Key, item.Count());
            Assert.AreEqual(0,morethanone.Count());
        }
    }
}