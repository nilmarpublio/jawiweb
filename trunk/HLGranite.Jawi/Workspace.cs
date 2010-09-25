using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace HLGranite.Jawi
{
    public class Workspace
    {
        #region Properties
        private PathViewModel selectedPath;
        /// <summary>
        /// Gets or sets current selected path.
        /// </summary>
        public PathViewModel SelectedPath { get { return this.selectedPath; } }
        /// <summary>
        /// Gets or sets PathViewModel collection for this workspace.
        /// </summary>
        public ObservableCollection<PathViewModel> Items { get; set; }
        /// <summary>
        /// Source folder to read from.
        /// </summary>
        protected string source;
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Workspace(string source)
        {
            this.source = source;
            Initialize();
        }

        #region Methods
        protected void Initialize()
        {
            this.Items = new ObservableCollection<PathViewModel>();
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(source);
            System.IO.FileInfo[] filesInfo = directoryInfo.GetFiles();
            foreach (System.IO.FileInfo info in filesInfo)
            {
                //System.Diagnostics.Debug.WriteLine("Reading " + info.Name + "...");

                SvgReader reader = new SvgReader(info.FullName);
                //At this moment only support one Path in a template file.
                var elements = reader.GetXMLElements("path");
                foreach (XElement element in elements)
                {
                    Path path = new Path();
                    path.Fill = Brushes.Black;
                    XAttribute attribute = element.Attribute(XName.Get("d"));
                    path.Data = (Geometry)new GeometryConverter().ConvertFromString(attribute.Value);//key

                    string name = info.Name.ToLower().TrimEnd(new char[] { 'g', 'v', 's', '.' });
                    string label = GetLabel(info.Name);
                    if (label.Length > 0) name = name.Replace(label, string.Empty);

                    PathViewModel item = new PathViewModel(name, path, label);
                    this.Items.Add(item);
                    break;
                }
            }//end loops
        }
        public void Sort()
        {
        }
        /// <summary>
        /// Search.
        /// </summary>
        /// <param name="name"></param>
        public void Search(string name)
        {
            foreach (PathViewModel item in this.Items)
            {
                if (item.Name.Contains(name))//todo: better matching algorithm maybe use regex
                    item.Visibility = Visibility.Visible;
                else
                    item.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// Select this path to indicate this path is toggle on then toggle off the rest.
        /// </summary>
        /// <param name="path"></param>
        public void Select(PathViewModel path)
        {
            this.selectedPath = path;
            foreach (PathViewModel item in this.Items)
            {
                if (item.Path.Data.ToString().Equals(this.selectedPath.Path.Data.ToString()))
                    item.IsChecked = true;
                else
                    item.IsChecked = false;
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Get digit in file name.
        /// </summary>
        /// <remarks>
        /// e.g. file21.svg return 21.
        /// </remarks>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetLabel(string fileName)
        {
            string result = string.Empty;
            Regex regex = new Regex(@"[0-9]+");//match digit or more.
            Match match = regex.Match(fileName.ToLower());
            if (match.Success) result = match.Groups[0].Value;

            return result;
        }
        #endregion
    }

    /// <summary>
    /// Action for adding jawi or moving.
    /// </summary>
    public enum Action
    {
        /// <summary>
        /// Default action. Nothing to do or in ready.
        /// </summary>
        Idle,
        /// <summary>
        /// Indicate in writing mode.
        /// </summary>
        Writing,
        /// <summary>
        /// It is moving character stage.
        /// </summary>
        Moving,
    }
}