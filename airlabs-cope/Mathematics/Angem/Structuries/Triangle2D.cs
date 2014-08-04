using System;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    public struct Triangle2D
    {
        public Triangle2D(Point2D vertexA, Point2D vertexB, Point2D vertexC) : this()
        {
            VertexC = vertexC;
            VertexB = vertexB;
            VertexA = vertexA;
        }

        public Point2D VertexA { get; private set; }
        public Point2D VertexB { get; private set; }
        public Point2D VertexC { get; private set; }

        public double LengthAb
        {
            get { return VertexA.GetDistanceTo(VertexB); }
        }

        public double LengthBc
        {
            get { return VertexB.GetDistanceTo(VertexC); }
        }

        public double LengthAc
        {
            get { return VertexA.GetDistanceTo(VertexC); }
        }

        public Angle AngleA
        {
            get { return GetAngle(LengthBc, LengthAc, LengthAb); }
        }

        public Angle AngleB
        {
            get { return GetAngle(LengthAc, LengthBc, LengthAb); }
        }

        public Angle AngleC
        {
            get { return GetAngle(LengthAb, LengthAc, LengthBc); }
        }

        public Line2D GetMedianFromA()
        {
            return new Line2D(VertexA, (VertexB + VertexC)/2);
        }

        public Line2D GetMedianFromB()
        {
            return new Line2D(VertexB, (VertexA + VertexC)/2);
        }

        public Line2D GetMedianFromC()
        {
            return new Line2D(VertexC, (VertexA + VertexB)/2);
        }

        public Line2D GetBissectorFromA()
        {
            return GetBissektor(VertexA, VertexB, VertexC);
        }

        public Line2D GetBissectorFromB()
        {
            return GetBissektor(VertexB, VertexA, VertexC);
        }

        public Line2D GetBissectorFromC()
        {
            return GetBissektor(VertexC, VertexA, VertexB);
        }

        public Circle2D GetInscribedCircle()
        {
            //T. вписанная окружность в треугольнике существует и единственная
            //Т. вписанная окружность лежит на пересечении биссектрис
            //Подробнее http://ru.wikipedia.org/wiki/Вписанная_окружность
            return new Circle2D(Geometry.GetCrossing(GetBissectorFromA(), GetBissectorFromB()),
                                GetInscribedCircleRadius());
        }

        private double GetInscribedCircleRadius()
        {
            double halfPerimeter = (LengthAb + LengthAc + LengthBc)/2;
            return
                Math.Sqrt((halfPerimeter - LengthAb)*(halfPerimeter - LengthAc)*(halfPerimeter - LengthBc)/halfPerimeter);
        }

        private static Line2D GetBissektor(Point2D startBissector, Point2D leftPoint, Point2D rightPoint)
        {
//			//Достроим треугольник до равнобедренного
//			double alpha = startBissector.GetDistanceTo(leftPoint) / startBissector.GetDistanceTo(rightPoint);
//			Point2D newLeftPoint = startBissector + (1 / alpha) * (leftPoint - startBissector);
//
//			var expected = startBissector.GetDistanceTo(newLeftPoint) - startBissector.GetDistanceTo(rightPoint);
//			Assert.That(expected, Is.LessThan(0.1).And.GreaterThan(-0.1));
//
//			//Теорема. В равнобедренном треугольнике медиана == биссектрисе
//			//Утверждение: найденная медиана и будет нужной нам прямой
//
//			Point2D middle = (newLeftPoint + rightPoint) / 2;
//
//			//Но точка middle не лежит на треугольнике... найдем ту что лежит
//
//			Line2D line = new Line2D(leftPoint, rightPoint);
//			Line2D fakeBissector = new Line2D(startBissector, middle);
//
//			Point2D finishBissector = Geometry.GetCrossing(line, fakeBissector);
//			return new Line2D(startBissector, finishBissector);

            double left = startBissector.GetDistanceTo(leftPoint);
            double right = startBissector.GetDistanceTo(rightPoint);

            double alpha = left/(left + right);

            Point2D finishBissector = leftPoint*(1 - alpha) + rightPoint*alpha;

            return new Line2D(startBissector, finishBissector);
        }

        private Angle GetAngle(double a, double b, double c)
        {
            double cos = (b*b + c*c - a*a)/(2*b*c);
            return Angle.FromRad(Math.Acos(cos));
        }
    }
}