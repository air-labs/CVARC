using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace CVARC.V2
{
    public class CvarcServerSideTcpClient :  IMessagingClient
    {
        readonly CvarcByteLevelClient byteLevel;
        readonly ISerializer serializer = new JsonSerializer();

        public CvarcServerSideTcpClient(TcpClient client)
        {
            byteLevel = new CvarcByteLevelClient(client);
        }


        public object Read(Type type)
        {
            var bytes = byteLevel.ReadLine();
            return serializer.Deserialize(type, bytes);
        }

        public void Write(object @object)
        {
            byteLevel.WriteLine(serializer.Serialize(@object));
        }

        public void Close()
        {
            byteLevel.Close();
        }
    }
}