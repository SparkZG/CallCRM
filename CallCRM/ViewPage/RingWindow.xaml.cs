using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CallCRM.Common;

namespace CallCRM.ViewPage
{
    /// <summary>
    /// RingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RingWindow : Window
    {
        string callNumber = string.Empty;
        public RingWindow(string number)
        {
            callNumber = number;
            InitializeComponent();
        }

        private void splashWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CallID.Text = callNumber;
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            CreteFaultList.CloseWaitWindow();
        }
    }
}
