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
                // when only happen check change
                //if (this.value.Equals(value) != true)

                this.value = value;
                this.OnPropertyChanged("Value");

                if (this.name == "Pending")
                {
                    if (value == true)
                    {
                        this.parent.IsPending = true;
                        foreach (FilterRule child in this.parent.Rules)
                            SetAllFalse(child, "Pending");
                    }
                }
                else if (this.name == "All")
                {
                    if (value == true)
                    {
                        this.parent.IsPending = false;
                        foreach (FilterRule child in this.parent.Rules)
                            SetAllFalse(child, "All");
                        foreach (FilterRule child in this.children)
                            child.Value = true;
                    }
                    else
                    {
                        //foreach (FilterRule child in this.parent.Rules)
                        //    SetAllFalse(child, "All");
                        //foreach (FilterRule child in this.children)
                        //    child.Value = false;
                    }
                }
                else
                {
                    // uncheck pending option
                    if (value == true)
                    {
                        this.parent.IsPending = false;
                        this.parent.Rules[0].Value = false;
                    }
                }

                this.parent.Execute();
            }
        }

        private void SetAllFalse(FilterRule rule, string except)
        {
            if (rule.Name != except) rule.Value = false;
            foreach (FilterRule child in rule.Children)
                SetAllFalse(child, except);
        }

        private Filter parent;
        public Filter Parent { get { return this.parent; } set { this.parent = value; } }

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
            this.name = "";
            this.value = false;
            this.children = new ObservableCollection<FilterRule>();
        }
        public FilterRule(string name)
        {
            this.name = name;
            this.value = false;
            this.children = new ObservableCollection<FilterRule>();
        }
        public FilterRule(string name, bool value)
        {
            this.name = name;
            this.value = value;
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