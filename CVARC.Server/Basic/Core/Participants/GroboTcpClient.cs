using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CVARC.Basic.Core.Participants
{
    public class GroboTcpClient : IDisposable
    {
        private readonly Stream stream;
        private const byte EndLine = (byte)'\n';
        private StreamReader streamReader;


        public GroboTcpClient(TcpClient client)
        {
            stream = client.GetStream();
            streamReader = new StreamReader(stream);
        }

        public void Send(byte[] message)
        {
            stream.Write(message);
            stream.WriteByte(EndLine);
            stream.Flush();
        }

        public byte[] ReadToEnd()
        {
            return Encoding.UTF8.GetBytes(streamReader.ReadLine());
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}