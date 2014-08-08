using System;
using AIRLab.Mathematics;
using CVARC.Basic.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class CommonTests
    {
        [TestMethod]
        public void NormilizeAngleTest()
        {
            Assert.IsTrue(0 == Convert.ToInt32(Angle.FromGrad(0).Normilize().Grad));
            Assert.AreEqual(0, Convert.ToInt32(Angle.FromGrad(360).Normilize().Grad));
            Assert.AreEqual(1, Convert.ToInt32(Angle.FromGrad(361).Normilize().Grad));
            Assert.AreEqual(0, Convert.ToInt32(Angle.FromGrad(-360).Normilize().Grad));
            Assert.AreEqual(-1, Convert.ToInt32(Angle.FromGrad(-361).Normilize().Grad));
            Assert.AreEqual(180, Convert.ToInt32(Angle.FromGrad(180).Normilize().Grad));
            Assert.AreEqual(-180, Convert.ToInt32(Angle.FromGrad(-180).Normilize().Grad));
            Assert.AreEqual(-90, Convert.ToInt32(Angle.FromGrad(270).Normilize().Grad));
            Assert.AreEqual(90, Convert.ToInt32(Angle.FromGrad(-270).Normilize().Grad));
            Assert.AreEqual(170, Convert.ToInt32(Angle.FromGrad(-190).Normilize().Grad));
            Assert.AreEqual(1, Convert.ToInt32(Angle.FromGrad(-359).Normilize().Grad));
            Assert.AreEqual(-1, Convert.ToInt32(Angle.FromGrad(359).Normilize().Grad));
        }
    }
}
