using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Entities;
using AIRLab.Mathematics;
using BEPUphysics.MathExtensions;
using BEPUphysics.Constraints.TwoEntity.JointLimits;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Constraints.SingleEntity;
using BEPUphysics.SolverSystems;

namespace CVARC.Core.Physics.BepuWrap
{
	class BepuBody : IPhysical
	{
		#region General properties
		/*
		private CollisionHandler _onCollision;
		/// <summary>
		/// При коллизии будет вызван данный делегат и ему будут переданны id столкнувшихся объектов.
		/// </summary>
		public CollisionHandler OnCollision
		{
			get
			{
				return _onCollision;
			}
			set
			{
				_onCollision = value;
				RealBody.CollisionInformation.Events.ContactCreated += 
					Events_ContactCreated;
			}
		}
		*/
		private bool _actAs2d = false;
		private RevoluteAngularJoint _noRevols = null;
		public bool ActAs2d
		{
			get
			{
				return _actAs2d;
			}
			set
			{
				_actAs2d = value;
				if (value)
				{
					//TODO. fix after new bodies.
					/*//RealBody.Position = new Vector3(RealBody.Position.X, RealBody.Position.Y, 15);
					Box floor = new Box(new Vector3(0, 0, -10), 1, 1, 1);
					_noRevols = new RevoluteAngularJoint(floor, RealBody, new Vector3(0, 0, 1));
					//new BallSocketJoint(
					BepuWorld.World.Add(_noRevols);

					//Box plane = new Box(new Vector3(20, 20, RealBody.Position.Z), 1, 1, 1);
					//PointOnPlaneJoint pl = new PointOnPlaneJoint(plane, RealBody, new Vector3(0, 0, RealBody.Position.Z),
					//    new Vector3(0, 0, 1), RealBody.Position);
					//new PointOnPlaneJoint(,,,,
					//BepuWorld.World.Add(pl);*/
				}
				else
				{
					if (_noRevols != null)
					{
						BepuWorld.World.Remove(_noRevols);
						_noRevols = null;
					}
				}
			}
		}

	   

		public int Id
		{
			get
			{
				try
				{
					return (int)RealBody.Tag;
				}
				catch { return Int32.MinValue; }
			}
			set
			{
				RealBody.Tag = value;
			}
		}

		private bool _isMaterial = false;
		public bool IsMaterial
		{
			get
			{
				return _isMaterial;
			}
			set
			{				
				if (RealBody != null)
				{
					//if (!value && _isMaterial) // надо убрать из мира
					//    BepuWorld.World.Remove(RealBody);
					//if (value && !_isMaterial) // надо добавить в мир
					//    BepuWorld.World.Add(RealBody);
					if (!value && BepuWorld.World.Entities.Contains(RealBody)) // надо убрать из мира
						BepuWorld.World.Remove(RealBody);
					if (value && !BepuWorld.World.Entities.Contains(RealBody)) // надо добавить в мир
						BepuWorld.World.Add(RealBody);
				}
				_isMaterial = value;
			}
		}

		private bool _isStatic = false;
		public bool IsStatic
		{
			get
			{				
				return _isStatic;
			}
			set
			{
				_isStatic = value;
				if (RealBody != null)
				{
					if (!value)
						RealBody.BecomeDynamic((float)_mass);
					else RealBody.BecomeKinematic();
				}
				//RealBody.IsStatic = value;
			}
		}

		private const double _minMass = 10e-3;
		private double _mass = _minMass;
		public double Mass
		{
			get
			{
				return RealBody.Mass;
			}
			set
			{
				if (value < _minMass) // Нельзя ставить нулевую массу
					value = _minMass;
				_mass = value;
				if (RealBody != null && RealBody.ActivityInformation.IsDynamic)
					RealBody.Mass = (float)value;
			}
		}

		private bool _floorFricitionEnabled = true;
		public bool FloorFrictionEnabled
		{
			get
			{
				return _floorFricitionEnabled;
			}
			set
			{
				_floorFricitionEnabled = value;
				if (RealBody != null)
				{
					if (!value)
					{
						RealBody.Material.KineticFriction = 0;
						//RealBody.LinearDamping = 0;
						//RealBody.AngularDamping = 0;
					}
					else
					{
						RealBody.Material.KineticFriction = (float)_frictionCoefficient;
						//RealBody.LinearDamping = (float)_frictionCoefficient;
						//RealBody.AngularDamping = (float)_frictionCoefficient;
					}
				}

			}
		}

		private double _frictionCoefficient;
		public double FrictionCoefficient
		{
			get
			{
				return _frictionCoefficient;
			}
			set
			{
				_frictionCoefficient = value;
				if (FloorFrictionEnabled)
				{
					if (RealBody != null)
					{
						//RealBody.LinearDamping = (float)_frictionCoefficient;
						//RealBody.AngularDamping = (float)_frictionCoefficient;
						RealBody.Material.KineticFriction = (float)_frictionCoefficient; //TODO side friction, floor friction
					}
				}
			}
		}

		/// <summary>
		/// Начально положение объекта в мире физики. Нужно для объектов, которые должны быть изначально повёрнуты, например 
		/// цилиндр без этого будет кататься на боку.
		/// </summary>
		public Quaternion InitialOrientation = Quaternion.CreateFromYawPitchRoll(0, 0, 0);
		private Frame3D _location;
		public Frame3D Location
		{
			get
			{
				if (RealBody != null)
				{
					_location = BepuConverter.GetFrame(RealBody, InitialOrientation);
				}
				return _location;
			}

			set
			{
				_location = value;
				if (RealBody != null)
				{
					BepuConverter.PutInFrame(RealBody, value, InitialOrientation);
				}
			}
		}

		private Frame3D _velocity;
		public Frame3D Velocity
		{
			get
			{
				Vector3 av = RealBody.AngularVelocity;
				_velocity = BepuConverter.Vector3ToFrame3D(BepuConverter.ToDisplayUnits(RealBody.LinearVelocity)); 
				_velocity = new Frame3D(_velocity.X, _velocity.Y, _velocity.Z, Angle.FromRad(av.Z), Angle.FromRad(av.X), Angle.FromRad(-av.Y));
				return _velocity;
			}
			set
			{
				_velocity = value;
				if (RealBody != null)
				{
					//RealBody.IsStatic = false;
					// Повернём на нужный угол					
					RealBody.AngularVelocity = new Vector3(
						//(float)value.Pitch.Radian, (float)value.Roll.Radian, -(float)value.Yaw.Radian);
						(float)value.Roll.Radian, (float)value.Pitch.Radian, -(float)value.Yaw.Radian);

					// Едем через скорость
					Vector3 v = BepuConverter.ToSimUnits(BepuConverter.Frame3DToVector3(value));
					RealBody.LinearVelocity = new Vector3(-v.X, v.Y, v.Z);
					//System.Diagnostics.Debug.WriteLine("");
				}
			}
		}

		#endregion

		//--------------------------------------------------------------------------

		#region Joints

		private class Connection
		{
			private List<SolverUpdateable> _joints = new List<SolverUpdateable>();

			private Entity _otherBody;
			public Entity OtherBody { get { return _otherBody; } }

			public Connection(Entity otherBody) { _otherBody = otherBody; }

			public void Add(SolverUpdateable joint) { _joints.Add(joint); }

			public void Disconnect() { foreach (var j in _joints) BepuWorld.RemoveJoint(j); }
		}


		private List<Connection> _connections=new List<Connection>();
	  

		/// <summary>
		/// Присоединит переданный объект, используя его Location.
		/// </summary>		
		public void Attach(IPhysical body, bool joinWithFriction = true)
		{
			Attach(body, body.Location, joinWithFriction);
		}

		/// <summary>
		/// Присоединит переданный объект, используя переданный Location.
		/// </summary>		
		public void Attach(IPhysical body, Frame3D realLocation, bool joinWithFriction = true)
		{
			var bb = (BepuBody) body;
			var rb = bb.RealBody;
			BepuConverter.PutInFrame(rb, realLocation, bb.InitialOrientation);
			//bb.RealBody.Position = new Vector3(-bb.RealBody.Position.X, bb.RealBody.Position.Y, bb.RealBody.Position.Z);
			rb.Position += RealBody.Position;
			rb.Orientation *= RealBody.Orientation;
			SolverGroup wj = BepuWorld.MakeWeldJoint(RealBody, rb);
			Connection c;
			try//есть connection
			{
				c = _connections.First(x => x.OtherBody == rb);
				c.Add(wj);
			}
			catch (Exception)//нет connection
			{
				c = new Connection(rb);
				c.Add(wj);
				_connections.Add(c);//connection добавляем для обоих тел! М.К.
				bb._connections.Add(c);
			}
		}

		/// <summary>
		/// Отсоединит переданный объект, пересчитает его Location так, чтобы он был абсолютным.
		/// </summary>		
		public void Detach(IPhysical body)
		{
			var bb = (BepuBody) body;
			var rb = bb.RealBody;
			foreach (var c in _connections.ToList().Where(c => c.OtherBody == rb))
			{
				try
				{
					c.Disconnect();
					_connections.Remove(c);
					bb._connections.Remove(c);
				}
				catch
				{
				}
				break;
			}
		}

		#endregion

		//--------------------------------------------------------------------------

		/// <summary>
		/// Настоящее тело движка Бепу.  
		/// </summary>
		public Entity RealBody { get; set; }
		/// <summary>
		/// Тело эмулятора
		/// </summary>
		public Body Body { get; set; }

		/// <summary>
		/// Не будет обращать внимание на изменение свойств тела. Нужно, когда эти свойства меняет сама физика
		/// </summary>
		public bool SuprressPropertyChangedEvents { get; set; }

		//public BepuBody()
		//{ }
		private static readonly Dictionary<EntityCollidable, BepuBody> BepuEntityCollidableToBepuBody =
		  new Dictionary<EntityCollidable, BepuBody>();

		public BepuBody(Entity realBody)
		{
			RealBody = realBody;
			RealBody.Tag = -1;
			RealBody.BecomeDynamic((float)_mass);
			//RealBody.CollisionInformation.LocalPosition = new Vector3(0, 0, -10); //center of mass?
			BepuEntityCollidableToBepuBody.Add(realBody.CollisionInformation, this);
		 //TODO. fix after new bodies.
			/*   RealBody.CollisionInformation.Events.InitialCollisionDetected += 
			(thisBody, other, x)=>
				{
					if (other is EntityCollidable)
						Body.RaiseCollisionEvent(BepuEntityCollidableToBepuBody[other as EntityCollidable].Body);
				};*/
			//IsStatic = false;
		}
	}
}
