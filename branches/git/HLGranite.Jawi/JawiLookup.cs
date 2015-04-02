using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HLGranite.Jawi
{
    public class JawiLookup
    {
        private DataTable dataSource;
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public JawiLookup()
        {
            this.dataSource = ReadXml("jawiname.xml");
            foreach (DataRow row in this.dataSource.Rows)
            {
                dictionary.Add(row["rumi"].ToString(), row["jawi"].ToString());
            }
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
                    table = dataset.Tables["rootname"].Copy();

                return table;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return table;
            }
            finally { dataset.Dispose(); }
        }
        /// <summary>
        /// Lookup corresponding value from data source.
        /// </summary>
        /// <param name="rumi"></param>
        /// <returns></returns>
        public string Lookup(string rumi)
        {
            string value = "";
            if (rumi != null) rumi = rumi.ToLower();
            this.dictionary.TryGetValue(rumi, out value);

            // و phoenic
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("o", "u");
                this.dictionary.TryGetValue(newRumi, out value);
            }
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("u", "o");
                this.dictionary.TryGetValue(newRumi, out value);
            }

            // ي phoenic
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("y", "i");
                this.dictionary.TryGetValue(newRumi, out value);
            }
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("ee", "i");
                this.dictionary.TryGetValue(newRumi, out value);
            }

            // ف phoenic
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("p", "f");
                this.dictionary.TryGetValue(newRumi, out value);
            }
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("f", "p");
                this.dictionary.TryGetValue(newRumi, out value);
            }

            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("eu", "y");
                this.dictionary.TryGetValue(newRumi, out value);
            }

            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("ff", "f");
                this.dictionary.TryGetValue(newRumi, out value);
            }
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("f", "ff");
                this.dictionary.TryGetValue(newRumi, out value);
            }
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("ss", "s");
                this.dictionary.TryGetValue(newRumi, out value);
            }
            if (string.IsNullOrEmpty(value))
            {
                string newRumi = rumi.Replace("s", "ss");
                this.dictionary.TryGetValue(newRumi, out value);
            }



            return value;
        }
    }
}