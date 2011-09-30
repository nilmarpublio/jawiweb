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
            this.Title += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Refresh();

            //lianduis = Lianduis.LoadFromFile("Lianduis.xml");
            //System.Diagnostics.Debug.WriteLine("Load: " + lianduis.Liandui.Count);
            //DataGrid1.DataContext = lianduis;

            //viewSource = new CollectionViewSource();
            //viewSource.Source = lianduis.Liandui;
            //viewSource.Filter += new FilterEventHandler(viewSource_Filter);
            //DataGrid1.DataContext = viewSource.View;
        }

        #region Methods
        private void Refresh()
        {
            lianduis = Lianduis.LoadFromFile("Lianduis.xml");
            System.Diagnostics.Debug.WriteLine("Load: " + lianduis.Liandui.Count);
            DataGrid1.DataContext = lianduis;
            WordCount.Content = lianduis.Liandui.Count + " found";
        }
        /// <summary>
        /// Search liandui entry by given keyword.
        /// </summary>
        /// <remarks>
        /// Keyword can be in one character or a long string.
        /// </remarks>
        /// <param name="keyword"></param>
        /// <param name="isFirstCharacter">True if only match first character otherwise false.</param>
        /// <returns></returns>
        private Lianduis Search(string keyword, bool isFirstCharacter)
        {
            //todo: get the traddional & simplified character only do the contains
            char[] keys = keyword.ToCharArray();
            Lianduis original = Lianduis.LoadFromFile("Lianduis.xml");
            for (int i = original.Liandui.Count - 1; i >= 0; i--)
            {
                bool contains = false;
                foreach (char key in keys)
                {
                    if (isFirstCharacter)
                    {
                        char first = Convert.ToChar(original.Liandui[i].Value.Substring(0, 1));
                        contains = (first.CompareTo(key) == 0) ? true : false;
                    }
                    else
                    {
                        if (original.Liandui[i].Value.Contains(key))
                        {
                            contains = true;
                            break;
                        }
                    }
                }
                if (!contains) original.Liandui.RemoveAt(i);
            }

            return original;
        }
        #endregion

        #region Events
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //todo: highlight the match text.
            if (TextBox1.Text.Trim().Length > 0)
            {
                bool isFirst = (FirstRadio.IsChecked == true) ? true : false;
                lianduis = Search(TextBox1.Text.Trim(), isFirst);
                DataGrid1.DataContext = lianduis;
                WordCount.Content = lianduis.Liandui.Count + " found";
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