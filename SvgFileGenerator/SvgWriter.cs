﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

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
            StopWriting,
            IsWritingAge,
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
            new Coordinate(263.04117,411.14032),
            new Coordinate(269.04117,401.14032),
            new Coordinate(253.04117,405.14032),
            new Coordinate(231.04117,401.14032),
            new Coordinate(245.04117,405.14032),
            new Coordinate(231.04117,401.14032),
            new Coordinate(267.04117,419.14032),
            new Coordinate(263.04117,403.14032),
            new Coordinate(253.04117,401.14032),
            new Coordinate(271.04117,409.14032),
            new Coordinate(251.04117,401.14032),
            new Coordinate(249.04117,405.14032),
        };
        /// <summary>
        /// Determine tolerance for relative coordinate if it is a different set template of default.
        /// </summary>
        private Coordinate tolerance;
        /// <summary>
        /// Indicate relative coordinate for month template to include.
        /// </summary>
        private Dictionary<string, Coordinate> relativeMonthCoordinates;
        /// <summary>
        /// The output generated file location.
        /// </summary>
        /// <remarks>
        /// A folder named as 'Output'.
        /// </remarks>
        private string outputLocation = "Output";
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
            this.action = Action.None;
            this.tolerance = new Coordinate();

            //initialize position for new muslim month template suppose to located.
            this.relativeMonthCoordinates = new Dictionary<string, Coordinate>();
            for (int i = 0; i < 12; i++)
                this.relativeMonthCoordinates.Add(muslimMonths[i], monthCoordinates[i]);

            /**
             * If only maintain death date use 'nisan_L.svg' (for male) or 'nisan_P.svg' (for female).
             * If maintain born use 'nisan_L2.svg'.
             * If maintain age as well use 'nisan_L3.svg'.
             */
            string file = string.Empty;
            if (!string.IsNullOrEmpty(order.age))
            {
                file = templatePath.Replace(".svg", "3.svg");
                this.tolerance = new Coordinate(0, -20.00);
            }
            else if (!string.IsNullOrEmpty(order.born))
            {
                file = templatePath.Replace(".svg", "2.svg");
            }
            else
                file = templatePath;

            if (!File.Exists(file)) return;
            this.reader = new StreamReader(file);

            if (!Directory.Exists(outputLocation)) Directory.CreateDirectory(outputLocation);
            this.writer = new StreamWriter(outputLocation + System.IO.Path.DirectorySeparatorChar + order.name.ToLower() + ".svg");
        }

        #region Methods
        public bool Write()
        {
            bool done = true;
            if (null == reader) return false;

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
                return done;
            }
            catch (Exception ex)
            {
                //todo: how to handle error in SvgWriter.Write()?
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            finally
            {
                if (null != reader) reader.Close();
                if (null != writer) writer.Close();
            }
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
                this.action = Action.IsWritingMuslimMonth;
            else if (line.Contains("id=\"muslimMonthGlyph\"")) //else if (line.Contains("<svg"))
                this.action = Action.IsWritingMuslimMonthGlyph;
            else if (line.Contains("id=\"death\""))
                this.action = Action.IsWritingDeath;
            else if (line.Contains("id=\"borndate\""))
                this.action = Action.IsWritingBorn;
            else if (line.Contains("id=\"age\""))
                this.action = Action.IsWritingAge;


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
                case Action.IsWritingAge:
                    WaitToWriteAge(line);
                    break;
                case Action.StopWriting:
                    //do nothing
                    break;
                default:
                    writer.WriteLine(line);
                    break;
            }

            if (line.Contains("</svg>"))
                this.action = Action.None;
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

        /// <summary>
        /// Write glyph.
        /// </summary>
        /// <remarks>
        /// No use temporarily.
        /// </remarks>
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
        /// <summary>
        /// Retrieve svg path tag value.
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        private string GetMuslimMonthSvgPath(string templateName)
        {
            string line = string.Empty;
            var elements = new SvgReader(templateName).GetXMLElements("path");

            line += "<path";
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
        private void WriteMuslimMonth(int month)
        {
            try
            {
                string line = "<svg id=\"muslimMonthGlyph\"";
                line += "\nx=\"" + (monthCoordinates[month - 1].X + tolerance.X) + "\"";
                line += "\ny=\"" + (monthCoordinates[month - 1].Y + tolerance.Y) + "\"";
                line += ">";

                string fileName = muslimMonthFileNames[month - 1] + ".svg";
                line += GetMuslimMonthSvgPath(fileName);
                line += "</svg>";
                writer.WriteLine(line);
            }
            finally { this.action = Action.StopWriting; }
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
        private void WaitToWriteAge(string line)
        {
            WriteElement(order.age, line);
        }

        public static System.Windows.Shapes.Path CreatePath(List<System.Windows.Point> rawData)
        {
            System.Windows.Shapes.Path finalPath = new System.Windows.Shapes.Path();
            PathGeometry finalPathGeometry = new PathGeometry();
            PathFigure primaryFigure = new PathFigure();

            //if you want the path to be a shape, you want to close the PathFigure
            //   that makes up the Path. If you want it to simply by a line, set
            //   primaryFigure.IsClosed = false;
            primaryFigure.IsClosed = true;
            primaryFigure.StartPoint = rawData[0];

            PathSegmentCollection lineSegmentCollection = new PathSegmentCollection();
            for (int i = 1; i < rawData.Count; i++)
            {
                LineSegment newSegment = new LineSegment();
                newSegment.Point = rawData[i];
                lineSegmentCollection.Add(newSegment);
            }

            primaryFigure.Segments = lineSegmentCollection;
            finalPathGeometry.Figures.Add(primaryFigure);
            finalPath.Data = finalPathGeometry;

            return finalPath;
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