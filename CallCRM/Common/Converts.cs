using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace CallCRM.Common
{
    #region 取反转换
    public class ReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "Start")
            {
                if ((bool)value)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            else if (parameter.ToString() == "Refresh")
            {
                if ((bool)value)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
            else
            {
                bool state = !(bool)value;
                return state;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Access数据转换
    public class ReverseAlarmRankConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "CreateEnable")
            {
                if ((int)value == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (parameter.ToString() == "Call")
            {
                return BLLCommon.GetImage("Assets/assignto_32x32.png");
            }
            else
            {
                if ((int)value == 0)
                {
                    return "建单";
                }
                else
                {
                    return "已建单";
                }
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
