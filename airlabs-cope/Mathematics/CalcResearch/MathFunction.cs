//using System;
//using System.Linq;
//using NUnit.Framework;
//using AIRLab.Mathematics;
//using System.Linq;

//namespace AIRLab.Mathematics
//{
//    public class SystemOfHomogeneousLinearEquations
//    {
//        public double[] FindX(double[][] system)
//        {
//            for (var currentX = 0; currentX < system.Length; currentX++)
//            {
//                var currentEq = system[currentX];
//                for (var i = 0; i < currentEq.Length; i++)
//                {
//                    if (currentX != i)
//                        currentEq[i] /= currentEq[currentX];
//                }
//                currentEq[currentX] = 1;
//                foreach (var equation in system)
//                {
//                    if (equation == currentEq)
//                        continue;
//                    var q = equation[currentX];
//                    for (int j = 0; j < equation.Length; j++)
//                        equation[j] -= q*currentEq[j];
//                }
//            }
//            var arrayX = new double[system.Length];
//            for (int i = 0; i < arrayX.Length; i++)
//                arrayX[i] = system[i][system[i].Length - 1];
//            return arrayX;
//        }
//    }

//    public class MathFunction
//    {
//        MathSemiFunction NegativeFunction;
//        MathSemiFunction PositiveFunction;
//        public double GetValue(double x)
//        {
//            if (x < 0) return NegativeFunction.GetValueFunc(-x);
//            return PositiveFunction.GetValueFunc(x);
//        }
//        public static MathFunction Regress(Point2D[] points, string type)
//        {
//            var f = new MathFunction();
//            var pm = new SimpleRegressionModule();
//            pm.Sample = points.Where(z => z.X > 0).ToArray();
//            f.PositiveFunction = pm.GetOptimalFunction(type);
//            var nm = new SimpleRegressionModule();
//            nm.Sample = points.Where(z => z.X < 0).Select(z => new Point2D(-z.X, z.Y)).ToArray();
//            f.NegativeFunction = nm.GetOptimalFunction(type);
//            return f;
//        }
//    }


//    public abstract class MathSemiFunction
//    {
//        public abstract double[] ARGs { get; set; }
//        public abstract double GetValueFunc(double x);
//    }

//    public class SimpleRegressionModule
//    {
//        public SimpleRegressionModule()
//        {
//            Sample = new Point2D[0];
//        }

//        public Point2D[] Sample { get; set; }

//        public MathSemiFunction GetOptimalFunction(string type)
//        {
//            if (type == "exp")
//                return GetExp();
//            if (type == "pol3")
//                return GetPol3();
//            throw new Exception();
//        }

//        private MathSemiFunction GetPol3()
//        {
//            double x = 0,
//                   y = 0,
//                   x2 = 0,
//                   x3 = 0,
//                   x4 = 0,
//                   x5 = 0,
//                   x6 = 0,
//                   xy = 0,
//                   x2y = 0,
//                   x3y = 0;
//            foreach (var point in Sample)
//            {
//                x += point.X;
//                x2 += Math.Pow(point.X, 2);
//                x3 += Math.Pow(point.X, 3);
//                x4 += Math.Pow(point.X, 4);
//                x5 += Math.Pow(point.X, 5);
//                x6 += Math.Pow(point.X, 6);
//                y += point.Y;
//                xy += point.X*point.Y;
//                x2y += Math.Pow(point.X, 2)*point.Y;
//                x3y += Math.Pow(point.X, 3)*point.Y;
//            }
//            x /= Sample.Length;
//            x2 /= Sample.Length;
//            x3 /= Sample.Length;
//            x4 /= Sample.Length;
//            x5 /= Sample.Length;
//            x6 /= Sample.Length;
//            y /= Sample.Length;
//            xy /= Sample.Length;
//            x2y /= Sample.Length;
//            x3y /= Sample.Length;
//            var equations = new double[4][];
//            equations[0] = new[] {x3, x2, x, 1, y};
//            equations[1] = new[] {x4, x3, x2, x, xy};
//            equations[2] = new[] {x5, x4, x3, x2, x2y};
//            equations[3] = new[] {x6, x5, x4, x3, x3y};

//            var nonX = new SystemOfHomogeneousLinearEquations().FindX(equations);

//            return new Polynom {ARGs = nonX};
//        }

//        private MathSemiFunction GetExp()
//        {
//            var sampleNorm = Sample.Select(s => new Point2D(s.X, Math.Log(s.Y, Math.E))).ToArray();
//            double averageX = 0, averageY = 0, averageXX = 0, averageXY = 0;
//            foreach (var point in sampleNorm)
//            {
//                averageX += point.X;
//                averageXX += point.X*point.X;
//                averageXY += point.X*point.Y;
//                averageY += point.Y;
//            }
//            averageX /= sampleNorm.Length;
//            averageXX /= sampleNorm.Length;
//            averageXY /= sampleNorm.Length;
//            averageY /= sampleNorm.Length;

//            var a = (averageXY - averageX*averageY)/(averageXX - averageX*averageX);
//            var b = averageY - a*averageX;
//            return new ExponentialFunctiuon
//                       {
//                           A = a,
//                           B = b
//                       };
//        }
//    }

//    public class Polynom : MathSemiFunction
//    {
//        public override double[] ARGs { get; set; }

//        public override double GetValueFunc(double x)
//        {
//            return ARGs.Select((t, i) => t*Math.Pow(x, ARGs.Length - 1 - i)).Sum();
//        }
//    }

//    public class ExponentialFunctiuon : MathSemiFunction
//    {
//        public double A { get; set; }
//        public double B { get; set; }

//        public override double[] ARGs
//        {
//            get { return new[] {A, B}; }
//            set
//            {
//                if (value.Length != 2)
//                    throw new Exception();
//                A = value[0];
//                B = value[1];
//            }
//        }

//        public override double GetValueFunc(double x)
//        {
//            return Math.Pow(Math.E, A*x + B);
//        }
//    }

//    [TestFixture]
//    public class NewRegressModuleTest
//    {
//        [Test]
//        public void LinerySystem()
//        {
//            var system = new[]
//                             {
//                                 new double[] {2, 3, -1, 4},
//                                 new double[] {9, 5, 3, 0},
//                                 new double[] {4, 0, -2, -10}
//                             };
//            var SystemOfHomogeneousLinearEquations = new SystemOfHomogeneousLinearEquations();

//            SystemOfHomogeneousLinearEquations.FindX(system);
//        }

//        [Test]
//        public void TestExp()
//        {
//            var sample = new[]
//                             {
//                                 new Point2D(0, 3.669296668),
//                                 new Point2D(0.1, 3.935350695),
//                                 new Point2D(0.2, 4.220695817),
//                                 new Point2D(0.3, 4.526730794),
//                                 new Point2D(0.4, 4.854955811),
//                                 new Point2D(0.5, 5.206979827),
//                                 new Point2D(0.6, 5.584528464),
//                                 new Point2D(0.7, 5.989452466),
//                                 new Point2D(0.8, 6.423736771),
//                                 new Point2D(0.9, 6.889510242),
//                                 new Point2D(1, 7.389056099)
//                             };
//            var regressModule = new SimpleRegressionModule {Sample = sample};
//            regressModule.GetOptimalFunction("exp");
//        }

//        [Test]
//        public void TestPol()
//        {
//            var sample = new[]
//                             {
//                                 new Point2D(0, 4),
//                                 new Point2D(1, 10),
//                                 new Point2D(2, 26),
//                                 new Point2D(3, 58),
//                                 new Point2D(4, 112),
//                                 new Point2D(5, 194),
//                                 new Point2D(6, 310),
//                                 new Point2D(7, 466)
//                             };
//            var regressModule = new SimpleRegressionModule {Sample = sample};
//            regressModule.GetOptimalFunction("pol3");
//        }
//    }
//}