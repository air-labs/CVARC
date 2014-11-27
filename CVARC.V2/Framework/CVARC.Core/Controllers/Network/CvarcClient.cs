using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CVARC.V2
{
    public class CvarcClient : IMessagingClient
    {
        const byte EndLine = (byte)'\n';
        TcpClient client;
        public CvarcClient(TcpClient client)
        {
            this.client = client;
        }

        public void WriteLine(byte[] bytes)
        {
            bytes = bytes.Where(z => z != EndLine).Concat(new[] { EndLine }).ToArray();
            client.Client.Send(bytes);
        }

        public byte[] ReadLine()
        {
            var buffer = new byte[1];
            var read = new List<byte>();
            while (true)
            {
                client.Client.Receive(buffer);
                if (buffer[0] == EndLine) break;
                read.Add(buffer[0]);
            }
            return read.ToArray();
        }

        public void Close()
        {
            client.Close();
        }
    }
}
