using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.ProviderBase;
using System.Data;
using DevExpress.XtraGrid;
using JRO;
using System.IO;
using CallCRM.Log;

namespace CallCRM.DataFactory
{
    /// <summary>
    /// 数据库的操作类库
    /// </summary>
    public class OperateAccess
    {
        //数据库连接
        public OdbcConnection odbCon;
        public string ConnString;//连接字符串  

        public OperateAccess(string strPath)
        {
            ConnString = string.Format(@"Driver={0};dbq=C:\Users\Administrator\Desktop\CallLog.dat;Uid=;", "{Microsoft Access Driver (*.mdb)}");
            odbCon = new OdbcConnection(ConnString);
            Open();
        }

        /// <summary>   
        /// 打开数据源链接   
        /// </summary>   
        /// <returns></returns>   
        public void Open()
        {
            try
            {
                odbCon.Open();
            }
            catch (Exception ex)
            {
                Log4Helper.Error(this.GetType(), "数据库打开Error.", ex);
                throw;
            }
        }

        /**/
        /// <summary>   
        /// 请在数据传递完毕后调用该函数，关闭数据链接。   
        /// </summary>   
        public void Close()
        {
            try
            {
                odbCon.Close();
            }
            catch (Exception ex)
            {
                Log4Helper.Error(this.GetType(), "数据库关闭Error.", ex);
                throw;
            }
        }
        /**/
        /// <summary>   
        /// 根据SQL命令返回数据DataTable的行数
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public int SelectToNumber(string SQL)
        {
            OdbcCommand cmd = new OdbcCommand(SQL, odbCon);
            int number = Convert.ToInt32(cmd.ExecuteScalar());
            return number;
        }
        /**/
        /// <summary>   
        /// 根据SQL命令返回数据DataTable数据表,   
        /// 可直接作为dataGridView的数据源   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public DataTable SelectToDataTable(string SQL)
        {
            try
            {
                OdbcDataAdapter adapter = new OdbcDataAdapter();
                OdbcCommand command = new OdbcCommand(SQL, odbCon);
                adapter.SelectCommand = command;
                DataTable Dt = new DataTable();
                adapter.Fill(Dt);
                return Dt;
            }
            catch (Exception ex)
            {
                Log4Helper.Error(this.GetType(), "数据库查询Error", ex);
                throw;
            }

        }

        /**/
        /// <summary>   
        /// 根据SQL命令返回数据DataSet数据集，其中的表可直接作为dataGridView的数据源。   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <param name="subtableName">在返回的数据集中所添加的表的名称</param>   
        /// <returns></returns>   
        public DataSet SelectToDataSet(string SQL, string subtableName)
        {
            OdbcDataAdapter adapter = new OdbcDataAdapter();
            OdbcCommand command = new OdbcCommand(SQL, odbCon);
            adapter.SelectCommand = command;
            DataSet Ds = new DataSet();
            Ds.Tables.Add(subtableName);
            adapter.Fill(Ds, subtableName);
            return Ds;
        }

        /**/
        /// <summary>   
        /// 在指定的数据集中添加带有指定名称的表，由于存在覆盖已有名称表的危险，返回操作之前的数据集。   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <param name="subtableName">添加的表名</param>   
        /// <param name="DataSetName">被添加的数据集名</param>   
        /// <returns></returns>   
        public DataSet SelectToDataSet(string SQL, string subtableName, DataSet DataSetName)
        {
            OdbcDataAdapter adapter = new OdbcDataAdapter();
            OdbcCommand command = new OdbcCommand(SQL, odbCon);
            adapter.SelectCommand = command;
            DataTable Dt = new DataTable();
            DataSet Ds = new DataSet();
            Ds = DataSetName;
            adapter.Fill(DataSetName, subtableName);
            return Ds;
        }

        /**/
        /// <summary>   
        /// 根据SQL命令返回OleDbDataAdapter，   
        /// 使用前请在主程序中添加命名空间System.Data.OleDb   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public OdbcDataAdapter SelectToOleDbDataAdapter(string SQL)
        {
            OdbcDataAdapter adapter = new OdbcDataAdapter();
            OdbcCommand command = new OdbcCommand(SQL, odbCon);
            adapter.SelectCommand = command;
            return adapter;
        }

        /**/
        /// <summary>   
        /// 执行SQL命令，不需要返回数据的修改，删除可以使用本函数   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public bool ExecuteSQLNonquery(string SQL)
        {
            OdbcCommand cmd = new OdbcCommand(SQL, odbCon);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 压缩Access数据库
        /// <summary>
        /// 压缩Access数据库
        /// </summary>
        /// <param name="DBPath">数据库绝对路径</param>
        public bool CompactAccess(string DBPath)
        {
            bool Isok = true;
            try
            {
                if (!File.Exists(DBPath))
                {
                    throw new Exception("目标数据库不存在,无法压缩");
                }

                //声明临时数据库名称
                string temp = DateTime.Now.Year.ToString();
                temp += DateTime.Now.Month.ToString();
                temp += DateTime.Now.Day.ToString();
                temp += DateTime.Now.Hour.ToString();
                temp += DateTime.Now.Minute.ToString();
                temp += DateTime.Now.Second.ToString() + ".bak";
                temp = DBPath.Substring(0, DBPath.LastIndexOf("\\") + 1) + temp;
                //定义临时数据库的连接字符串
                string temp2 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + temp + @";JET OLEDB:";//";Jet OLEDB:Database Password=123456";
                //定义目标数据库的连接字符串
                string DBPath2 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBPath + @";JET OLEDB:";//JET OLEDB:Engine Type=5";// ";Jet OLEDB:Database Password=123456";
                //创建一个JetEngineClass对象的实例
                JRO.JetEngineClass jt = new JRO.JetEngineClass();
                //使用JetEngineClass对象的CompactDatabase方法压缩修复数据库
                jt.CompactDatabase(DBPath2, temp2);
                //拷贝临时数据库到目标数据库(覆盖)
                File.Copy(temp, DBPath, true);
                //最后删除临时数据库
                File.Delete(temp);
            }
            catch (Exception ex)
            {
                Isok = false;
                Log4Helper.Error(this.GetType(), "数据库压缩Error", ex);
            }
            return Isok;
        }
        #endregion 
    }
}
