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
    public class PathCollection : GraphicCollection
    {
        #region Properties
        /// <summary>
        /// Gets or sets current selected path.
        /// </summary>
        public PathViewModel SelectedPath { get { return (PathViewModel)this.selectedGraphic; } }
        /// <summary>
        /// Source folder to read from.
        /// </summary>
        protected string source;
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PathCollection(string source)
            : base()
        {
            this.source = source;
            if (System.IO.Directory.Exists(this.source))
                Initialize();
        }

        #region Methods
        protected void Initialize()
        {
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(source);
            System.IO.FileInfo[] filesInfo = directoryInfo.GetFiles();
            var files = filesInfo.OrderBy(f => f.FullName);//sort alphabetically
            //foreach (System.IO.FileInfo info in filesInfo)
            foreach(System.IO.FileInfo info in files)
            {
                //System.Diagnostics.Debug.WriteLine("Reading " + info.Name + "...");

                SvgReader reader = new SvgReader(info.FullName);
                //HACK: At this moment only support one Path in a template file.
                //ideal case is get a group of graphic object.
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
                    this.items.Add(item);
                    break;
                }
            }//end loops
        }
        public void Sort()
        {
        }
        /// <summary>
        /// Match exactly keyword and return match number.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected int MatchExactly(string name)
        {
            int found = 0;
            if (string.IsNullOrEmpty(name))
            {
                SetVisibility(Visibility.Visible);
                return found;
            }

            foreach (PathViewModel item in this.items)
            {
                if (item.Name == name)
                {
                    found++;
                    item.Visibility = Visibility.Visible;
                }
                else
                    item.Visibility = Visibility.Collapsed;
            }

            return found;
        }
        /// <summary>
        /// Match if contains and return found number.
        /// </summary>
        /// <remarks>No case sensitive.</remarks>
        /// <param name="fullName"></param>
        protected int Contains(string fullName)
        {
            int found = 0;
            //todo: fullName = fullName.Replace(',',' ');
            string[] names = fullName.Split(new char[] { ' ' });
            foreach (PathViewModel item in this.items)
            {
                //if (item.Name.Contains(fullName))
                if (Contains(names, item.Name))
                {
                    found++;
                    item.Visibility = Visibility.Visible;
                }
                else
                    item.Visibility = Visibility.Collapsed;
            }

            return found;
        }
        private bool Contains(string[] names, string source)
        {
            bool contains = false;
            foreach (string name in names)
            {
                if (source.Contains(name)) //if (name.Contains(source))
                    return true;
            }

            return contains;
        }
        /// <summary>
        /// Select this path to indicate this path is toggle on then toggle off the rest.
        /// </summary>
        /// <param name="path"></param>
        public void Select(PathViewModel path)
        {
            this.selectedGraphic = path;
            foreach (PathViewModel item in this.items)
            {
                if (item.Path.Data.ToString().Equals(path.Path.Data.ToString()))
                    item.IsChecked = true;
                else
                    item.IsChecked = false;
            }
        }
        /// <summary>
        /// Delete the selected path and the physical file as well.
        /// </summary>
        /// <param name="path"></param>
        public void Delete(PathViewModel path)
        {
            this.items.Remove(path);
            System.IO.File.Delete(source + System.IO.Path.DirectorySeparatorChar + path.Name + path.Label + ".svg");
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
        private void SetVisibility(Visibility visibility)
        {
            foreach (PathViewModel item in this.items)
                item.Visibility = visibility;
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