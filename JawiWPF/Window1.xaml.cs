﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
//using System.IO;
using HLGranite.WPF;
using HLGranite.Jawi;

namespace JawiWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        #region Fields
        /// <summary>
        /// Gets or sets current selected path by mouse click.
        /// </summary>
        private Path SelectedPath;
        private Color SelectedColor;

        private PunctuationSpace punctuationSpace;
        private WordSpace wordManager;
        private HLGranite.Jawi.Action action;
        #endregion

        public Window1()
        {
            InitializeComponent();
            this.Title += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Reload();
        }
        //todo: can this done in xaml with trigger?
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (UIElement child in workSpace.Children)
            {
                if (child is Line)
                    (child as Line).X2 = Math.Max(450, this.ActualWidth - 20 * 2);
            }
        }

        #region Methods
        private void AddBaseCharacter(Panel sender, string folderName)
        {
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(folderName);
            System.IO.FileInfo[] filesInfo = directoryInfo.GetFiles();
            foreach (System.IO.FileInfo info in filesInfo)
            {
                System.Diagnostics.Debug.WriteLine("Reading " + info.Name + "...");
                this.statusText.Text = "Reading " + info.Name + "...";
                Grid grid = new Grid();

                SvgReader reader = new SvgReader(info.FullName);
                var elements = reader.GetXMLElements("path");
                foreach (XElement element in elements)
                {
                    Path path = new Path();
                    path.Fill = Brushes.Black;
                    XAttribute attribute = element.Attribute(XName.Get("d"));
                    path.Data = (Geometry)new GeometryConverter().ConvertFromString(attribute.Value);//key

                    grid.Children.Add(path);
                }

                ToggleButton toggleButton = new ToggleButton();
                toggleButton.Content = grid;
                toggleButton.ToolTip = info.Name.TrimEnd(new char[] { 'g', 'v', 's', '.' });
                sender.Children.Add(toggleButton);
            }
        }
        private UIElement GetFirstUIElement(Grid grid)
        {
            foreach (UIElement child in grid.Children)
                return child;
            return null;
        }
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
        private void Moving(Key key)
        {
            Thickness margin = new Thickness();
            double step = 2.00;
            switch (key)
            {
                case Key.Right:
                    margin = new Thickness(
                        this.SelectedPath.Margin.Left + step,
                        this.SelectedPath.Margin.Top,
                        this.SelectedPath.Margin.Right,
                        this.SelectedPath.Margin.Bottom);
                    this.SelectedPath.Margin = margin;
                    break;
                case Key.Left:
                    margin = new Thickness(
                        this.SelectedPath.Margin.Left - step,
                        this.SelectedPath.Margin.Top,
                        this.SelectedPath.Margin.Right,
                        this.SelectedPath.Margin.Bottom);
                    this.SelectedPath.Margin = margin;
                    break;
                case Key.Up:
                    if (this.SelectedPath.VerticalAlignment == VerticalAlignment.Bottom)
                        margin = new Thickness(
                        this.SelectedPath.Margin.Left,
                        this.SelectedPath.Margin.Top,
                        this.SelectedPath.Margin.Right,
                        this.SelectedPath.Margin.Bottom + step);
                    else
                        margin = new Thickness(
                            this.SelectedPath.Margin.Left,
                            this.SelectedPath.Margin.Top - step,
                            this.SelectedPath.Margin.Right,
                            this.SelectedPath.Margin.Bottom);
                    this.SelectedPath.Margin = margin;
                    break;
                case Key.Down:
                    if (this.SelectedPath.VerticalAlignment == VerticalAlignment.Bottom)
                        margin = new Thickness(
                        this.SelectedPath.Margin.Left,
                        this.SelectedPath.Margin.Top,
                        this.SelectedPath.Margin.Right,
                        this.SelectedPath.Margin.Bottom - step);
                    else
                        margin = new Thickness(
                            this.SelectedPath.Margin.Left,
                            this.SelectedPath.Margin.Top + step,
                            this.SelectedPath.Margin.Right,
                            this.SelectedPath.Margin.Bottom);
                    this.SelectedPath.Margin = margin;
                    break;
                case Key.Delete:
                    this.workSpace.Children.Remove(this.SelectedPath);
                    break;
                default:
                    break;
            }

            System.Diagnostics.Debug.WriteLine("Set new margin:" + margin);
        }
        /// <summary>
        /// Reload library collection into screen.
        /// </summary>
        private void Reload()
        {
            //add basic character into screen
            punctuationSpace = new PunctuationSpace();
            khotSpace.ItemsSource = punctuationSpace.Items;

            wordManager = new WordSpace();
            wordSpace.ItemsSource = wordManager.Items;

            this.statusText.Text = "Ready";
            this.wordCount.Text = wordManager.Items.Count + " words";
        }
        #endregion

        #region Events
        /// <summary>
        /// Printing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(workSpace, "Testing");
                this.statusText.Text = "Sent to printer";
            }
        }
        private void load_Click(object sender, RoutedEventArgs e)
        {
            Reload();
        }
        /// <summary>
        /// SaveAs xaml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //http://blogs.msdn.com/b/ashish/archive/2008/01/15/dynamically-producing-xaml-files-using-xamlwriter-save-method.aspx
            string xml = XamlWriter.Save(workSpace);
            System.IO.FileStream fs = System.IO.File.Create("output.xaml");
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
            sw.Write(xml);
            sw.Close();
            fs.Close();

            //fail again
            //string output = string.Empty;
            //XamlToSvgTransform("output.xaml", "xaml2svg.xsl", "output.svg");
            this.statusText.Text = "Export xaml done";
        }
        /// <summary>
        /// SaveAs by manual work.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)
        {
            SvgWriter writer = new SvgWriter("output.svg", this.workSpace);
            writer.Write();
            this.statusText.Text = "Export svg done";
        }
        /// <summary>
        /// Clear all path element in workspace panel except guideline.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            //this.workSpace.Children.Clear();
            for (int i = this.workSpace.Children.Count - 1; i >= 0; i--)
            {
                if (this.workSpace.Children[i] is Path)
                    this.workSpace.Children.RemoveAt(i);
            }
            this.statusText.Text = "Clear all";
        }
        private void merge_Click(object sender, RoutedEventArgs e)
        {
            SimpleDialog dialog = new SimpleDialog();
            dialog.ShowDialog();
            //System.Diagnostics.Debug.WriteLine("ShowDialog:" + show);
            if (!string.IsNullOrEmpty(dialog.output.Text))
            {
                SvgWriter writer = new SvgWriter(dialog.output.Text + ".svg");
                bool success = writer.Merge("output.svg");
                if (success)
                    this.statusText.Text = "Merge success";
                else
                    this.statusText.Text = "Merge fail. Please try again";
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.action = HLGranite.Jawi.Action.Writing;

            PathViewModel viewModel = (PathViewModel)(sender as ToggleButton).DataContext;
            this.SelectedPath = viewModel.Path;
            this.SelectedPath.Name = viewModel.Name.Replace(' ', '_');

            punctuationSpace.Select(viewModel);
            wordManager.Select(viewModel);
        }
        private void searchCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            punctuationSpace.Match(seachCharacter.Text.Trim());
        }
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            wordManager.Contains(searchText.Text.Trim());
        }

        private void workSpace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            workSpace.Focus();
            (e.Device.Target as Path).Focus();
            this.SelectedPath = (e.Device.Target as Path);
            System.Diagnostics.Debug.WriteLine(this.SelectedPath.Data.ToString());
            System.Diagnostics.Debug.WriteLine("Workspace mouse click position: " + e.GetPosition(workSpace).ToString());
            this.action = HLGranite.Jawi.Action.Moving;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Window mouse down position: " + e.GetPosition(workSpace).ToString());
            this.statusText.Text = "Current position: " + e.GetPosition(workSpace).ToString();
            if (null == this.SelectedPath) return;
            if (action == HLGranite.Jawi.Action.Moving) return;

            Point position = e.GetPosition(workSpace);
            bool isAlignTop = false;
            if (position.Y < (100 - 10))//10 as a buffer space
                isAlignTop = true;

            Path path = new Path();
            //Path selectedPath = (Path)GetFirstUIElement((child as ToggleButton).Content as Grid);
            path.Data = this.SelectedPath.Data;
            path.Fill = Brushes.Black;
            Thickness margin = new Thickness();
            margin.Left = position.X;
            if (!isAlignTop) margin.Top = position.Y;
            path.Margin = margin;

            workSpace.Children.Add(path);
            this.statusText.Text = this.SelectedPath.Name + " added";
        }
        private void khotSpace_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("khotSpace_KeyDown..");
            if (action == HLGranite.Jawi.Action.Moving)
                Moving(e.Key);
        }
        private void wordSpace_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("wordSpace_KeyDown..");
            if (action == HLGranite.Jawi.Action.Moving)
                Moving(e.Key);
            else if (e.Key == Key.Delete)
                wordManager.Delete(wordManager.SelectedPath);
        }
        //todo: workSpace_MouseMove
        private void workSpace_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(workSpace);
            hRuler.Chip = vRuler.Unit == Unit.Cm ? DipHelper.DipToCm(p.X) : DipHelper.DipToInch(p.X);
            vRuler.Chip = vRuler.Unit == Unit.Cm ? DipHelper.DipToCm(p.Y) : DipHelper.DipToInch(p.Y);
        }
        #endregion
    }
}