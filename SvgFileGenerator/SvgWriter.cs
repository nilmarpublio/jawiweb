using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace SvgFileGenerator
{
    /// <summary>
    /// An svg writer class.
    /// </summary>
    /// <remarks>
    /// 2010-08-25: Use a StreamReader to find and replace the text context then TextWriter write a file.
    /// In this version, must ensure maintain the svg file according to predefined template.
    /// future: Use XElement, Xml serializer method for better performance.
    /// http://blogs.msdn.com/b/xmlteam/archive/2007/03/24/streaming-with-linq-to-xml-part-2.aspx
    /// http://www.albahari.com/nutshell/ch11.aspx
    /// http://msdn.microsoft.com/en-us/library/system.xml.linq.xelement.writeto.aspx
    /// </remarks>
    public class SvgWriter
    {
        /// <summary>
        /// Writing mode.
        /// </summary>
        enum Action
        {
            None,
            IsWritingName,
            IsWritingJawi,
            IsWritingDeath,
            IsWritingMuslimDeath,
            /// <summary>
            /// Lookup correct arabic character then.
            /// </summary>
            IsWritingMuslimMonth,
            /// <summary>
            /// Determine muslim month lookup a template svg template file.
            /// </summary>
            IsWritingMuslimMonthGlyph,
            IsWritingBorn,
            /// <summary>
            /// Temporarily stop writing until svg read to valid line.
            /// </summary>
            StopWrite,
        }

        #region Fields
        private string templatePath;
        private nisanOrder order;
        private StreamReader reader;
        private TextWriter writer;
        private Action action;
        private string[] muslimMonthFileNames = new string[12]{
            "muharram","safar","rabiulawal","rabiulakhir","jamadilawal","jamadilakhir",
            "rejab","syaaban","ramadhan","syawal","zulkaedah","zulhijjah"
        };
        private string[] muslimMonths = new string[12]{
               "محرّم","صفر","ربيع الاول","ربيع الاخير","جمادالاول","جمادالاخير",
               "رجب","شعبان","رمضان","شوال","ذوالقعده","ذوالحجه"
        };
        private Coordinate[] monthCoordinates = new Coordinate[12]{
            new Coordinate(),
            new Coordinate(),
            new Coordinate(),
            new Coordinate(229.04117,383.33331),
            new Coordinate(),
            new Coordinate(),
            new Coordinate(),
            new Coordinate(265.04117,385.33331),
            new Coordinate(),
            new Coordinate(),
            new Coordinate(),
            new Coordinate(),
        };
        /// <summary>
        /// Indicate relative coordinate for month template to include.
        /// </summary>
        private Dictionary<string, Coordinate> relativeMonthCoordinates;
        /// <summary>
        /// Indicate how many svg syntax appear in this document.
        /// </summary>
        private int svgSyntaxCounter;
        /// <summary>
        /// Markup to keep track indent of new svg syntax.
        /// </summary>
        private int indent;
        #endregion

        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="order">Order.</param>
        /// <param name="templatePath">Lookup template file.</param>
        public SvgWriter(nisanOrder order, string templatePath)
        {
            this.order = order;
            this.templatePath = templatePath;

            this.reader = string.IsNullOrEmpty(order.born)
                ? new StreamReader(templatePath)
                : new StreamReader(templatePath.Replace(".svg", "2.svg"));
            this.writer = new StreamWriter(order.name.ToLower() + ".svg");
            this.action = Action.None;

            //initialize position for new muslim month template suppose to located.
            this.relativeMonthCoordinates = new Dictionary<string, Coordinate>();
            for (int i = 0; i < 12; i++)
                this.relativeMonthCoordinates.Add(muslimMonths[i], monthCoordinates[i]);
        }

        #region Methods
        public void Write()
        {
            try
            {
                string line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    CheckAction(line);
                    //writer.WriteLine(line);
                    line = reader.ReadLine();
                }

                writer.Flush();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            finally { reader.Close(); writer.Close(); }
        }
        private void CheckAction(string line)
        {
            //"name","jawi","deathmuslim","deathmonth","death"
            if (line.Contains("id=\"name\""))
                this.action = Action.IsWritingName;
            else if (line.Contains("id=\"jawi\""))
                this.action = Action.IsWritingJawi;
            else if (line.Contains("id=\"deathmuslim\""))
                this.action = Action.IsWritingMuslimDeath;
            else if (line.Contains("id=\"deathmonth\""))
            {
                if (this.action != Action.StopWrite)
                    this.action = Action.IsWritingMuslimMonth;
            }

            else if (line.Contains("<svg"))
            {
                this.svgSyntaxCounter++;
                this.indent++;
                if (this.svgSyntaxCounter > 1)
                    this.action = Action.IsWritingMuslimMonthGlyph;
            }
            //else if (line.Contains("<"))
            //{
            //    if (this.svgSyntaxCounter > 1)
            //        this.indent++;
            //}
            //else if (line.Contains("/>"))
            //{
            //    if (this.svgSyntaxCounter > 1)
            //        this.indent--;
            //    if (this.svgSyntaxCounter > 1 && this.indent == 0)
            //        this.action = Action.None;
            //}

            else if (line.Contains("<svg>"))
            {
                this.svgSyntaxCounter++;
                if (this.svgSyntaxCounter > 1)
                    this.action = Action.IsWritingMuslimMonthGlyph;
            }
            else if (line.Contains("</svg>"))
            {
                if (this.svgSyntaxCounter > 1)
                    this.action = Action.None;
            }
            else if (line.Contains("id=\"death\""))
                this.action = Action.IsWritingDeath;
            else if (line.Contains("id=\"borndate\""))
                this.action = Action.IsWritingBorn;


            switch (this.action)
            {
                case Action.IsWritingName:
                    WaitToWriteName(line);
                    break;
                case Action.IsWritingJawi:
                    WaitToWriteJawi(line);
                    break;
                case Action.IsWritingMuslimDeath:
                    WaitToWriteMuslimDeath(line);
                    break;
                case Action.IsWritingMuslimMonth:
                    WaitToWriteMuslimMonth(line);
                    break;
                case Action.IsWritingMuslimMonthGlyph:
                    WaitToWriteMuslimMonthGlyph(line);
                    break;
                case Action.IsWritingDeath:
                    WaitToWriteDeath(line);
                    break;
                case Action.IsWritingBorn:
                    WaitToWriteBorn(line);
                    break;
                case Action.StopWrite:
                    //do nothing
                    break;
                default:
                    writer.WriteLine(line);
                    break;
            }
        }
        private void WriteElement(string value, string line)
        {
            int start = line.IndexOf('>');
            if (start > -1)
            {
                try
                {
                    string newLine = string.Empty;
                    newLine += line.Substring(0, start + 1);
                    newLine += value;
                    int end = line.IndexOf('<');
                    if (end > -1)
                    {
                        //string original = line.Substring(start + 1, end - start - 1);
                        newLine += line.Substring(end, line.Length - end);
                    }

                    writer.WriteLine(newLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return;
                }
                finally { this.action = Action.None; }
            }
            else
                writer.WriteLine(line);
        }
        private void WriteMuslimMonth(int month)
        {
            try
            {
                string line = "<svg";
                line += "\nx=\"" + monthCoordinates[month - 1].X + "\"";
                line += "\ny=\"" + monthCoordinates[month - 1].Y + "\"";
                line += ">";

                string fileName = muslimMonthFileNames[month - 1] + ".svg";
                line += GetMuslimMonthSvgPath(fileName);
                //let draw at writer.WriteLine() after set Action to none.
                //line += "</svg>";
                writer.WriteLine(line);
            }
            finally { this.action = Action.StopWrite; }
        }
        /// <summary>
        /// Write glyph.
        /// </summary>
        /// <param name="fileName"></param>
        /// <seealso>http://support.microsoft.com/kb/307548</seealso>
        private void WriteGlyph(string fileName)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(fileName);
                while (reader.Read())
                {
                    System.Diagnostics.Debug.WriteLine(reader.Name);
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            System.Diagnostics.Debug.Write("<" + reader.Name);
                            while (reader.MoveToNextAttribute())
                                System.Diagnostics.Debug.Write(" " + reader.Name + "='" + reader.Value + "'");
                            break;
                        case XmlNodeType.Text:
                            System.Diagnostics.Debug.WriteLine(reader.Value);
                            break;
                        case XmlNodeType.EndElement:
                            System.Diagnostics.Debug.Write("<" + reader.Name + ">");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally { this.action = Action.None; }
        }
        private string GetMuslimMonthSvgPath(string templateName)
        {
            string line = string.Empty;
            var elements = GetXMLElements(templateName, "path");

            line += "<path";
            line += "\nid=\"deathmonth\"";
            foreach (XElement e in elements)
            {
                //line = e.ToString();

                line += "\n";
                XAttribute attribute = e.Attribute(XName.Get("d"));
                line += attribute.Name + "=\"" + attribute.Value + "\" ";

                line += "\n";
                attribute = e.Attribute(XName.Get("style"));
                line += attribute.Name + "=\"" + attribute.Value + "\" ";
            }
            line += "/>";

            return line;
        }

        private void WaitToWriteName(string line)
        {
            WriteElement(order.name.ToUpper(), line);
        }
        private void WaitToWriteJawi(string line)
        {
            WriteElement(order.jawi, line);
        }
        private void WaitToWriteDeath(string line)
        {
            string[] dates = order.death.Split(new char[] { '-' });
            string date = string.Empty;
            if (dates.Length == 3)
            {
                date += Convert.ToInt32(dates[2].Substring(0, 2)).ToString() + ".";
                date += Convert.ToInt32(dates[1].Substring(0, 2)).ToString() + ".";
                date += dates[0];
                //for (int i = dates.Length - 1; i >= 0; i--)
                //    date += dates[i] + ".";
                //date = date.TrimEnd(new char[] { '.' });

                WriteElement(date, line);
            }
        }
        private void WaitToWriteMuslimDeath(string line)
        {
            string[] dates = order.deathm.Split(new char[] { '-' });
            string date = string.Empty;
            if (dates.Length == 3)
            {
                date = dates[0] + Convert.ToInt32(dates[2].Substring(0, 2)).ToString();
                WriteElement(date, line);
            }
        }
        private void WaitToWriteMuslimMonth(string line)
        {
            string[] dates = order.deathm.Split(new char[] { '-' });
            string date = string.Empty;
            if (dates.Length == 3)
            {
                int month = Convert.ToInt32(dates[1]);
                date = muslimMonths[month - 1];
                WriteElement(date, line);
            }
        }
        private void WaitToWriteMuslimMonthGlyph(string line)
        {
            string[] dates = order.deathm.Split(new char[] { '-' });
            if (dates.Length > 2)
            {
                int month = Convert.ToInt32(dates[1]);
                //WriteGlyph(muslimMonthFileNames[month - 1]+".svg");
                WriteMuslimMonth(month);
            }
        }
        private void WaitToWriteBorn(string line)
        {
            string[] dates = order.born.Split(new char[] { '-' });
            string date = string.Empty;
            if (dates.Length == 3)
            {
                date += Convert.ToInt32(dates[2].Substring(0, 2)).ToString() + ".";
                date += Convert.ToInt32(dates[1].Substring(0, 2)).ToString() + ".";
                date += dates[0];

                WriteElement(date, line);
            }
        }
        #endregion

        #region Functions
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
        private IEnumerable<XElement> GetXMLElements(string inputUrl, string elementName)
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
        #endregion
    }
    /// <summary>
    /// Coordinate class for svg positioning use.
    /// </summary>
    public class Coordinate
    {
        private double x;
        public double X { get { return this.x; } }
        private double y;
        public double Y { get { return this.y; } }
        /// <summary>
        /// Default constructor. This is a zero coordinate by default.
        /// </summary>
        /// <remarks>
        /// x=0, y=0
        /// </remarks>
        public Coordinate()
        {
            this.x = 0;
            this.y = 0;
        }
        /// <summary>
        /// Specific a valid coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Coordinate(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}