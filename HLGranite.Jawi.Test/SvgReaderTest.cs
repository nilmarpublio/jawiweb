using HLGranite.Jawi;
using NUnit.Framework;
using System.Windows;

namespace HLGranite.Jawi.Test
{    
    /// <summary>
    /// This is a test class for SvgReaderTest and is intended
    /// to contain all SvgReaderTest Unit Tests
    /// </summary>
    [TestFixture]
    public class SvgReaderTest
    {
        public SvgReaderTest()
        {
        }

        /// <summary>
        /// Get size of an alef caligraphy.
        ///</summary>
        [Test]
        public void GetSizeTest()
        {
            //alef raw data
            string rawDataString = "m 2.8853219,14.535578 0.55997,9.87823 2.63737,48.2687 c 0,0 1.7899,-1.79365 2.86234,-3.42732 1.5211701,-2.31487 1.6011701,-4.30477 2.0586501,-6.52715 0.35873,-1.74366 0.38248,-2.13239 0.23498,-3.89355 l -2.8873401,-34.43693 -0.27499,-5.84594 c 0.28749,0.15749 0.56122,0.29998 0.81871,0.42622 l 4.4422601,2.19989 -0.84745,-2.33738 c -0.0962,-0.26498 -0.19874,-0.54622 -0.30124,-0.82871 -1.23243,-3.39482 -1.91615,-6.75214 -2.7373501,-10.2332103 -0.78496,-3.33232 -1.83241,-7.77084 -1.83241,-7.77084 0,0 -0.74121,-0.0387 -1.42492,0.0587 -1.8774,0.26874 -5.88594005,6.24842 -6.14593005,7.6071 -0.025,0.12874 -0.04,0.25749 -0.045,0.38498 -0.0113,0.28123 0.01,0.57622 0.0575,0.8812 0.34373,2.1898903 1.44618005,4.0585403 2.82485005,5.5959603 z";
            Size expected = new Size(72.683,13.337);
            Size actual;
            actual = SvgReader.GetSize(rawDataString);
            Assert.AreEqual(expected, actual);
        }
    }
}
