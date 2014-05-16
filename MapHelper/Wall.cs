namespace MapHelper
{
    public class Wall
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Type { get; set; }
        public double RealX { get; set; }
        public double RealY { get; set; }

        public Wall(int x, int y, string type, double realX, double realY)
        {
            X = x;
            Y = y;
            Type = type;
            RealX = realX;
            RealY = realY;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Type: {2}, RealX: {3}, RealY: {4}", X, Y, Type, RealX, RealY);
        }
    }
}