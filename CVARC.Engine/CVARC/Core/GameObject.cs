namespace CVARC.Basic
{
    public class GameObject : IGameObject
    {
        public string Type { get; set; }
        public string Id { get; set; }

        public GameObject(string id, string type)
        {
            Type = type;
            Id = id;
        }
    }
}