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
        /// Gets represented stock color.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Windows.Media.Brush stockColor
        {
            get
            {
                if (this.item == null) return Brushes.Black;
                if (this.item.ToLower().Contains("hijau"))
                    return Brushes.LightGreen;
                else if (this.item.ToLower().Contains("batik"))
                    return Brushes.Blue;
                else if (this.item.ToLower().Contains("putih"))
                    return Brushes.LightGray;
                else if (this.item.ToLower().Contains("marble"))
                    return Brushes.LightGray;
                else
                    return Brushes.Black;
            }
        }

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
                if (this.delivered == null) return false;
                return (this.delivered.Length > 0);
            }
        }

    }
}