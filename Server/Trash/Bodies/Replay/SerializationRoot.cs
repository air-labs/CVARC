using System;
using System.Collections.Generic;

namespace CVARC.Core.Replay
{
	[Serializable]
	public class SerializationRoot
	{
		public int RobotCountForScores;
		public double DT;
		public double LastRecordedTime;
		public List<LoggingObject> LoggingObjects;
		public List<ScoreAtTime> Scores;
		public List<Penalty> Penalties;
	}
}