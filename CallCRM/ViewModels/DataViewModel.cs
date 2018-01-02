using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Threading;
using CallCRM.Common;
using CallCRM.DataFactory;
using CallCRM.Models;
using CallCRM.Log;
using Microsoft.Win32;
using CallCRM.ViewPage;

namespace CallCRM.ViewModels
{
    class DataViewModel : ViewModelBase
    {
        DataTable callTable = new DataTable();
        public DataViewModel()
        {
            GetAllCall();
        }

        public void GetAllCall()
        {
            callTable.Clear();
            CallList.Clear();
            string sql = "select * from CallLogTable where CallType=0";
            callTable = AccessHelper.Adapter(sql, null);
            foreach (DataRow item in callTable.Rows)
            {
                try
                {
                    var dm = new DataModel();
                    dm.SetData(item);
                    if (dm.CallerID == "")
                    {
                        continue;
                    }
                    CallList.Add(dm);
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), "数据库存在异常数据", ex);
                    continue;
                }
            }
        }


        private ObservableCollection<DataModel> callList = new ObservableCollection<DataModel> { };
        public ObservableCollection<DataModel> CallList
        {
            get { return callList; }
            set
            {
                SetProperty<ObservableCollection<DataModel>>(ref callList, value, "CallList");
            }
        }

        private DataModel selectedRow;
        public DataModel SelectedRow
        {
            get { return selectedRow; }
            set
            {
                SetProperty<DataModel>(ref selectedRow, value, "SelectedRow");
            }
        }

        public void CreateSelectedRowData()
        {
            var dm = new DataModel();
            dm.SetData(SelectedRow);
            var bs = new BombScreen(dm);
            bs.ShowDialog();
            if (bs.IsCreateSuccess)
            {
                foreach (var item in CallList)
                {
                    if (item.ID == dm.ID)
                    {
                        item.IsDisposed = 1;
                        return;
                    }
                }
            }
        }
    }
}
