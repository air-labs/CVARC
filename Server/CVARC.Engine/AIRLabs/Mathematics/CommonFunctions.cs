namespace AIRLab.Mathematics
{
    public class CommonFunctions
    {
        public static double ConvexCombination(double a, double b, double weightCoeff)
        {
            return weightCoeff*a + (1 - weightCoeff)*b;
        }
    }
}