using System;
using System.Data;

namespace HLGranite.Jawi
{
    /// <summary>
    /// Muslim calendar format.
    /// </summary>
    public class MuslimCalendar
    {
        private int year;
        private int month;
        private int day;
        private DataTable DataSource;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MuslimCalendar()
        {
        }
        /// <summary>
        /// Recomended constructor.
        /// </summary>
        /// <param name="table"></param>
        public MuslimCalendar(DataTable table)
        {
            DataSource = table;
            GetDate(DateTime.Now);
        }

        #region Properties
        /// <summary>
        /// Islamic Year.
        /// </summary>
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        /// <summary>
        /// Islamic month.
        /// </summary>
        public int Month
        {
            get { return month; }
            set { month = value; }
        }
        /// <summary>
        /// Islamic day.
        /// </summary>
        public int Day
        {
            get { return day; }
            set { day = value; }
        }
        /// <summary>
        /// DataSource index.
        /// </summary>
        public int Index;
        #endregion

        #region Methods
        /// <summary>
        /// Convert Gregorian month to islamic month in english spelling.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetRumiMonth(int i)
        {
            if (i < 1 || i > 12) return "";

            string month = "";
            switch (i)
            {
                case 1:
                    month = "Muharram";
                    break;
                case 2:
                    month = "Safar";
                    break;
                case 3:
                    month = "Rabiulawal";
                    break;
                case 4:
                    month = "Rabiulakhir";
                    break;
                case 5:
                    month = "Jamadilawal";
                    break;
                case 6:
                    month = "Jamadilakhir";
                    break;
                case 7:
                    month = "Rejab";
                    break;
                case 8:
                    month = "Syaaban";
                    break;
                case 9:
                    month = "Ramadhan";
                    break;
                case 10:
                    month = "Syawal";
                    break;
                case 11:
                    month = "Zulkaedah";
                    break;
                case 12:
                    month = "Zulhijjah";
                    break;
                default:
                    break;
            }

            return month;
        }
        /// <summary>
        /// Convert Gregorian month to islamic month in arabic word.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetMuslimMonth(int i)
        {
            if (i < 1 || i > 12) return "";

            string month = "";
            switch (i)
            {
                case 1:
                    month = "Muharram";
                    break;
                case 2:
                    month = "Safar";
                    break;
                case 3:
                    month = "Rabiulawal";
                    break;
                case 4:
                    month = "Rabiulakhir";
                    break;
                case 5:
                    month = "Jamadilawal";
                    break;
                case 6:
                    month = "Jamadilakhir";
                    break;
                case 7:
                    month = "Rejab";
                    break;
                case 8:
                    month = "Syaaban";
                    break;
                case 9:
                    month = "Ramadhan";
                    break;
                case 10:
                    month = "Syawal";
                    break;
                case 11:
                    month = "Zulkaedah";
                    break;
                case 12:
                    month = "Zulhijjah";
                    break;
                default:
                    break;
            }

            return month;
        }
        /// <summary>
        /// From Gregorian date to islamic date.
        /// </summary>
        /// <param name="date"></param>
        public void GetDate(DateTime date)
        {
            if (DataSource == null)
                return;
            else if (DataSource.Rows.Count == 0)
                return;

            /* match a nearest record from database base on gregorian input date
             * plus after day method - due to floating day of each muslim month
             * calculate today date = islamic date
             * */

            try
            {
                DateTime ldt_Islam = date;
                TimeSpan interval = new TimeSpan(30, 0, 0, 0);
                TimeSpan diff = interval;//indicate timespan from database


                DateTime ldt_From = date.Subtract(interval);
                DateTime ldt_To = date.Add(interval);
                for (int i = DataSource.Rows.Count - 1; i >= 0; i--)
                {
                    if (date.CompareTo(Convert.ToDateTime(DataSource.Rows[i]["sun"])) > 0) //key
                    {
                        Index = i;

                        DateTime ldt_AbsoluteDate = new DateTime(date.Year, date.Month, date.Day);
                        diff = ldt_AbsoluteDate - Convert.ToDateTime(DataSource.Rows[Index]["sun"]);
                        ldt_Islam = Convert.ToDateTime(DataSource.Rows[Index]["date"]);
                        break;
                    }
                }//end loops

                #region last method
                //				DataRow[] ldr_selected = DataSource.Select("sun >= '"+ldt_From.ToString("yyyy-MM-dd")+"' AND sun <= '"+ldt_To.ToString("yyyy-MM-dd")+"'", "sun ASC");
                //				for(int i=0;i<ldr_selected.Length;i++)
                //				{
                //					if(date.CompareTo( Convert.ToDateTime(ldr_selected[i]["sun"]) ) > 0)
                //					{ 
                //						DateTime ldt_AbsoluteDate = new DateTime(date.Year,date.Month,date.Day);
                //						diff =  ldt_AbsoluteDate - Convert.ToDateTime(ldr_selected[i]["sun"]);
                //						ldt_Islam = Convert.ToDateTime(ldr_selected[i]["date"]);
                //						break;
                //					}
                //				}//end seek


                //double check (secure step) *fail to handle
                //				if(ldt_Islam == date && diff == interval)
                //				{
                //					if(ldr_selected.Length>0)
                //					{
                //						DateTime ldt_AbsoluteDate = new DateTime(date.Year,date.Month,date.Day);
                //						diff =  ldt_AbsoluteDate - Convert.ToDateTime(ldr_selected[0]["sun"]);
                //						ldt_Islam = Convert.ToDateTime(ldr_selected[0]["date"]);
                //					}
                //				}
                #endregion

                year = ldt_Islam.Year;
                if (diff.Days <= 30)
                {
                    month = ldt_Islam.Month;
                    day = diff.Days + 1;//plus today
                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }
        }
        /// <summary>
        /// Convert Muslim date to gregorian date.
        /// todo
        /// </summary>
        /// <returns></returns>
        public DateTime ReturnGregorianDate()
        {
            DateTime gregorian = new DateTime(1, 1, 1);

            gregorian = new DateTime(2006, 1, 13);//test


            return gregorian;
        }
        #endregion
    }
}