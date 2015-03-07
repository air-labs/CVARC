using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	/// <summary>
	/// This part of the class deals with storing the different competitions.
	/// </summary>
	public partial class Loader
	{
		/// <summary>
		/// Contains all available competitions. The factory for a competition can be obtained by the competition name and the level name.
		/// </summary>
		public readonly Dictionary<string, Dictionary<string, Func<Competitions>>> Levels = new Dictionary<string, Dictionary<string, Func<Competitions>>>();

		public void AddLevel(string competitions, string level, Func<Competitions> factory)
		{
			if (!Levels.ContainsKey(competitions))
				Levels[competitions] = new Dictionary<string, Func<Competitions>>();
			Levels[competitions][level] = factory;
		}

		public Competitions GetCompetitions(string assemblyName, string level)
		{
			return Levels[assemblyName][level]();
		}

		public Competitions GetCompetitions(LoadingData data)
		{
			return GetCompetitions(data.AssemblyName, data.Level);
		}
        
	}
}
