using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

using HLGranite.Jawi;

namespace NisanWPF.BusinessLogic
{
    public partial class nisanOrder : IDataErrorInfo
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
                }

                this.OnPropertyChanged("hasCut");
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

        private bool ValidateStock()
        {
            System.Diagnostics.Debug.WriteLine("ValidateStock");
            if (this.item != null && this.name != null)
            {
                if (this.name.Contains("bin") && this.item.ToString().Contains("(P)"))
                    return false;
                if (this.name.Contains("bt") && this.item.ToString().Contains("(L)"))
                    return false;
                if (this.name.Contains("binti") && this.item.ToString().Contains("(L)"))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Automate price value after picked stock type.
        /// </summary>
        private void SetPrice()
        {
            System.Diagnostics.Debug.WriteLine("SetPrice");
            if (this.itemField.Contains("PV")) this.priceField = 35;
            if (this.itemField.Contains("PA")) this.priceField = 35;
            if (this.itemField.ToLower().Contains("sticker(l)")) this.priceField = 12;
            if (this.itemField.ToLower().Contains("sticker(p)")) this.priceField = 12;
            if (this.itemField.ToLower().Contains("tarazo(l)")) this.priceField = 12; // rare case
            if (this.itemField.ToLower().Contains("tarazo(p)")) this.priceField = 12;
            if (this.itemField.ToLower().Contains("batik")) this.priceField = 250;
            if (this.itemField.ToLower().Contains("putih")) this.priceField = 250;
            if (this.itemField.ToLower().Contains("hitam")) this.priceField = 350;
            if (this.itemField.ToLower().Contains("hijau")) this.priceField = 350;

            this.OnPropertyChanged("price");
        }

        /// <summary>
        /// Automate convert to jawi when provide rumi name.
        /// </summary>
        private void ConvertToJawi()
        {
            // Translate word by word
            string output = string.Empty;
            JawiLookup localTranslator = new JawiLookup();
            JawiTranslator webTranslator = new JawiTranslator();
            string[] words = this.nameField.Split(new char[] { ' ' });
            foreach (string word in words)
            {
                string jawi = localTranslator.Lookup(word);
                if (string.IsNullOrEmpty(jawi))
                    jawi = webTranslator.Translate(this.nameField);

                if (output.Length > 0) output += " ";
                output += jawi;
            }

            this.jawiField = output;
            this.OnPropertyChanged("jawi");
        }

        /// <summary>
        /// Convert Gregorian date value to Muslim date.
        /// </summary>
        private void ConvertToMuslimDate()
        {
            System.Diagnostics.Debug.WriteLine("ConvertToMuslimDate");
            int year = Convert.ToInt16(this.deathField.Substring(0, 4));
            int month = Convert.ToInt16(this.deathField.Substring(5, 2));
            int day = Convert.ToInt16(this.deathField.Substring(8, 2));
            DateTime date = new DateTime(year, month, day);
            nisan.Calendar.GetDate(date);
            this.deathmField = nisan.Calendar.Value.ToString("yyyy-MM-dd");
            this.OnPropertyChanged("deathm");
        }

        #region IDataErrorInfo members
        private string error;
        public string Error { get { return null; } }
        public string this[string columnName]
        {
            get
            {
                this.error = null;
                if ("name" == columnName)
                {
                    // Show message of wrong gender of stock
                    if (!ValidateStock())
                        this.error = "Please make sure you pick the right stock!";
                }

                if ("death" == columnName)
                {
                    if (this.death != null)
                    {
                        if (this.death.Length >= 10)
                        {
                            int year = Convert.ToInt16(this.deathField.Substring(0, 4));
                            int month = Convert.ToInt16(this.deathField.Substring(5, 2));
                            int day = Convert.ToInt16(this.deathField.Substring(8, 2));
                            DateTime date = new DateTime(year, month, day);
                            if (date > DateTime.Today)
                            {
                                this.error = "Are you cursing people to die?";
                            }
                        }
                    }
                }

                if ("age" == columnName)
                {
                    if (!string.IsNullOrEmpty(this.age))
                    {
                        if (Convert.ToDecimal(this.age) > 130)
                        {
                            this.error = "Are you sure human can live so long?";
                        }
                    }
                }

                return this.error;
            }
        }
        #endregion
    }
}