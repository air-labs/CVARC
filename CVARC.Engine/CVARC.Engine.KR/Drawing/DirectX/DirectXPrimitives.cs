using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D9;

namespace CVARC.Graphics.DirectX
{
	public class DirectXPrimitives
	{
		public static Mesh CreateCylinderMesh(Device device, double rbottom, double rtop, double height)
		{
			const int thetaDiv = 32;
			float vertOffset = (float)height / 2;
			var pt = new Vector3(0, 0, vertOffset);
			var pb = new Vector3(0, 0, -vertOffset);
			var topPoints = new Vector3[thetaDiv];
			var bottomPoints = new Vector3[thetaDiv];
			for(int i = 0; i < thetaDiv; i++)
			{
				topPoints[i] = GetCirclePosition(rtop, i * 360.0 / (thetaDiv - 1), vertOffset);
				bottomPoints[i] = GetCirclePosition(rbottom, i * 360.0 / (thetaDiv - 1), -vertOffset);
			}
			var startVertices = new List<Vector3>
			                    	{
			                    		//top
			                    		pt,
			                    		topPoints[0],
			                    		topPoints[1],
			                    		//bottom
			                    		pb,
			                    		bottomPoints[1],
			                    		bottomPoints[0],
			                    		//side
			                    		topPoints[0],
			                    		bottomPoints[0],
			                    		bottomPoints[1],
			                    		topPoints[1]
			                    	};
			var startTextures = new List<Vector2>();
			for(int i = 0; i < startVertices.Count; i++)
			{
				double r = (i < 3) ? rtop : rbottom;
				startTextures.Add(GetFlatTextureCoordinates(startVertices[i], r));
			}
			var vertices = new List<CustomVertex>(startVertices.Select((vertexPos, i) =>
			                                                           new CustomVertex
			                                                           	{
			                                                           		Position = vertexPos,
			                                                           		Texture = i < 6 ? startTextures[i] : default(Vector2),
			                                                           		Normal = i < 6 ? GetFlatNormal(vertexPos) : GetSideNormal(vertexPos, height, rtop, rbottom)
			                                                           	}));
			var indices = new List<Int16>
			              	{
			              		0, 1, 2,
			              		3, 4, 5,
			              		6, 7, 8,
			              		8, 9, 6
			              	};
			var attributes = new List<Int32> {0, 1, 2, 2};
			short prevtop = 2;
			short prevbot = 4;
			short prevtopside = 9;
			short prevbotside = 8;
			for(short i = 1; i < thetaDiv - 1; i++)
			{
				var vertcount = (short)(10 + 4 * i);
				// Top surface:
				Vector3 topPoint = topPoints[i + 1];
				vertices.Add(new CustomVertex
				             	{
				             		Position = topPoint,
				             		Texture = GetFlatTextureCoordinates(topPoint, rtop),
				             		Normal = GetFlatNormal(topPoint)
				             	});
				indices.Add(0);
				indices.Add(prevtop);
				indices.Add(vertcount);
				attributes.Add(0);
				// Bottom surface:
				Vector3 bottomPoint = bottomPoints[i + 1];
				vertices.Add(new CustomVertex
				             	{
				             		Position = bottomPoint,
				             		Texture = GetFlatTextureCoordinates(bottomPoint, rbottom),
				             		Normal = GetFlatNormal(bottomPoint)
				             	});
				indices.Add(3); //center
				indices.Add((short)(vertcount + 1)); //latest
				indices.Add(prevbot); //previous
				attributes.Add(1);
				// Side surface:
				vertices.Add(new CustomVertex
				             	{
				             		Position = bottomPoint,
				             		Normal = GetSideNormal(bottomPoint, height, rtop, rbottom)
				             	});
				vertices.Add(new CustomVertex
				             	{
				             		Position = topPoint,
				             		Normal = GetSideNormal(bottomPoint, height, rtop, rbottom)
				             	});
				indices.Add(prevtopside);
				indices.Add(prevbotside);
				indices.Add((short)(vertcount + 2));
				indices.Add((short)(vertcount + 2));
				indices.Add((short)(vertcount + 3));
				indices.Add(prevtopside);
				attributes.Add(2);
				attributes.Add(2);
				prevtop = vertcount;
				prevbot = (short)(vertcount + 1);
				prevbotside = (short)(vertcount + 2);
				prevtopside = (short)(vertcount + 3);
			}
			//top
			indices.Add(0);
			indices.Add(thetaDiv - 1);
			indices.Add(1);
			attributes.Add(0);
			indices.Add(3); //center
			indices.Add(5); //latest
			indices.Add(thetaDiv - 1);
			attributes.Add(1);
			indices.Add(prevtopside);
			indices.Add(prevbotside);
			indices.Add(7);
			indices.Add(7);
			indices.Add(6);
			indices.Add(prevtop);
			attributes.Add(2);
			attributes.Add(2);
			var mesh = new Mesh(device, indices.Count / 3, vertices.Count, MeshFlags.Managed, CustomVertex.TheVertexFormat);
			const LockFlags lockFlags = LockFlags.None;
			using(DataStream vertexStream = mesh.LockVertexBuffer(lockFlags),
			                 indexStream = mesh.LockIndexBuffer(lockFlags),
			                 attributeStream = mesh.LockAttributeBuffer(lockFlags))
			{
				vertices.ForEach(vertexStream.Write);
				indices.ForEach(indexStream.Write);
				attributes.ForEach(attributeStream.Write);
			}
			mesh.UnlockVertexBuffer();
			mesh.UnlockIndexBuffer();
			mesh.UnlockAttributeBuffer();
			return mesh;
		}

		private static Vector3 GetFlatNormal(Vector3 vertex)
		{
			return (vertex.Z > 0) ? Vector3.UnitZ : -Vector3.UnitZ;
		}

		private static Vector3 GetSideNormal(Vector3 vertex, double height,
		                                     double rtop, double rbottom)
		{
			var yaw = (float)Math.Atan2(vertex.Y, vertex.X);
			var pitch = (float)Math.Atan(Math.Abs(rbottom - rtop) / height);
			Vector3 vec = Vector3.UnitX;
			return Vector3.TransformCoordinate(vec, Matrix.RotationYawPitchRoll(yaw, pitch, 0));
		}

		private static Vector2 GetFlatTextureCoordinates(Vector3 vertex, double radius)
		{
			if(radius == 0)
				radius = 0.01;
			Vector2 vec = new Vector2(vertex.X, vertex.Y) / (float)radius;
			vec = Vector2.TransformCoordinate(vec, Matrix.RotationZ((float)(Math.PI * 3 / 2)));
			vec = new Vector2(vec.X + 1, vec.Y + 1) / 2;
			return vec;
		}

		private static Vector3 GetCirclePosition(double radius, double angle, double z)
		{
			var sn = (float)Math.Sin(angle * Math.PI / 180);
			var cn = (float)Math.Cos(angle * Math.PI / 180);
			var x = (float)(-radius * sn);
			var y = (float)(radius * cn);
			return new Vector3(x, y, (float)z);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CustomVertex
	{
		public Vector3 Position;
		public Vector3 Normal;
		public Vector2 Texture;
		public const VertexFormat TheVertexFormat = VertexFormat.PositionNormal | VertexFormat.Texture1;
	}
}