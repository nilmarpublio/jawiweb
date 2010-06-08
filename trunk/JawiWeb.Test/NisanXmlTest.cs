using System;
using System.Data;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JawiWeb.Test
{
    /// <summary>
    /// Unit test for JawiWeb project and some small pieces of customization snippet code.
    /// </summary>
    [TestClass]
    public class NisanXmlTest
    {
        public NisanXmlTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// A snippet code to convert existing deceased's name into lower case.
        /// </summary>
        /// <remarks>
        /// Completed at 2010-06-08 successfully.
        /// </remarks>
        [TestMethod]
        public void ConvertNameToLowerCase()
        {
            int rowCount = 0;
            int index = 0;
            string expected = string.Empty;
            string actual = string.Empty;

            string file = "nisan.xml";
            DataSet dataSet = new DataSet();
            Random random = new Random();

            try
            {
                dataSet.ReadXml(file);
                rowCount = dataSet.Tables["order"].Rows.Count;
                DataTable holdingTable = dataSet.Tables["order"].Copy();

                //start converting to lower case
                foreach (DataRow row in dataSet.Tables["order"].Rows)
                    row["name"] = row["name"].ToString().ToLower();
                dataSet.AcceptChanges();

                //try to assert at least 3 times randomly
                for (int i = 0; i < 3; i++)
                {
                    index = random.Next(rowCount);
                    expected = holdingTable.Rows[index]["name"].ToString().ToLower();
                    actual = dataSet.Tables["order"].Rows[index]["name"].ToString();
                    Assert.AreEqual(expected, actual);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally { dataSet.WriteXml(file); }
        }
    }
}