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
using DevExpress.Xpf.Core;
using CallCRM.Log;
using CallCRM.DataFactory;


namespace CallCRM.ViewPage
{
    /// <summary>
    /// Interaction logic for ConfigDialog.xaml
    /// </summary>
    public partial class ConfigDialog : DXWindow
    {
        public ConfigDialog()
        {
            InitializeComponent();
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {

            GetDefaultData();
        }

        public void GetDefaultData()
        {
            ServerIP.Text = Properties.Settings.Default.ServerIP;
            ServerPort.Text = Properties.Settings.Default.ServerPort;
            Database.Text = Properties.Settings.Default.Database;
            User.Text = Properties.Settings.Default.UserID;
            Password.Text = Properties.Settings.Default.Password;

        }
        public void SetDefaultData()
        {
            Properties.Settings.Default.ServerIP = ServerIP.Text;
            Properties.Settings.Default.ServerPort = ServerPort.Text;
            Properties.Settings.Default.Database = Database.Text;
            Properties.Settings.Default.UserID = User.Text;
            Properties.Settings.Default.Password = Password.Text;
            this.Close();
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as SimpleButton;
            switch (btn.Uid)
            {
                case "0":
                    GetDefaultData();
                    break;
                case "1":
                    try
                    {
                        SetDefaultData();
                    }
                    catch (Exception ex)
                    {
                        Log4Helper.Error(GetType(), "设置数据异常！", ex);
                        throw;
                    }
                    break;
                case "2":
                    string log = PostgresqlHelper.ConnectTest(ServerIP.Text, ServerPort.Text, Database.Text, User.Text, Password.Text);
                    DXMessageBox.Show(log == null ? "连接成功！" : log, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                default:
                    break;
            }
        }
    }
}
