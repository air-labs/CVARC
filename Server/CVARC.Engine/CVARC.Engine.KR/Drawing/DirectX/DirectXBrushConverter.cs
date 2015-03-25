using System;
using System.Drawing;
using System.Drawing.Imaging;
using CVARC.Core;
using CVARC.Graphics.DirectX.Utils;
using CVARC.Engine.KR.Properties;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	public class DirectXBrushConverter : IConverter<IBrush, ExtendedMaterial>
	{
		public DirectXBrushConverter(Device device)
		{
			_device = device;
		}

		public virtual ExtendedMaterial Convert(IBrush brush)
		{
			throw new Exception("Non-plane brushes are currently not supported");
		}

		public void OnError(IBrush t)
		{
		}

		public virtual ExtendedMaterial Convert(SolidColorBrush brush)
		{
			return brush.Color.GetDirectXMaterial();
		}

		public virtual ExtendedMaterial Convert(PlaneImageBrush brush)
		{
#pragma warning disable 612,618
			Image image = brush.Image;
#pragma warning restore 612,618
			if(!string.IsNullOrEmpty(brush.FilePath))
				image = Image.FromFile(brush.FilePath);
			else if(!string.IsNullOrEmpty(brush.ResourceName))
				image = (Image)Resources.ResourceManager.GetObject(brush.ResourceName);
			if(image != null && brush.RotateFlipType != RotateFlipType.RotateNoneFlipNone)
				image.RotateFlip(brush.RotateFlipType);
			LastTextureResult = Texture.FromStream(_device, image.ToStream(ImageFormat.Jpeg));
			ExtendedMaterial m = DefaultColor.GetDirectXMaterial();
			return m;
		}

		public ExtendedMaterial TryConvert(IBrush brush, out Texture texture)
		{
			LastTextureResult = null;
			ExtendedMaterial material = brush.Convert(this);
			texture = LastTextureResult;
			if (texture!=null)
				material.TextureFileName = "fakeTexture";
			return material;
		}

		public static readonly Color DefaultColor = Color.White;

		private Texture LastTextureResult { get; set; }
		private readonly Device _device;
	}
}