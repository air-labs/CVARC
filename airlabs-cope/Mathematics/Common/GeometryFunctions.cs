using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    public class GeometryFunctions
    {
        /// <summary>
        ///   Получить радиус окружности, касающейся оси Х в (0, 0) и проходящей через заданную точку
        /// </summary>
        /// <param name="point"> Точка </param>
        /// <returns> Радиус </returns>
        public static double GetRadiusOfCircle(Point2D point)
        {
            return Math.Abs(0.5*(point.X*point.X + point.Y*point.Y)/point.Y);
        }

        /// <summary>
        ///   Получить радиус окружности, касающейся оси Х в (0, 0) и имеющей в заданной абсцисса касательную с заданным углом наклона
        /// </summary>
        /// <param name="angle"> Угол наклона касательной </param>
        /// <param name="x"> Абсцисса </param>
        /// <returns> Радиус </returns>
        public static double GetRadiusOfCircle(Angle angle, double x)
        {
            return Math.Abs(x/Math.Sin(angle.Radian));
        }

        /// <summary>
        ///   Получить угол дуги окружности, ограниченной точкой (0, 0) и заданной точкой на этой окружности
        /// </summary>
        /// <param name="point"> Точка </param>
        /// <returns> Угол </returns>
        public static Angle GetAngleOfArc(Point2D point)
        {
            return Angle.FromRad(Math.Abs(point.X) <= Double.Epsilon
                                     ? Math.PI/2
                                     : 2*Math.Atan2(point.Y, point.X)); //????
        }

        /// <summary>
        ///   Получить угол дуги окружности, ограниченной точкой (0, 0) и точкой с заданными абсциссой и углом наклона касательной
        /// </summary>
        /// <param name="angle"> Угол наклона касательной </param>
        /// <param name="x"> Абсцисса </param>
        /// <returns> Угол </returns>
        public static Angle GetAngleOfArc(Angle angle, double x)
        {
            return angle;
        }

        /// <summary>
        ///   Формула Герона для вычисления площади треугольника
        /// </summary>
        /// <param name="A"> Вершина А </param>
        /// <param name="B"> Вершина В </param>
        /// <param name="C"> Вершина С </param>
        /// <returns> </returns>
        public static double GetTriangleSquareByGeron(Point2D A, Point2D B, Point2D C)
        {
            if (!A.IsEmpty && !B.IsEmpty && !C.IsEmpty)
            {
                double a = Point2D.GetDistance(A, B);
                double b = Point2D.GetDistance(B, C);
                double c = Point2D.GetDistance(C, A);
                double p = (a + b + c)/2;
                return Math.Sqrt(p*(p - a)*(p - b)*(p - c));
            }
            return 0;
        }

        #region Ф-ла Герона для фрейма

        public static double GetTriangleSquareByGeron(Frame2D A, Frame2D B, Frame2D C)
        {
            double a = Point2D.GetDistance(A.ToPoint2D(), B.ToPoint2D());
            double b = Point2D.GetDistance(B.ToPoint2D(), C.ToPoint2D());
            double c = Point2D.GetDistance(C.ToPoint2D(), A.ToPoint2D());
            double p = (a + b + c)/2;
            double res = p;
            res *= (p - a);
            res *= (p - b);
            res *= (p - c);
            double result = Math.Sqrt(res);
            if (double.IsNaN(result))
                return 0;
            return result;
        }

        #endregion
    }
}