using System;
namespace CVARC.V2
{
    public interface ISerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(Type type, byte[] bytes);
    }
}