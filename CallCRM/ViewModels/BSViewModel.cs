using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections;
using DevExpress.Mvvm;
using CallCRM.Models;
using System.Data;
using CallCRM.Common;
using CallCRM.DataFactory;
using CallCRM.ConnFactory;
using System.Threading;

namespace CallCRM.ViewModels
{
    public class SelfKeyValue
    {
        public SelfKeyValue(int _key, string _value)
        {
            Key = _key;
            Value = _value;
        }
        public SelfKeyValue(string _text, string _value)
        {
            Text = _text;
            Value = _value;
        }
        public SelfKeyValue(int _key, string _text, string _value)
        {
            Key = _key;
            Text = _text;
            Value = _value;
        }
        /// <summary>
        /// 键
        /// </summary>
        public int Key { set; get; }
        /// <summary>
        /// 键
        /// </summary>
        public string Text { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// 重写ToString()方法
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }
    public class BSViewModel : ViewModelBase
    {
        /// <summary>
        /// 资产类型
        /// </summary>
        public List<SelfKeyValue> AssetTypeDict = new List<SelfKeyValue> { };
        /// <summary>
        /// 用户
        /// </summary>
        public List<SelfKeyValue> UserDict = new List<SelfKeyValue> { };
        /// <summary>
        /// 区域
        /// </summary>
        public List<SelfKeyValue> CompanyDict = new List<SelfKeyValue> { };
        /// <summary>
        /// 故障类型
        /// </summary>
        public List<SelfKeyValue> FaultDict = new List<SelfKeyValue> { };
        /// <summary>
        /// 工单类型
        /// </summary>
        public List<SelfKeyValue> OrderDict = new List<SelfKeyValue> { };

        /// <summary>
        /// 用来存放手动创单传过来的数据
        /// </summary>
        public DataModel _DataModel = new DataModel();

        /// <summary>
        /// 来电时间
        /// </summary>
        public CallTime _callTime;

        public Action GetAccessAction = null;

        #region 自动填充数据段

        /// <summary>
        /// Access数据库ID（唯一标识）
        /// </summary>
        private int accessID = 0;
        public int AccessID
        {
            get { return accessID; }
            set
            {
                SetProperty<int>(ref accessID, value, "AccessID");
            }
        }
        /// <summary>
        /// 来电号码
        /// </summary>
        private string callerID = "";
        public string CallerID
        {
            get { return callerID; }
            set
            {
                SetProperty<string>(ref callerID, value, "CallerID");
            }
        }

        private int chan;
        public int Chan
        {
            get { return chan; }
            set
            {
                SetProperty<int>(ref chan, value, "Chan");
            }
        }

        private string lineID;
        public string LineID
        {
            get { return lineID; }
            set
            {
                SetProperty<string>(ref lineID, value, "LineID");
            }
        }

        private string startTime = "";
        public string StartTime
        {
            get { return startTime; }
            set
            {
                SetProperty<string>(ref startTime, value, "StartTime");
            }
        }

        private string duringTime = "";
        public string DuringTime
        {
            get { return duringTime; }
            set
            {
                SetProperty<string>(ref duringTime, value, "DuringTime");
            }
        }

        private string waveFilePath = "";
        public string WaveFilePath
        {
            get { return waveFilePath; }
            set
            {
                SetProperty<string>(ref waveFilePath, value, "WaveFilePath");
            }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                SetProperty<DateTime>(ref startDate, value, "StartDate");
            }
        }

        #endregion

        #region 手动填充数据段

        private string addres = "";
        public string Addres
        {
            get { return addres; }
            set
            {
                SetProperty<string>(ref addres, value, "Addres");
            }
        }

        private string note = "";
        public string Note
        {
            get { return note; }
            set
            {
                SetProperty<string>(ref note, value, "Note");
            }
        }

        private SelfKeyValue user;
        public SelfKeyValue User
        {
            get { return user; }
            set
            {
                SetProperty<SelfKeyValue>(ref user, value, "User");
                CheckNull();
            }
        }

        private SelfKeyValue assetType;
        public SelfKeyValue AssetType
        {
            get { return assetType; }
            set
            {
                SetProperty<SelfKeyValue>(ref assetType, value, "AssetType");
                CheckNull();
            }
        }

        private SelfKeyValue breakDownCate;
        public SelfKeyValue BreakDownCate
        {
            get { return breakDownCate; }
            set
            {
                SetProperty<SelfKeyValue>(ref breakDownCate, value, "BreakDownCate");
            }
        }

        private SelfKeyValue orderType;
        public SelfKeyValue OrderType
        {
            get { return orderType; }
            set
            {
                SetProperty<SelfKeyValue>(ref orderType, value, "OrderType");
            }
        }

        private SelfKeyValue company;
        public SelfKeyValue Company
        {
            get { return company; }
            set
            {
                SetProperty<SelfKeyValue>(ref company, value, "Company");
                CheckNull();
            }
        }

        private void CheckNull()
        {
            if (user != null && assetType != null && company != null)
            {
                Prompt = string.Empty;
            }
        }


        #endregion

        private string prompt;
        public string Prompt
        {
            get { return prompt; }
            set
            {
                SetProperty<string>(ref prompt, value, "Prompt");
            }
        }

        /// <summary>
        /// 表示当前创建故障单是在线创建还是手动创建
        /// </summary>
        public bool isOnline = true;
        public bool IsOnline
        {
            get { return isOnline; }
            set
            {
                SetProperty<bool>(ref isOnline, value, "IsOnline");
            }
        }

        public BSViewModel(string number, CallTime ct)
        {
            _callTime = ct;
            CallerID = number;
            GetMainData();
            //在线创建
            IsOnline = true;
        }
        public BSViewModel(DataModel dm)
        {
            AccessID = dm.ID;
            _DataModel = dm;
            CallerID = dm.CallerID;
            Chan = dm.Chan;
            LineID = dm.LineID;
            StartTime = dm.StartTime;
            DuringTime = dm.DuringTime;
            WaveFilePath = dm.WaveFilePath;
            StartDate = dm.StartDate;

            //在线创建
            IsOnline = false;

            GetMainData();
        }

        public void RemoveCallID()
        {
            if (IsOnline && ViewModel.CallRecord.Contains(CallerID))
            {
                ViewModel.CallRecord.Remove(CallerID);
            }
            //关闭前去掉委托绑定
            ViewModel.HangupAction -= GetAccessAction;
        }

        public void GetMainData()
        {
            string strsql = "select id,login,username from res_users";
            DataTable dtUser = PostgresqlHelper.ExecuteQuery(strsql).Tables[0];
            foreach (DataRow item in dtUser.Rows)
            {
                UserDict.Add(new SelfKeyValue(Convert.ToInt32(item["id"]), item["login"].ToString(), item["username"].ToString()));
            }


            strsql = "select id,name from asset_type";
            DataTable dtAssetType = PostgresqlHelper.ExecuteQuery(strsql).Tables[0];
            foreach (DataRow item in dtAssetType.Rows)
            {
                AssetTypeDict.Add(new SelfKeyValue(Convert.ToInt32(item["id"]), item["name"].ToString()));
            }

            strsql = "select id,name from res_company";
            DataTable dtCompany = PostgresqlHelper.ExecuteQuery(strsql).Tables[0];
            foreach (DataRow item in dtCompany.Rows)
            {
                CompanyDict.Add(new SelfKeyValue(Convert.ToInt32(item["id"]), item["name"].ToString()));
            }

            //插入故障类型
            FaultDict.Add(new SelfKeyValue("hardware", "硬件故障"));
            FaultDict.Add(new SelfKeyValue("soft", "软件故障"));
            FaultDict.Add(new SelfKeyValue("other", "其他"));

            OrderDict.Add(new SelfKeyValue("A", "故障申告"));
            OrderDict.Add(new SelfKeyValue("B", "耗材配送"));

            GetAccessAction = () =>
            {
                //电话挂机后为了防止录音软件没及时写入数据库，等待1秒,
                Thread.Sleep(TimeSpan.FromSeconds(1));

                RefreshMediaInfo();
            };
            //增加委托绑定
            ViewModel.HangupAction += GetAccessAction;
        }

        public void RefreshMediaInfo()
        {
            DataTable callTable = CreteFaultList.GetMediaInfo(CallerID, _callTime.Date);
            if (callTable.Rows.Count > 0)
            {
                DataRow dr = callTable.Rows[0];
                AccessID = Convert.ToInt32(dr["ID"]);
                Chan = Convert.ToInt32(dr["Chan"]);
                LineID = dr["LineID"].ToString();
                CallerID = dr["CallerID"].ToString();
                StartDate = Convert.ToDateTime(dr["StartDate"]);
                StartTime = dr["StartTime"].ToString();
                DuringTime = dr["DuringTime"].ToString();
                WaveFilePath = BLLCommon.GetWavPath() + dr["WaveFilePath"].ToString();

                //设置录音信息
                _DataModel.SetData(dr);
            }
        }

        public bool CreateFaultOrder()
        {
            var fdm = new FaultDataModel();
            if (User == null)
            {
                //不能创建
                Prompt = "报修人员未填写！";
                return false;
            }
            else
                fdm.user_id = User.Key;

            if (AssetType == null)
            {
                //不能创建
                Prompt = "资产类型未填写！";
                return false;
            }
            else
                fdm.asset_type_id = AssetType.Key;

            if (Company == null)
            {
                //不能创建
                Prompt = "请选择区域！";
                return false;
            }
            else
                fdm.company_id = Company.Key;

            if (OrderType == null)
            {
                //不能创建
                Prompt = "请选择工单类型！";
                return false;
            }
            else
                fdm.work_property = OrderType.Text;

            if (BreakDownCate == null)
            {
                fdm.breakdown_categ = FaultDict[2].Text;
            }
            else
                fdm.breakdown_categ = BreakDownCate.Text;

            fdm.note = Note;
            fdm.address = Addres;

            //设置录音记录信息
            fdm.SetData(_DataModel);


            //这里地址需要加上ip地址
            string ipStr = Provider.getIPAddress();
            if (ipStr == "36.7.68.136")
            {
                fdm.WaveFilePath = WaveFilePath;
            }
            else
            {
                fdm.WaveFilePath = ipStr + "\\" + WaveFilePath;
            }


            //插入服务器

            if (CreteFaultList.CreateOrder(fdm))
            {
                CreteFaultList.UpdateIsCreate(AccessID);
                return true;
            }
            return false;
        }


    }
}
