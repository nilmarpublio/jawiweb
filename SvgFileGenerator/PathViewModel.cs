using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Path view model class. Implementation for WPF data binding purpose.
    /// </summary>
    public class PathViewModel : GraphicViewModel
    {
        #region Properties
        private string label;
        /// <summary>
        /// Gets the label for this view model.
        /// </summary>
        public string Label { get { return this.label; } }
        private Path path;
        /// <summary>
        /// Gets the path content of this view model.
        /// </summary>
        public Path Path { get { return this.path; } }
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PathViewModel()
            : base()
        {
        }
        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="label"></param>
        public PathViewModel(string name, Path path, string label)
            : base(name)
        {
            this.path = path;
            this.label = label;
        }
    }
}