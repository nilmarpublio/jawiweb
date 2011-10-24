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
    /// Basic element of graphic view model.
    /// </summary>
    public class GraphicViewModel : INotifyPropertyChanged
    {
        #region Properties
        private string name;
        /// <summary>
        /// Gets the name or title for this view model.
        /// </summary>
        public string Name { get { return this.name; } }
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
        public GraphicViewModel()
        {
            this.visibility = Visibility.Visible;
        }
        /// <summary>
        /// Recommended constructor.
        /// </summary>
        /// <param name="name"></param>
        public GraphicViewModel(string name)
        {
            this.visibility = Visibility.Visible;
            this.name = name;
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