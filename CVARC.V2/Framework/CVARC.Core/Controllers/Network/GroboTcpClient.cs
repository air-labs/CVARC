using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace CVARC.V2
{
    public class CvarcTcpClient : IDisposable
    {
        private readonly Stream stream;
        private const byte EndLine = (byte)'\n';
        private StreamReader streamReader;
        private static readonly ISerializer Serializer = new JsonSerializer();

        public CvarcTcpClient(TcpClient client)
        {
            stream = client.GetStream();
            streamReader = new StreamReader(stream);
        }

        public void SerializeAndSend(object obj)
        {
            Send(Serializer.Serialize(obj));
        }

        public void Send(byte[] message)
        {
            message = message.Where(z => z != EndLine).ToArray();
            stream.Write(message,0,message.Length);
            stream.WriteByte(EndLine);
            stream.Flush();
        }

        public byte[] ReadToEnd()
        {
            var str = streamReader.ReadLine();
            if (str == null) return null;
            return Encoding.UTF8.GetBytes(str);
        }

        public object ReadObject(Type type)
        {
            var bytes = ReadToEnd();
            if (bytes == null) return null;
            return Serializer.Deserialize(type, bytes);
        }

        public T ReadObject<T>()
            where T : class
        {
            return (T)ReadObject(typeof(T));
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}