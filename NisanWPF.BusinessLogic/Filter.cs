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

            this.Rules = new ObservableCollection<FilterRule>();
            FilterRule pending = new FilterRule("Pending", true);
            pending.Parent = this;
            this.Rules.Add(pending);

            foreach (string customer in target.GetSoldToList())
            {
                FilterRule child = new FilterRule(customer);
                child.Parent = this;
                this.Rules.Add(child);
            }
        }
        public void Execute()
        {
            target.Filtering(this);
        }
    }
}