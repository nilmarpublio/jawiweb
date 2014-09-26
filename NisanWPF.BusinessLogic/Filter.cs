using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NisanWPF.BusinessLogic
{
    public class Filter
    {
        public bool IsPending { get; set; }
        public bool IsAllDate { get; set; }
        public ObservableCollection<FilterRule> Rules { get; set; }
        protected nisan target;
        public Filter(nisan nisan)
        {
            this.target = nisan;
            this.IsPending = true;
            this.IsAllDate = false;

            // adding pending option
            this.Rules = new ObservableCollection<FilterRule>();
            FilterRule pending = new FilterRule("Pending", true);
            pending.Parent = this;
            this.Rules.Add(pending);

            // adding all customer list
            foreach (string customer in target.GetSoldToList())
            {
                FilterRule child = new FilterRule(customer);
                child.Parent = this;
                this.Rules.Add(child);
            }

            // add date selection
            FilterDateRule dateRule = new FilterDateRule();
            dateRule.Parent = this;
            this.Rules.Add(dateRule);
        }
        public void Reset()
        {
            System.Diagnostics.Debug.WriteLine("Reset filtering");
            foreach (FilterRule rule in this.Rules)
            {
                rule.IsChecked = false;
                if (rule is FilterDateRule)
                {
                    (rule as FilterDateRule).From = DateTime.MinValue;
                    (rule as FilterDateRule).To = DateTime.MaxValue;
                }
            }
        }
        public void Execute()
        {
            target.Filtering(this);
        }
    }
}