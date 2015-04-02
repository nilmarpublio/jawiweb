using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace HLGranite.Jawi
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
        //TODO: relocate month glym position
        private Point[] monthCoordinates = new Point[12]{
            new Point(173.04117,351.14032),
            new Point(179.04117,341.14032),
            new Point(163.04117,345.14032),
            new Point(141.04117,341.14032),
            new Point(155.04117,345.14032),
            new Point(141.04117,341.14032),
            new Point(177.04117,359.14032),
            new Point(173.04117,343.14032),
            new Point(177.04117,355.14032),
            new Point(181.04117,349.14032),
            new Point(161.04117,341.14032),
            new Point(165.04117,349.14032),
        };
        /// <summary>
        /// Determine tolerance for relative coordinate if it is a different set template of default.
        /// </summary>
        private Point tolerance;
        /// <summary>
        /// Indicate relative coordinate for month template to include.
        /// </summary>
        private Dictionary<string, Point> relativeMonthCoordinates;
        /// <summary>
        /// The output generated file location.
        /// </summary>
        /// <remarks>
        /// A folder named as 'Output'.
        /// </remarks>
        private string outputLocation = "Output";
        private Panel workspace;
        private bool fileExist;
        #endregion
        
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
            this.tolerance = new Point();

            //initialize position for new muslim month template suppose to located.
            this.relativeMonthCoordinates = new Dictionary<string, Point>();
            for (int i = 0; i < 12; i++)
                this.relativeMonthCoordinates.Add(muslimMonths[i], monthCoordinates[i]);

            /**
             * If only maintain death date use 'nisan_L.svg' (for male) or 'nisan_P.svg' (for female).
             * If maintain born use 'nisan_L2.svg'.
             * If maintain age as well use 'nisan_L3.svg'.
             */
            string file = string.Empty;
            if(!string.IsNullOrEmpty(order.death))
              	file = templatePath.Replace(".svg", "0.svg");
            else if(!string.IsNullOrEmpty(order.age) && !string.IsNullOrEmpty(order.born))
            {
               	//tolerance for born template
                file = templatePath.Replace(".svg", "4.svg");
                this.tolerance = new Point(0, -20.00);
            }
            if (!string.IsNullOrEmpty(order.age))
            {
            	//tolerance for age template
                file = templatePath.Replace(".svg", "3.svg");
                this.tolerance = new Point(0, -4.00);
            }
            else if (!string.IsNullOrEmpty(order.born))
            {
            	//tolerance for born template
                file = templatePath.Replace(".svg", "2.svg");
                this.tolerance = new Point(0, -20.00);
            }
            else
                file = templatePath;

            //this is a female template with need to move on top a little bit
            if (order.item.Contains("(P)")) this.tolerance = new Point(0, -20.00);

            this.sourceFile = this.outputLocation + System.IO.Path.DirectorySeparatorChar + order.name.ToLower() + ".svg";
            if (File.Exists(this.sourceFile))
            {
                fileExist = true;
            }
            else
            {
                if (File.Exists(file)) this.reader = new StreamReader(file);
                if (!Directory.Exists(outputLocation)) Directory.CreateDirectory(outputLocation);
                this.writer = new StreamWriter(sourceFile);
            }
        }
        /// <summary>
        /// Constructor for write a jawi character only.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="workspace"></param>
        public SvgWriter(string fileName, Panel workspace)
        {
            this.writer = new StreamWriter(fileName);
            this.workspace = workspace;
        }

        private string sourceFile;
        /// <summary>
        /// For merge use only.
        /// </summary>
        /// <param name="fileName"></param>
        public SvgWriter(string fileName)
        {
            this.sourceFile = this.outputLocation + System.IO.Path.DirectorySeparatorChar + fileName;
            if (File.Exists(this.sourceFile)) fileExist = true;
        }

        public SvgWriter(nisanOrder order, ref MemoryStream stream)
        {
            this.order = order;
            this.templatePath = ConfigurationManager.AppSettings[order.item].ToString();
            this.action = Action.None;
            this.tolerance = new Point();

            //initialize position for new muslim month template suppose to located.
            this.relativeMonthCoordinates = new Dictionary<string, Point>();
            for (int i = 0; i < 12; i++)
                this.relativeMonthCoordinates.Add(muslimMonths[i], monthCoordinates[i]);

            /**
             * If only maintain death date use 'nisan_L.svg' (for male) or 'nisan_P.svg' (for female).
             * If maintain born use 'nisan_L2.svg'.
             * If maintain age as well use 'nisan_L3.svg'.
             */
            string file = string.Empty;
            if (!string.IsNullOrEmpty(order.death))
                file = templatePath.Replace(".svg", "0.svg");
            else if (!string.IsNullOrEmpty(order.age) && !string.IsNullOrEmpty(order.born))
            {
                //tolerance for born template
                file = templatePath.Replace(".svg", "4.svg");
                this.tolerance = new Point(0, -20.00);
            }
            if (!string.IsNullOrEmpty(order.age))
            {
                //tolerance for age template
                file = templatePath.Replace(".svg", "3.svg");
                this.tolerance = new Point(0, -4.00);
            }
            else if (!string.IsNullOrEmpty(order.born))
            {
                //tolerance for born template
                file = templatePath.Replace(".svg", "2.svg");
                this.tolerance = new Point(0, -20.00);
            }
            else
                file = templatePath;

            //this is a female template with need to move on top a little bit
            if (order.item.Contains("(P)")) this.tolerance = new Point(0, -20.00);

            this.writer = new StreamWriter(stream);
        }

        #region Methods
        /// <summary>
        /// Write to a svg file.
        /// </summary>
        /// <remarks>
        /// After library complete or mature, counter function can be skipped.
        /// </remarks>
        /// <returns></returns>
        public bool Write()
        {
            bool done = false;
            if(fileExist) return done;

            if (null != this.workspace) return done = WriteWorkspace();
            if (null != this.reader) return done = Cloning();
            return done;
        }
        /// <summary>
        /// Write into a svg file then increase the counter.
        /// </summary>
        /// <param name="wordCollection"></param>
        /// <returns></returns>
        public bool Write(WordCollection wordCollection)
        {
            bool done = false;
            if(fileExist) return done;

            CountWord(wordCollection);
            if (null != this.workspace) return done = WriteWorkspace();
            if (null != this.reader) return done = Cloning();
            return done;
        }
        /// <summary>
        /// Write counter into a file.
		/// </summary>
		/// <remarks>
		/// Count file name instead of name only.
        /// </remarks>
        /// <param name="wordCollection"></param>
        private void CountWord(WordCollection wordCollection)
        {
            Counter counter = new Counter();

            TextCollection textCollection = new TextCollection();
            PunctuationCollection punctuationCollection = new PunctuationCollection();
            foreach (PathViewModel item in wordCollection.SelectedWords)
            {
                bool isText = textCollection.Items.Where(f => f.Name.ToLower().Contains(item.Name.ToLower())).Count() == 0
                    ? false : true;
                bool isPunctuation = punctuationCollection.Items.Where(f => f.Name.ToLower().Contains(item.Name.ToLower())).Count() == 0
                    ? false : true;
                if (!isText && !isPunctuation)
                {
                    counter.Add(item.Name + item.Label);
                    System.Diagnostics.Debug.WriteLine(item.Name + item.Label);
                }
            }

            counter.Store();
        }
        private bool Cloning()
        {
            bool done = true;
            if (null == reader) return false;

            try
            {
                string line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    CheckAction(line);
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
            else if (line.Contains("id=\"muslimMonthGlyph\""))
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
                    	newLine += line.Substring(end, line.Length - end);

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

        private void WaitToWriteName(string line)
        {
            if (order.name == null) return;
            WriteElement(order.name.ToUpper(), line);
        }
        private void WaitToWriteJawi(string line)
        {
            if (order.jawi == null) return;
            WriteElement(order.jawi, line);
        }
        private void WaitToWriteDeath(string line)
        {
            if (order.death == null) return;
            string[] dates = order.death.Split(new char[] { '-' });
            string date = string.Empty;
            if (dates.Length == 1)
                date += dates[0];
            else if (dates.Length == 3)
            {
                date += Convert.ToInt32(dates[2].Substring(0, 2)).ToString() + ".";
                date += Convert.ToInt32(dates[1].Substring(0, 2)).ToString() + ".";
                date += dates[0];
            }

            WriteElement(date, line);
        }
        private void WaitToWriteMuslimDeath(string line)
        {
            if (order.deathm == null) return;
            string[] dates = order.deathm.Split(new char[] { '-' });
            string date = string.Empty;
            //if (dates.Length == 1)
            //    date += dates[0];
            if (dates.Length == 3)
            {
                date = dates[0] + Convert.ToInt32(dates[2].Substring(0, 2)).ToString();
                WriteElement(date, line);
            }
        }
        private void WaitToWriteMuslimMonth(string line)
        {
            if (order.deathm == null) return;
            string[] dates = order.deathm.Split(new char[] { '-' });
            string date = string.Empty;
            if (dates.Length == 3)
            {
                int month = Convert.ToInt32(dates[1]);
                date = muslimMonths[month - 1];
                WriteElement(date, line);
            }
        }
        private void WaitToWriteBorn(string line)
        {
            if (order.born == null) return;
            string[] dates = order.born.Split(new char[] { '-' });
            string date = string.Empty;
            if (dates.Length == 1)
                date += dates[0];
            else if (dates.Length == 3)
            {
                date += Convert.ToInt32(dates[2].Substring(0, 2)).ToString() + ".";
                date += Convert.ToInt32(dates[1].Substring(0, 2)).ToString() + ".";
                date += dates[0];
            }
            WriteElement(date, line);
        }
        private void WaitToWriteAge(string line)
        {
            if (order.age == null) return;
            WriteElement(order.age, line);
        }
        private void WaitToWriteMuslimMonthGlyph(string line)
        {
            IEnumerable<XElement> path = null;
            int month = 0;
            string[] dates = order.deathm.Split(new char[] { '-' });
            if (dates.Length > 2)
            {
                month = Convert.ToInt32(dates[1]);
                path = GetSvgPath(muslimMonthFileNames[month - 1] + ".svg");
            }

            if (line.Contains("transform="))
            {
                int start = line.IndexOf("transform=");
                string newLine = string.Empty;
                if (start > -1) newLine += line.Substring(0, start);
                if (month == 0)
                    newLine += string.Format("transform=\"translate({0},{1})\"", 0, 0);
                else
                    newLine += string.Format("transform=\"translate({0},{1})\"",
                        monthCoordinates[month - 1].X + tolerance.X,
                        monthCoordinates[month - 1].Y + tolerance.Y);

                string endTag = string.Empty;
                CheckEndTag(line, out endTag);
                newLine += endTag;

                WriteElement(string.Empty, newLine);
            }
            else if (null == path)
                WriteElement(string.Empty, line);
            else if (line.Contains("d=") && !line.Contains("id="))
            {
                int start = line.IndexOf("d=");
                string newLine = string.Empty;
                if (start > -1) newLine += line.Substring(0, start);
                newLine += string.Format("d=\"{0}\"", GetSvgAttribute(path, "d"));

                string endTag = string.Empty;
                CheckEndTag(line, out endTag);
                newLine += endTag;

                WriteElement(string.Empty, newLine);
            }
            else if (line.Contains("style="))
            {
                int start = line.IndexOf("style=");
                string newLine = string.Empty;
                if (start > -1) newLine += line.Substring(0, start);
                newLine += string.Format("style=\"{0}\"", GetSvgAttribute(path, "style"));

                string endTag = string.Empty;
                CheckEndTag(line, out endTag);
                newLine += endTag;

                WriteElement(string.Empty, newLine);
            }
            else
                WriteElement(string.Empty, line);
        }
        
        private void CheckEndTag(string source, out string output)
        {
            output = string.Empty;
            if (source.Contains("/>"))
            {
                output = "/>";
                return;
            }

            if (source.Contains(">"))
            {
                output = ">";
                return;
            }
        }
        private IEnumerable<XElement> GetSvgPath(string templateName)
        {
            string line = string.Empty;
            var elements = new SvgReader(templateName).GetXMLElements("path");
            return elements;
        }
        private string GetSvgAttribute(IEnumerable<XElement> sender, string attribute)
        {
            string result = string.Empty;
            foreach (XElement e in sender)
            {
                XAttribute att = e.Attribute(XName.Get(attribute));
                return result = att.Value;
            }

            return result;
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

        private string ConvertToSvgPathSyntax(System.Windows.Shapes.Path path)
        {
            string output = "<path";
            double top = path.Margin.Top;
            double left = path.Margin.Left;
            if (path.VerticalAlignment == VerticalAlignment.Bottom)
            {
                top = workspace.ActualHeight - path.ActualHeight;
                if (!path.Margin.Bottom.Equals(0.00))
                    top -= path.Margin.Bottom;
            }
            else
            {
                //allow WYSWYG
                //top = 0.00;
            }
            output += string.Format("\n\ttransform=\"translate({0},{1})\"", left, top);

            output += string.Format("\n\td=\"{0}\"", path.Data.ToString());
            //todo: handle white color with border thinkness 1
            if (path.Fill == Brushes.Black || path.Fill == Brushes.White)
                output += string.Format("\n\tstyle=\"fill:{0};stroke:none\"", path.Fill.ToString());//#000000
            else
                output += string.Format("\n\tstyle=\"fill:{0};stroke:none;fill-opacity:1\"", path.Fill.ToString().Replace("#FF", "#"));

            //path.Margin
            output += " />";

            return output;
        }
        private bool WriteWorkspace()
        {
            bool done = true;
            if (null == this.workspace) return false;

            try
            {
                //writer.WriteLine("<?xml version=\"1.0\"?>");
                //writer.WriteLine("<svg>");                
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                writer.WriteLine("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">");
                writer.WriteLine("<svg viewBox=\"0 0 600 600\" version=\"1.1\" xmlns:svg=\"http://www.w3.org/2000/svg\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">");
                foreach (UIElement child in this.workspace.Children)
                {
                    if (child is System.Windows.Shapes.Path)
                        writer.WriteLine(ConvertToSvgPathSyntax(child as System.Windows.Shapes.Path));
                }

                writer.WriteLine("</svg>");
                writer.Flush();

                return done;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            finally { if (null != writer) writer.Close(); }
        }
        /// <summary>
        /// TODO: XamlToSvgTransform.
        /// </summary>
        /// <param name="xamlFile"></param>
        /// <param name="styleSheet"></param>
        /// <param name="svgFile"></param>
        /// <returns></returns>
        /// <remarks>
        /// <example>
        /// XamlToSvgTransform("output.xaml", "xaml2svg.xsl", "output.svg");
        /// </example>
        /// </remarks>
        /// <seealso>http://blogs.msdn.com/b/ashish/archive/2008/01/15/dynamically-producing-xaml-files-using-xamlwriter-save-method.aspx</seealso>
        private string XamlToSvgTransform(string xamlFile, string styleSheet, string svgFile)
        {
            try
            {
                XsltSettings settings = new XsltSettings(true, true);
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(styleSheet, settings, new XmlUrlResolver());
                xslt.Transform(xamlFile, svgFile);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Group all jawi caligraphy and merge into generated svg file if has.
        /// </summary>
        /// <returns></returns>
        public bool Merge(string targetFile)
        {
            bool success = true;
            string stopper = "</svg>";
            if (string.IsNullOrEmpty(this.sourceFile)) return false;
            if (string.IsNullOrEmpty(targetFile)) return false;

            try
            {
                string body = string.Empty;
                string append = string.Empty;

                string line = string.Empty;
                TextReader rootReader = new StreamReader(this.sourceFile);
                while (!string.IsNullOrEmpty(line = rootReader.ReadLine()))
                {
                    if (!line.Equals(stopper))
                        body += line + "\n";
                }
                rootReader.Close();

                bool start = false;
                append += "<g id=\"jawi\"" + "\n";
                append += string.Format("\ttransform=\"translate({0},{1})\">", -60, 132);//align center of frame
                TextReader targetReader = new StreamReader(targetFile);
                while (!string.IsNullOrEmpty(line = targetReader.ReadLine()))
                {
                    if (line.Contains("<path")) start = true;
                    if (line.Contains(stopper)) start = false;

                    if (start) append += line + "\n";
                }
                append += "</g>" + "\n";
                targetReader.Close();

                this.writer = new StreamWriter(this.sourceFile);
                this.writer.Write(body);
                this.writer.Write(append);
                this.writer.WriteLine(stopper);
                this.writer.Flush();

                return success;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
            finally { if (null != this.writer) this.writer.Close(); }
        }
        #endregion

        #region Obsolete
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
        #endregion
    }
}