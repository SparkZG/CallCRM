using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CallCRM.DataFactory
{
    /// <summary>
    /// 抽象类,定义数据库访问的方法
    /// SqlServer有完整的实现,可以使用
    /// Odbc和OleDb尚未完整实现,不建议适用
    /// </summary>
    public abstract class AbstractDataBase
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connectionString;

        protected abstract IDbConnection GetConnection();

        protected abstract IDbCommand GetCommand();

        protected abstract IDbCommand GetCommand(string sql, IDbConnection iConn);

        protected abstract IDataAdapter GetAdapter(IDbCommand iCmd);

        protected abstract IDataAdapter GetAdapter(string sql, IDbConnection iConn);

        public abstract string ConnectTest(string serverIp, string port, string database, string uid, string password);

        /// <summary>
        /// 初始化IDbCommand对象,并给SQL语句的参数赋值
        /// </summary>
        /// <param name="iCmd"></param>
        /// <param name="iConn"></param>
        /// <param name="iTrans"></param>
        /// <param name="sql"></param>
        /// <param name="iParams"></param>
        protected void PrepareCommand(out IDbCommand iCmd, IDbConnection iConn, IDbTransaction iTrans, string sql, IDataParameter[] iParams)
        {
            if (iConn.State != ConnectionState.Open)
            {
                iConn.Open();
            }
            iCmd = this.GetCommand();
            iCmd.Connection = iConn;
            iCmd.CommandText = sql;
            if (iTrans != null)
            {
                iCmd.Transaction = iTrans;
            }
            iCmd.CommandType = CommandType.Text;

            if (iParams != null)
            {
                foreach (IDataParameter param in iParams)
                {
                    iCmd.Parameters.Add(param);
                }
            }
        }

        /// <summary>
        /// 构建IDbCommand对象,返回一个结果集
        /// </summary>
        /// <param name="iConn">数据库连接</param>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="iParams">存储过程参数</param>
        /// <returns></returns>
        protected IDbCommand BuildQueryCommand(IDbConnection iConn, string storedProcName, IDataParameter[] iParams)
        {
            IDbCommand iCmd = this.GetCommand(storedProcName, iConn);
            iCmd.CommandType = CommandType.StoredProcedure;
            if (iParams == null)
            {
                return iCmd;
            }
            foreach (IDataParameter parameter in iParams)
            {
                iCmd.Parameters.Add(parameter);
            }
            return iCmd;
        }

        protected abstract IDbCommand BuildIntCommand(IDbConnection iConn, string storedProcName, IDataParameter[] iParams);

        #region 基础的增删改查操作

        /// <summary>
        /// 执行不带参数的单条sql语句的增,删,改操作
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public abstract int ExcuteSql(string sql);

        /// <summary>
        /// 执行带参数的单条sql语句的增,删,改操作
        /// </summary>
        /// <param name="sql">SQL语句,如："insert into UserInfo (username,password) values (@username,@password)"</param>
        /// <param name="iParams">SqlServer参数格式：SqlParameter[] parms = { new SqlParameter("@username",username),new SqlParameter("@password",password) };</param>
        /// <returns>受影响的行数</returns>
        public abstract int ExcuteSql(string sql, params IDataParameter[] iParams);

        /// <summary>
        /// 执行无参数的查询语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public abstract DataSet Query(string sql, string tableName);

        /// <summary>
        /// 执行带参数的查询语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <param name="iParams">参数</param>
        /// <returns>DataSet</returns>
        public abstract DataSet Query(string sql, string tableName, params IDataParameter[] iParams);

        /// <summary>
        /// 执行一条计算查询结果的SQL语句，返回计算结果（单行单列）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="iParams">参数</param>
        /// <returns>object</returns>
        public abstract object GetSingle(string sql, params IDataParameter[] iParams);

        #endregion


        #region 存储过程

        /// <summary>
        /// 执行存储过程的增、删、改操作,返回受影响的行数
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="iParams">存储过程参数</param>
        /// <returns>受影响的行数</returns>
        public abstract int ProcedureSql(string storedProcName, IDataParameter[] iParams);

        /// <summary>
        /// 执行存储过程的增、删、改操作,返回受影响的行数,并获取存储过程的返回值
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="iParams">存储过程参数</param>
        /// <param name="value">存储过程的返回值</param>
        /// <returns>受影响的行数</returns>
        public abstract int ProcedureSql(string storedProcName, IDataParameter[] iParams, out int value);

        /// <summary>
        /// 执行存储过程的查询
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <param name="iParams">参数</param>
        /// <returns>DataSet</returns>
        public abstract DataSet ProcedureQuery(string storedProcName, string tableName, params IDataParameter[] iParams);

        /// <summary>
        /// 存储过程的分页查询
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <param name="startIndex">开始记录索引,第一条记录从0开始</param>
        /// <param name="pageSize">每页记录个数</param>
        /// <param name="iParams">存储过程参数</param>
        /// <returns>DataSet</returns>
        public abstract DataSet ProcedureQuery(string storedProcName, string tableName, int startIndex, int pageSize, params IDataParameter[] iParams);

        /// <summary>
        /// 执行存储过程，返回单行单列
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="iParams">存储过程参数</param>
        /// <returns></returns>
        public abstract string ProcedureSingle(string storedProcName, params IDataParameter[] iParams);

        #endregion


        #region 数据库事务操作

        /// <summary>
        /// 执行多条增、删、改SQL语句
        /// </summary>
        /// <param name="list">SQL语句的哈希表（key为sql语句，value是该语句的参数SqlParameter[]）</param>
        public abstract void ExcuteTransaction(System.Collections.Hashtable list);

        #endregion
    }
}
