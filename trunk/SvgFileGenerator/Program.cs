using System;
using System.Collections.Generic;
//using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Configuration;

namespace SvgFileGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            XmlSerializer serializer = new XmlSerializer(typeof(nisan));
            FileStream stream = new FileStream("nisan.xml", FileMode.Open);
            nisan nisans = serializer.Deserialize(stream) as nisan;
            stream.Flush();
            stream.Close();
            System.Diagnostics.Debug.WriteLine(nisans.Items.Length);

            List<nisanOrder> orders = new List<nisanOrder>();
            List<nisanPurchase> purchases = new List<nisanPurchase>();
            foreach (object obj in nisans.Items)
            {
                if (obj.GetType() == typeof(nisanOrder))
                    orders.Add(obj as nisanOrder);
                if (obj.GetType() == typeof(nisanPurchase))
                    purchases.Add(obj as nisanPurchase);
            }
            System.Diagnostics.Debug.WriteLine("Total order: " + orders.Count);
            System.Diagnostics.Debug.WriteLine("Total purchase: " + purchases.Count);


            //get undelivered order
            int length = ConfigurationManager.AppSettings.Keys.Count;
            for (int i = 0; i < length; i++)
            {
                ConfigurationManager.AppSettings.Keys[i].ToString();
                string item = ConfigurationManager.AppSettings.Keys[i].ToString();
                string[] lookupFiles = ConfigurationManager.AppSettings.GetValues(i);
                if (lookupFiles.Length < 1) continue;
                List<nisanOrder> undelivered = orders
                    .Where(f => f.delivered.Length == 0 && f.item == item).ToList<nisanOrder>();
                Console.WriteLine(string.Format("There are {0}:{1} pending", item, undelivered.Count));
                foreach (nisanOrder order in undelivered)
                {
                    Console.WriteLine("Writing " + order.name + ".svg...");
                    SvgWriter writer = new SvgWriter(order, lookupFiles[0]);
                    writer.Save();
                }
                Console.WriteLine();
            }
            Console.WriteLine("Complete");

            //wait for user input
            Console.Read();
        }

        /*static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            XmlSerializer serializer = new XmlSerializer(typeof(nisan));
            FileStream stream = new FileStream("nisan.xml", FileMode.Open);
            nisan nisans = serializer.Deserialize(stream) as nisan;
            stream.Flush();
            stream.Close();
            System.Diagnostics.Debug.WriteLine(nisans.Items.Length);

            List<nisanOrder> orders = new List<nisanOrder>();
            List<nisanPurchase> purchases = new List<nisanPurchase>();
            foreach (object obj in nisans.Items)
            {
                if (obj.GetType() == typeof(nisanOrder))
                    orders.Add(obj as nisanOrder);
                if (obj.GetType() == typeof(nisanPurchase))
                    purchases.Add(obj as nisanPurchase);
            }
            System.Diagnostics.Debug.WriteLine("Total order: " + orders.Count);
            System.Diagnostics.Debug.WriteLine("Total purchase: " + purchases.Count);


            //get undelivered order
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.AppSettings.Settings["dataPath"].Value
            int length = ConfigurationManager.AppSettings.Keys.Count;
            for (int i = 0; i < length; i++)
            {
                ConfigurationManager.AppSettings.Keys[i].ToString();
                string item = ConfigurationManager.AppSettings.Keys[i].ToString();
                string[] lookupFiles = ConfigurationManager.AppSettings.GetValues(i);
                if (lookupFiles.Length < 1) continue;
                List<nisanOrder> undelivered = orders
                    .Where(f => f.delivered.Length == 0 && f.item == item).ToList<nisanOrder>();
                System.Diagnostics.Debug.WriteLine(string.Format("There are {0}:{1} pending", item, undelivered.Count));
                foreach (nisanOrder order in undelivered)
                {
                    var elements = GetXMLElements(lookupFiles[0], "text");
                    System.Diagnostics.Debug.Write("found text element: " + elements.Count());
                    foreach (XElement e in elements)
                    {
                        XAttribute attribute = e.Attribute(XName.Get("id"));
                        string message = string.Format("Name: {0} Value:{1}", e.Name, e.Value);
                        System.Diagnostics.Debug.WriteLine(message);
                    }
                }
            }
        }*/
        /// <summary>
        /// Returns element of an XML file.
        /// </summary>
        /// <param name="inputUrl"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        /// <remarks>
        /// Alternative code:
        /// <example>
        /// <code>
        /// XmlReader reader = XmlReader.Create(lookupFiles[0]);
        /// while (reader.Read())
        /// {
        ///    switch (reader.NodeType)
        ///    {
        ///        case XmlNodeType.Element:
        ///            string message = string.Format("Name: {0} Value:{1}", reader.Name, reader.Value);
        ///            System.Diagnostics.Debug.WriteLine(message);
        ///            break;
        ///        case XmlNodeType.Attribute:
        ///            break;
        ///    }
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        /// <seealso>http://stackoverflow.com/questions/2441673/reading-xml-with-xmlreader-in-c</seealso>
        /// <seealso>http://support.microsoft.com/kb/307548</seealso>
        static IEnumerable<XElement> GetXMLElements(string inputUrl, string elementName)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            using (XmlReader reader = XmlReader.Create(inputUrl, settings))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == elementName)
                        {
                            XElement e = XNode.ReadFrom(reader) as XElement;
                            if (null != e) yield return e;
                        }
                    }
                }
            }
        }
    }
}