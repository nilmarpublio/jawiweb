using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JawiWPF
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string date = string.Empty;
            string[] dates = version.Split(new char[] { '.' });
            date += "20" + dates[1]
                + "-" + dates[2].Substring(0, dates[2].Length - 2)
                + "-" + dates[2].Substring(dates[2].Length - 2, 2);

            this.textBlock1.Text += "\n version " + version;
            this.textBlock1.Text += "\n" + date;
        }
    }
}