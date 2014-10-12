using System;

namespace CVARC.Core.Replay
{
	[Serializable]
	public class ScoreAtTime
	{
		public ScoreAtTime(double time, int[] scores)
		{
			Time = time;
			TempScores = new int[scores.Length];
			scores.CopyTo(TempScores, 0);
		}

		public double Time { get; private set; }
		public int[] TempScores { get; private set; }

		public override string ToString()
		{
			return string.Format("Time: {0}, TempScores: {1}", Time, TempScores);
		}
	}
}