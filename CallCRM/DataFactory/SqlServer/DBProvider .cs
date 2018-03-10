using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCRM.DataFactory
{
    /// <summary>
    /// 数据库类型枚举值
    /// </summary>
    public enum DBType { OleDb, SqlServer, Odbc }

    public class DBProvider
    {
        private DBProvider() { }
    }
}
