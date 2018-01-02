using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.Web;
using CallCRM.Log;

namespace CallCRM.DataFactory
{
    public class AccessHelper
    {
        //连接字符串
        static string connStr = string.Empty;

        public static void SetConnStr(string accessPath)
        {
            connStr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", accessPath);
        }

        /// <summary>
        /// 打开数据测试
        /// </summary>
        /// <returns></returns>
        public static bool OpenConn()
        {
            SetConnStr(Properties.Settings.Default.AccessPath);
            bool flag = false;
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Helper.Error(new AccessHelper().GetType(), "打开数据库操作Error", ex);
                flag = false;
            }
            return flag;
            
        }
        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="comText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string comText, params OleDbParameter[] param)
        {
            try
            {
                int val = 0;
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand(comText, conn))
                    {

                        if (param != null && param.Length != 0)
                        {
                            cmd.Parameters.AddRange(param);
                        }
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        val = cmd.ExecuteNonQuery();

                    }
                }
                return val;
            }
            catch (Exception ex)
            {
                Log4Helper.Error(new AccessHelper().GetType(), "数据库操作Error.", ex);
                throw (ex);
            }
        }
        /// <summary>
        /// 返回数据对象
        /// </summary>
        /// <param name="comText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string comText, params OleDbParameter[] param)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                using (OleDbCommand cmd = new OleDbCommand(comText, conn))
                {
                    if (param != null && param.Length != 0)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
        /// <summary>
        /// 返回table
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataTable Adapter(string cmdText, params OleDbParameter[] param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter())
                    {
                        using (OleDbCommand command = new OleDbCommand(cmdText, conn))
                        {
                            oda.SelectCommand = command;
                            if (param != null && param.Length != 0)
                            {
                                oda.SelectCommand.Parameters.AddRange(param);
                            }
                            if (conn.State == ConnectionState.Closed)
                            {
                                conn.Open();
                            }
                            oda.Fill(dt);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Helper.Error(new AccessHelper().GetType(), "数据库读取Error.", ex);
            }
            return dt;
        }
        /// <summary>
        /// 向前读取记录
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static OleDbDataReader ExectueReader(string cmdText, params OleDbParameter[] param)
        {
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                using (OleDbCommand cmd = new OleDbCommand(cmdText, conn))
                {
                    if (param != null && param.Length != 0)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
        }
        /// <summary>
        /// 读取存储过程
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataTable GetPro(string cmdText, CommandType type, params OleDbParameter[] param)
        {
            DataTable dt = new DataTable();
            using (OleDbDataAdapter sda = new OleDbDataAdapter(cmdText, connStr))
            {
                new OleDbCommand().CommandType = CommandType.StoredProcedure;
                if (param != null && param.Length != 0)
                {
                    sda.SelectCommand.Parameters.AddRange(param);
                }
                sda.Fill(dt);
            }
            return dt;
        }
    }
}
