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
            client.Client.Send(message);
        }

        List<byte> ReadBuffer = new List<byte>();

        public byte[] ReadLine()
        {
            while (true)
            {
                var buffer=new byte[1000];
                int length=client.Client.Receive(buffer);
                for (int i = 0; i < length; i++)
                    ReadBuffer.Add(buffer[i]);
                var newLineIndex = ReadBuffer.IndexOf(EndLine);
                if (newLineIndex == -1) continue;
                var message = new Byte[newLineIndex];
                ReadBuffer.CopyTo(0, message, 0, newLineIndex);
                ReadBuffer.RemoveRange(0, newLineIndex + 1);
                return message;
            }

        }

        public void Close()
        {
            client.Close();
        }
       

    }
}
