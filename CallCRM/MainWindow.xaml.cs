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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CallCRM.ViewModels;
using DevExpress.Xpf.Core;
using CallCRM.DataFactory;
using System.Data;
using CallCRM.Common;
using CallCRM.ViewPage;

namespace CallCRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow
    {
        ViewModel vm = new ViewModel();
        public MainWindow()
        {
            this.DataContext = vm;
            vm.FocusLastItem += AutoScroll;
            InitializeComponent();
        }

        /// <summary>
        /// 滚动条自动滚动
        /// </summary>
        private void AutoScroll()
        {
            StatusList.ScrollIntoView(StatusList.Items[StatusList.Items.Count - 1]);
        }

        private void OpenPort_Checked(object sender, RoutedEventArgs e)
        {
            OpenPort.ToolTip = "关闭主机服务";
            string log = vm.StartConnent();
            if (log != null)
            {
                DXMessageBox.Show("开启主机服务失败/n" + log, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                OpenPort.IsChecked = false;
            }
        }

        private void OpenPort_Unchecked(object sender, RoutedEventArgs e)
        {
            vm.StopConnent();
            OpenPort.ToolTip = "开启主机服务";
        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpenPort_Unchecked(this, null);
        }

        #region Command-SelectAccess
        /// <summary>
        /// 下发命令
        /// </summary>
        private static RoutedUICommand selectCommand = new RoutedUICommand("SelectCommand", "SelectCommand", typeof(MainWindow));
        public static RoutedUICommand SelectCommand
        {
            get { return selectCommand; }
        }


        private void SelectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void SelectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BLLCommon.GetAccessPath();
        }
        #endregion

        #region Command-关机指令
        /// <summary>
        /// 测试
        /// </summary>
        private static RoutedUICommand exitApp = new RoutedUICommand("ExitApp", "ExitApp", typeof(MainWindow));
        public static RoutedUICommand ExitApp
        {
            get { return exitApp; }
        }
        private void ExitApp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void ExitApp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Command-关机指令
        /// <summary>
        /// 测试
        /// </summary>
        private static RoutedUICommand serverConfigCommand = new RoutedUICommand("ServerConfigCommand", "ServerConfigCommand", typeof(MainWindow));
        public static RoutedUICommand ServerConfigCommand
        {
            get { return serverConfigCommand; }
        }
        private void ServerConfigCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void ServerConfigCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ConfigDialog cd = new ConfigDialog();
            cd.ShowDialog();
        }

        #endregion

        private void DXWindow_Closed(object sender, EventArgs e)
        {
            vm.StopConnent();
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OpenPort.IsChecked = true;
        }

    }
}
