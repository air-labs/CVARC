using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CVARC.Core;
using SlimDX;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	/// <summary>
	/// Сцена в DirectX - трехмерное представление дерева тел
	/// </summary>
	public sealed class DirectXScene : DrawingBodyWorker<DirectXModel, Matrix>
	{
		internal DirectXScene(DeviceWorker deviceWorker, Body root, Body floor)
		{
			DeviceWorker = deviceWorker;
			RootBody = root;
			Floor = floor; //TODO. get rid of floor by determining intersection
			List<Light> lights =
				SceneConfig.Lights.Select(lightSetting => lightSetting.ToDirectXLight()).ToList();
			DeviceWorker.Disposing += Dispose;
			Effect = new DefaultEffect(DeviceWorker.Device, lights);
			DeviceWorker.AfterReset += Effect.Reset;
			Models = new Dictionary<Body, DirectXModel>(new BodyComparer());
			_modelFactory = new DirectXModelFactory(DeviceWorker.Device);
		}

		/// <summary>
		/// Создает сцену из дерева тел на основе переданного корня дерева
		/// </summary>
		/// <param name="root">Тело, являющееся корнем дерева тел</param>
		/// <param name="floor"> </param>
		public DirectXScene(Body root, Body floor)
			: this(new DeviceWorker(), root, floor)
		{
		}

		/// <summary>
		/// Создает сцену из дерева тел на основе переданного корня дерева
		/// </summary>
		/// <param name="rootBody">Тело, являющееся корнем дерева тел</param>
		public DirectXScene(Body rootBody)
			: this(rootBody, null)
		{
		}

		//TODO. тошнотворный костыль для того, чтобы тела могли быть прозрачными
		public void UpdateModelsBase(Body root, Body doNotDraw)
		{
			UpdateModels(root, doNotDraw);
			Matrix worldTransform = Effect.WorldTransform;
			foreach(var pair in _delayedTransparentBodies)
			{
				DrawSingleBody(pair.Key, pair.Value);
				Effect.WorldTransform = worldTransform;
			}
			_delayedTransparentBodies.Clear();
		}

		public override void UpdateModels(Body root, Body doNotDraw)
		{
			if(_firstTime)
				_stopwatch.Start();
			if(Equals(root, null) || Equals(root, doNotDraw))
				return;
			if(!IsTransparent(root))
				base.UpdateModels(root, doNotDraw);
			else
			{
				Matrix transform = ApplyTransformMatrix(root);
				_delayedTransparentBodies.Add(root, transform);
				ResetTransformMatrix(transform);
			}
			if(!_firstTime)
				return;
			_stopwatch.Stop();
			LogInfo("Models initialized in {0} ms", _stopwatch.ElapsedMilliseconds);
			_firstTime = false;
		}

		public Effect Effect { get; private set; }
		public Body RootBody { get; private set; }
		public Body Floor { get; private set; }
		internal DeviceWorker DeviceWorker { get; private set; }

		protected override bool TryCreateModelForBody(Body body, out DirectXModel model)
		{
			model = null;
			if(_invisibleBodies.Contains(body))
				return false;
			if(_modelFactory.TryGetResult(body, out model))
				return true;
			_invisibleBodies.Add(body);
			return false;
		}

		protected override void DrawModel(DirectXModel model)
		{
			Effect.DrawModel(model);
		}

		protected override void ResetTransformMatrix(Matrix m)
		{
			Effect.WorldTransform = m;
		}

		protected override Matrix ApplyTransformMatrix(Body root)
		{
			Effect.MultiplyWorldTransform(root.GetRelativeLocationMatrix());
			return Effect.WorldTransform;
		}

		private void DrawSingleBody(Body body, Matrix transformFromBody)
		{
			Effect.MultiplyWorldTransform(transformFromBody);
			DrawModel(Models[body]);
		}

		private bool IsTransparent(Body root)
		{
			DirectXModel model;
			if(!Models.TryGetValue(root, out model))
				return false;
			ExtendedMaterial[] mats = model.Mesh.GetMaterials();
			return mats.Any(x => x.MaterialD3D.Diffuse.Alpha < 0.9);
		}

		private void Dispose()
		{
			Effect.Dispose();
			DisposeModels();
			LogInfo("Scene disposed");
		}

		private static void LogInfo(string message, params object[] args)
		{
			//Console.WriteLine(message, args);
		}

		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly DirectXModelFactory _modelFactory;
		private bool _firstTime;
		private readonly HashSet<Body> _invisibleBodies = new HashSet<Body>();

		private readonly Dictionary<Body, Matrix> _delayedTransparentBodies
			= new Dictionary<Body, Matrix>();
	}
}