namespace RepairTheStarship.MapBuilder
{
    public class StarshipObject
    {
        public Point DiscreteCoordinate{ get; set; }
        public Point AbsoluteCoordinate{ get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return string.Format("DiscreteCoordinate: {0}, AbsoluteCoordinate: {1}, Type: {2}", DiscreteCoordinate, AbsoluteCoordinate, Type);
        }
    }
}