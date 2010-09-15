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
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
//using System.IO;
using SvgFileGenerator;

namespace JawiWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// Gets or sets current selected path by mouse click.
        /// </summary>
        public Path SelectedPath;
        public Window1()
        {
            InitializeComponent();

            //runtime compose
            /*
             * <Path
            Fill="#000000"
            Data="m 2.8853219 14.535578 0.55997 9.87823 2.63737 48.2687 c 0 0 1.7899 -1.79365 2.86234 -3.42732 1.5211701 -2.31487 1.6011701 -4.30477 2.0586501 -6.52715 0.35873 -1.74366 0.38248 -2.13239 0.23498 -3.89355 l -2.8873401 -34.43693 -0.27499 -5.84594 c 0.28749 0.15749 0.56122 0.29998 0.81871 0.42622 l 4.4422601 2.19989 -0.84745 -2.33738 c -0.0962 -0.26498 -0.19874 -0.54622 -0.30124 -0.82871 -1.23243 -3.39482 -1.91615 -6.75214 -2.7373501 -10.2332103 -0.78496 -3.33232 -1.83241 -7.77084 -1.83241 -7.77084 0 0 -0.74121 -0.0387 -1.42492 0.0587 -1.8774 0.26874 -5.88594005 6.24842 -6.14593005 7.6071 -0.025 0.12874 -0.04 0.25749 -0.045 0.38498 -0.0113 0.28123 0.01 0.57622 0.0575 0.8812 0.34373 2.1898903 1.44618005 4.0585403 2.82485005 5.5959603 z"/>
             * 
             */

            //adding on top
            //@see http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/b73290de-f6ef-4c8f-9671-dff06f9038a2
            //List<Point> data = new List<Point>();
            //SvgReader reader = new SvgReader("alef.svg");
            //string pathString = reader.GetFirstPathValue();
            //Path path = new Path();
            //path.Fill = Brushes.Black;
            //path.Data = (Geometry)new GeometryConverter().ConvertFromString(pathString);//key

            //WrapPanel wrapPanel = new WrapPanel();
            //wrapPanel.Children.Add(path);
            //Size rectOfAlef = new Size(wrapPanel.ActualWidth, wrapPanel.ActualHeight);
            //System.Diagnostics.Debug.WriteLine("actual:" + rectOfAlef.ToString());

            /*
            this.wrapPanel1.Children.Add(path);

            reader = new SvgReader("damma.svg");
            pathString = reader.GetFirstPathValue();
            path = new Path();
            path.Fill = Brushes.Black;
            path.Data = (Geometry)new GeometryConverter().ConvertFromString(pathString);
            this.wrapPanel1.Children.Add(path);

            reader = new SvgReader("sukun.svg");
            pathString = reader.GetFirstPathValue();
            path = new Path();
            path.Fill = Brushes.Black;
            path.Data = (Geometry)new GeometryConverter().ConvertFromString(pathString);
            this.wrapPanel1.Children.Add(path);

            //adding bottom
            reader = new SvgReader("heh_bottom.svg");
            pathString = reader.GetFirstPathValue();
            path = new Path();
            path.Fill = Brushes.Black;
            path.Data = (Geometry)new GeometryConverter().ConvertFromString(pathString);
            this.wrapPanel2.Children.Add(path);

            reader = new SvgReader("seen_bottom.svg");
            pathString = reader.GetFirstPathValue();
            path = new Path();
            path.Fill = Brushes.Black;
            path.Data = (Geometry)new GeometryConverter().ConvertFromString(pathString);
            this.wrapPanel2.Children.Add(path);

            reader = new SvgReader("meem_bottom.svg");
            pathString = reader.GetFirstPathValue();
            path = new Path();
            path.Fill = Brushes.Black;
            path.Data = (Geometry)new GeometryConverter().ConvertFromString(pathString);
            this.wrapPanel2.Children.Add(path);*/

            /*StreamGeometry geometry = new StreamGeometry();
           //geometry.FillRule = FillRule.EvenOdd;
           using (StreamGeometryContext ctx = geometry.Open())
           {
               ctx.BeginFigure(new Point(2.8853219, 14.535578), true, true);
               ctx.LineTo(new Point(0.55997, 9.87823), true, true);//ctx.PolyLineTo(new List<Point>{new Point(0.55997, 9.87823)},true,false);
               ctx.LineTo(new Point(2.63737, 48.2687), true, true);//ctx.PolyLineTo(new List<Point>{new Point(2.63737, 48.2687)}, true, false);
               ctx.BezierTo(new Point(0, 0),
                   new Point(1.7899, -1.79365),
                   new Point(2.86234, -3.42732),
                   true, true);
           };
           geometry.Freeze();
           path.Data = geometry;*/
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //attempt to get path's height
            SvgReader reader = new SvgReader("alef.svg");
            string pathString = reader.GetFirstPathValue();
            Path path = new Path();
            path.Fill = Brushes.Black;
            path.Data = (Geometry)new GeometryConverter().ConvertFromString(pathString);//key
            //System.Diagnostics.Debug.WriteLine("Path's Height: " + path.Height);

            Size actual = new Size(path1.ActualWidth, path1.ActualHeight);
            System.Diagnostics.Debug.WriteLine("actual:" + actual.ToString());
            Size size = SvgReader.GetSize(pathString);
            System.Diagnostics.Debug.WriteLine(size.ToString());

            foreach (UIElement child in this.workSpace.Children)
            {
                if (child is Path)
                {
                    System.Diagnostics.Debug.WriteLine((child as Path).Margin);
                }
            }
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

        //Print
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
                dialog.PrintVisual(workSpace, "Testing");
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
        //SaveAs xaml
        private void Button_Click_1(object sender, RoutedEventArgs e)
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
            System.Diagnostics.Debug.WriteLine("Export xaml done");
        }
        //SaveAs by manual work
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SvgWriter writer = new SvgWriter("output.svg", this.workSpace);
            writer.Write();
            System.Diagnostics.Debug.WriteLine("Export svg done");
        }

        private void workSpace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectedPath = (e.Device.Target as Path);
            System.Diagnostics.Debug.WriteLine(this.SelectedPath.Data.ToString());
            //System.Diagnostics.Debug.WriteLine("Workspace mouse click position: " + e.GetPosition(workSpace).ToString());
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Current mouse click position: " + e.GetPosition(workSpace).ToString());
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (null == this.SelectedPath) return;

            Thickness margin = new Thickness();
            double step = 2.00;
            switch (e.Key)
            {
                case Key.Right:
                    margin = new Thickness(
                        this.SelectedPath.Margin.Left + step,
                        this.SelectedPath.Margin.Top,
                        this.SelectedPath.Margin.Right,
                        this.SelectedPath.Margin.Bottom);
                    break;
                case Key.Left:
                    margin = new Thickness(
                        this.SelectedPath.Margin.Left - step,
                        this.SelectedPath.Margin.Top,
                        this.SelectedPath.Margin.Right,
                        this.SelectedPath.Margin.Bottom);
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
                    break;
                default:
                    break;
            }

            this.SelectedPath.Margin = margin;
            System.Diagnostics.Debug.WriteLine("Set new margin:" + margin);
        }
    }
}