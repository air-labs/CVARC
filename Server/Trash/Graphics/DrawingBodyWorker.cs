using System;
using System.Collections.Generic;
using CVARC.Core;

namespace CVARC.Graphics
{
	public abstract class DrawingBodyWorker<TModel, TTransform> 
		where TModel : IDisposable
	{
		/// <summary>
		/// Рисуем модели, используя Location соответствующих им тел.
		/// Первый раз вызываем для Drawer.RootBody, затем рекурсивно.
		/// </summary>
		/// <param name="root">Текущее корневое тело</param>
		/// <param name="doNotDraw">Список тел, которые НЕ отрисовываются</param>
		public virtual void UpdateModels(Body root, Body doNotDraw)
		{
			if(root == null || Equals(root, doNotDraw))
				return;
			TTransform m = ApplyTransformMatrix(root);
			TModel model;
			if(Models.TryGetValue(root, out model))
				DrawModel(model);

			else if(TryCreateModelForBody(root, out model))
				AddModel(root, model);

			foreach(Body body in root.Nested)
			{
				UpdateModels(body, doNotDraw);
				ResetTransformMatrix(m);
			}
		}

		public void UpdateModels(Body root)
		{
			UpdateModels(root, null);
		}

		public void DisposeModels()
		{
			foreach (var pair in Models)
				pair.Value.Dispose();
		}

		public Dictionary<Body, TModel> Models = new Dictionary<Body, TModel>();

		protected abstract bool TryCreateModelForBody(Body body, out TModel model);

		protected virtual void AddModel(Body body, TModel mod)
		{
			Models.Add(body, mod);
		}

		protected virtual void DrawModel(TModel model)
		{
		}

		protected virtual TTransform ApplyTransformMatrix(Body root)
		{
			return default(TTransform);
		}

		protected virtual void ResetTransformMatrix(TTransform matrix)
		{
		}
	}
}