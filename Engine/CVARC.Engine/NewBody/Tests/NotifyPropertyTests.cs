using System.ComponentModel;
using AIRLab.Mathematics;
using NUnit.Framework;

namespace CVARC.Core
{
	internal class NotifyPropertyTests
	{
		[Test]
		public void TestLocationNotified()
		{
			_body = new Body();
			_body.PropertyChanged += CheckLocationChanged;
			SetLocation(new Frame3D(1, 0, 0));
			SetLocation(new Frame3D(1, 0, 0));
			SetLocation(new Frame3D(2, 0, 0));
		}

		private void SetLocation(Frame3D frame3D)
		{
			_prevLocation = _body.Location;
			_body.Location = frame3D;
		}

		private void CheckLocationChanged(object sender,
		                                  PropertyChangedEventArgs e)
		{
			Assert.AreEqual("Location", e.PropertyName);
			Assert.AreEqual(_body, sender as Body);
			Assert.AreNotEqual(_prevLocation, _body.Location);
		}

		private Body _body;
		private Frame3D _prevLocation;
	}
}