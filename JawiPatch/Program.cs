using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace JawiPatch
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Start rename sticker item according to male or female item.");
            //BreakStickerItem();
            //Console.WriteLine("Complete");

            //20100917tys
            WordScales wordScales = new WordScales();
            wordScales.Option = WordScalesOption.Word;

            string content = string.Empty;
            DirectoryInfo directoryInfo = new DirectoryInfo(@"E:\jawiname");
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
                content += file.Name.ToLower() + " ";
            wordScales.Count(content);
            foreach (KeyValuePair<string, int> item in wordScales.Result)
                Console.WriteLine("{0}\t{1}", item.Key, item.Value);

            Console.Read();
        }
        /// <summary>
        /// Rename sticker item according to male or female item if has.
        /// </summary>
        /// <remarks>
        /// since 2010-09-07.
        /// </remarks>
        private static void BreakStickerItem()
        {
            DataSet dataSet = new DataSet();
            dataSet.ReadXml("nisan.xml");
            if (dataSet.Tables.Count == 0) return;

            DataTable table = dataSet.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                if (row["item"].ToString() == "1½' Sticker")
                {
                    if (row["name"].ToString().Contains(" bin "))
                        row["item"] = "1½' Sticker(L)";
                    if (row["name"].ToString().Contains(" bt "))
                        row["item"] = "1½' Sticker(P)";
                }
                if (row["item"].ToString() == "2' Sticker")
                {
                    if (row["name"].ToString().Contains(" bin "))
                        row["item"] = "2' Sticker(L)";
                    if (row["name"].ToString().Contains(" bt "))
                        row["item"] = "2' Sticker(P)";
                }
                if (row["item"].ToString() == "2½' Sticker")
                {
                    if (row["name"].ToString().Contains(" bin "))
                        row["item"] = "2½' Sticker(L)";
                    if (row["name"].ToString().Contains(" bt "))
                        row["item"] = "2½' Sticker(P)";
                }
            }
            table.AcceptChanges();
            table.WriteXml("nisan2.xml");
        }
    }
}