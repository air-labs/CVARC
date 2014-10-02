using System.Collections.Generic;

namespace CVARC.Core.Replay
{
	public partial class ObjectLoader
	{
		public ObjectLoader(LoggingObject lo, Body world)
		{
			_world = world;
			_loadedBody = lo.Body;
			_loadedBody.Location = lo.InitialLocation;
			Movements = lo.Movements;
			VisibilityStates = lo.VisibilityStates;
		}

		public void Update(double totalTime)
		{
			LoadLocation(totalTime);
			LoadVisibility(totalTime);
		}

		private void LoadVisibility(double totalTime)
		{
			if(_currentVisibilityState < VisibilityStates.Count && 
				VisibilityStates[_currentVisibilityState].StartTime <= totalTime)
			{
                if (_loadedBody.Parent != null && _loadedBody.Parent != _world)
			    {
			        _loadedBody.Parent.Remove(_loadedBody);
                    _world.Add(_loadedBody);
			    }
			    VisibilityStates[_currentVisibilityState].Apply(_loadedBody, _world);
				_currentVisibilityState++;
			}
		}

		public void LoadLocation(double totalTime)
		{
			if(_currentMove >= Movements.Count) 
				return;
			if(Movements[_currentMove].StartTime <= totalTime)
				if(Movements[_currentMove].HasNextChange())
					_loadedBody.Location = Movements[_currentMove].NextLocation();
			else
				_currentMove++;
		}


		public List<Movement> Movements { get; private set; }
		public List<Visibility> VisibilityStates { get; private set; }

		private int _currentMove;
		private int _currentVisibilityState;
		private readonly Body _world;
		private readonly Body _loadedBody;
	}
}