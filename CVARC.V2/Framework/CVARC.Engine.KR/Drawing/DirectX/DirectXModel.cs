using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CVARC.Core;
using SlimDX;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	public class DirectXModel : IDisposable
	{
		public DirectXModel()
		{
			ModifierMatrix = Matrix.Identity;
		}

		public void Dispose()
		{
			//Console.WriteLine("Model disposed");
			Mesh.Dispose();
			foreach(var texture in Textures)
				texture.Value.Dispose();
		}

		public static DirectXModel FromModel(Device device, byte[] content)
		{
			if(device == null)
				throw new Exception("Must have a Device to create model");
			var drmodel = new DirectXModel();
			if(content != null)
				drmodel.Mesh = Mesh.FromMemory(device, content, MeshFlags.Managed);
			ExtendedMaterial[] materials = drmodel.Mesh.GetMaterials();
			for(int i = 0; i < materials.Length; i++)
			{
				if(string.IsNullOrEmpty(materials[i].TextureFileName))
					continue;
				var textureImage = GetTextureResourceObject(materials[i].TextureFileName);
				drmodel.Textures.Add(i, Texture.FromStream(device, textureImage.ToStream(ImageFormat.Jpeg)));
			}
			return drmodel;
		}

		public static DirectXModel CreateCyllinder(Device device, Cylinder cyl)
		{
			var drmodel = new DirectXModel
			              	{
			              		Mesh = DirectXPrimitives.CreateCylinderMesh(device, cyl.RBottom, cyl.RTop, cyl.Height),
			              		ModifierMatrix = Matrix.Translation(0, 0, (float)(cyl.Height / 2))
			              	};

			var brushes = new[]
			              	{
			              		cyl.Top, cyl.Bottom, cyl.Side
			              	};

			drmodel.Mesh.SetMaterials(GetMaterialsFromBrushes(device, cyl.DefaultColor, drmodel, brushes));
			return drmodel;
		}

		public static DirectXModel CreateBall(Device device, Ball ball)
		{
			var drmodel = new DirectXModel
			              	{
			              		Mesh = Mesh.CreateSphere(device, (float)ball.Radius, Slices, Slices),
			              		ModifierMatrix = Matrix.Translation(0, 0, (float)ball.Radius)
			              	};
			drmodel.Mesh.SetMaterials(new[] {ball.DefaultColor.GetDirectXMaterial()});
			return drmodel;
		}

		public static DirectXModel CreateCustomBoxModel(Device device,
		                                                Core.Box box)
		{
			Color defaultColor = box.DefaultColor;
			var m = new DirectXModel
			        	{
                            Mesh = Mesh.FromMemory(device, CVARC.Engine.KR.Properties.Resources.custombox, MeshFlags.Managed),
			        		ModifierMatrix = Matrix.Scaling((float)box.XSize,
			        		                                (float)box.YSize, (float)box.ZSize) * Matrix.Translation(0, 0, (float)box.ZSize / 2)
			        	};
			var brushesInOrder = new[]
			                     	{
			                     		box.Right, box.Left, box.Front, box.Back, box.Top,
			                     		box.Bottom
			                     	};
			m.Mesh.SetMaterials(GetMaterialsFromBrushes(device, defaultColor, m, brushesInOrder));
			return m;
		}

		private static ExtendedMaterial[] GetMaterialsFromBrushes(Device device, Color defaultColor, DirectXModel m, IPlaneBrush[] brushesInOrder)
		{
			var materials = new ExtendedMaterial[brushesInOrder.Length];
			var converter = new DirectXBrushConverter(device);
			for(int i = 0; i < materials.Length; i++)
			{
				materials[i] = defaultColor.GetDirectXMaterial();
				if(brushesInOrder[i] == null)
					continue;
				Texture texture;
				materials[i] = converter.TryConvert(brushesInOrder[i], out texture);
				if(texture != null)
					m.Textures[i] = texture;
			}
			return materials;
		}

		public Matrix ModifierMatrix { get; private set; }
		public Mesh Mesh { get; private set; }
		public readonly Dictionary<int, Texture> Textures = new Dictionary<int, Texture>();

		private static Image GetTextureResourceObject(string textureFileName)
		{
            var rm = CVARC.Engine.KR.Properties.Resources.ResourceManager;
			string resourceName = Path.GetFileNameWithoutExtension(textureFileName);
			return (Image) rm.GetObject(resourceName);
		}

		private const int Slices = 32;
	}
}