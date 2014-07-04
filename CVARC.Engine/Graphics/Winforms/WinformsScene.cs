using System.Collections.Generic;
using AIRLab.Mathematics;
using CVARC.Core;
using Matrix = System.Drawing.Drawing2D.Matrix;

namespace CVARC.Graphics.Winforms
{	
	public sealed class WinformsScene : DrawingBodyWorker<WinformsModel, Matrix>
	{
		public WinformsScene()
		{
			Models = new Dictionary<Body, WinformsModel>();//TODO. comparator.
		}

		public override void UpdateModels(Body root, Body doNotDraw)
		{
			if(root == null || root.Equals(doNotDraw))
				return;
			var bodies = new List<Body>(root.GetSubtreeChildrenFirst());
			bodies.Sort((a, b) => a.Location.Z.CompareTo(b.Location.Z));
			Matrix tr = Graphics.Transform;
			foreach(var body in bodies)
			{
				Graphics.MultiplyTransform(GetMatrix(body.GetAbsoluteLocation()));
				WinformsModel model;
				if (Models.TryGetValue(body, out model))
					model.Draw(Graphics);
				else if(TryCreateModelForBody(body, out model))
					AddModel(body, model);
				Graphics.Transform = tr;
			}
		}

		public System.Drawing.Graphics Graphics { get; set; }

		protected override bool TryCreateModelForBody(Body body, out WinformsModel model)
		{
			return _modelFactory.TryGetResult(body, out model);
		}

		private static Matrix GetMatrix(Frame3D loc)
		{
			var m = new Matrix();
			m.Translate((float)loc.X, -(float)loc.Y);
			m.Rotate(-(float)loc.Yaw.Grad);
			return m;
		}
		private readonly WinformsModel.WinformsModelFactory _modelFactory=new WinformsModel.WinformsModelFactory();
	}
}