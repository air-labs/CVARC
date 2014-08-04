//using AIRLab.Mathematics;
//using NUnit.Framework;

//namespace AIRLab.Mathematics
//{
//    [TestFixture]
//    internal class TestsAngem
//    {
//        [Test]
//        public void TestPointIsFromRegion()
//        {
//            var point = new Point2D(1, 1);
//            var points = new[] {new Point2D(0, 0), new Point2D(2, 0), new Point2D(5, 2), new Point2D(0, 2)};
//            Assert.IsTrue(point.IsFromRegion(points));
//        }

//        [Test]
//        public void TestTwoGetBissectorAndSimmetryFunc()
//        {
//            var triangle = new Triangle2D(new Point2D(0, 3), new Point2D(0, 0), new Point2D(5, 0));
//            Point2D expected = triangle.GetBissectorFromA().End;
//            var triangleSymmetricalArg = new Triangle2D(new Point2D(0, 3), new Point2D(5, 0), new Point2D(0, 0));
//            Point2D expectedSymmetricalArg = triangleSymmetricalArg.GetBissectorFromA().End;

//            Assert.That(expected.X,
//                        Is.LessThan(expectedSymmetricalArg.X + 0.0001).And.GreaterThan(expectedSymmetricalArg.X - 0.0001));
//            Assert.That(expectedSymmetricalArg.X, Is.GreaterThan(1.68).And.LessThan(1.7));
//            Assert.That(expected.Y, Is.GreaterThan(-0.0001).And.LessThan(0.0001));
//        }

//        [Test]
//        public void Test_One_GetBissector()
//        {
//            var triangle = new Triangle2D(new Point2D(0, 0), new Point2D(10, 0), new Point2D(0, 10));
//            Line2D expectedLine = triangle.GetBissectorFromA();
//            Assert.That(expectedLine.End.X, Is.EqualTo(5));
//            Assert.That(expectedLine.End.Y, Is.EqualTo(5));
//        }
//    }
//}