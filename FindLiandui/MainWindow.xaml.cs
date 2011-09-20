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
using System.Collections.ObjectModel;
using HLGranite;

namespace FindLiandui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Lianduis lianduis = Lianduis.LoadFromFile("Lianduis.xml");
            System.Diagnostics.Debug.WriteLine("Load: " + lianduis.Liandui.Count);
            DataGrid1.DataContext = lianduis;
            //todo: datagrid customization http://windowsclient.net/wpf/wpf35/wpf-35sp1-toolkit-datagrid-feature-walkthrough.aspx
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Saving: " + (DataGrid1.DataContext as Lianduis).Liandui.Count);
            (DataGrid1.DataContext as Lianduis).SaveToFile("Lianduis.xml");
        }
    }
}