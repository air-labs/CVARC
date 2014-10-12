namespace AIRLab.Mathematics
{
    public class ArythmeticProgressionFunctions
    {
        public static double GetNthTermOfArythmeticProgression(double firstTerm, int numberOfTerm, double d)
        {
            return firstTerm + (numberOfTerm - 1)*d;
        }

        public static double GetSumOfTheFirstNTermsOfArithmeticProgression(double firstTerm, int numberOfTerms, double d)
        {
            return (firstTerm + GetNthTermOfArythmeticProgression(firstTerm, numberOfTerms, d))*numberOfTerms/2;
        }

        public static double GetDifferenceOfArythmeticProgression(double firstTerm, double lastTerm, int numberOfTerms)
        {
            return ((lastTerm - firstTerm)/(numberOfTerms - 1));
        }
    }
}