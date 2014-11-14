using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IMessagingClient
    {
        object Read(Type type);
        void Write(object @object);
        void Close();
    }

    public static class IMessagingClientExtensions
    {
        public static T Read<T>(this IMessagingClient client)
        {
            return (T)client.Read(typeof(T));
        }
    }
}
