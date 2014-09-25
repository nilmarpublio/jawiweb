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
            FilterRule pending = new FilterRule { Name = "Pending" };
            pending.Parent = this;
            this.Rules.Add(pending);

            FilterRule all = new FilterRule { Name = "All" };
            all.Parent = this;
            foreach (string customer in target.GetSoldToList())
            {
                FilterRule child = new FilterRule { Name = customer };
                child.Parent = this;
                all.Children.Add(child);
            }
            this.Rules.Add(all);
        }
        public void Execute()
        {
            target.Filtering(this);
        }
    }
}