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


            List<nisanOrder> orders = new List<nisanOrder>();
            NisanReader reader = new NisanReader("nisan.xml");
            reader.Read(out orders);
            //System.Diagnostics.Debug.WriteLine("Total order: " + orders.Count);
            //System.Diagnostics.Debug.WriteLine("Total purchase: " + purchases.Count);

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
                    writer.Write();
                }
                Console.WriteLine();
            }
            Console.WriteLine("Complete");

            //wait for user input
            Console.Read();
        }
    }
}