using System;
using System.Globalization;
using System.Windows.Data;

namespace OrderEntrySystem
{
    public class DoubleToStringConverter : IValueConverter
    {
        private string lastEnteredValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";

            if (lastEnteredValue != null)
            {
                result = lastEnteredValue;
            }
            else
            {
                result = value.ToString();
            }

            lastEnteredValue = null;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result;
            string stringVar = value.ToString();

            bool val = double.TryParse(stringVar, out result);

            if (val)
            {
                lastEnteredValue = stringVar;
            }

            return result;
        }
    }
}
