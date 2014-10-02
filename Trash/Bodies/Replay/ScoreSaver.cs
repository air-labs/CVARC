using System;
using System.Collections.Generic;
using System.Linq;

namespace CVARC.Core.Replay
{
	public class ScoreSaver
	{
		public void SaveTempScores(double totalTime, int[] scores)
		{
			if (scores==null)
				return;
			if (_lastScores == null || !scores.SequenceEqual(_lastScores))
				SavedScores.Add(new ScoreAtTime(totalTime, scores));
			_lastScores = SavedScores[SavedScores.Count - 1].TempScores;
			RobotCount = Math.Max(scores.Length, RobotCount);
		}

		public int RobotCount { get; private set; }

		public readonly List<ScoreAtTime> SavedScores = new List<ScoreAtTime>(64);
		private int[] _lastScores;
	}
}