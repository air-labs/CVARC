using System;
using System.IO;
using System.Net.Sockets;

namespace CVARC.Basic.Core
{
    public static class HelpExtensions
    {
        public static byte[] ReadToEnd(this Stream stream)
        {
            const int bufferSize = 5;
            var buffer = new byte[bufferSize];
            var result = new MemoryStream();
            int size;
            do
            {
                size = stream.Read(buffer, 0, bufferSize);
                result.Write(buffer, 0, size);
            } while (size > 0);
            return result.ToArray();
        }

        public static byte[] ReadBytes(this NetworkStream stream)
        {
            byte[] myReadBuffer = new byte[1024];
            using (var memoryStream = new MemoryStream())
            {
                do
                {
                    int size = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                    memoryStream.Write(myReadBuffer, 0, size);
                } while (stream.DataAvailable);
                return memoryStream.ToArray();
            }
        }

        public static void Write(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof (T), value);
        }

        public static int SafeParseInt(this string value, int defaultValue = 0)
        {
            int i;
            if (int.TryParse(value, out i))
                return i;
            return defaultValue;
        }
    }
}
