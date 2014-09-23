using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace NisanWPF.BusinessLogic
{
    public partial class nisanOrder
    {
        /// <summary>
        /// Gets total days since accepted order.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public int aging
        {
            get
            {
                if (this.date == null) return 0;
                TimeSpan diff = DateTime.Now - Convert.ToDateTime(this.date);
                return (int)diff.TotalDays;
            }
        }

        /// <summary>
        /// Gets this order has been delivered.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hasDeliver
        {
            get
            {
                if (this.delivered == null)
                {
                    this.hasCutField = false;
                    return false;
                }
                else
                {
                    if (this.delivered.Length > 0)
                        this.hasCutField = true;
                    else
                        this.hasCutField = false;
                }

                return (this.delivered.Length > 0);
            }
        }

        private bool hasCutField;
        /// <summary>
        /// Gets or sets this order has plotted the sticker work.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hasCut
        {
            get
            {
                return this.hasCutField;
            }
            set
            {
                if ((hasCutField.Equals(value) != true))
                {
                    this.hasCutField = value;
                    this.OnPropertyChanged("hasCut");
                }
            }
        }

    }
}