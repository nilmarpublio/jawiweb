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
using System.Net;
using System.IO;
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
        //private int counter;//performance use
        private Lianduis lianduis;
        //private CollectionViewSource viewSource;
        private string NUMBER_FORMAT = "###,###,###,###";
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
            WordCount.Content = lianduis.Liandui.Count.ToString(NUMBER_FORMAT) + " found";
        }
        private bool ContainsCharacter(char target, char[] sender)
        {
            foreach (char c in sender)
            {
                if (target.CompareTo(c) == 0)
                    return true;
            }

            return false;
        }
        private bool ContainsCharacter(string target, char[] sender)
        {
            foreach (char c in sender)
            {
                if (target.Contains(c))
                    return true;
            }

            return false;
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
            //counter = 0;//reset
            char[] keys = keyword.ToCharArray();
            Lianduis merge = new Lianduis();
            foreach (char key in keys)
            {
                //why linq union fail?
                //hack: merge.Liandui.Union(Search(key, isFirstCharacter).Liandui);
                //merge.Liandui.Union(from l in Search(key, isFirstCharacter).Liandui select l);
                foreach (Liandui liandui in Search(key, isFirstCharacter).Liandui)
                {
                    if (!merge.Liandui.Contains(liandui)) merge.Liandui.Add(liandui);
                }
            }

            //System.Diagnostics.Debug.Write("run " + counter);
            return merge;
        }
        private Lianduis Search(char key, bool isFirstCharacter)
        {
            //get the traddional & simplified character only do the contains
            Lianduis lianduis = Lianduis.LoadFromFile("Lianduis.xml");
            char[] translates = FindRelativeCharacters(key);
            for (int i = lianduis.Liandui.Count - 1; i >= 0; i--)
            {
                bool contains = false;
                if (isFirstCharacter)
                {
                    char first = Convert.ToChar(lianduis.Liandui[i].Value.Substring(0, 1));
                    contains = ContainsCharacter(first, translates);
                    //contains = (first.CompareTo(key) == 0) ? true : false;
                }
                else
                {
                    contains = ContainsCharacter(lianduis.Liandui[i].Value, translates);
                }

                if (!contains) lianduis.Liandui.RemoveAt(i);
            }

            return lianduis;
        }
        /// <summary>
        /// TODO: Get traddtional & simplified chinese character through Google Translate.
        /// </summary>
        /// <param name="source">One character of tradditional or simplified chinese character.</param>
        /// <returns></returns>
        private char[] FindRelativeCharacters(char source)
        {
            //counter++;
            List<char> holder = new List<char>();
            char c1 = ToTraditionalCharacter(source);
            char c2 = ToSimplifiedCharacter(source);

            holder.Add(source);
            if (!holder.Contains(c1)) holder.Add(c1);
            if (!holder.Contains(c2)) holder.Add(c2);
            return holder.ToArray();
        }
        /// <summary>
        /// Convert source to traddtional Chinese character. If provide a Traddtional character, return the same source.
        /// </summary>
        /// <param name="source">Simplified Chinese character.</param>
        /// <returns></returns>
        private char ToTraditionalCharacter(char source)
        {
            return Convert.ToChar(Microsoft.VisualBasic.Strings.StrConv(
                source.ToString(), Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0));
        }
        /// <summary>
        /// Convert source to simplified Chinese character. If provide a simplified character, return the same source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private char ToSimplifiedCharacter(char source)
        {
            return Convert.ToChar(Microsoft.VisualBasic.Strings.StrConv(
                source.ToString(), Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0));
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
                WordCount.Content = lianduis.Liandui.Count.ToString(NUMBER_FORMAT) + " found";
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