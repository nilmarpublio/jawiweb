﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NisanWPF.BusinessLogic
{
    public class FilterDateRule : FilterRule
    {
        private DateTime from;
        public DateTime From
        {
            get { return this.from; }
            set
            {
                this.from = value;
                this.OnPropertyChanged("From");
            }
        }

        private DateTime to;
        public DateTime To
        {
            get { return this.to; }
            set
            {
                this.to = value;
                this.OnPropertyChanged("To");
            }
        }

        public FilterDateRule()
            : base()
        {
            base.name = "Date";
            this.from = DateTime.MinValue;
            this.to = DateTime.MaxValue;
        }

        public FilterDateRule(string name, bool isChecked)
            : base(name, isChecked)
        {
            this.from = DateTime.MinValue;
            this.to = DateTime.MaxValue;
        }
    }
}