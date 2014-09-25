using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;

namespace NisanWPF.BusinessLogic
{
    public class FilterRule
    {
        private string name;
        /// <summary>
        /// Gets and sets display name of filter.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                if ((this.name != null))
                {
                    if ((name.Equals(value) != true))
                    {
                        this.name = value;
                        this.OnPropertyChanged("Name");
                    }
                }
                else
                {
                    this.name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        private bool value;
        /// <summary>
        /// Gets and sets value.
        /// </summary>
        public bool Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                if (this.name == "Pending" && value == true)
                {
                }
                this.OnPropertyChanged("Value");
            }
        }

        private ObservableCollection<FilterRule> children;
        /// <summary>
        /// Gets or sets child rules if has.
        /// </summary>
        public ObservableCollection<FilterRule> Children
        {
            get { return this.children; }
            set
            {
                this.children = value;
                this.OnPropertyChanged("Children");
            }
        }

        public FilterRule()
        {
            this.children = new ObservableCollection<FilterRule>();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null))
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}