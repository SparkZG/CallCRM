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

        //public bool IsCreateSuccess = false;
        //public FaultType type;
        public BombScreen(string number, CallTime ct)
        {
            vm = new BSViewModel(number, ct);
            this.DataContext = vm;
            InitializeComponent();
        }
        public BombScreen(DataModel dm)
        {
            vm = new BSViewModel(dm);
            this.DataContext = vm;
            vm.GetCall_logData();
            InitializeComponent();
        }

        private void DXWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                vm.RemoveCallID();
                //vm = null;
                //GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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

            FalutLookUpEdit.ItemsSource = vm.KnowLedgeDict;
            FalutLookUpEdit.ValueMember = "Key";
            FalutLookUpEdit.DisplayMember = "Value";

            OrderTypeComo.ItemsSource = vm.OrderDict;
            OrderTypeComo.ValueMember = "Text";
            OrderTypeComo.DisplayMember = "Value";
            OrderTypeComo.SelectedIndex = 0;

            DepartLookUpEdit.ItemsSource = vm.DepartmentDict;
            DepartLookUpEdit.ValueMember = "Key";
            DepartLookUpEdit.DisplayMember = "Value";

            SetVisibility(vm._DataModel.IsDisposed.ToString());
        }

        public void SetVisibility(string str)
        {
            switch (str)
            {
                case "0":
                    OperateItem.Visibility = Visibility.Visible;
                    break;
                case "1":
                    OperateItem.Visibility = Visibility.Visible;
                    break;
                case "2":
                    OperateItem.Visibility = Visibility.Collapsed;
                    break;
                case "3":
                    OperateItem.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            var btn = (SimpleButton)sender;
            FaultType type = CreteFaultList.GetFaultType(btn.Uid);
            if (vm.CreateFaultOrder(type))
            {
                DXMessageBox.Show("操作成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //更新主界面此记录的状态
                vm._DataModel.IsDisposed = Convert.ToInt32(CreteFaultList.GetState(type));
                //IsCreateSuccess = true;
                this.Close();
            }
            else
                DXMessageBox.Show("操作失败", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            vm.RefreshMediaInfo();
        }

        private void ButtonCanle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FalutLookUpEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (vm.KnowLedge == null)
            {
                vm.Note_Result = string.Empty;
                return;
            }
            vm.Note_Result = vm.KnowLedge.Text;
        }

    }
}
