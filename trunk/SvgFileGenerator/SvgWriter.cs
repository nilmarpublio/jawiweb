using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
    /// </remarks>
    public class SvgWriter
    {
        #region Fields
        private string templatePath;
        private nisanOrder order;
        private StreamReader reader;
        private TextWriter writer;
        private Action action;
        private string[] muslimMonths = new string[12]{
               "محرّم","صفر","ربيع الاول","ربيع الاخير","جمادالاول","جمادالاخير",
               "رجب","شعبان","رمضان","شوال","ذوالقعده","ذوالحجه"
        };
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

            if (string.IsNullOrEmpty(order.born))
                this.reader = new StreamReader(templatePath);
            else
                this.reader = new StreamReader(templatePath.Replace(".svg", "2.svg"));
            this.writer = new StreamWriter(order.name + ".svg");
            this.action = Action.None;
        }

        #region Methods
        public void Save()
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
                this.action = Action.IsWritingMuslimMonth;
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
                case Action.IsWritingDeath:
                    WaitToWriteDeath(line);
                    break;
                case Action.IsWritingBorn:
                    WaitToWriteBorn(line);
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
                int end = line.IndexOf('<');
                string newLine = string.Empty;
                newLine += line.Substring(0, start + 1);
                newLine += value;
                newLine += line.Substring(end, line.Length - end);

                writer.WriteLine(newLine);
                this.action = Action.None;
            }
            else
                writer.WriteLine(line);
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

        enum Action
        {
            None,
            IsWritingName,
            IsWritingJawi,
            IsWritingDeath,
            IsWritingMuslimDeath,
            IsWritingMuslimMonth,
            IsWritingBorn,
        }
    }
}