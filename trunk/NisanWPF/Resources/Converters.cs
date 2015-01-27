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
            if (value.ToString() == string.Empty) return Brushes.Gray;
            switch (value.ToString())
            {
                case "ADI": return (Brush)brushConverter.ConvertFromString("#008080");
                case "HAM": return (Brush)brushConverter.ConvertFromString("#00ff00");
                case "KEN": return Brushes.Khaki;
                case "PAS": return Brushes.PaleVioletRed;
                case "SEM": return (Brush)brushConverter.ConvertFromString("#ff5555");
                case "SEL": return (Brush)brushConverter.ConvertFromString("#808000");
                default:
                    string firstCharacter = value.ToString().Substring(0, 1);
                    return ColorConverter.Colors[firstCharacter]; //return Brushes.Gray;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Define color for first customer character randomly.
    /// TODO: Enhance algorithm with simple incremental loop.
    /// </summary>
    public class ColorConverter
    {
        public static Dictionary<string, Brush> Colors
        {
            get
            {
                BrushConverter brushConverter = new BrushConverter();
                Dictionary<string, Brush> colors = new Dictionary<string, Brush>();
                colors.Add("A", (Brush)brushConverter.ConvertFrom("#800000"));
                colors.Add("B", (Brush)brushConverter.ConvertFrom("#ff0000"));
                colors.Add("C", (Brush)brushConverter.ConvertFrom("#808000"));
                colors.Add("D", (Brush)brushConverter.ConvertFrom("#ffff00"));
                colors.Add("E", (Brush)brushConverter.ConvertFrom("#008000"));
                colors.Add("F", (Brush)brushConverter.ConvertFrom("#00ff00"));
                colors.Add("G", (Brush)brushConverter.ConvertFrom("#008080"));
                colors.Add("H", (Brush)brushConverter.ConvertFrom("#00ffff"));
                colors.Add("I", (Brush)brushConverter.ConvertFrom("#000080"));
                colors.Add("J", (Brush)brushConverter.ConvertFrom("#0000ff"));
                colors.Add("K", (Brush)brushConverter.ConvertFrom("#800080"));
                colors.Add("L", (Brush)brushConverter.ConvertFrom("#ff00ff"));
                colors.Add("M", (Brush)brushConverter.ConvertFrom("#ff5555"));
                colors.Add("N", (Brush)brushConverter.ConvertFrom("#ff9955"));
                colors.Add("O", (Brush)brushConverter.ConvertFrom("#ffcc00"));
                colors.Add("P", (Brush)brushConverter.ConvertFrom("#d3bc5f"));
                colors.Add("Q", (Brush)brushConverter.ConvertFrom("#88aa00"));
                colors.Add("R", (Brush)brushConverter.ConvertFrom("#55d400"));
                colors.Add("S", (Brush)brushConverter.ConvertFrom("#00aa88"));
                colors.Add("T", (Brush)brushConverter.ConvertFrom("#37c8ab"));
                colors.Add("U", (Brush)brushConverter.ConvertFrom("#00aad4"));
                colors.Add("V", (Brush)brushConverter.ConvertFrom("#0055d4"));
                colors.Add("W", (Brush)brushConverter.ConvertFrom("#5f5fd3"));
                colors.Add("X", (Brush)brushConverter.ConvertFrom("#6600ff"));
                colors.Add("Y", (Brush)brushConverter.ConvertFrom("#cc00ff"));
                colors.Add("Z", (Brush)brushConverter.ConvertFrom("#ff0066"));
                return colors;
            }
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
    /// Set foreground as red when stock more than two weeks aging.
    /// </summary>
    public class AgingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Brushes.LightGray;
            if (value is int)
            {
                if (System.Convert.ToInt32(value) > 13) return Brushes.Red;
                return Brushes.LightGray;
            }
            else if (value is Int16)
            {
                if (System.Convert.ToInt16(value) > 13) return Brushes.Red;
                return Brushes.LightGray;
            }
            else if (value is Int32)
            {
                if (System.Convert.ToInt32(value) > 13) return Brushes.Red;
                return Brushes.LightGray;
            }
            else if (value is decimal)
            {
                if (System.Convert.ToDecimal(value) > 13) return Brushes.Red;
                return Brushes.LightGray;
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