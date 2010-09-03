using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JawiWeb.Test
{
    /// <summary>
    /// Summary description for JawiNameXmlTest
    /// </summary>
    [TestClass]
    public class JawiNameXmlTest
    {
        public JawiNameXmlTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        [TestMethod]
        public void CountDeceasedNameRepeatency()
        {
            DataSet dataSet = new DataSet();
            dataSet.ReadXml("jawiname.xml");
            DataTable original = dataSet.Tables[0];


            DataTable result = new DataTable("jawiname");
            Dictionary<string, character> characters = new Dictionary<string, character>();
            //mount to a relative path
            DirectoryInfo directoryInfo = new DirectoryInfo("..\\..\\..\\..\\..\\JawiName");
            FileInfo[] filesInfo = directoryInfo.GetFiles();
            foreach (FileInfo info in filesInfo)
            {
                string[] rootNames = info.FullName.ToLower()
                    .TrimEnd(new char[] { 's', 'f', '.' })
                    .Split(new char[] { ' ' });
                foreach (string s in rootNames)
                {
                    if (!characters.ContainsKey(s))
                        characters.Add(s, new character(s, GetArabicString(original, s)));
                    else
                        characters[s].counter += 1;
                }
            }

            dataSet.Tables.Add(result);
            dataSet.AcceptChanges();
            dataSet.WriteXml("result.xml");
        }
        private string GetArabicString(DataTable sender, string english)
        {
            string result = string.Empty;
            foreach (DataRow row in sender.Rows)
            {
                if (english.ToLower().Equals(
                    row["english"].ToString().ToLower()))
                    return row["arabic"].ToString();
            }

            return result;
        }
    }

    class character
    {
        public string english { get; set; }
        public string arabic { get; set; }
        public int counter { get; set; }
        public character(string english, string arabic)
        {
            this.english = english;
            this.arabic = arabic;
            this.counter = 0;
        }
    }
}