using System;
using System.Collections.Generic;
using System.Linq;
using AIRLab.Mathematics;

namespace CVARC.Core.Replay
{
	[Serializable]
	public partial class LoggingObject
	{
		public LoggingObject(Body pb, Body root)
		{
			Body = pb;
			_root = root;
			InitialLocation = pb.GetAbsoluteLocation();
			_lastLocation = InitialLocation.NewX(InitialLocation.X + 1);
			_lastVisibility = !IsVisible();
		}

		public void SaveBody(Frame3D location, double totalTime)
		{
			SaveLocation(location, totalTime);
			SaveVisibilityState(totalTime);
		}

		public Body Body { get; private set; }

		public Frame3D InitialLocation { get; private set; }
		public readonly List<Movement> Movements = new List<Movement>();

		public readonly List<Visibility> VisibilityStates = new List<Visibility>();

		private void SaveLocation(Frame3D newLocation, double totalTime)
		{
			if(newLocation.Equals(_lastLocation))
			{
				_isCurrentlyMoving = false;
				return;
			}
			if(!_isCurrentlyMoving)
			{
				Movements.Add(new Movement(totalTime));
				_isCurrentlyMoving = true;
			}
			_lastLocation = newLocation;
			Movements.Last().SaveLocation(newLocation);
		}

		public void SaveVisibilityState(double totalTime)
		{
			bool isVisible = IsVisible();
			if(isVisible == _lastVisibility)
				return;
			_lastVisibility = isVisible;
			VisibilityStates.Add(new Visibility(isVisible, totalTime));
		}

		private bool IsVisible()
		{
			return Body.TreeRoot.Equals(_root) && !Body.Equals(_root);
		}

		[NonSerialized]
		private Frame3D _lastLocation;

		[NonSerialized]
		private bool _isCurrentlyMoving;

		[NonSerialized]
		private bool _lastVisibility;

		[NonSerialized]
		private readonly Body _root;
	}
}