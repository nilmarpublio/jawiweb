using System;
using System.Diagnostics;
using System.Data;

namespace HLGranite.BusinessLogic
{
    public class MuslimCalendar
    {
        #region Properties
        private DataTable DataSource;
        private int day;
        public int Index;
        private int month;
        private int year;
        public int Day
        {
            get
            {
                return this.day;
            }
            set
            {
                this.day = value;
            }
        }
        public int Month
        {
            get
            {
                return this.month;
            }
            set
            {
                this.month = value;
            }
        }
        public int Year
        {
            get
            {
                return this.year;
            }
            set
            {
                this.year = value;
            }
        }
        #endregion

        public MuslimCalendar()
        {
        }
        public MuslimCalendar(DataTable table)
        {
            this.DataSource = table;
            this.GetDate(DateTime.Now);
        }

        #region Methods
        public void GetDate(DateTime sender)
        {
            if (this.DataSource == null) return;
            if (this.DataSource.Rows.Count == 0) return;

            try
            {
                DateTime time = sender;
                TimeSpan span = new TimeSpan(30, 0, 0, 0);
                TimeSpan span2 = span;
                DateTime time2 = sender.Subtract(span);
                DateTime time3 = sender.Add(span);
                for (int i = this.DataSource.Rows.Count - 1; i >= 0; i--)
                {
                    if (sender.CompareTo(Convert.ToDateTime(this.DataSource.Rows[i]["sun"])) >= 0)
                    {
                        this.Index = i;
                        DateTime time4 = new DateTime(sender.Year, sender.Month, sender.Day);
                        span2 = (TimeSpan)(time4 - Convert.ToDateTime(this.DataSource.Rows[this.Index]["sun"]));
                        time = Convert.ToDateTime(this.DataSource.Rows[this.Index]["date"]);
                        break;
                    }
                }
                this.year = time.Year;
                if (span2.Days <= 30)
                {
                    this.month = time.Month;
                    this.day = span2.Days + 1;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
        public static string GetMuslimMonth(int i)
        {
            if ((i < 1) || (i > 12))
            {
                return "";
            }
            switch (i)
            {
                case 1:
                    return "محرّم";

                case 2:
                    return "صفر";

                case 3:
                    return "ربيع الاول";

                case 4:
                    return "ربيع الاخير";

                case 5:
                    return "جمادالاول";

                case 6:
                    return "جمادالاخير";

                case 7:
                    return "رجب";

                case 8:
                    return "شعبان";

                case 9:
                    return "رمضان";

                case 10:
                    return "شوال";

                case 11:
                    return "ذوالقعده";

                case 12:
                    return "ذوالحجه";
            }
            return "";
        }
        public static string GetRumiMonth(int i)
        {
            if ((i < 1) || (i > 12))
            {
                return "";
            }
            switch (i)
            {
                case 1:
                    return "Muharram";

                case 2:
                    return "Safar";

                case 3:
                    return "Rabiulawal";

                case 4:
                    return "Rabiulakhir";

                case 5:
                    return "Jamadilawal";

                case 6:
                    return "Jamadilakhir";

                case 7:
                    return "Rejab";

                case 8:
                    return "Syaaban";

                case 9:
                    return "Ramadhan";

                case 10:
                    return "Syawal";

                case 11:
                    return "Zulkaedah";

                case 12:
                    return "Zulhijjah";
            }
            return "";
        }
        public DateTime ReturnGregorianDate()
        {
            DateTime time = new DateTime(1, 1, 1);
            return new DateTime(0x7d6, 1, 13);
        }
        #endregion

    }//end class

}//end namespace