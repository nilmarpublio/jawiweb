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

    }
}