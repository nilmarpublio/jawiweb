using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HLGranite.Jawi;

namespace MuslimCalendar
{
    public partial class Form1 : Form
    {
        private HLGranite.Jawi.MuslimCalendar calendar;
        public Form1()
        {
            InitializeComponent();

            DisplayMuslimMonth();
            calendar = new HLGranite.Jawi.MuslimCalendar(ReadXml("muslimcal.xml"));
            DisplayResult(DateTime.Now);
        }
        private DataTable ReadXml(string fileName)
        {
            DataTable table = new DataTable();
            DataSet dataset = new DataSet();

            try
            {
                if (System.IO.File.Exists(fileName))
                    dataset.ReadXml(fileName);
                if (dataset.Tables.Count > 0)
                    table = dataset.Tables[0].Copy();

                return table;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return table;
            }
            finally { dataset.Dispose(); }
        }
        private void DisplayMuslimMonth()
        {
            string output = string.Empty;
            string[] rumi = new string[12]{
                "Muharam",
                "Safar",
                "Rabiulawal",
                "Rabiulakhir",
                "Jamadilawal",
                "Jamadilakhir",
                "Rejab",
                "Syaaban",
                "Ramadhan",
                "Syawal",
                "Zulkaedah",
                "Zulhijjah",
            };
            string[] muslim = new string[12]{
                "محرّم",
                "صفر",
                "ربيع الاول",
                "ربيع الاخير",
                "جمادالاول",
                "جمادالاخير",
                "رجب",
                "شعبان",
                "رمضان",
                "شوال",
                "ذوالقعده",
                "ذوالحجه",
            };

            for (int i = 0; i < rumi.Length; i++)
                output += Convert.ToString(i + 1) + ". " + rumi[i] + "\t" + muslim[i] + "\n";

            this.label2.Text = output;
        }
        private void DisplayResult(DateTime date)
        {
            calendar.GetDate(date);
            //label1.Text = calendar.Day + "/" + calendar.Month + "/" + calendar.Year;
            dateTimePicker1.Value = date;
            textBox2.Text = date.ToString("yyyy-MM-dd");
            textBox1.Text = calendar.Year + "-" + calendar.Month.ToString("00") + "-" + calendar.Day.ToString("00");
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DisplayResult((sender as DateTimePicker).Value);
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string input = textBox2.Text.Trim();
                input = input.Substring(0, 10);
                string year = input.Substring(0, 4);
                string month = input.Substring(5, 2);
                string day = input.Substring(8, 2);
                
                try
                {
                	//Regex regex = new Regex(@"[0-9]{4}[-][0-9]{2}[-][0-9]{2}");// validate the input text has valid format 2011-08-22
                	Regex regex = new Regex(@"[0-9]{4}");
	                if (!regex.IsMatch(year))
	                {
	                	MessageBox.Show("Incorrect date format, please try again.");
	                	return;
	                }
	                regex = new Regex(@"[0-9]{2}");
	                if (!regex.IsMatch(month))
	                {
	                	MessageBox.Show("Incorrect date format, please try again.");
	                	return;
	                }
	                regex = new Regex(@"[0-9]{2}");
	                if (!regex.IsMatch(day))
	                {
	                	MessageBox.Show("Incorrect date format, please try again.");
	                	return;
	                }
	                
                    DateTime date = new DateTime(Convert.ToInt16(year), Convert.ToInt16(month), Convert.ToInt16(day));
                    DisplayResult(date);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }               
            }
        }
    }
}