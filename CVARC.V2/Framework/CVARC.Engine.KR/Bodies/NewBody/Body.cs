using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using AIRLab.Mathematics;
using System.Linq;

namespace CVARC.Core
{
	[Serializable]
	public class Body :
		IDeserializationCallback,
		IEnumerable<Body>,
		INotifyPropertyChanged,
		IVisitable<IBodyVisitor>
	{
		public Body()
		{
			TreeRoot = this;
			_id = idCounter++;
		}

		public void OnDeserialization(object sender)
		{
			_nested = new HashSet<Body>();
			TreeRoot = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void AcceptVisitor(IBodyVisitor visitor)
		{
			visitor.Visit(this);
		}

		public event Action<Body> ChildAdded;
		public event Action<Body> ChildRemoved;

		/// <summary>
		/// Контракт: для тела, имеющего детей event должен вызываться, 
		/// если любой ребенок сталкивается с другим телом.
		/// Если второе тело тоже имеет детей, 
		/// в качестве aргумента event-у должен передаться лист второго дерева (тела)
		/// </summary>
		public event Action<Body> Collision;

	    public virtual void OnCollision(Body obj)
	    {
	        if (Collision != null) Collision(obj);
	    }

	    public void Add(Body body)
		{
			if(body == null)
				throw new ArgumentNullException();
			lock(_writeLock)
			{
				if(Equals(body.Parent))
					return;
				if(body.Parent != null)
					throw new InvalidOperationException("Body already added to a parent");
				//С nested работаем как с copy-on-write чтобы избавиться от лока на чтение
				_nested = new HashSet<Body>(_nested) {body};
				body.Parent = this;
				Body newTreeRoot = (TreeRoot == this) ? this : TreeRoot;
				foreach(Body child in body.GetSubtreeChildrenFirst())
					child.TreeRoot = newTreeRoot;
				if(ChildAdded != null)
					ChildAdded(body);
			}
		}

		public void Remove(Body body)
		{
			if(body == null)
				throw new ArgumentNullException();
			lock(_writeLock)
			{
				if(!_nested.Contains(body))
					return;
				//С nested работаем как с copy-on-write чтобы избавиться от лока на чтение
				_nested = new HashSet<Body>(_nested);
				_nested.Remove(body);
				RemoveInternal(body);
			}
		}

		/// <summary>
		/// Отсоединит <paramref name="newChild"/> от его текущего родителя
		/// и присоединит к данному телу, сохраняя при этом абсолютные координаты
		/// </summary>
		/// <param name="newChild"></param>
		public void DetachAttachMaintaingLoction(Body newChild)
		{
			Frame3D childAbsolute = newChild.GetAbsoluteLocation();
			if(newChild.Parent != null)
				newChild.Parent.Remove(newChild);
			newChild.Location = GetAbsoluteLocation().Invert().Apply(childAbsolute);
			Add(newChild);
		}

		public void Clear()
		{
			lock(_writeLock)
			{
				if(_nested.Count == 0)
					return;
				HashSet<Body> oldNested = _nested;
				_nested = new HashSet<Body>();
				foreach(Body body in oldNested)
					RemoveInternal(body);
			}
		}

		public Frame3D GetAbsoluteLocation()
		{
			return GetParents().Aggregate(Location, (current, parent) => parent.Location.Apply(current));
		}

		/// <summary>
		/// Возвращает поддерево включая данный элемент
		/// </summary>
		public IEnumerable<Body> GetSubtreeChildrenFirst()
		{
			var stack = new Stack<Body>();
			stack.Push(this);
			SubtreeInternal(this, stack);
			return stack;
		}

		public virtual void Update(double time)
		{
		    if(!IsMaterial)
		        Location += Velocity;
		}

		public IEnumerable<Body> GetParents()
		{
			var parent = Parent;
			while(parent != null)
			{
				yield return parent;
				parent = parent.Parent;
			}
		} 
		/// <summary>
		/// Является ли данное тело потомком переданного тела
		/// </summary>
		public bool ParentsContain(Body possibleParent)
		{
			return !Equals(possibleParent) && GetParents().Contains(possibleParent);
		}

		/// <summary>
		/// Является ли данное тело предком переданного тела
		/// </summary>
		public bool SubtreeContainsChild(Body possibleChild)
		{
			return possibleChild != null && possibleChild.ParentsContain(this);
		}

        protected virtual double dZ() {
            return 0.0;
        }

        protected virtual double dPitchGrad()
        {
            return 0;
        }

        public List<object> getEditJSON()
        {
            List<object> result = new List<object>();

            result.Add(_id);
            
            result.Add(Math.Round(_location.X, 2));
            result.Add(Math.Round(_location.Y, 2));
            result.Add(Math.Round(_location.Z + dZ(), 2));

            result.Add(Math.Round(_location.Yaw.Radian, 3));
            result.Add(Math.Round(_location.Pitch.Radian, 3));

            Angle t = Angle.FromRad(_location.Roll.Radian).AddGrad(dPitchGrad());
            result.Add(Math.Round(t.Radian, 3));

            result.Add(_parent.Id);
            result.Add("#" + _defaultColor.R.ToString("X2") + _defaultColor.G.ToString("X2") + _defaultColor.B.ToString("X2"));

            return result;
        }

        public virtual List<object> getCreateJSON()
        {
            var result = getEditJSON();
            result.Insert(0, 0);
            return result;
        }

		/// <summary>
		/// Корень дерева тел. Если тело не добавлено ни к чему, равен this (самому телу)
		/// </summary>
		public Body TreeRoot { get { return _treeRoot; } private set { _treeRoot = value; } }

		public virtual double Volume { get { return 0; } }

		public Body Parent { get { return _parent; } private set { _parent = value; } }

		public IEnumerable<Body> Nested { get { return _nested; } }

		public Action<Body, double> CustomAnimationDelegate;
		


        [Obsolete]
		public int Id { get { return _id; } }

        public string NewId { get; set; }

	    #region IEnumerable members

		public IEnumerator<Body> GetEnumerator()
		{
			return _nested.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region NotifyPropertyChanged members and properties

		public Frame3D Location { get { return _location; } set { SetField(ref _location, value, LocationPropertyName); } }

		public Frame3D Velocity { get { return _velocity; } set { SetField(ref _velocity, value, VelocityPropertyName); } }

		/// <summary>
		/// Контракт:
		/// При IsMaterial=true все поддерево должно становится
		/// жестко связанным и физически взаимодействующим
		/// </summary>
		public bool IsMaterial { get { return _isMaterial; } set { SetField(ref _isMaterial, value, MaterialPropertyName); } }

		public Density Density { get { return _density; } set { SetField(ref _density, value, DensityPropertyName); } }

		//TODO. Мб и коэфициенты трения брать из таблицы?
		public double FrictionCoefficient { get { return _frictionCoefficient; } set { SetField(ref _frictionCoefficient, value, FrictionPropertyName); } }

		public bool IsStatic { get { return _isStatic; } set { SetField(ref _isStatic, value, IsStaticPropertyName); } }

		/// <summary>
		/// Дефолтный цвет. Используется, если не определены никакие другие 
		/// характеристики для рисования.
		/// </summary>
		public virtual Color DefaultColor { get { return _defaultColor; } set { SetField(ref _defaultColor, value, DefaultColorPropertyName); } }

		public Model Model { get { return _model; } set { SetField(ref _model, value, ModelPropertyName); } }

	    public const string NamePropertyName = "Name";
		public const string DensityPropertyName = "Density";
		public const string IsStaticPropertyName = "IsStatic";
		public const string FrictionPropertyName = "FrictionCoefficient";
		public const string MaterialPropertyName = "IsMaterial";
		public const string VelocityPropertyName = "Velocity";
		public const string LocationPropertyName = "Location";
		public const string DefaultColorPropertyName = "DefaultColor";
		public const string ModelPropertyName = "Model";

		protected bool SetField<T>(ref T field, T value, string propertyName)
		{
			if(EqualityComparer<T>.Default.Equals(field, value))
				return false;
			field = value;
			if(PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}

		// ReSharper disable UnusedParameter.Local
		protected static void CheckGreaterOrZero(double value)
			// ReSharper restore UnusedParameter.Local
		{
			if(value < 0)
				throw new ArgumentOutOfRangeException();
		}

		#endregion

		private void RemoveInternal(Body body)
		{
			body.Parent = null;
			foreach(Body child in body.GetSubtreeChildrenFirst())
				child.TreeRoot = body;
			if(ChildRemoved != null)
				ChildRemoved(body);
		}

		private static void SubtreeInternal(Body root, Stack<Body> stack)
		{
			foreach(Body body in root.Nested)
			{
				stack.Push(body);
				SubtreeInternal(body, stack);
			}
		}

		[NonSerialized]
		private Body _treeRoot;

		[NonSerialized]
		private Body _parent;

		private Color _defaultColor = Color.Transparent;
		private Model _model;

		[NonSerialized]
		private Density _density;

		[NonSerialized]
		private double _frictionCoefficient;

		[NonSerialized]
		private HashSet<Body> _nested = new HashSet<Body>();

		[NonSerialized]
		private Frame3D _location;

		[NonSerialized]
		private bool _isMaterial;

		[NonSerialized]
		private readonly object _writeLock = new object();

		[NonSerialized]
		private bool _isStatic;

		[NonSerialized]
		private Frame3D _velocity;

		private readonly int _id;

		private static int idCounter;
	}
}