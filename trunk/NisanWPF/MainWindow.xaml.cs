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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NisanWPF.BusinessLogic;

namespace NisanWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class MainWindow : Window
    {
        private nisan nisan;
        public MainWindow()
        {
            InitializeComponent();
            versionLabel.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            nisan.LoadFromFile("nisan.xml", out nisan);
            nisan.Initialize(nisan);
            // show pending order at startup
            nisan.FilterPendingOrder();

            // bind nisan order
            this.DataContext = nisan;

            // bind filtering options
            this.filterList.ItemsSource = new Filter(nisan).Rules;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            nisan.SaveToFile("nisan.xml");
        }
    }
}