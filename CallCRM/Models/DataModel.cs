using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CallCRM.Common;
using DevExpress.Mvvm;

namespace CallCRM.Models
{
    /// <summary>
    /// 为了实现创建工单成功后主页面的建单信息实时刷新（变成已创建），临时将此类实现ViewModelBase接口
    /// </summary>
    public class DataModel : ViewModelBase
    {
        public DataModel()
        {
            ID = 0;
            //0代表呼入
            CallType = 0;
            Chan = 0;
            LineID = "";
            CallerID = "";
            StartDate = DateTime.Now.Date;
            StartTime = DateTime.Now.ToShortTimeString();
            DuringTime = "";
            WaveFilePath = "";
            IsDisposed = 0;
        }

        public void SetData(DataModel dm)
        {
            ID = dm.ID;
            CallType = dm.CallType;
            Chan = dm.Chan;
            LineID = dm.LineID;
            CallerID = dm.CallerID;
            StartDate = dm.StartDate;
            StartTime = dm.StartTime;
            DuringTime = dm.DuringTime;
            WaveFilePath = dm.WaveFilePath;
            IsDisposed = dm.IsDisposed;
        }
        public void SetData(DataRow dr)
        {
            ID = Convert.ToInt32(dr["ID"]);
            CallType = Convert.ToInt32(dr["CallType"]);
            Chan = Convert.ToInt32(dr["Chan"]);

            LineID = dr["LineID"].ToString();
            CallerID = dr["CallerID"].ToString();
            StartDate = Convert.ToDateTime(dr["StartDate"]).Date;
            StartTime = dr["StartTime"].ToString();
            DuringTime = dr["DuringTime"].ToString();
            WaveFilePath = BLLCommon.GetWavPath() + dr["WaveFilePath"].ToString();
            if (dr["IsDisposed"].ToString() == "")
                IsDisposed = 0;
            else
                IsDisposed = Convert.ToInt32(dr["IsDisposed"]);

            //如果access里面数据是未接听
            if (CallType == 0 && IsDisposed == 0)
            {
                IsDisposed = 1;
                CreteFaultList.UpdateIsCreate(ID, FaultType.Pending);
            }
            if (CallType == 2)
            {
                WaveFilePath = "";
            }
        }
        /// <summary>
        /// 序号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public int CallType { get; set; }
        /// <summary>
        /// 通道
        /// </summary>
        public int Chan { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string LineID { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string CallerID { get; set; }
        /// <summary>
        /// 通话日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 通话时长
        /// </summary>
        public string DuringTime { get; set; }
        /// <summary>
        /// 录音文件地址
        /// </summary>
        public string WaveFilePath { get; set; }
        /// <summary>
        /// 状态int值
        /// </summary>
        private int isDisposed;
        public int IsDisposed
        {
            get { return isDisposed; }
            set
            {
                SetProperty<int>(ref isDisposed, value, "IsDisposed");
                StateStr = CreteFaultList.GeStateStr(value);
            }
        }
        /// <summary>
        /// 状态string
        /// </summary>
        private string stateStr;
        public string StateStr
        {
            get { return stateStr; }
            set
            {
                SetProperty<string>(ref stateStr, value, "StateStr");
            }
        }
    }
}
