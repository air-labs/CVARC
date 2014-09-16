using System;
using System.Net.Sockets;

namespace CVARC.Basic.Core.Participants
{
    public class GroboTcpClient
    {
        private readonly NetworkStream stream;

        public GroboTcpClient(TcpClient client)
        {
            stream = client.GetStream();
        }

        public void Send(byte[] message)
        {
            var lengthBytes = BitConverter.GetBytes(message.Length);
            stream.Write(lengthBytes);
            stream.Write(message);
            stream.Flush();
        }

        public byte[] ReadToEnd()
        {
            var lengthBytes = new byte[4];
            stream.Read(lengthBytes, 0, lengthBytes.Length);
            int messageLength = BitConverter.ToInt32(lengthBytes, 0);
            var message = new byte[messageLength];
            stream.Read(message, 0, messageLength);
            return message;
        }
    }
}