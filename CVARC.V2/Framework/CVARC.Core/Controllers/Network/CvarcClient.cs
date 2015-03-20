using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CVARC.V2
{
    public class CvarcClient : IMessagingClient
    {

		public bool EnableDebug { get; set; }
        const byte EndLine = (byte)'\n';
        TcpClient client;
        public CvarcClient(TcpClient client)
        {
            this.client = client;
        }

		string BytesToArray(byte[] bytes)
		{
			return System.Text.Encoding.UTF8.GetString(bytes);
		}

        public void WriteLine(byte[] bytes)
        {
            bytes = bytes.Where(z => z != EndLine).Concat(new[] { EndLine }).ToArray();
			if (EnableDebug) Debugger.Log(DebuggerMessageType.Protocol, BytesToArray(bytes));
			client.Client.Send(bytes);
        }

        bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 & part2)
            {//connection is closed
                return false;
            }
            return true;
        }

        public byte[] ReadLine()
        {
            var buffer = new byte[1];
            var read = new List<byte>();
            while (true)
            {
				if (externallyClosed)
					throw new Exception("The connection was terminated");
                var length = client.Client.Receive(buffer);
                if (length == 0)
                {
                    //Это устанавливает таймаут в одну секунду, и выбрасывает исключение, даже если соединение открыто, но временно молчит
                    //if (!SocketConnected(client.Client)) throw new IOException("The connection was terminated");
                    continue;
                }
                if (buffer[0] == EndLine) break;
                read.Add(buffer[0]);
            }
			if (EnableDebug)
				Debugger.Log(DebuggerMessageType.Protocol, BytesToArray(read.ToArray()));
            return read.ToArray();
        }

		bool externallyClosed = false;

        public void Close()
        {
			externallyClosed = true;
            client.Close();
        }
    }
}
