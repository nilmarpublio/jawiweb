using System;
using System.Collections.Generic;
//using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Configuration;
using HLGranite.Jawi;

namespace SvgFileGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// TODO: refactor into a class?
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            int x = 0;//pending count
            int y = 0;//success generated count

            List<nisanOrder> orders = new List<nisanOrder>();
            NisanReader reader = new NisanReader("nisan.xml");
            reader.Read(out orders);
            //System.Diagnostics.Debug.WriteLine("Total order: " + orders.Count);
            //System.Diagnostics.Debug.WriteLine("Total purchase: " + purchases.Count);

            //get undelivered order
            int length = ConfigurationManager.AppSettings.Keys.Count;
            for (int i = 0; i < length; i++)
            {
                //hack: refactor by get all item values and query with linq
                string item = ConfigurationManager.AppSettings.Keys[i].ToString();
                string[] lookupFiles = ConfigurationManager.AppSettings.GetValues(i);
                if (lookupFiles.Length < 1) continue;
                List<nisanOrder> undelivered = orders
                    .Where(f => f.name.Length > 0 && f.delivered.Length == 0 && f.item == item)
                    .ToList<nisanOrder>();
                if (undelivered.Count > 0)
                {
                    x += undelivered.Count;
                    Console.WriteLine();
                    Console.WriteLine(string.Format("There are {0}:{1} pending", item, undelivered.Count));
                }

                foreach (nisanOrder order in undelivered)
                {
                    Console.Write("Writing " + order.name.ToLower() + ".svg...");
                    // validation
                    if (order.death.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) > 0) Console.Write(" Are you cursing people?");
                    if(order.age != null && order.age.Length > 0)
                    {
                        if (Convert.ToInt32(order.age) > 120) Console.Write(" Are you kidding?");
                    }
                    SvgWriter writer = new SvgWriter(order, lookupFiles[0]);
                    if (writer.Write())
                    {
                        Console.WriteLine(" done");
                        y++;
                    }
                    else
                        Console.WriteLine(" exist or fail");
                }
            }
            Console.WriteLine("Complete");
            Console.WriteLine(string.Format("{0} pending. {1} files generated.", x, y));

            //wait for user input
            Console.Read();
        }
    }
}