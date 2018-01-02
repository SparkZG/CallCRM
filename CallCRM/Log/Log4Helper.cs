using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CallCRM.Log
{
    /// <summary>
    /// 用于日志记录
    /// </summary>
    public static class Log4Helper
    {
        public static void Fatal(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Fatal(message);
            else
                log.Fatal(message, exception);
        }

        /// <summary>
        /// 程序发生错误时记录
        /// </summary>
        public static void Error(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Error(message);
            else
                log.Error(message, exception);
        }

        /// <summary>
        /// 程序发生警告时记录
        /// </summary>
        public static void Warn(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Warn(message);
            else
                log.Warn(message, exception);
        }

        /// <summary>
        /// 程序发生提示时记录
        /// </summary>
        public static void Info(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Info(message);
            else
                log.Info(message, exception);
        }

        /// <summary>
        /// 程序发生崩溃时记录
        /// </summary>
        public static void Debug(Type type, object message, Exception exception = null)
        {
            ILog log = LogManager.GetLogger(type);
            if (exception == null)
                log.Debug(message);
            else
                log.Debug(message, exception);
        }
    }
}
