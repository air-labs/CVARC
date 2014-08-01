namespace MapHelper
{
    public class Wall
    {
        public Point DiscreteCoordinate{ get; set; }
        public Point AbsoluteCoordinate{ get; set; }
        public string Type { get; set; }

        public Wall(Point discreteCoordinate, Point absoluteCoordinate, string type)
        {
            DiscreteCoordinate = discreteCoordinate;
            AbsoluteCoordinate = absoluteCoordinate;
            Type = type;
        }

        public override string ToString()
        {
            return string.Format("DiscreteCoordinate: {0}, AbsoluteCoordinate: {1}, Type: {2}", DiscreteCoordinate, AbsoluteCoordinate, Type);
        }
    }
}