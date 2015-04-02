using HLGranite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Microsoft.VisualBasic;

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
            Assert.AreEqual(0, morethanone.Count());
        }
        /// <summary>
        /// Test read a file with UTF8 encoding.
        /// </summary>
        [TestMethod()]
        public void EncodingUTF8Test()
        {
            StreamReader reader = new StreamReader(@"C:\Users\yeang-shing.then\Desktop\t", Encoding.UTF8);
            string expected = "[[[\"廣\",\"广\",\"Guǎng\",\"Guǎng\"]],,\"zh-CN\",,[[\"廣\",,0,0,0,0,0,0]],,,,[[\"zh-CN\"]],98]";
            string actual = reader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test to get Spanish through Google Translate.
        /// </summary>
        /// <remarks>
        /// <returns></returns>
        [TestMethod()]
        public void GoogleTranslateSpanishTest()
        {
            string url = string.Format("http://translate.google.com/translate_a/t?client=t&text={0}&hl=en&sl=en&tl=es&multires=1&otf=1&ssel=4&tsel=4&sc=1", "thank");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //request.Proxy = null;
            //uncomment when there is a proxy setting
            NetworkCredential credential = new NetworkCredential("yeang-shing.then", "Q1w2e3r4v");
            request.Credentials = credential;
            WebProxy proxy = new WebProxy("http://co-proxy-003");
            proxy.Credentials = credential;
            request.Proxy = proxy;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string actual = reader.ReadToEnd();
            string expected = "agradecer";
            Assert.IsTrue(actual.Contains(expected));
        }
        /// <summary>
        /// TODO: FAIL: implemntation for auto translate rumi to jawi.
        /// </summary>
        [TestMethod()]
        public void GoogleTranslateArabicTest()
        {
            string url = string.Format("http://translate.google.com/translate_a/t?client=t&text={0}&hl=en&sl=ar&tl=en&multires=1&otf=1&pc=1&trs=1&ssel=5&tsel=5&sc=1", "sahila");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //request.Proxy = null;
            //uncomment when there is a proxy setting
            NetworkCredential credential = new NetworkCredential("yeang-shing.then", "Q1w2e3r4v");
            request.Credentials = credential;
            WebProxy proxy = new WebProxy("http://co-proxy-003");
            proxy.Credentials = credential;
            request.Proxy = proxy;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string actual = reader.ReadToEnd();
            string expected = "سهيلة";
            Assert.IsTrue(actual.Contains(expected));
        }
        /// <summary>
        /// FAIL: A test to get Japanese through Google Translate.
        /// </summary>
        /// <remarks>
        /// <returns></returns>
        [TestMethod()]
        public void GoogleTranslateJapaneseTest()
        {
            string url = string.Format("http://translate.google.com/translate_a/t?client=t&text={0}&hl=en&sl=en&tl=ja&multires=1&otf=1&ssel=4&tsel=4&sc=1", "thank you");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Proxy = null;
            //uncomment when there is a proxy setting
            NetworkCredential credential = new NetworkCredential("yeang-shing.then", "Q1w2e3r4v");
            request.Credentials = credential;
            WebProxy proxy = new WebProxy("http://co-proxy-003");
            proxy.Credentials = credential;
            request.Proxy = proxy;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string actual = reader.ReadToEnd();
            string expected = "ありがとう";
            Assert.IsTrue(actual.Contains(expected));
        }
        /// <summary>
        /// A test to get traddtional & simplified chinese character through Google Translate.
        /// </summary>
        /// <remarks>
        /// FAIL on translate simplified chinese to traditional chinese using this method. However
        /// it is success on ASCII Encoding which is used by other languages like Spanish, French.
        /// </remarks>
        /// <returns></returns>
        [TestMethod()]
        public void GoogleTranslateTest()
        {
            string url = string.Format("http://translate.google.com/translate_a/t?client=t&text={0}&hl=en&sl=zh-CN&tl=zh-TW&multires=1&otf=1&pc=1&trs=1&ssel=4&tsel=4&sc=1", "广");
            //url = string.Format("http://translate.google.com/#zh-CN|zh-TW|{0}", source);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;            
            //request.SendChunked = true;
            //request.TransferEncoding = "utf-8";

            //request.Proxy = null;
            //uncomment when there is a proxy setting
            NetworkCredential credential = new NetworkCredential("yeang-shing.then", "Q1w2e3r4v");
            request.Credentials = credential;
            WebProxy proxy = new WebProxy("http://co-proxy-003");
            proxy.Credentials = credential;
            request.Proxy = proxy;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream,Encoding.UTF8);// Encoding.GetEncoding("Big5"));
            string message = reader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine(message);
        }
        /// <summary>
        /// FAIL.
        /// </summary>
        [TestMethod()]
        public void GoogleTranslateByParamsTest()
        {
            string url = "http://translate.google.com/translate_a/t";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;
            //request.Proxy = null;
            //uncomment when there is a proxy setting
            NetworkCredential credential = new NetworkCredential("yeang-shing.then", "Q1w2e3r4v");
            request.Credentials = credential;
            WebProxy proxy = new WebProxy("http://co-proxy-003");
            proxy.Credentials = credential;
            request.Proxy = proxy;


            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //convert param to byte[]
            string paramString = "client=t&text=广&hl=en&sl=zh-CN&tl=zh-TW&multires=1&otf=1&pc=1&trs=1&ssel=4&tsel=4&sc=1";
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] postData = encoding.GetBytes(paramString);

            //write into request
            request.ContentLength = postData.Length;
            using (var dataStream = request.GetRequestStream())
                dataStream.Write(postData, 0, postData.Length);

            //get response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);// Encoding.GetEncoding("Big5"));
            string message = reader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine(message);
        }
        /// <summary>
        /// FAIL: A test to get traddtional & simplified chinese character through Google Translate.
        /// </summary>
        /// <remarks>Save a file from url then read the file content.</remarks>
        /// <returns></returns>
        [TestMethod()]
        public void UTF8FileTest()
        {
            string url = string.Format("http://translate.google.com/translate_a/t?client=t&text={0}&hl=en&sl=zh-CN&tl=zh-TW&multires=1&otf=1&pc=1&trs=1&ssel=4&tsel=4&sc=1", "广");
            WebClient webClient = new WebClient();
            webClient.Proxy = null;
            webClient.DownloadFile(url, "t");

            StreamReader reader = new StreamReader("t", Encoding.Default); //Encoding.UTF8
            string message = reader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine(message);
        }
        /// <summary>
        /// A test for simplified chinese to tradditional chinese by Micosoft embedded library.
        /// </summary>
        /// <see>http://yanziyang.wordpress.com/2010/11/29/simplified-traditional-chinese-conversion/</see>
        [TestMethod()]
        public void SimplifiedToTraditionalChineseByMicrosoftTranslateTest()
        {
            string target = "简体测试";
            string expected = "簡體測試";
            string actual = Microsoft.VisualBasic.Strings.StrConv(target, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void SimplifiedToTraditionalChineseGivenTraddtionalSourceTest()
        {
            string target = "簡體測試";//"简体测试";
            string expected = "簡體測試";
            string actual = Microsoft.VisualBasic.Strings.StrConv(target, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// A test for tradditional chinese to simplified chinese by Micosoft embedded library.
        /// </summary>
        /// <see>http://yanziyang.wordpress.com/2010/11/29/simplified-traditional-chinese-conversion/</see>
        [TestMethod()]
        public void TradditionalToSimplifiedChineseByMicrosoftTranslateTest()
        {
            string target = "繁體測試";
            string expected = "繁体测试";
            string actual = Microsoft.VisualBasic.Strings.StrConv(target, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// A test for tradditional chinese to simplified chinese by Micosoft embedded library.
        /// </summary>
        /// <see>http://yanziyang.wordpress.com/2010/11/29/simplified-traditional-chinese-conversion/</see>
        [TestMethod()]
        public void TradditionalToSimplifiedChineseGivenSimplifiedSourceTranslateTest()
        {
            string target = "繁体测试";//"繁體測試";
            string expected = "繁体测试";
            string actual = Microsoft.VisualBasic.Strings.StrConv(target, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
            Assert.AreEqual(expected, actual);
        }
    }
}