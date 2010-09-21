using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Path view model class. Implementation for WPF data binding purpose.
    /// </summary>
    public class PathViewModel
    {
        #region Properties
        private string label;
        /// <summary>
        /// Gets the lable for this view model.
        /// </summary>
        public string Label { get { return this.label; } }
        private string name;
        /// <summary>
        /// Gets the name or title for this view model.
        /// </summary>
        public string Name { get { return this.name; } }
        private Path path;
        /// <summary>
        /// Gets the path content of this view model.
        /// </summary>
        public Path Path { get { return this.path; } }
        /// <summary>
        /// Gets or sets visibility for this view model.
        /// </summary>
        public Visibility Visibility { get; set; }
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PathViewModel()
        {
            this.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="label"></param>
        public PathViewModel(string name, Path path, string label)
        {
            this.Visibility = Visibility.Visible;
            this.name = name;
            this.path = path;
            this.label = label;
        }
    }
}