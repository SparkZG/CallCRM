using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CallCRM.Log;

namespace CallCRM.ConnFactory
{
    public class UDPServer : MySocket
    {
        private UdpClient UCS;
        private UdpClient UCR;
        public Thread ThreadUDP;
        private bool _Stop = false;

        public string HostName = "";
        public int Port;

        public UDPServer(string ip, int port)
        {
            //发送的udpClient
            UCS = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), 8888));
            HostName = ip;
            Port = port;
        }
        /// <summary>
        /// 类型的析构，IDisposable接口
        /// </summary>
        public override void Dispose()
        {
            _Stop = true;
            if (UCS!=null)
            {
                UCS.Close();
                UCS = null;
            }
            if (UCR != null)
            {
                UCR.Close();
                UCR = null;
            }            
            ThreadUDP = null;
            GC.Collect();            
        }
        public override string Run()
        {
            if (ThreadUDP == null)
            {
                try
                {
                    _Stop = false;
                    IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse(HostName), Port); // 设置IP和监听端口号 
                    UCR = new UdpClient(localIpep);
                    ThreadUDP = new Thread(new ThreadStart(ReceiveThread));
                    ThreadUDP.Name = "ReceiveThread";
                    ThreadUDP.Priority = ThreadPriority.Normal;
                    ThreadUDP.IsBackground = true; //一定要加上，否则接收不到数据,
                    ThreadUDP.Start();
                    return null;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), "主机启动异常！", ex);
                    return ex.ToString();
                }
            }
            return "主机正在运行！";
        }
        public override void Stop()
        {
            try
            {
                bool _IsStop = false;
                _Stop = true;
                if (HostName.Length > 0)
                {
                    _IsStop = Send("STOP", HostName, Port);
                }
                if ((!_IsStop) || HostName.Length == 0)
                {
                    if ((ThreadUDP.ThreadState & ThreadState.Running) == ThreadState.Running)
                    {
                        ThreadUDP.Abort();
                    }
                    if ((ThreadUDP.ThreadState & ThreadState.AbortRequested) == ThreadState.AbortRequested)
                    {
                        GC.Collect();
                    }
                    if ((ThreadUDP.ThreadState & ThreadState.Aborted) == ThreadState.Aborted
                      || (ThreadUDP.ThreadState & ThreadState.Stopped) == ThreadState.Stopped)
                    {
                        ThreadUDP = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Helper.Error(this.GetType(), "主机关闭异常！", ex);
                this.SendEvent(new SocketEvent(SocketEventType.Other, ex.ToString()));
            }
            this.SendEvent(new SocketEvent(SocketEventType.StopEvent, "主机已关闭！"));
        }
        private void ReceiveThread()
        {
            try
            {
                this.SendEvent(new SocketEvent(SocketEventType.StartEvent, "主机开启！"));
                IPEndPoint Sender = new IPEndPoint(IPAddress.Any, 0);
                while (!_Stop)
                {
                    //Receive处于等待，但不影响发送
                    byte[] Data = UCR.Receive(ref Sender);
                    this.SendEvent(new SocketEvent(SocketEventType.ReceEvent, Data));
                    Thread.Sleep(1);
                }
                if (UCR != null)
                {
                    UCR.Close();
                    UCR = null;
                }  
            }
            catch(Exception ex)
            {
                Log4Helper.Error(this.GetType(), "监听发生异常！", ex);
                Stop();                
            }
        }
        public override bool Send(string cmd, string _remoteHostName, int _sendPort)
        {
            bool _sendSuccess = false;
            if (_remoteHostName.Length > 0)
            {
                try
                {
                    Byte[] sendBytes = Encoding.Default.GetBytes(cmd);
                    UCS.Send(sendBytes, sendBytes.Length, _remoteHostName, _sendPort);
                    this.SendEvent(new SocketEvent(SocketEventType.SendEvent, cmd));
                    _sendSuccess = true;
                }
                catch (Exception ex)
                {
                    Log4Helper.Error(this.GetType(), "发送UDP异常！", ex);
                    this.SendEvent(new SocketEvent(SocketEventType.Other, "发送失败！"));
                }
            }
            else
            {
                this.SendEvent(new SocketEvent(SocketEventType.Other, "远程主机没有定义，发送失败！"));
            }
            return _sendSuccess;
        }
    }
}