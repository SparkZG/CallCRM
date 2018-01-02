using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;
using CallCRM.Log;

namespace CallCRM.DataFactory
{
    public class PostgresqlHelper
    {
        //数据库连接字符串
        public static string strConn = "Server=36.7.68.136;Database=itms;Uid=openpg;Password=openpgpwd;port=5432;";

        public static DataSet ExecuteQuery(string sqrstr)
        {
            DataSet ds = new DataSet();
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(strConn))
            {
                try
                {
                    if (SqlConn.State != ConnectionState.Open)
                        SqlConn.Open();
                    using (NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sqrstr, SqlConn))
                    {
                        sqldap.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(new PostgresqlHelper().GetType(), "数据库操作异常！", ex);
                    throw(ex);
                }

            }
            return ds;
        }

        public static int ExecuteNonQuery(string sqrstr, NpgsqlParameter[] cmdParms)
        {
            int val = 0;
            using (NpgsqlConnection SqlConn = new NpgsqlConnection(strConn))
            {
                try
                {
                    if (SqlConn.State != ConnectionState.Open)
                        SqlConn.Open();
                    using (NpgsqlCommand SqlCommand = new NpgsqlCommand(sqrstr, SqlConn))
                    {
                        if (cmdParms != null)
                        {
                            foreach (NpgsqlParameter parm in cmdParms)
                                SqlCommand.Parameters.Add(parm);
                        }
                        val = SqlCommand.ExecuteNonQuery();  //执行查询并返回受影响的行数                    
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(new PostgresqlHelper().GetType(), "数据库操作异常！", ex);
                    throw(ex);
                }
            }
            return val; //r如果是>0操作成功！ 
        }

    }
}
