using System;
using HLGranite.Jawi;
using NUnit.Framework;

namespace HLGranite.Jawi.Test
{
    /// <summary>
    /// This is a test class for CharacterCollectionTest and is intended
    /// to contain all CharacterCollectionTest Unit Tests
    /// </summary>
    [TestFixture]
    public class CharacterCollectionTest
    {
        public CharacterCollectionTest()
        {
        }

        /// <summary>
        /// A test for CharacterCollection Constructor
        /// </summary>
        [Test]
        public void CharacterCollectionConstructorTest()
        {
            string fontFamily = "Arial";
            CharacterCollection target = new CharacterCollection(fontFamily);
            Assert.AreEqual(55296, target.Items.Count);
            //Assert.AreEqual(65509, target.Items.Count);
            //Assert.AreEqual(32767 + 1, target.Items.Count);
        }
        /// <summary>
        /// Test a correct maximum Int16 value.
        /// </summary>
        [Test]
        public void MaxPositiveInt16Test()
        {
            int expected = 32767;
            int actual = 0;
            actual = Int16.MaxValue;
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// A test for UnicodeFormatter
        /// </summary>
        [Test]
        public void UnicodeFormatterTest()
        {
            //TODO: CharacterCollection_Accessor target = new CharacterCollection_Accessor("Arial");
            int i = 1069;
            string expected = "U+042D";
            string actual  = string.Empty;
            //actual = target.UnicodeFormatter(i);
            Assert.AreEqual(expected, actual);
        }
    }
}