using System.IO;
using CVARC.Core;
using SlimDX.Direct3D9;
using Resources = CVARC.Engine.KR.Properties.Resources;

namespace CVARC.Graphics.DirectX
{
	internal partial class DirectXModelFactory : ModelFactory<DirectXModel>
	{
		public DirectXModelFactory(Device device)
		{
			_device = device;
		}

		public override void Visit(Core.Box visitable)
		{

			if(!TryCreateFromStoredModel(visitable) && !visitable.IsInvisible())
				InternalResult = DirectXModel.CreateCustomBoxModel(_device, visitable);
		}

		public override void Visit(Ball visitable)
		{
			if(!TryCreateFromStoredModel(visitable) && !visitable.IsInvisible())
				InternalResult = DirectXModel.CreateBall(_device, visitable);
		}

		public override void Visit(Cylinder visitable)
		{
			if(!TryCreateFromStoredModel(visitable) && !visitable.IsInvisible())
				InternalResult = DirectXModel.CreateCyllinder(_device, visitable);
		}

		public override void Visit(Body visitable)
		{
			TryCreateFromStoredModel(visitable);
		}

		private bool TryCreateFromStoredModel(Body visitable)
		{
			byte[] content;
			if(TryGetContentFromModel(visitable.Model, out content))
			{
				InternalResult = DirectXModel.FromModel(_device, content);
				return true;
			}
			return false;
		}

		private static bool TryGetContentFromModel(Model model, out byte[] content)
		{
			content = null;
			if(model == null)
				return false;
#pragma warning disable 612,618
			content = model.Content;
#pragma warning restore 612,618
			if(!string.IsNullOrEmpty(model.ResourceName))
				content = (byte[])Resources.ResourceManager.GetObject(model.ResourceName);
			else if(!string.IsNullOrEmpty(model.FilePath))
				content = File.ReadAllBytes(model.FilePath);
			return content != null;
		}

		private readonly Device _device;
	}
}