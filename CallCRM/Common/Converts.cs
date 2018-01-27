using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Drawing;

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
                    return Visibility.Collapsed;
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
            //如果在有控件collpase时，并且gridcontrol设为滚轮动态效果的时候可能出现value为null的情况
            if (value != null)
            {
                switch (parameter.ToString())
                {
                    case "CreatePicture":
                        switch ((string)value)
                        {
                            case "未接听":
                                return BLLCommon.GetImage("Assets/design_16x16.png");
                            case "待处理":
                                return BLLCommon.GetImage("Assets/design_16x16.png");
                            case "电话已解决":
                                return BLLCommon.GetImage("Assets/show_16x16.png");
                            case "已报修":
                                return BLLCommon.GetImage("Assets/show_16x16.png");
                            default:
                                return BLLCommon.GetImage("Assets/design_16x16.png");
                        }
                    case "CreateText":
                        switch ((string)value)
                        {
                            case "未接听":
                                return "编辑创单";
                            case "待处理":
                                return "编辑创单";
                            case "电话已解决":
                                return "查看明细";
                            case "已报修":
                                return "查看明细";
                            default:
                                return "编辑创单";
                        }
                    case "StatusText":
                        switch ((string)value)
                        {
                            case "未接听":
                                return "未接听";
                            case "待处理":
                                return "待处理";
                            case "电话已解决":
                                return "电话已解决";
                            case "已报修":
                                return "已报修";
                            default:
                                return "未接听";
                        }
                    case "StatusPicture":
                        switch ((string)value)
                        {
                            case "未接听":
                                return BLLCommon.GetImage("Assets/delete_16x16.png");
                            case "待处理":
                                return BLLCommon.GetImage("Assets/tag_16x16.png");
                            case "电话已解决":
                                return BLLCommon.GetImage("Assets/phone_16x16.png");
                            case "已报修":
                                return BLLCommon.GetImage("Assets/botask_16x16.png");
                            default:
                                return BLLCommon.GetImage("Assets/delete_16x16.png");
                        }
                    default:
                        break;
                }

            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region 行号+1
    public class AddOneHandleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value + 1;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
