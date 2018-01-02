using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Drawing;

namespace CallCRM.ConnFactory
{
    public class SocketEvent : EventArgs
    {
        public object message { get; set; }
        public string remark { get; set; }
        public SocketEventType eventType { get; set; }

        public string localIp { get; set; }

        public int localPort { get; set; }

        public string remoteIp { get; set; }

        public int remotePort { get; set; }

        public SocketEvent(SocketEventType eventType, byte[] buffer, int len, string ip, int port)
        {
            //此时 是将 数组 所有的元素 都转成字符串，而真正接收到的 只有服务端发来的几个字符
            message = System.Text.Encoding.UTF8.GetString(buffer, 0, len);

            this.eventType = eventType;
            this.remoteIp = ip;
            this.remotePort = port;
        }

        public SocketEvent(SocketEventType eventType, byte[] buffer)
        {
            this.eventType = eventType;

            DataFrame callFrame=DataFrame.NormalizeData(buffer);
            if (callFrame != null)
            {
                this.message = callFrame;
            }
            else
            {
                this.remark = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                this.message = null;
            }            
        }
        public SocketEvent(SocketEventType eventType, string _info)
        {
            this.eventType = eventType;
            this.remark = _info;
        }
    }
}
