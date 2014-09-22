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
        private BrushConverter brushConverter = new BrushConverter();

        /// <summary>
        /// Gets abbreviation of soldto customer.
        /// </summary>
        /// <remarks>
        /// TODO: Move abbrev setting to configuration.
        /// </remarks>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public string abbrev
        {
            get
            {
                if (this.soldto == null) return "";
                switch (this.soldto)
                {
                    case "ADI": return "A";
                    case "HAM": return "H";
                    case "KEN": return "K";
                    case "PAS": return "P";
                    case "SEM": return "M";
                    case "SEL": return "S";
                    default: return this.soldto.Substring(0, 1);
                }
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Windows.Media.Brush soldtoColor
        {
            get
            {
                if (this.soldto == null) return Brushes.Gray;
                switch (this.soldto)
                {
                    case "ADI": return (Brush)brushConverter.ConvertFromString("#008080");
                    case "HAM": return (Brush)brushConverter.ConvertFromString("#00ff00");
                    case "KEN": return Brushes.Khaki;
                    case "PAS": return Brushes.PaleVioletRed;
                    case "SEM": return (Brush)brushConverter.ConvertFromString("#ff5555");
                    case "SEL": return (Brush)brushConverter.ConvertFromString("#808000");
                    default: return Brushes.Gray;
                }
            }
        }

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
                if(this.delivered == null) return false;
                return (this.delivered.Length > 0);
            }
        }

    }
}