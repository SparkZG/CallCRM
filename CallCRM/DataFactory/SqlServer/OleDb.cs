using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace CallCRM.DataFactory
{
    public class OleDb:AbstractDataBase
    {
        public OleDb() { }

        public OleDb(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public override string ConnectTest(string serverIp, string port, string database, string uid, string password)
        {
            throw new NotImplementedException();
        }

        protected override System.Data.IDbConnection GetConnection()
        {
            return new OleDbConnection(connectionString);
        }

        protected override System.Data.IDbCommand GetCommand()
        {
            return new OleDbCommand();
        }

        protected override System.Data.IDbCommand GetCommand(string sql, System.Data.IDbConnection iConn)
        {
            return new OleDbCommand(sql, (OleDbConnection)iConn);
        }

        protected override System.Data.IDataAdapter GetAdapter(System.Data.IDbCommand iCmd)
        {
            return new OleDbDataAdapter((OleDbCommand)iCmd);
        }

        protected override System.Data.IDataAdapter GetAdapter(string sql, System.Data.IDbConnection iConn)
        {
            return new OleDbDataAdapter(sql, (OleDbConnection)iConn);
        }

        protected override IDbCommand BuildIntCommand(IDbConnection iConn, string storedProcName, IDataParameter[] iParams)
        {
            IDbCommand iCmd = BuildQueryCommand(iConn, storedProcName, iParams);
            //((OleDbCommand)iCmd).Parameters.Add(new OleDbParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, String.Empty, DataRowVersion.Default, null));
            return iCmd;
        }

        #region 基础的增删改查操作

        public override int ExcuteSql(string sql)
        {
            throw new NotImplementedException();
        }

        public override int ExcuteSql(string sql, params IDataParameter[] iParams)
        {
            throw new NotImplementedException();
        }

        public override DataSet Query(string sql, string tableName)
        {
            throw new NotImplementedException();
        }

        public override DataSet Query(string sql, string tableName, params IDataParameter[] iParams)
        {
            throw new NotImplementedException();
        }

        public override object GetSingle(string sql, params IDataParameter[] iParams)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region 存储过程
        public override int ProcedureSql(string storedProcName, IDataParameter[] iParams)
        {
            throw new NotImplementedException();
        }

        public override int ProcedureSql(string storedProcName, IDataParameter[] iParams, out int value)
        {
            throw new NotImplementedException();
        }

        public override DataSet ProcedureQuery(string storedProcName, string tableName, params IDataParameter[] iParams)
        {
            throw new NotImplementedException();
        }

        public override DataSet ProcedureQuery(string storedProcName, string tableName, int startIndex, int pageSize, params IDataParameter[] iParams)
        {
            throw new NotImplementedException();
        }

        public override string ProcedureSingle(string storedProcName, params IDataParameter[] iParams)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 数据库事务操作
        public override void ExcuteTransaction(System.Collections.Hashtable list)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
