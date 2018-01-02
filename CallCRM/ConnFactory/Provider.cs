using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace CallCRM.ConnFactory
{
    public class Provider
    {
        public Provider() { }

        /// <summary>
        /// 获取本地IP的方法
        /// </summary>
        /// <returns></returns>
        public static string getIPAddress()
        {
            //获取本地所有IP地址
            IPHostEntry ipe = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] ip = ipe.AddressList;
            for (int i = 0; i < ip.Length; i++)
            {
                if (ip[i].AddressFamily.ToString().Equals("InterNetwork"))
                {

                    return ip[i].ToString();
                }
            }
            return null;
        }
    }

    public enum ServerType { UDP, TCPServer, TCPClient };

    /// <summary>
    /// Socket事件类型
    /// </summary>
    public enum SocketEventType
    {
        StartEvent,//服务器启动
        StopEvent,//服务器停止
        SendEvent,//服务器发送数据
        ReceEvent,//服务器接收数据
        Other//其他信息
    };




    /// <summary>
    /// 协议数据索引以及长度定义
    /// </summary>
    public class IALDS
    {
        /// <summary>
        /// 总长度
        /// </summary>
        public const int Total = 64;

        /// <summary>
        /// 包头标识
        /// </summary>
        public static IndexAndLenDefine headSign = new IndexAndLenDefine(0, 4, TypeCode.String);


        /// <summary>
        /// 消息类型
        /// </summary>
        public static IndexAndLenDefine messageType = new IndexAndLenDefine(4, 1, TypeCode.Byte);


        /// <summary>
        /// 附加信息长度
        /// </summary>
        public static IndexAndLenDefine addLen = new IndexAndLenDefine(5, 1, TypeCode.Byte);


        /// <summary>
        /// 附加信息,可更改！
        /// </summary>
        public static IndexAndLenDefine addLenInfo = new IndexAndLenDefine(6, 0, TypeCode.String);
    }
    public class IndexAndLenDefine
    {
        public IndexAndLenDefine(int di, int db, TypeCode _type)
        {
            defineIndex = di;
            defineByteNum = db;
            type = _type;
        }
        /// <summary>
        /// R103协议define索引
        /// </summary>
        public int defineIndex { get; set; }
        /// <summary>
        /// R103协议define字节数
        /// </summary>
        public int defineByteNum { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public TypeCode type { get; set; }
    }

    /// <summary>
    /// 消息类型定义
    /// </summary>
    public class CS_R103
    {
        /// <summary>
        /// 来电号码
        /// </summary>
        public const byte CallNumber = 0x01;
        /// <summary>
        /// 挂机信息
        /// </summary>
        public const byte Hang_up = 0x02;
        /// <summary>
        /// 摘机信息
        /// </summary>
        public const byte OffHook = 0x03;
        /// <summary>
        /// 振铃消失（振了一段时间后，没接，后来振铃消失了）
        /// </summary>
        public const byte RingEnd = 0x04;
        /// <summary>
        /// 振铃
        /// </summary>
        public const byte Ringing = 0x05;
        /// <summary>
        /// 来电号码校验
        /// </summary>
        public const byte CallerNumberCheck = 0x06;
        /// <summary>
        /// 未接线（即该电话口未接电话线，根据电话线路电压来判断的）
        /// </summary>
        public const byte Unwired = 0x07;
    }

    /// <summary>
    /// specificData定义
    /// </summary>
    public class DataFrame : object
    {
        public DataFrame() { }

        public DataFrame(byte type, byte len, string info)
        {
            dataType = type;
            addLen = len;
            addInfo = info;
        }
        /// <summary>
        /// 包头标志
        /// </summary>
        public static string headSign = Encoding.ASCII.GetString(new byte[] { 0x55, 0x66, 0x77, 0x88 });
        /// <summary>
        /// 消息类型
        /// </summary>
        public byte dataType;
        /// <summary>
        /// 附加消息长度
        /// </summary>
        public byte addLen;
        /// <summary>
        /// 电话号码等附加信息
        /// </summary>
        public string addInfo = string.Empty;


        public static DataFrame NormalizeData(byte[] arrData)
        {
            if (arrData.Length >= IALDS.Total)
            {
                DataFrame callFrame = new DataFrame();
                if (GetValue<String>(arrData, IALDS.headSign) == DataFrame.headSign)
                {
                    callFrame.dataType = GetValue<Byte>(arrData, IALDS.messageType);
                    callFrame.addLen = GetValue<Byte>(arrData, IALDS.addLen);
                    IALDS.addLenInfo.defineByteNum = callFrame.addLen;
                    callFrame.addInfo = GetValue<String>(arrData, IALDS.addLenInfo);
                    return callFrame;
                }
            }
            return null;
        }
        public static T GetValue<T>(byte[] buffer, IndexAndLenDefine IALD)
        {
            T result = default(T);
            switch (IALD.type)
            {
                case TypeCode.Byte:
                    Byte value_byte = buffer[IALD.defineIndex];
                    if (value_byte is T)
                    {
                        result = (T)(object)value_byte;
                    }
                    return result;
                case TypeCode.String:
                    String value_string = Encoding.ASCII.GetString(buffer, IALD.defineIndex, IALD.defineByteNum);
                    if (value_string is T)
                    {
                        result = (T)(object)value_string;
                    }
                    return result;
                default:
                    break;
            }
            return result;

        }
    }

}
