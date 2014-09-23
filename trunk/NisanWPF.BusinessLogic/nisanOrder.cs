using System;
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

        public string death
        {
            get
            {
                return this.deathField;
            }
            set
            {
                if ((this.deathField != null))
                {
                    if ((deathField.Equals(value) != true))
                    {
                        this.deathField = value;
                        this.OnPropertyChanged("death");
                    }

                    // set muslim date
                    if (this.deathField.Length == 10)
                    {
                        ConvertToMuslimDate();
                    }
                }
                else
                {
                    this.deathField = value;
                    this.OnPropertyChanged("death");
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

        #region IDataErrorInfo members
        private string error;
        public string Error { get { return null; } }
        public string this[string columnName]
        {
            get
            {
                this.error = null;
                if ("item" == columnName)
                {
                    // TODO: Validate wrong gender on stock.
                    //if(this.name
                    //this.error = "Please make sure you pick a right stock!";
                }

                if ("death" == columnName)
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

                return this.error;
            }
        }
        #endregion
    }
}