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
using CallCRM.ViewModels;
using DevExpress.Xpf.Core;
using CallCRM.Common;
using CallCRM.Models;


namespace CallCRM.ViewPage
{
    /// <summary>
    /// Interaction logic for BombScreen.xaml
    /// </summary>
    public partial class BombScreen : DXWindow
    {
        private BSViewModel vm;

        public bool IsCreateSuccess = false;

        public BombScreen(string number,CallTime ct)
        {
            vm = new BSViewModel(number,ct);
            this.DataContext = vm;
            InitializeComponent();
        }
        public BombScreen(DataModel dm)
        {
            vm = new BSViewModel(dm);
            this.DataContext = vm;
            InitializeComponent();
        }

        private void DXWindow_Closed(object sender, EventArgs e)
        {
            vm.RemoveCallID();
            vm = null;
            GC.Collect();
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            assetComo.ItemsSource = vm.AssetTypeDict;
            assetComo.ValueMember = "Key";
            assetComo.DisplayMember = "Value";

            UserLookUpEdit.ItemsSource = vm.UserDict;
            UserLookUpEdit.ValueMember = "Key";
            UserLookUpEdit.DisplayMember = "Value";

            CompanyComo.ItemsSource = vm.CompanyDict;
            CompanyComo.ValueMember = "Key";
            CompanyComo.DisplayMember = "Value";

            FaultTypeComo.ItemsSource = vm.FaultDict;
            FaultTypeComo.ValueMember = "Text";
            FaultTypeComo.DisplayMember = "Value";
            FaultTypeComo.SelectedIndex = 2;
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (vm.CreateFaultOrder())
            {
                DXMessageBox.Show("创建工单成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                IsCreateSuccess = true;
                this.Close();
            }

            else
                DXMessageBox.Show("创建工单失败", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            vm.RefreshMediaInfo();
        }

        private void ButtonCanle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
