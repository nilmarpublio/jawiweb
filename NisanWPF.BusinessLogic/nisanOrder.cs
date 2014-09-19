using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NisanWPF.BusinessLogic
{
    public partial class nisanOrder
    {
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
                    default: return this.soldto;
                }
            }
        }

    }
}