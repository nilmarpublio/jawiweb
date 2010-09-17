using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using System.Drawing;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace HLGranite.Jawi
{
    public class SvgReader
    {
        /// <summary>
        /// Source location.
        /// </summary>
        private string inputUrl;
        public SvgReader(string fileName)
        {
            this.inputUrl = fileName;
        }

        #region Methods
        public string GetFirstPathValue()
        {
            string pathString = string.Empty;
            var elements = GetXMLElements("path");
            foreach (XElement e in elements)
            {
                XAttribute attribute = e.Attribute(XName.Get("d"));
                pathString = attribute.Value;
                break;
            }
            return pathString;
        }
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
        public IEnumerable<XElement> GetXMLElements(string elementName)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            //todo: handle null inputUrl
            //if (string.IsNullOrEmpty(this.inputUrl)) return new List<XElement>();
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
        public static List<Point> ConvertToPointCollection(string dataString)
        {
            List<Point> data = new List<Point>();
            string[] segments = dataString.Split(new char[] { ' ' });
            foreach (string segment in segments)
            {
                if (segment.Contains(","))
                {
                    string[] xy = segment.Split(new char[] { ',' });
                    data.Add(new Point(Convert.ToDouble(xy[0]), Convert.ToDouble(xy[1])));
                }
            }

            return data;
        }
        /// <summary>
        /// TODO: Gets the rectangle size for a path raw data.
        /// </summary>
        /// <remarks>FAIL</remarks>
        /// <param name="rawDataString"></param>
        /// <returns></returns>
        public static Size GetSize(string rawDataString)
        {
            Size size = new Size();

            List<double> x = new List<double>();
            List<double> y = new List<double>();
            string[] points = rawDataString.Split(new char[] { ' ' });
            foreach (string point in points)
            {
                if (point.Contains(','))
                {
                    string[] xy = point.Split(new char[] { ',' });
                    x.Add(Convert.ToDouble(xy[0]));//Math.Abs();
                    y.Add(Convert.ToDouble(xy[1]));
                }
            }

            double height = 0.00;
            double width = 0.00;
            width = x.Max() - x.Min();
            height = y.Max() - y.Min();
            size = new Size(width, height);

            return size;
        }
        #endregion
    }
}