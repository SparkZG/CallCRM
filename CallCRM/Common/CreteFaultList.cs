using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CallCRM.ViewPage;
using Npgsql;
using System.Data;
using CallCRM.Models;
using System.Data.OleDb;
using NpgsqlTypes;
using CallCRM.DataFactory;

namespace CallCRM.Common
{
    public class CallTime
    {
        public CallTime(DateTime dtNow)
        {
            Date = dtNow.ToString("yyyy-MM-dd");
            Time = dtNow.ToShortTimeString();
        }
        public string Date { get; set; }
        public string Time { get; set; }
    }

    public class CreteFaultList
    {
        public static RingWindow rw = null;


        public static void ShowCreateWindow(string number, CallTime ct)
        {
            BombScreen bs = new BombScreen(number, ct);
            bs.Show();
        }

        //来电动画
        public static void ShowWaitWindow(string number)
        {
            if (rw == null)
            {
                rw = new RingWindow(number);
                rw.Show();
            }
        }
        public static void CloseWaitWindow()
        {
            if (rw != null)
            {
                rw.Close();
                rw = null;
            }
        }

        /// <summary>
        /// 创建故障单
        /// </summary>
        /// <param name="fdm"></param>
        /// <returns></returns>
        public static bool CreateOrder(FaultDataModel fdm)
        {
            string sql = "insert into call_log(date,start_time,during_time,phone,note,file_path,line_no,chan_id,user_id,asset_type_id,breakdown_categ,company_id,address,state)" +
                " values(@date,@start_time,@during_time,@phone,@note,@file_path,@line_no,@chan_id,@user_id,@asset_type_id,@breakdown_categ,@company_id,@address,@state)";
            NpgsqlParameter[] para = { 
                                         new NpgsqlParameter("@date",  NpgsqlDbType.Date),
                                         new NpgsqlParameter("@start_time",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@during_time",  NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@phone",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@note",  NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@file_path",  NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@line_no",  NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@chan_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@user_id",  NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@asset_type_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@breakdown_categ",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@company_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@address",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@state",NpgsqlDbType.Varchar),
                                    };
            para[0].Value = fdm.StartDate;
            para[1].Value = fdm.StartTime;
            para[2].Value = fdm.DuringTime;
            para[3].Value = fdm.CallerID;
            para[4].Value = fdm.note;
            para[5].Value = fdm.WaveFilePath;
            para[6].Value = fdm.LineID;
            para[7].Value = fdm.Chan;
            para[8].Value = fdm.user_id;
            para[9].Value = fdm.asset_type_id;
            para[10].Value = fdm.breakdown_categ;
            para[11].Value = fdm.company_id;
            para[12].Value = fdm.address;
            para[13].Value = "0";

            int rows = PostgresqlHelper.ExecuteNonQuery(sql, para);

            if (rows > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 获取媒体文件信息
        /// </summary>
        /// <param name="_callID"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public static DataTable GetMediaInfo(string _callID,string _date)
        {
            string sql = "select * from CallLogTable where CallerID = @CallerID and StartDate = @StartDate order by ID desc";//and StartDate = @StartDate order by ID desc";
            OleDbParameter[] para = { 
                                         new OleDbParameter("@CallerID",OleDbType.VarChar),
                                         new OleDbParameter("@StartDate",OleDbType.VarChar),
                                     };
            para[0].Value = _callID;
            para[1].Value = _date;

            return AccessHelper.Adapter(sql, para);
        }

        /// <summary>
        /// 更新access数据库中创单状态
        /// </summary>
        /// <param name="accessID"></param>
        /// <returns></returns>
        public static bool UpdateIsCreate(int accessID)
        {
            string sql = "update CallLogTable set IsDisposed = @IsDisposed where ID = @ID";//and StartDate = @StartDate order by ID desc";
            OleDbParameter[] para = { 
                                         new OleDbParameter("@IsDisposed",OleDbType.Integer),
                                         new OleDbParameter("@ID",OleDbType.Integer),
                                     };
            para[0].Value = 1;
            para[1].Value = accessID;

            int rows=AccessHelper.ExecuteNonQuery(sql, para);

            if (rows > 0)
                return true;
            else
                return false;
        }
    }
}
