using System;
using System.Collections.Generic;
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
}
