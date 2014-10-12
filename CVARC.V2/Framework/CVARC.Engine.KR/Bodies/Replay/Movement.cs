using System;
using System.Collections.Generic;
using AIRLab.Mathematics;

namespace CVARC.Core.Replay
{
	[Serializable]
	public class Movement
	{
		public Movement(double startTime)
		{
			StartTime = startTime;
		}

		public void SaveLocation(Frame3D newLocation)
		{
			_locations.Add(newLocation);
		}

		public bool HasNextChange()
		{
			return _counter < _locations.Count;
		}

		public Frame3D NextLocation()
		{
			Frame3D frame = _locations[_counter];
			_counter++;
			return frame;
		}

		public override string ToString()
		{
			return string.Format("Locations: {0}, StartTime: {1}", _locations, StartTime);
		}

		public double StartTime { get; private set; }

		private readonly List<Frame3D> _locations = new List<Frame3D>();

		[NonSerialized]
		private int _counter;
	}
}