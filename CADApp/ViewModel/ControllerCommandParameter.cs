using System;
using System.Windows.Data;
using System.Globalization;
using CADApp.Tool.Controller;

namespace CADApp.ViewModel
{
    public class ControllerCommandParameter
    {
        public ControllerCommandParameter()
        {
            ControllerArgs = new ControllerArgs();
        }

        public ControllerType ControllerType { get; set; }
        public ControllerArgs ControllerArgs { get; set; }
    }

    public class ControllerCommandParameterMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = new ControllerCommandParameter();
            if (values[0] is ControllerType)
            {
                value.ControllerType = (ControllerType)values[0];
            }

            value.ControllerArgs.Parameter = new object[values.Length - 1];
            Array.Copy(values, 1, value.ControllerArgs.Parameter, 0, value.ControllerArgs.Parameter.Length);

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
