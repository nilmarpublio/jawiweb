using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;

namespace NisanWPF.BusinessLogic
{
    public class FilterRule : System.ComponentModel.INotifyPropertyChanged
    {
        protected string name;
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

        protected bool isChecked;
        /// <summary>
        /// Gets and sets value.
        /// </summary>
        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                // when only happen check change
                //if (this.value.Equals(value) != true)

                this.isChecked = value;
                this.OnPropertyChanged("IsChecked");

                if (this.name == "Pending")
                    this.parent.IsPending = value;
                if (this.name == "Date")
                    this.parent.HasDateRange = value;

                this.parent.Execute();
            }
        }

        private void SetAllFalse(FilterRule rule, string except)
        {
            if (rule.Name != except) rule.IsChecked = false;
            foreach (FilterRule child in rule.Children)
                SetAllFalse(child, except);
        }

        protected Filter parent;
        public Filter Parent { get { return this.parent; } set { this.parent = value; } }

        protected ObservableCollection<FilterRule> children;
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
            this.name = "";
            this.isChecked = false;
            this.children = new ObservableCollection<FilterRule>();
        }
        public FilterRule(string name)
        {
            this.name = name;
            this.isChecked = false;
            this.children = new ObservableCollection<FilterRule>();
        }
        public FilterRule(string name, bool isChecked)
        {
            this.name = name;
            this.isChecked = isChecked;
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