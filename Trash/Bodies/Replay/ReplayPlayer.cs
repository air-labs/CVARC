using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
//using Ionic.Zlib;

namespace CVARC.Core.Replay
{
	public class ReplayPlayer
	{
		/// <summary>
		/// Создает новый проигрыватель на основе строки с содержимым реплея
		/// </summary>
		/// <param name="replayContent"> строка с содержимым реплея</param>
		public ReplayPlayer(string replayContent)
		{
			RootBody = new Body();
			const string header = "<Feedback>";
			if(string.Equals(replayContent.Substring(0, header.Length), header, StringComparison.InvariantCultureIgnoreCase))
				replayContent = replayContent.Substring(header.Length, replayContent.Length - header.Length * 2 - 1);
			using(Stream memoryStream = new MemoryStream(Convert.FromBase64String(replayContent)),
						zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				_serializationRoot = DeserializeInternal(zipStream);
			DT = _serializationRoot.DT;
			_objectLoaders = new List<ObjectLoader>(
				_serializationRoot.LoggingObjects
								.Select(x => new ObjectLoader(x, RootBody)));
			_scoreLoader = new ScoreLoader(_serializationRoot.Scores, _serializationRoot.Penalties, _serializationRoot.RobotCountForScores);
		}
        public ReplayPlayer(SerializationRoot serializationRoot)
        {
            _serializationRoot = serializationRoot;
            RootBody = new Body();
            DT = _serializationRoot.DT;
			_objectLoaders = new List<ObjectLoader>(
				_serializationRoot.LoggingObjects
								.Select(x => new ObjectLoader(x, RootBody)));
			_scoreLoader = new ScoreLoader(_serializationRoot.Scores, _serializationRoot.Penalties, _serializationRoot.RobotCountForScores);
        }

		/// <summary>
		/// Создает новый проигрыватель, считав данные реплея из файла
		/// </summary>
		/// <param name="filePath">Путь к файлу</param>
		/// <returns>Проигрыватель</returns>
		public static ReplayPlayer FromFile(string filePath)
		{
			return new ReplayPlayer(File.ReadAllText(filePath, Encoding.ASCII));
		}

		/// <summary>
		/// Обновляет дерево тел и набранные роботами очки
		/// </summary>
		public void Update()
		{
			UpdateBodies();
			UpdateScores();
		}

		/// <summary>
		/// Обновляет дерево тел
		/// </summary>
		public void UpdateBodies()
		{
			//TODO. To Private.
			foreach(ObjectLoader lo in _objectLoaders)
				lo.Update(_totalTime);
			_totalTime += DT;
		}

		/// <summary>
		/// Обновляет набранные роботами очки
		/// </summary>
		public void UpdateScores()
		{
			//TODO. To Private.
			_scoreLoader.UpdateScores(_totalTime);
		}

		/// <summary>
		/// Очки, набранные роботами во время матча. Обновляется при помощи <see cref="UpdateScores" />
		/// </summary>
		public ScoreCollection Scores { get { return _scoreLoader.LoadedScoreCollection; } }

		/// <summary>
		/// Интервал времени, которому соответствует один вызов Update()
		/// </summary>
		public double DT { get; private set; }

		/// <summary>
		/// True, если записанный реплей закончился и больше воспроизводить нечего.
		/// </summary>
		public bool IsAtEnd { get { return _totalTime > _serializationRoot.LastRecordedTime; } }

		/// <summary>
		/// Корень дерева тел. Содержит все тела: робота, стол, перемещаемые роботом объекты, итд.
		/// Обновляется при помоши <see cref="UpdateBodies" />
		/// </summary>
		public Body RootBody { get; private set; }

		private static SerializationRoot DeserializeInternal(Stream stream)
		{
			var bf = new BinaryFormatter();
			return (SerializationRoot)bf.Deserialize(stream);
		}

		private readonly List<ObjectLoader> _objectLoaders = new List<ObjectLoader>();
		private readonly SerializationRoot _serializationRoot;
		private readonly ScoreLoader _scoreLoader;
		private double _totalTime;
	}
}