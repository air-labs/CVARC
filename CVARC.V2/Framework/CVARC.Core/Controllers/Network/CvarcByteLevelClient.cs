using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CVARC.V2
{
    public class CvarcByteLevelClient 
    {
        private const byte EndLine = (byte)'\n';
        readonly TcpClient client;
        readonly Stream stream;
        
        private static readonly ISerializer Serializer = new JsonSerializer();

        public CvarcByteLevelClient(TcpClient client)
        {
            this.client=client;
            stream = client.GetStream();
        }

        public void WriteLine(byte[] message)
        {
            message = message.Where(z => z != EndLine).Concat(new [] { EndLine}).ToArray();
            client.GetStream().Write(message);
            stream.Flush();
        }

        public byte[] ReadLine()
        {
            stream.ReadTimeout = 1000;
            var bytes = new List<byte>();
            Console.WriteLine(stream.ReadTimeout);
            while (true)
            {
                var bt = (byte)stream.ReadByte();
                if (bt == EndLine) break;
                bytes.Add(bt);
            }
            return bytes.ToArray();
        }

        public void Close()
        {
            client.Close();
        }
       

    }
}
