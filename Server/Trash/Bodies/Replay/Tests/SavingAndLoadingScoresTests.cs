using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CVARC.Core.Replay
{
	public class SavingAndLoadingScoresTests
	{
		[SetUp]
		public void SetUp()
		{
			_totalTime = 0;
		}

		[Test]
		public void TestSavingScores()
		{
			var sc = new ScoreSaver();
			var firstScores = new[] {10, 20};
			SaveScoresAndCheck(firstScores, sc, 1, 0);
			SaveScoresAndCheck(firstScores, sc, 1, 0);
			SaveScoresAndCheck(new[] {200, 30}, sc, 2, 6);
		}
		[Test]
		public void TestLoadingScores()
		{
			var recorded = new List<ScoreAtTime>
			               	{
			               		new ScoreAtTime(10, new[] {10, 20}),
			               		new ScoreAtTime(30, new[] {20, 10})
			               	};
			var penalties = new List<Penalty>
			                        	{
			                        		new Penalty
			                        			{
			                        				Time = 10,
			                        				Value = -10,
			                        				RobotNumber = 2
			                        			}
			                        	};
			var sl = new ScoreLoader(Copy(recorded), Copy(penalties),2);
			var scoreCollection = sl.LoadedScoreCollection;
			for (int i = 0; i < 31; i++)
			{
				sl.UpdateScores(i);
				var expectedScores = recorded.LastOrDefault(x => x.Time <= i);
				var expectedTempSums = (expectedScores != null) ? expectedScores.TempScores : new int[sl.LoadedScoreCollection.RobotCount];

				CollectionAssert.AreEqual(expectedTempSums,new[]{scoreCollection.GetTemp(0), scoreCollection.GetTemp(1)} );
				int i1 = i;
				var expectedPenalties = new List<Penalty>(penalties.Where(x => x.Time <= i1));
				
				CollectionAssert.AreEquivalent(expectedPenalties.Select(x=>x.Value), 
					sl.LoadedScoreCollection.Penalties.Select(x=>x.Value));
			}

		}

		private static T Copy<T>(T obj)
		{
			return ObjectLoader.LoadingBodiesTests.SerializeAndDeserialize(obj);
		}

		private void SaveScoresAndCheck(int[] scores, ScoreSaver sc, int expectedScoreCount, int expectedStartTime)
		{
			for (int i = 0; i < 3; i++)
			{
				sc.SaveTempScores(_totalTime, scores);
				Assert.AreEqual(expectedScoreCount, sc.SavedScores.Count);
				Assert.AreEqual(expectedStartTime, sc.SavedScores.Last().Time);
				CollectionAssert.AreEqual(scores, sc.SavedScores.Last().TempScores);
				_totalTime++;
			}
		}

		private int _totalTime;
	}
}