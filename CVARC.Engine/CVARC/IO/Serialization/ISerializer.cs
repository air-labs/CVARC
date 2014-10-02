namespace CVARC.Basic.Core.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize(object obj);
        T Deserialize<T>(byte[] bytes);
    }
}