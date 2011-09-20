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
    /// <remarks>
    /// TODO: datagrid customization
    /// http://windowsclient.net/wpf/wpf35/wpf-35sp1-toolkit-datagrid-feature-walkthrough.aspx
    /// http://blog.smoura.com/wpf-toolkit-datagrid-part-iv-templatecolumns-and-row-grouping/
    /// </remarks>
    public partial class MainWindow : Window
    {
        private Lianduis lianduis;
        //private CollectionViewSource viewSource;
        public MainWindow()
        {
            InitializeComponent();

            Refresh();
            
            //lianduis = Lianduis.LoadFromFile("Lianduis.xml");
            //System.Diagnostics.Debug.WriteLine("Load: " + lianduis.Liandui.Count);
            //DataGrid1.DataContext = lianduis;

            //viewSource = new CollectionViewSource();
            //viewSource.Source = lianduis.Liandui;
            //viewSource.Filter += new FilterEventHandler(viewSource_Filter);
            //DataGrid1.DataContext = viewSource.View;
        }

        #region Methods & Events
        private void Refresh()
        {
            lianduis = Lianduis.LoadFromFile("Lianduis.xml");
            System.Diagnostics.Debug.WriteLine("Load: " + lianduis.Liandui.Count);
            DataGrid1.DataContext = lianduis;
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //todo: highlight the match text.
            if (TextBox1.Text.Trim().Length > 0)
            {
                Lianduis original = Lianduis.LoadFromFile("Lianduis.xml");
                //original.Liandui.Where(f => f.Value.Contains(TextBox1.Text.Trim()));
                //foreach (Liandui liandui in original.Liandui)
                for (int i = original.Liandui.Count - 1; i >= 0; i--)
                {
                    if (!original.Liandui[i].Value.Contains(TextBox1.Text.Trim()))
                        original.Liandui.RemoveAt(i);
                }

                lianduis = original;
                DataGrid1.DataContext = lianduis;
            }
            else
                Refresh();
        }
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchButton_Click(SearchButton, e);
        }
        private void viewSource_Filter(object sender, FilterEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("filtering " + TextBox1.Text.Trim());
            if (e.Item != null)
            {
                e.Accepted = true;//(e.Item as Liandui).Value.Contains(TextBox1.Text.Trim());
                System.Diagnostics.Debug.WriteLine(String.Format("{0} is {1}", (e.Item as Liandui).Value, e.Accepted));
            }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Saving: " + (DataGrid1.DataContext as Lianduis).Liandui.Count);
            (DataGrid1.DataContext as Lianduis).SaveToFile("Lianduis.xml");
        }
        #endregion
    }
}