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

namespace CallCRM.ViewPage
{
    /// <summary>
    /// DataViewPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataViewPage : UserControl
    {
        private DataViewModel dvm = null;
        public DataViewPage()
        {
            dvm = new DataViewModel();
            this.DataContext = dvm;
            InitializeComponent();
        }

        #region Command-Create
        /// <summary>
        /// 下发命令
        /// </summary>
        private static RoutedUICommand createCommand = new RoutedUICommand("CreateCommand", "CreateCommand", typeof(DataViewPage));
        public static RoutedUICommand CreateCommand
        {
            get { return createCommand; }
        }


        private void CreateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void CreateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            dvm.CreateSelectedRowData();
        }
        #endregion

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            dvm.GetAllCall();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GridView.BestFitColumns();
        }
    }
}
