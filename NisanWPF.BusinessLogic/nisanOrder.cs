﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

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

        /// <summary>
        /// Convert Gregorian date value to Muslim date.
        /// </summary>
        private void ConvertToMuslimDate()
        {
            int year = Convert.ToInt16(this.deathField.Substring(0, 4));
            int month = Convert.ToInt16(this.deathField.Substring(5, 2));
            int day = Convert.ToInt16(this.deathField.Substring(8, 2));
            DateTime date = new DateTime(year, month, day);
            nisan.Calendar.GetDate(date);
            this.deathmField = nisan.Calendar.Value.ToString("yyyy-MM-dd");
            this.OnPropertyChanged("deathm");
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
        private void SetPrice()
        {
            System.Diagnostics.Debug.WriteLine("SetPrice");
            if (this.itemField.Contains("PV")) this.priceField = 35;
            if (this.itemField.Contains("PA")) this.priceField = 35;
            if (this.itemField.ToLower().Contains("sticker")) this.priceField = 12;
            if (this.itemField.ToLower().Contains("batik")) this.priceField = 250;
            if (this.itemField.ToLower().Contains("putih")) this.priceField = 250;
            if (this.itemField.ToLower().Contains("hitam")) this.priceField = 350;
            if (this.itemField.ToLower().Contains("hijau")) this.priceField = 350;

            this.OnPropertyChanged("price");
        }

        #region IDataErrorInfo members
        private string error;
        public string Error { get { return null; } }
        public string this[string columnName]
        {
            get
            {
                this.error = null;
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
                    if (this.age != null)
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