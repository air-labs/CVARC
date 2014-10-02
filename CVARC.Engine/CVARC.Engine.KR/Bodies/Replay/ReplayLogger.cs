using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using AIRLab.Mathematics;
using System.IO.Compression;

namespace CVARC.Core.Replay
{
	public class ReplayLogger
	{
		/// <summary>
		/// Создает объект для записи реплеев
		/// </summary>
		/// <param name="root">Корень дерева тел, которое будет сохраняться</param>
		/// <param name="dt">
		/// Интервал "виртуального" времени,
		/// которое проходит между вызовами Update
		/// </param>
		public ReplayLogger(Body root, double dt)
		{
			_root = root;
			_dt = dt;
		}

		/// <summary>
		/// Сохраняет врЕменные очки (на данный момент времени)
		/// </summary>
		/// <param name="scores"></param>
		public void LogTempScores(int[] scores)
		{
			_scoreSaver.SaveTempScores(_totalTime, scores);
			_serializationRoot.LastRecordedTime = _totalTime;
			//TODO. Проблема с недобавлением времени к totalTime?
		}

		/// <summary>
		/// Сохраняет список пенальти, набранных роботами за весь матч
		/// (Метод следует вызывать 1 раз по окончании матча)
		/// </summary>
		/// <param name="penalties"></param>
		public void LogPenalties(List<Penalty> penalties)
		{
			_serializationRoot.Penalties = penalties;
		}

		/// <summary>
		/// Сохраняет конфигурацию дерева тел в данный момент времени
		/// </summary>
		public void LogBodies()
		{
			LogObjectsRecursively(_root, _root.Location);
			//Находим удалившиеся из дерева тела. 
			foreach(Body deletedBody in _loggingObjects.Select(x => x.Key).Except(_currentlyExistingBodies))
				_loggingObjects[deletedBody].SaveVisibilityState(_totalTime);
			_currentlyExistingBodies.Clear();
			_serializationRoot.LastRecordedTime = _totalTime;
			_totalTime += _dt;
		}

		/// <summary>
		/// Записывает реплей в файл
		/// </summary>
		/// <param name="filePath">Путь к файлу</param>
		public void WriteReplayToFile(string filePath)
		{
			string replay = GetReplayString();
			File.WriteAllText(filePath, replay);
		}
        public SerializationRoot SerializationRoot{get
        {
            _serializationRoot.DT = _dt;
            _serializationRoot.LoggingObjects = _loggingObjects.Values.ToList();
            _serializationRoot.Scores = _scoreSaver.SavedScores;
            _serializationRoot.RobotCountForScores = _scoreSaver.RobotCount;
            return _serializationRoot;
        }}
		/// <summary>
		/// Возвращает реплей в строке
		/// </summary>
		/// <returns></returns>
		public string GetReplayString()
		{
			_serializationRoot.DT = _dt;
			_serializationRoot.LoggingObjects = _loggingObjects.Values.ToList();
			_serializationRoot.Scores = _scoreSaver.SavedScores;
			_serializationRoot.RobotCountForScores = _scoreSaver.RobotCount;
			var s = new MemoryStream();
			using(var zipstream = new GZipStream(s, CompressionMode.Compress))
			{
				var bf = new BinaryFormatter();
				bf.Serialize(zipstream, _serializationRoot);
			}
			return Convert.ToBase64String(s.ToArray());
		}

		private void LogObjectsRecursively(Body body, Frame3D offset)
		{
			LoggingObject logged;
			if(!_loggingObjects.TryGetValue(body, out logged))
			{
				logged = new LoggingObject(body, _root);
				_loggingObjects.Add(body, logged);
			}
			logged.SaveBody(offset, _totalTime);
			_currentlyExistingBodies.Add(body);
			foreach(Body child in body.Nested)
				LogObjectsRecursively(child, offset.Apply(child.Location));
		}

		private readonly Dictionary<Body, LoggingObject> _loggingObjects =
			new Dictionary<Body, LoggingObject>();

		private readonly HashSet<Body> _currentlyExistingBodies = new HashSet<Body>();
		private double _totalTime;
		private readonly SerializationRoot _serializationRoot = new SerializationRoot();
		private readonly ScoreSaver _scoreSaver = new ScoreSaver();
		private readonly Body _root;
		private readonly double _dt;
	}
}