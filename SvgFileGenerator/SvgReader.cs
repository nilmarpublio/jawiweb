using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace SvgFileGenerator
{
    public class SvgReader
    {
        private nisan nisans;
        public SvgReader(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(nisan));
            FileStream stream = new FileStream(path, FileMode.Open);
            nisans = serializer.Deserialize(stream) as nisan;
            stream.Flush();
            stream.Close();
            System.Diagnostics.Debug.WriteLine(nisans.Items.Length);
        }
        public void Read(out List<nisanOrder> orders)
        {
            orders = new List<nisanOrder>();
            foreach (object obj in nisans.Items)
            {
                if (obj.GetType() == typeof(nisanOrder))
                    orders.Add(obj as nisanOrder);
                //if (obj.GetType() == typeof(nisanPurchase))
                //purchases.Add(obj as nisanPurchase);
            }
        }
    }
}
