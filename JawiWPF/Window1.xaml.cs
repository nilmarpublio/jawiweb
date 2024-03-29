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
        private Path selectedPath;
        private PunctuationCollection punctuationManager;
        private TextCollection textManager;
        private WordCollection wordManager;
        private HLGranite.Jawi.Action action;

        /// <summary>
        /// Gets or sets current selected color for painting use.
        /// </summary>
        private ColorViewModel selectedColor;
        private ColorCollection colorManager;
        #endregion

        public Window1()
        {
            InitializeComponent();
            this.Title += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.selectedColor = new ColorViewModel(Brushes.Black, "Black");

            colorManager = new ColorCollection();
            this.colorPallete.ItemsSource = colorManager.Items;

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
        /// <summary>
        /// Reload library collection into screen.
        /// </summary>
        private void Reload()
        {
            //add basic character into screen
            punctuationManager = new PunctuationCollection();
            textManager = new TextCollection();
            //hack: merge character into punctuation temp
            foreach (PathViewModel item in textManager.Items)
                punctuationManager.Items.Add(item);
            khotSpace.ItemsSource = punctuationManager.Items;

            wordManager = new WordCollection();
            wordSpace.ItemsSource = wordManager.Items;

            this.statusText.Text = "Ready";
            this.wordCount.Text = wordManager.Items.Count + " words";
            this.richTextBox2.Document.Blocks.Clear();
        }
        /// <summary>
        /// Add path object into workspace.
        /// </summary>
        /// <param name="position"></param>
        private void AddPath(Point position)
        {
            bool isAlignTop = false;
            bool isAlignBottom = false;
            //25% as a buffer space
            if (position.Y < 100 / 4) isAlignTop = true;
            if (position.Y > 100 / 4) isAlignBottom = true;

            Path path = new Path();
            //Path selectedPath = (Path)GetFirstUIElement((child as ToggleButton).Content as Grid);
            path.Data = this.selectedPath.Data;

            //todo: how to handle white color scenario?
            Brush brush = (Brush)new BrushConverter().ConvertFromString(this.selectedColor.Name);//Brushes.Black;
            path.Fill = brush;
            Thickness margin = new Thickness();
            margin.Left = position.X;
            if (!isAlignTop) margin.Top = position.Y;
            if (isAlignBottom) margin.Top = 100 - path.Data.Bounds.Height;
            path.Margin = margin;

            this.workSpace.Children.Add(path);
        }
        /// <summary>
        /// Move the graphic object by arrow key only.
        /// </summary>
        /// <remarks>Support left, right, up, down, and delete only.</remarks>
        /// <param name="key"></param>
        private void Moving(Key key)
        {
            Thickness margin = new Thickness();
            double step = 2.00;
            switch (key)
            {
                case Key.Right:
                    margin = new Thickness(
                        this.selectedPath.Margin.Left + step,
                        this.selectedPath.Margin.Top,
                        this.selectedPath.Margin.Right,
                        this.selectedPath.Margin.Bottom);
                    this.selectedPath.Margin = margin;
                    break;
                case Key.Left:
                    margin = new Thickness(
                        this.selectedPath.Margin.Left - step,
                        this.selectedPath.Margin.Top,
                        this.selectedPath.Margin.Right,
                        this.selectedPath.Margin.Bottom);
                    this.selectedPath.Margin = margin;
                    break;
                case Key.Up:
                    if (this.selectedPath.VerticalAlignment == VerticalAlignment.Bottom)
                        margin = new Thickness(
                        this.selectedPath.Margin.Left,
                        this.selectedPath.Margin.Top,
                        this.selectedPath.Margin.Right,
                        this.selectedPath.Margin.Bottom + step);
                    else
                        margin = new Thickness(
                            this.selectedPath.Margin.Left,
                            this.selectedPath.Margin.Top - step,
                            this.selectedPath.Margin.Right,
                            this.selectedPath.Margin.Bottom);
                    this.selectedPath.Margin = margin;
                    break;
                case Key.Down:
                    if (this.selectedPath.VerticalAlignment == VerticalAlignment.Bottom)
                        margin = new Thickness(
                        this.selectedPath.Margin.Left,
                        this.selectedPath.Margin.Top,
                        this.selectedPath.Margin.Right,
                        this.selectedPath.Margin.Bottom - step);
                    else
                        margin = new Thickness(
                            this.selectedPath.Margin.Left,
                            this.selectedPath.Margin.Top + step,
                            this.selectedPath.Margin.Right,
                            this.selectedPath.Margin.Bottom);
                    this.selectedPath.Margin = margin;
                    break;
                case Key.Delete:
                    this.workSpace.Children.Remove(this.selectedPath);
                    break;
                default:
                    break;
            }

            System.Diagnostics.Debug.WriteLine("Set new margin:" + margin);
        }
        /// <summary>
        /// Write to temporarily output file then merge into existing raw svg file.
        /// </summary>
        private void Save()
        {
            SvgWriter writer = new SvgWriter("output.svg", this.workSpace);
            writer.Write(this.wordManager);
            this.statusText.Text = "Export svg done";

            SimpleDialog dialog = new SimpleDialog();
            dialog.ShowDialog();

            string fileName = dialog.output.Text.Trim() + ".svg";
            SvgWriter writer2 = new SvgWriter(fileName);
            bool success = writer2.Merge("output.svg");
            string message = success ? "Merge success" : "Merge fail. Please try again";
            this.statusText.Text = message;
            if (success)
            {
                if (MessageBox.Show(message) == MessageBoxResult.OK)
                    clear_Click(clear, new RoutedEventArgs());
            }
            else
                MessageBox.Show(message);
        }
        /// <summary>
        /// Get input text from searchbox.
        /// </summary>
        /// <returns></returns>
        private string GetInputText()
        {
            TextRange textRange = new TextRange(richTextBox2.Document.ContentStart, richTextBox2.Document.ContentEnd);
            return textRange.Text.Trim();
        }
        /// <summary>
        /// Get inline text from rictTextBox control.
        /// </summary>
        /// <param name="myRichTextBox"></param>
        /// <returns></returns>
        /// <remarks>Reverse engineering depend on what UI control you design begining.</remarks>
        private string GetInlineText(RichTextBox myRichTextBox)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Block b in myRichTextBox.Document.Blocks)
            {
                if (b is Paragraph)
                {
                    foreach (Inline inline in ((Paragraph)b).Inlines)
                    {
                        if (inline is InlineUIContainer)
                        {
                            InlineUIContainer uiContainer = (InlineUIContainer)inline;
                            if (uiContainer.Child is Button)
                                sb.Append(((Button)uiContainer.Child).Content);
                            else if (uiContainer.Child is TextBlock)
                                sb.Append(((TextBlock)uiContainer.Child).Text);//key
                        }
                        else if (inline is Run)
                        {
                            Run run = (Run)inline;
                            sb.Append(run.Text);
                        }
                    }
                }
            }
            return sb.ToString();
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
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    string xml = XamlWriter.Save(workSpace);
        //    System.IO.FileStream fs = System.IO.File.Create("output.xaml");
        //    System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
        //    sw.Write(xml);
        //    sw.Close();
        //    fs.Close();

        //    this.statusText.Text = "Export xaml done";
        //}
        /// <summary>
        /// SaveAs by manual work.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)
        {
            Save();
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
            this.richTextBox2.Document.Blocks.Clear();
            this.statusText.Text = "Clear all";
        }

        private void ColorButton_Checked(object sender, RoutedEventArgs e)
        {
            ColorViewModel viewModel = (sender as ToggleButton).DataContext as ColorViewModel;
            colorManager.Select(viewModel);
            this.selectedColor = colorManager.SelectedColor;
            this.statusText.Text = "Picked color " + viewModel.Name;
        }
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.action = HLGranite.Jawi.Action.Writing;

            PathViewModel viewModel = (PathViewModel)(sender as ToggleButton).DataContext;
            this.selectedPath = viewModel.Path;
            this.selectedPath.Name = viewModel.Name.Replace(' ', '_');

            punctuationManager.Select(viewModel);
            wordManager.Select(viewModel);
        }
        private void seachCharacter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                searchCharacterButton_Click(sender, e);
        }
        private void searchCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            int found = punctuationManager.Match(seachCharacter.Text.Trim());
            this.statusText.Text = found + " found";
        }
        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                searchButton_Click(sender, e);
        }
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            //@see http://www.devx.com/dotnet/Article/34644
            //get text from a RichTextBox
            string inputText = GetInputText();

            List<KeyValuePair<string, bool>> result = new List<KeyValuePair<string, bool>>();
            int found = wordManager.Contains(inputText, out result);//searchText.Text.Trim()
            this.statusText.Text = found + " found";

            //adding text into a RichTextBox
            richTextBox2.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph();
            foreach (KeyValuePair<string, bool> key in result)
            {
                if (!key.Value)
                    paragraph.Inlines.Add(new TextBlock { Text = key.Key, Foreground = Brushes.Red });
                else
                    paragraph.Inlines.Add(new TextBlock { Text = key.Key });
                paragraph.Inlines.Add(new TextBlock { Text = " " });
            }
            richTextBox2.Document.Blocks.Add(paragraph);
        }

        private void workSpace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            workSpace.Focus();
            (e.Device.Target as Path).Focus();
            this.selectedPath = (e.Device.Target as Path);
            System.Diagnostics.Debug.WriteLine(this.selectedPath.Data.ToString());
            System.Diagnostics.Debug.WriteLine("Workspace mouse click position: " + e.GetPosition(workSpace).ToString());
            this.action = HLGranite.Jawi.Action.Moving;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Window mouse down position: " + e.GetPosition(workSpace).ToString());
            this.statusText.Text = "Current position: " + e.GetPosition(workSpace).ToString();
            if (null == this.selectedPath) return;
            if (action == HLGranite.Jawi.Action.Moving) return;

            AddPath(e.GetPosition(workSpace));
            this.statusText.Text = this.selectedPath.Name + " added";
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
        private void colorPallete_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("colorPallete_KeyDown..");
            if (action == HLGranite.Jawi.Action.Moving)
                Moving(e.Key);
        }
        //todo: workSpace_MouseMove
        private void workSpace_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(workSpace);
            hRuler.Chip = vRuler.Unit == Unit.Cm ? DipHelper.DipToCm(p.X) : DipHelper.DipToInch(p.X);
            vRuler.Chip = vRuler.Unit == Unit.Cm ? DipHelper.DipToCm(p.Y) : DipHelper.DipToInch(p.Y);
        }

        private void openWordSvg_Click(object sender, RoutedEventArgs e)
        {
            string root = string.Empty;
            int index = System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\');
            if (index == -1)
                index = System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('/');
            if (index > -1)
                root = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, index);

            PathViewModel viewModel = (((sender as MenuItem).Parent as ContextMenu)
                .PlacementTarget as ToggleButton).DataContext as PathViewModel;
            string fileName = root + System.IO.Path.DirectorySeparatorChar
                + "words" + System.IO.Path.DirectorySeparatorChar
                + viewModel.Name + viewModel.Label + ".svg";
            System.Diagnostics.Process.Start(fileName);
            //System.Diagnostics.Debug.WriteLine(sender);
        }
        private void characterMenu_Click(object sender, RoutedEventArgs e)
        {
            CharacterMapViewer window = new CharacterMapViewer();
            window.Show();
        }
        private void aboutMenu_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Owner = this;
            about.ShowDialog();
        }
        #endregion
    }
}