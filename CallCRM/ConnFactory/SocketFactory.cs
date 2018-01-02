using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCRM.ConnFactory
{
    public class SocketFactory
    {
        public SocketFactory() { }

        public MySocket CreateSocket(ServerType socketType, string ip, int port)
        {
            switch (socketType)
            {
                case ServerType.TCPClient:
                    return null;
                case ServerType.TCPServer:
                    return null;
                case ServerType.UDP:
                    return new UDPServer(ip, port);
                default:
                    return null;
            }
        }
    }
}
