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
        private nisan nisan;
        public Filter(nisan nisan)
        {
            this.nisan = nisan;
            this.IsPending = true;
            this.IsAllDate = false;

            this.Rules = new ObservableCollection<FilterRule>();
            FilterRule pending = new FilterRule { Name = "Pending" };
            this.Rules.Add(pending);

            FilterRule all = new FilterRule { Name = "All" };
            foreach (string customer in nisan.GetSoldToList())
                all.Children.Add(new FilterRule { Name = customer });
            this.Rules.Add(all);
            this.Rules.CollectionChanged += Rules_CollectionChanged;
        }

        private void Rules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Rules_CollectionChanged");
            //foreach (FilterRule rule in this.Rules)
            //{
            //    if (rule.Name == "Pending" && rule.Value == true)
            //    {
            //    }
            //}
        }
    }
}