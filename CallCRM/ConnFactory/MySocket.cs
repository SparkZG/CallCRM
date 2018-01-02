using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CallCRM.ConnFactory
{
    public abstract class MySocket : IDisposable
    {
        public event EventHandler SocketEventHandler;

        public abstract string Run();

        public abstract bool Send(string buffer, string remoteIP, int remotePort);

        public abstract void Stop();

        public abstract void Dispose();

        protected void SendEvent(EventArgs args)
        {
            if (SocketEventHandler != null)
            {
                SocketEventHandler(this, args);
            }
        }        
    }
}
