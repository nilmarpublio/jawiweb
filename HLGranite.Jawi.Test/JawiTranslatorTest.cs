﻿using HLGranite.Jawi;
using NUnit.Framework;
using System;

namespace HLGranite.Jawi.Test
{
    /// <summary>
    /// This is a test class for JawiTranslatorTest and is intended
    /// to contain all JawiTranslatorTest Unit Tests
    /// </summary>
    [TestFixture]
    public class JawiTranslatorTest
    {
        /// <summary>
        ///A test for Translate
        ///</summary>
        [Test]
        public void TranslatePagiTest()
        {
            JawiTranslator target = new JawiTranslator();
            string rumi = "pagi";
            string expected = "ڤاݢي";
            string actual = target.Translate(rumi);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for Translate
        ///</summary>
        [Test]
        public void TranslateMohdTest()
        {
            JawiTranslator target = new JawiTranslator();
            string rumi = "mohd";
            string expected = "محمد";
            string actual = target.Translate(rumi);
            Assert.AreEqual(expected, actual);
        }
    }
}