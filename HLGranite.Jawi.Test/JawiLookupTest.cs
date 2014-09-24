using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HLGranite.Jawi;
using NUnit.Framework;

namespace HLGranite.Jawi.Test
{
    [TestFixture]
    public class JawiLookupTest
    {
        [Test]
        public void LookupMohdTest()
        {
            string expected = "محمد";
            string actual = "";
            JawiLookup target = new JawiLookup();
            actual = target.Lookup("Mohd");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void LookupMuhdTest()
        {
            string expected = "محمد";
            string actual = "";
            JawiLookup target = new JawiLookup();
            actual = target.Lookup("Muhd");
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void LookupAriffTest()
        {
            string expected = "عريف";
            string actual = "";
            JawiLookup target = new JawiLookup();
            actual = target.Lookup("ariff");
            Assert.AreEqual(expected, actual);
        }
    }
}
