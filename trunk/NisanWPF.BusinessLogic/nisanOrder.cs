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
        public string abbrev
        {
            get
            {
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

        public System.Windows.Media.Brush soldtoColor
        {
            get
            {
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
        public System.Windows.Media.Brush stockColor
        {
            get
            {
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
        public int aging
        {
            get
            {
                TimeSpan diff = DateTime.Now - Convert.ToDateTime(this.date);
                return (int)diff.TotalDays;
            }
        }

    }
}