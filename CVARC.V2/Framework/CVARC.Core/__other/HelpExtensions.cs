using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using AIRLab.Mathematics;

namespace CVARC.V2
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

        public static void SafeAdd<K, V>(this Dictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        public static V SafeGet<K, V>(this Dictionary<K, V> dictionary, K key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : default(V);
        }

        public static string PrintArray<T>(this T[,] array)
        {
            var offsetsByColumns = new int[array.GetLength(0)];
            for (int i = 0; i < array.GetLength(0); i++)
                offsetsByColumns[i] = array.GetColumn(i).Max(x => x.ToString().Length) + 1;

            var s = "";
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                    s += array[j, i].ToString().PadRight(offsetsByColumns[j]);
                s += "\r\n";
            }
            return s;
        }

        public static T[] GetColumn<T>(this T[,] array, int columnNum)
        {
            var length = array.GetLength(1);
            var column = new T[length];
            for (int i = 0; i < length; i++)
                column[i] = array[columnNum, i];
            return column;
        }

        public static Angle Normilize(this Angle angle)
        {
            var grad = angle.Grad%360;
            if (grad > 180)
                grad = grad - 360;
            if (grad < -180)
                grad = 360 + grad;
            return Angle.FromGrad(grad);
        }
    }
}
