using System;
using SlimDX;

namespace CVARC.Graphics.DirectX
{
	public abstract class Effect:IDisposable
	{
		public virtual void Dispose()
		{
		}

		public abstract void Initialize();

		public virtual void Reset()
		{
			Initialize();
		}

		public abstract void DrawScene(DirectXScene scene);
		public abstract void DrawModel(DirectXModel model);
		public abstract Matrix WorldTransform { get; set; }
		public abstract Matrix ViewTransform { get; set; }
		public abstract Matrix ProjectionTransform { get; set; }
		public abstract void MultiplyWorldTransform(Matrix matrix);
	}
}