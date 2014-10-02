using System.Collections.Generic;

namespace CVARC.Core.Replay
{
	public class ScoreLoader
	{
		public ScoreLoader(List<ScoreAtTime> scores, 
			List<Penalty> penalties, 
			int robotCount)
		{
			_scores = scores??(_scores=new List<ScoreAtTime>());
			_penalties = penalties?? (_penalties=new List<Penalty>());
			LoadedScoreCollection = new ScoreCollection(robotCount);
			_penalties.Sort((x, y) => x.Time.CompareTo(y.Time));
		}

		public void UpdateScores(double totalTime)
		{
			if (_currentScore>=_scores.Count)
				return;
			if(_scores[_currentScore].Time <= totalTime)
				LoadedScoreCollection.SetTemp(_scores[_currentScore++].TempScores);
			
			if (_currentPenalty>=_penalties.Count)
				return;
			if(_penalties[_currentPenalty].Time <= totalTime)
				LoadedScoreCollection.AddPenalty(_penalties[_currentPenalty++]);
		}

		public ScoreCollection LoadedScoreCollection { get; private set; }

		private readonly List<ScoreAtTime> _scores;
		private readonly List<Penalty> _penalties;
		private int _currentScore;
		private int _currentPenalty;
	}
}