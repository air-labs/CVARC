using System;

namespace CVARC.Core.Replay
{
	[Serializable]
	public class Visibility
	{
		public Visibility(bool isVisible, double startTime)
		{
			IsVisible = isVisible;
			StartTime = startTime;
		}

		public void Apply(Body loadedBody, Body world)
		{
			if(IsVisible)
				world.Add(loadedBody);
			else
				world.Remove(loadedBody);
		}

		public override string ToString()
		{
			return string.Format("IsVisible: {0}, StartTime: {1}", IsVisible, StartTime);
		}

		public double StartTime { get; private set; }
		public readonly bool IsVisible;
	}
}