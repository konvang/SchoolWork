using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace OrderEntrySystem.Utilities
{
    public class EnumToStringConverter : IValueConverter
    {
        private string lastEnteredValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (value != null)
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

                if (fieldInfo != null)
                {
                    result = DisplayUtil.GetFieldDescription(fieldInfo);
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}