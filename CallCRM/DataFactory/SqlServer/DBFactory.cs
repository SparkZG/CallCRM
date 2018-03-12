using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCRM.DataFactory
{

    /// <summary>
    /// 数据库工厂
    /// SqlServer有完整的实现,可以使用
    /// Odbc和OleDb尚未完整实现,如果要用,部分代码需要自己实现
    /// 用法:AbstractDataBase db = DBFactory.Create(DBType.SqlServer, "Data Source=127.0.0.1;Initial Catalog=testdb;User ID=sa;Password=battery");
    /// </summary>
    public class DBFactory
    {
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private DBFactory() { }


        //private static string connectionString = "";

        /// <summary>
        /// 将字符串转化为枚举类型
        /// </summary>
        /// <param name="src">字符串</param>
        /// <returns>DBType枚举类型</returns>
        private static DBType ToDataBaseType(string src)
        {
            switch (src.ToLower())
            { 
                case "oledb":
                    return DBType.OleDb;
                case "odbc":
                    return DBType.Odbc;
                case "sqlserver":
                    return DBType.SqlServer;
                default:
                    throw new Exception("指定的数据库类型不存在");
            }
        }

        /// <summary>
        /// 创建数据库实例
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static AbstractDataBase Create(DBType dbType)
        {
            switch (dbType)
            { 
                case DBType.OleDb:
                    return new OleDb();
                case DBType.Odbc:
                    return new Odbc();
                case DBType.SqlServer:
                    return new SqlServer();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 创建数据库实例
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static AbstractDataBase Create(string dbType)
        {
            return Create(ToDataBaseType(dbType));
        }

        /// <summary>
        /// 创建数据库实例
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectString">数据库连接字符串</param>
        /// <returns></returns>
        public static AbstractDataBase Create(DBType dbType, string connectString)
        {
            switch (dbType)
            {
                case DBType.OleDb:
                    return new OleDb(connectString);
                case DBType.Odbc:
                    return new Odbc(connectString);
                case DBType.SqlServer:
                    return new SqlServer(connectString);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 创建数据库实例
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectString">数据库连接字符串</param>
        /// <returns></returns>
        public static AbstractDataBase Create(string dbType, string connectString)
        {
            return Create(ToDataBaseType(dbType), connectString);
        }

        public static string GetConn()
        {
            return string.Format("Server={0},{1};Database={2};Uid={3};Password={4};integrated security=false;", Properties.Settings.Default.ServerIP,
                Properties.Settings.Default.ServerPort,
                Properties.Settings.Default.Database,
                Properties.Settings.Default.UserID,
                Properties.Settings.Default.Password);
        }

    }
}
