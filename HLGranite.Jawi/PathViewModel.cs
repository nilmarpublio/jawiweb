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
    public class PathViewModel : INotifyPropertyChanged
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
        private Visibility visibility;
        /// <summary>
        /// Gets or sets visibility for this view model.
        /// </summary>
        public Visibility Visibility
        {
            get { return this.visibility; }
            set
            {
                this.visibility = value;
                RaisePropertyChanged("Visibility");
            }
        }
        private bool isChecked;
        /// <summary>
        /// Gets or sets this view model is selected (or toggle on in GUI).
        /// </summary>
        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PathViewModel()
        {
            this.visibility = Visibility.Visible;
        }
        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="label"></param>
        public PathViewModel(string name, Path path, string label)
        {
            this.visibility = Visibility.Visible;
            this.name = name;
            this.path = path;
            this.label = label;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var args = new PropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
        }
        #endregion
    }
}