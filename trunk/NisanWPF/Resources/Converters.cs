using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace NisanWPF
{
    /// <summary>
    /// If true then visible otherwise hidden.
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is Boolean)
            {
                return (Boolean)value ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is int)
            {
                return ((int)value) > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is Int32)
            {
                return System.Convert.ToInt32(value) > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is decimal)
            {
                return System.Convert.ToDecimal(value) > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is float)
            {
                float money = (float)value;
                return (money > 0) ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value == null)
            {
                return Visibility.Visible;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// If true then invisible otherwise visible.
    /// </summary>
    /// <remarks>
    /// Opposite with VisibilityConverter.
    /// </remarks>
    public class InvisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value is Boolean)
            {
                return (Boolean)value ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value is int)
            {
                return ((int)value) > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value is Int32)
            {
                return System.Convert.ToInt32(value) > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value is decimal)
            {
                return System.Convert.ToDecimal(value) > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value is float)
            {
                float money = (float)value;
                return (money > 0) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value == null)
            {
                return Visibility.Collapsed;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// Fade off background if true otherwise do nothing.
    /// </summary>
    public class FadeOffConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? 0.5d : 1d;
            }
            else if (value is Boolean)
            {
                return (Boolean)value ? 0.5d : 1d;
            }
            else if (value is int)
            {
                return ((int)value) > 0 ? 0.5d : 1d;
            }
            else if (value is Int32)
            {
                return System.Convert.ToInt32(value) > 0 ? 0.5d : 1d;
            }
            else if (value is decimal)
            {
                return System.Convert.ToDecimal(value);
            }
            else if (value is float)
            {
                float money = (float)value;
                return (money > 0) ? 0.5d : 1d;
            }
            else if (value == null)
            {
                return 0.5d;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// Gets abbreviation of soldto customer.
    /// </summary>
    /// <remarks>
    /// TODO: Move abbrev setting to configuration.
    /// </remarks>
    public class SoldToAbbrevConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";
            if (value.ToString() == "") return "";
            if (value is string)
            {
                switch (value.ToString())
                {
                    case "ADI": return "A";
                    case "HAM": return "H";
                    case "KEN": return "K";
                    case "PAS": return "P";
                    case "SEM": return "M";
                    case "SEL": return "S";
                    default: return value.ToString().Substring(0, 1);
                }
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                switch (value.ToString())
                {
                    case "A": return "ADI";
                    case "H": return "HAM";
                    case "K": return "KEN";
                    case "P": return "PAS";
                    case "M": return "SEM";
                    case "S": return "SEL";
                    default: return value.ToString();
                }
            }

            throw new ArgumentException("Not supported type of " + targetType);
        }
    }

    /// <summary>
    /// Gets abbreviation of soldto customer.
    /// </summary>
    /// <remarks>
    /// TODO: Move soldto color setting to configuration.
    /// </remarks>
    public class SoldToColorConverter : IValueConverter
    {
        private BrushConverter brushConverter = new BrushConverter();
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Brushes.Gray;
            switch (value.ToString())
            {
                case "ADI": return (Brush)brushConverter.ConvertFromString("#008080");
                case "HAM": return (Brush)brushConverter.ConvertFromString("#00ff00");
                case "KEN": return Brushes.Khaki;
                case "PAS": return Brushes.PaleVioletRed;
                case "SEM": return (Brush)brushConverter.ConvertFromString("#ff5555");
                case "SEL": return (Brush)brushConverter.ConvertFromString("#808000");
                default: return Brushes.Gray;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert to represented stock color.
    /// </summary>
    public class StockColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Brushes.Black;
            if (value is string)
            {
                if (value.ToString().ToLower().Contains("hijau"))
                    return Brushes.LightGreen;
                else if (value.ToString().ToLower().Contains("batik"))
                    return Brushes.Blue;
                else if (value.ToString().ToLower().Contains("putih"))
                    return Brushes.LightGray;
                else if (value.ToString().ToLower().Contains("marble"))
                    return Brushes.LightGray;
                else
                    return Brushes.Black;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert to local display format.
    /// </summary>
    public class LocalizationConverter : IValueConverter
    {
        private string integerFormat = "###,###,###,##0";
        private string decimalFormat = "###,###,###,##0.00";
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                return System.Convert.ToInt32(value).ToString(integerFormat);
            }
            else if (value is Int16)
            {
                return System.Convert.ToInt16(value).ToString(integerFormat);
            }
            else if (value is Int32)
            {
                return System.Convert.ToInt32(value).ToString(integerFormat);
            }
            else if (value is decimal)
            {
                return System.Convert.ToDecimal(value).ToString(decimalFormat);
            }
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert to display value with local currency.
    /// </summary>
    public class LocalCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol
            if (value is int)
            {
                return String.Format("{0:c}", System.Convert.ToInt32(value));
            }
            else if (value is Int16)
            {
                return String.Format("{0:c}", System.Convert.ToInt16(value));
            }
            else if (value is Int32)
            {
                return String.Format("{0:c}", System.Convert.ToInt32(value));
            }
            else if (value is decimal)
            {
                return String.Format("{0:c}", System.Convert.ToDecimal(value));
            }
            else if (value is double)
            {
                return String.Format("{0:c}", System.Convert.ToDouble(value));
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Return tooltip for nisanOrder by mixed of creation date and remarks.
    /// </summary>
    public class NisanToolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() == 2)
            {
                string date = (values[0] == null) ? string.Empty : values[0].ToString();
                string remarks = (values[1] == null) ? string.Empty : values[1].ToString();
                return "since " + date + " " + remarks;
            }

            throw new ArgumentException("Not supported");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}