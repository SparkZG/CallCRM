using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CallCRM.Log;

namespace CallCRM.DataFactory
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlServer:AbstractDataBase
    {
        public SqlServer() { }

        public SqlServer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override string ConnectTest(string serverIp, string port, string database, string uid, string password)
        {
            string conn = string.Format("Server={0},{1};Database={2};Uid={3};Password={4};integrated security=false;", serverIp, port, database, uid, password);
            using (SqlConnection SqlConn = new SqlConnection(conn))
            {
                try
                {
                    if (SqlConn.State != ConnectionState.Open)
                        SqlConn.Open();
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return null;
        }

        protected override System.Data.IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        protected override System.Data.IDbCommand GetCommand()
        {
            return new SqlCommand();
        }

        protected override System.Data.IDbCommand GetCommand(string sql, System.Data.IDbConnection iConn)
        {
            return new SqlCommand(sql, (SqlConnection)iConn);
        }

        protected override System.Data.IDataAdapter GetAdapter(System.Data.IDbCommand iCmd)
        {
            return new SqlDataAdapter((SqlCommand)iCmd);
        }

        protected override System.Data.IDataAdapter GetAdapter(string sql, System.Data.IDbConnection iConn)
        {
            return new SqlDataAdapter(sql, (SqlConnection)iConn);
        }

        protected override IDbCommand BuildIntCommand(IDbConnection iConn, string storedProcName, IDataParameter[] iParams)
        {
            IDbCommand iCmd = BuildQueryCommand(iConn, storedProcName, iParams);
            ((SqlCommand)iCmd).Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, String.Empty, DataRowVersion.Default, null));
            return iCmd;
        }

        #region 基础的增删改查操作

        public override int ExcuteSql(string sql)
        {
            int effectRows = 0;
            using (IDbConnection iConn = this.GetConnection())
            {
                using (IDbCommand iCmd = this.GetCommand(sql, iConn))
                {                    
                    try
                    {
                        iConn.Open();
                        effectRows = iCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Log4Helper.Error(GetType(), "SqlServer数据库操作异常！", ex);
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (iConn.State != ConnectionState.Closed)
                        {
                            iConn.Close();
                        }
                    }
                }
            }
            return effectRows;
        }

        public override int ExcuteSql(string sql, params IDataParameter[] iParams)
        {
            int effectRows = 0;
            using (IDbConnection iConn = this.GetConnection())
            {
                IDbCommand iCmd = null;
                try
                {
                    PrepareCommand(out iCmd, iConn, null, sql, iParams);
                    effectRows = iCmd.ExecuteNonQuery();
                    iCmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(GetType(), "SqlServer数据库操作异常！", ex);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    iCmd.Dispose();
                    if (iConn.State != ConnectionState.Closed)
                    {
                        iConn.Close();
                    }
                }
            }

            return effectRows;
        }

        public override DataSet Query(string sql, string tableName)
        {
            DataSet ds = new DataSet();
            using (IDbConnection iConn = this.GetConnection())
            {
                try
                {
                    iConn.Open();
                    IDataAdapter ida = this.GetAdapter(sql, iConn);
                    ida.TableMappings.Add("Table", tableName);
                    ida.Fill(ds);
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(GetType(), "SqlServer数据库操作异常！", ex);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (iConn.State != ConnectionState.Closed)
                    {
                        iConn.Close();
                    }
                }
            }
            return ds;
        }

        public override DataSet Query(string sql, string tableName, params IDataParameter[] iParams)
        {
            DataSet ds = new DataSet();
            using (IDbConnection iConn = this.GetConnection())
            {
                IDbCommand iCmd = null;
                try
                {
                    PrepareCommand(out iCmd, iConn, null, sql, iParams);
                    IDataAdapter ida = this.GetAdapter(iCmd);
                    ida.TableMappings.Add("Table", tableName);
                    ida.Fill(ds);
                    iCmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(GetType(), "SqlServer数据库操作异常！", ex);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    iCmd.Dispose();
                    if (iConn.State != ConnectionState.Closed)
                    {
                        iConn.Close();
                    }
                }
            }
            return ds;
        }

        public override object GetSingle(string sql, params IDataParameter[] iParams)
        {
            using (IDbConnection iConn = this.GetConnection())
            {
                IDbCommand iCmd = null;
                try
                {
                    iConn.Open();
                    PrepareCommand(out iCmd, iConn, null, sql, iParams);
                    object obj = iCmd.ExecuteScalar();
                    if (Object.Equals(obj, null) || Object.Equals(obj, System.DBNull.Value))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(GetType(), "SqlServer数据库操作异常！", ex);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    iCmd.Dispose();
                    if (iConn.State != ConnectionState.Closed)
                    {
                        iConn.Close();
                    }
                }
            }
        }

        #endregion


        #region 存储过程

        public override int ProcedureSql(string storedProcName, IDataParameter[] iParams)
        {
            int effectRows = 0;
            using (IDbConnection iConn = this.GetConnection())
            {
                iConn.Open();
                using (IDbCommand iCmd = this.BuildQueryCommand(iConn, storedProcName, iParams))
                {
                    try
                    {
                        effectRows = iCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (iConn.State != ConnectionState.Closed)
                        {
                            iConn.Close();
                        }
                    }
                }
            }
            return effectRows;
        }

        public override int ProcedureSql(string storedProcName, IDataParameter[] iParams, out int value)
        {
            int effectRows = 0;
            using (IDbConnection iConn = this.GetConnection())
            {
                iConn.Open();
                using (IDbCommand iCmd = this.BuildIntCommand(iConn, storedProcName, iParams))
                {
                    try
                    {
                        effectRows = iCmd.ExecuteNonQuery();
                        value = (int)((SqlCommand)iCmd).Parameters["ReturnValue"].Value;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (iConn.State != ConnectionState.Closed)
                        {
                            iConn.Close();
                        }
                    }
                }
            }
            return effectRows;
        }

        public override DataSet ProcedureQuery(string storedProcName, string tableName, params IDataParameter[] iParams)
        {
            DataSet ds = new DataSet();
            using (IDbConnection iConn = this.GetConnection())
            {
                iConn.Open();
                using (IDbCommand iCmd = this.BuildQueryCommand(iConn, storedProcName, iParams))
                {
                    try
                    {
                        IDataAdapter ida = this.GetAdapter(iCmd);
                        if (!String.IsNullOrEmpty(tableName))
                        {
                            ida.TableMappings.Add("Table", tableName);
                        }
                        ida.Fill(ds);
                        iCmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (iConn.State != ConnectionState.Closed)
                        {
                            iConn.Close();
                        }
                    }
                }
            }
            return ds;
        }

        public override DataSet ProcedureQuery(string storedProcName, string tableName, int startIndex, int pageSize, params IDataParameter[] iParams)
        {
            DataSet ds = new DataSet();
            using (IDbConnection iConn = this.GetConnection())
            {
                iConn.Open();
                using (IDbCommand iCmd = this.BuildQueryCommand(iConn, storedProcName, iParams))
                {
                    try
                    {
                        IDataAdapter ida = this.GetAdapter(iCmd);

                        ((SqlDataAdapter)ida).Fill(ds, startIndex, pageSize, tableName);

                        iCmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (iConn.State != ConnectionState.Closed)
                        {
                            iConn.Close();
                        }
                    }
                }
                return ds;
            }
        }

        public override string ProcedureSingle(string storedProcName, params IDataParameter[] iParams)
        {
            string value = String.Empty;

            using (IDbConnection iConn = this.GetConnection())
            {
                iConn.Open();
                using (IDbCommand iCmd = this.BuildQueryCommand(iConn, storedProcName, iParams))
                {
                    try
                    {
                        object obj = iCmd.ExecuteScalar();
                        if (obj == null)
                        {
                            value = null;
                        }
                        else
                        {
                            value = obj.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (iConn.State != ConnectionState.Closed)
                        {
                            iConn.Close();
                        }
                    }
                }
            }
            return value;
        }

        #endregion


        #region 数据库事务操作
        public override void ExcuteTransaction(System.Collections.Hashtable list)
        {
            using (IDbConnection iConn = this.GetConnection())
            {
                iConn.Open();
                using (IDbTransaction iTrans = iConn.BeginTransaction())
                {
                    IDbCommand iCmd = null;
                    try
                    {

                        foreach (System.Collections.DictionaryEntry entry in list)
                        {
                            string sql = entry.Key.ToString();

                            IDataParameter[] iParams = (IDataParameter[])entry.Value;

                            PrepareCommand(out iCmd, iConn, iTrans, sql, iParams);

                            iCmd.ExecuteNonQuery();

                            iCmd.Parameters.Clear();

                        }
                        iTrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        iTrans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        iCmd.Dispose();
                        if (iConn.State != ConnectionState.Closed)
                        {
                            iConn.Close();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
