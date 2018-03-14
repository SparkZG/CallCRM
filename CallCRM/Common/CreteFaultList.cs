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
using System.Data.SqlClient;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CallCRM.Common
{
    public enum FaultType
    {
        CallDispose,
        Created,
        Pending,
        NoAnswer
    }
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

        //public static AbstractDataBase sqlOperateClass = DBFactory.Create(DBType.SqlServer, DBFactory.GetConn());
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
        public static bool CreateOrder(FaultDataModel fdm, FaultType _type, bool _IsInsert)
        {
            string sql = string.Empty;
            int rows = 0;
            //只有当未接听的时候才插入，否则都是更新
            #region postgreSQL
            /*
            if (_IsInsert)
            {
                sql = "insert into call_log(date,start_time,during_time,phone,note,file_path,line_no,chan_id,user_id,asset_type_id,breakdown_categ,company_id,address,state,work_property,department_id,knowledge_id,note_result,source_id)" +
                " values(@date,@start_time,@during_time,@phone,@note,@file_path,@line_no,@chan_id,@user_id,@asset_type_id,@breakdown_categ,@company_id,@address,@state,@work_property,@department_id,@knowledge_id,@note_result,@source_id)";
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
                                         new NpgsqlParameter("@work_property",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@department_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@knowledge_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@note_result",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@source_id",NpgsqlDbType.Integer),
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
                para[13].Value = GetState(_type);
                para[14].Value = fdm.work_property;
                para[15].Value = fdm.department_id;
                para[16].Value = fdm.knowledge_id;
                para[17].Value = fdm.note_result;
                para[18].Value = fdm.ID;

                rows = PostgresqlHelper.ExecuteNonQuery(sql, para);
            }
            else
            {
                sql = "update call_log set note=@note,user_id=@user_id,asset_type_id=@asset_type_id,breakdown_categ=@breakdown_categ,company_id=@company_id,address=@address,state=@state,"
                    + "work_property=@work_property,department_id=@department_id,knowledge_id=@knowledge_id,note_result=@note_result where source_id=@source_id and phone=@phone";
                NpgsqlParameter[] para = { 
                                         
                                         new NpgsqlParameter("@phone",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@note",  NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@user_id",  NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@asset_type_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@breakdown_categ",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@company_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@address",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@state",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@work_property",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@department_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@knowledge_id",NpgsqlDbType.Integer),
                                         new NpgsqlParameter("@note_result",NpgsqlDbType.Varchar),
                                         new NpgsqlParameter("@source_id",NpgsqlDbType.Integer),
                                    };
                para[0].Value = fdm.CallerID;
                para[1].Value = fdm.note;
                para[2].Value = fdm.user_id;
                para[3].Value = fdm.asset_type_id;
                para[4].Value = fdm.breakdown_categ;
                para[5].Value = fdm.company_id;
                para[6].Value = fdm.address;
                para[7].Value = GetState(_type);
                para[8].Value = fdm.work_property;
                para[9].Value = fdm.department_id;
                para[10].Value = fdm.knowledge_id;
                para[11].Value = fdm.note_result;
                para[12].Value = fdm.ID;

                rows = PostgresqlHelper.ExecuteNonQuery(sql, para);
            }*/
            #endregion

            //sqlserver
            if (_IsInsert)
            {
                sql = "insert into call_log(date,start_time,during_time,phone,note,file_path,line_no,chan_id,user_id,asset_type_id,breakdown_categ,company_id,address,state,work_property,department_id,knowledge_id,note_result,source_id)" +
                " values(@date,@start_time,@during_time,@phone,@note,@file_path,@line_no,@chan_id,@user_id,@asset_type_id,@breakdown_categ,@company_id,@address,@state,@work_property,@department_id,@knowledge_id,@note_result,@source_id)";
                MySqlParameter[] para = { 
                                         new MySqlParameter("@date",   MySqlDbType.Date),
                                         new MySqlParameter("@start_time",SqlDbType.VarChar),
                                         new MySqlParameter("@during_time",  MySqlDbType.VarChar),
                                         new MySqlParameter("@phone",MySqlDbType.VarChar),
                                         new MySqlParameter("@note",  MySqlDbType.VarChar),
                                         new MySqlParameter("@file_path",  MySqlDbType.VarChar),
                                         new MySqlParameter("@line_no",  MySqlDbType.VarChar),
                                         new MySqlParameter("@chan_id",MySqlDbType.Int32),
                                         new MySqlParameter("@user_id",  MySqlDbType.Int32),
                                         new MySqlParameter("@asset_type_id",MySqlDbType.Int32),
                                         new MySqlParameter("@breakdown_categ",MySqlDbType.VarChar),
                                         new MySqlParameter("@company_id",MySqlDbType.Int32),
                                         new MySqlParameter("@address",MySqlDbType.VarChar),
                                         new MySqlParameter("@state",MySqlDbType.VarChar),
                                         new MySqlParameter("@work_property",MySqlDbType.VarChar),
                                         new MySqlParameter("@department_id",MySqlDbType.Int32),
                                         new MySqlParameter("@knowledge_id",MySqlDbType.Int32),
                                         new MySqlParameter("@note_result",MySqlDbType.VarChar),
                                         new MySqlParameter("@source_id",MySqlDbType.Int32),
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
                para[13].Value = GetState(_type);
                para[14].Value = fdm.work_property;
                para[15].Value = fdm.department_id;
                para[16].Value = fdm.knowledge_id;
                para[17].Value = fdm.note_result;
                para[18].Value = fdm.ID;

                rows = MySqlHelperClass.ExecuteNonQuery(MySqlHelperClass.GetConn(), System.Data.CommandType.Text, sql, para); 
            }
            else
            {
                sql = "update call_log set note=@note,user_id=@user_id,asset_type_id=@asset_type_id,breakdown_categ=@breakdown_categ,company_id=@company_id,address=@address,state=@state,"
                    + "work_property=@work_property,department_id=@department_id,knowledge_id=@knowledge_id,note_result=@note_result where source_id=@source_id and phone=@phone";
                MySqlParameter[] para = { 
                                         
                                         new MySqlParameter("@phone",MySqlDbType.VarChar),
                                         new MySqlParameter("@note",  MySqlDbType.VarChar),
                                         new MySqlParameter("@user_id",  MySqlDbType.Int32),
                                         new MySqlParameter("@asset_type_id",MySqlDbType.Int32),
                                         new MySqlParameter("@breakdown_categ",MySqlDbType.VarChar),
                                         new MySqlParameter("@company_id",MySqlDbType.Int32),
                                         new MySqlParameter("@address",MySqlDbType.VarChar),
                                         new MySqlParameter("@state",MySqlDbType.VarChar),
                                         new MySqlParameter("@work_property",MySqlDbType.VarChar),
                                         new MySqlParameter("@department_id",MySqlDbType.Int32),
                                         new MySqlParameter("@knowledge_id",MySqlDbType.Int32),
                                         new MySqlParameter("@note_result",MySqlDbType.VarChar),
                                         new MySqlParameter("@source_id",MySqlDbType.Int32),
                                    };
                para[0].Value = fdm.CallerID;
                para[1].Value = fdm.note;
                para[2].Value = fdm.user_id;
                para[3].Value = fdm.asset_type_id;
                para[4].Value = fdm.breakdown_categ;
                para[5].Value = fdm.company_id;
                para[6].Value = fdm.address;
                para[7].Value = GetState(_type);
                para[8].Value = fdm.work_property;
                para[9].Value = fdm.department_id;
                para[10].Value = fdm.knowledge_id;
                para[11].Value = fdm.note_result;
                para[12].Value = fdm.ID;

                rows = MySqlHelperClass.ExecuteNonQuery(MySqlHelperClass.GetConn(), System.Data.CommandType.Text, sql, para); 
            }



            if (rows > 0)
                return true;
            else
                return false;
        }

        public static DataTable GetUser()
        {
            string strsql = "select a.id,a.login,a.username,a.department_id,b.name as department_name from res_users a left join hr_department b on a.department_id=b.id";
            return MySqlHelperClass.GetDataSet(MySqlHelperClass.GetConn(), System.Data.CommandType.Text, strsql, null).Tables[0]; 
        }
        public static DataTable GetDepartment()
        {
            string strsql = "select id,code,name from hr_department ";
            return MySqlHelperClass.GetDataSet(MySqlHelperClass.GetConn(), System.Data.CommandType.Text, strsql, null).Tables[0]; 
        }
        public static DataTable GetAssetType()
        {
            string strsql = "select id,name from asset_type ";
            return MySqlHelperClass.GetDataSet(MySqlHelperClass.GetConn(), System.Data.CommandType.Text, strsql, null).Tables[0]; 
        }
        public static DataTable GetCompany()
        {
            string strsql = "select id,name from res_company ";
            return MySqlHelperClass.GetDataSet(MySqlHelperClass.GetConn(), System.Data.CommandType.Text, strsql, null).Tables[0]; 
        }
        public static DataTable GetKnowledge()
        {
            string strsql = "select id,name,note_result from knowledge_case ";
            return MySqlHelperClass.GetDataSet(MySqlHelperClass.GetConn(), System.Data.CommandType.Text, strsql, null).Tables[0]; 
        }
        public static DataTable GetOrder(int _accessID, string _callerID)
        {
            string strsql = string.Format("select * from call_log where source_id={0} and phone='{1}'", _accessID, _callerID);
            return MySqlHelperClass.GetDataSet(MySqlHelperClass.GetConn(), System.Data.CommandType.Text, strsql, null).Tables[0]; 
        }


        public static string GetState(FaultType type)
        {
            switch (type)
            {
                case FaultType.CallDispose:
                    return "2";
                case FaultType.Created:
                    return "3";
                case FaultType.Pending:
                    return "1";
                default:
                    return "0";
            }
        }
        public static FaultType GetFaultType(string str)
        {
            switch (str)
            {
                case "2":
                    return FaultType.CallDispose;
                case "3":
                    return FaultType.Created;
                case "1":
                    return FaultType.Pending;
                default:
                    return FaultType.NoAnswer;
            }
        }
        public static string GeStateStr(int state)
        {
            switch (state)
            {
                case 2:
                    return "电话已解决";
                case 3:
                    return "已报修";
                case 1:
                    return "待处理";
                default:
                    return "未接听";
            }
        }
        /// <summary>
        /// 获取媒体文件信息
        /// </summary>
        /// <param name="_callID"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public static DataTable GetMediaInfo(string _callID, string _date)
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
        public static bool UpdateIsCreate(int accessID, FaultType _type)
        {
            string sql = "update CallLogTable set IsDisposed = @IsDisposed where ID = @ID";//and StartDate = @StartDate order by ID desc";
            OleDbParameter[] para = { 
                                         new OleDbParameter("@IsDisposed",OleDbType.Integer),
                                         new OleDbParameter("@ID",OleDbType.Integer),
                                     };
            para[0].Value = Convert.ToInt32(GetState(_type)); ;
            para[1].Value = accessID;

            int rows = AccessHelper.ExecuteNonQuery(sql, para);

            if (rows > 0)
                return true;
            else
                return false;
        }
    }
}
