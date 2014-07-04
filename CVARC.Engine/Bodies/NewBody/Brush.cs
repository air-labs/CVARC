using System;
using System.Drawing;
using System.Linq.Expressions;

namespace CVARC.Core
{
	public interface IBrush
	{
	}

	public interface IPlaneBrush : IBrush
	{
	}

	[Serializable]
	public class SolidColorBrush : IPlaneBrush
	{
		public bool Equals(SolidColorBrush other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return Color.RgbEquals(other.Color);
		}

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			if(ReferenceEquals(this, obj)) return true;
			if(obj.GetType() != typeof(SolidColorBrush)) return false;
			return Equals((SolidColorBrush)obj);
		}

		public override string ToString()
		{
			return string.Format("Color: {0}", Color);
		}

		public override int GetHashCode()
		{
			return Color.GetHashCode();
		}

		public Color Color;
	}

	[Serializable]
	public class PlaneImageBrush : IPlaneBrush
	{
		/// <summary>
		/// Инициализировать изображение для кисти из ресурсов
		/// </summary>
		/// <param name="func">
		/// Лямбда, текст тела которой будет использован в качестве названия ресурса.
		/// Например, ()=>Resources.testtexture загрузит testtexture.
		/// </param>
		public static PlaneImageBrush FromResource(Expression<Func<Image>> func)
		{
			return new PlaneImageBrush
			       	{
			       		ResourceName = CommonUtils.GetPropertyName(func)
			       	};
		}

		public bool Equals(PlaneImageBrush other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return Equals(other._image, _image) 
				&& Equals(other.FilePath, FilePath) 
				&& Equals(other.ResourceName, ResourceName);
		}

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj)) return false;
			if(ReferenceEquals(this, obj)) return true;
			if(obj.GetType() != typeof(PlaneImageBrush)) return false;
			return Equals((PlaneImageBrush)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (_image != null ? _image.GetHashCode() : 0);
				result = (result * 397) ^ (FilePath != null ? FilePath.GetHashCode() : 0);
				result = (result * 397) ^ (ResourceName != null ? ResourceName.GetHashCode() : 0);
				return result;
			}
		}

		public override string ToString()
		{
			return string.Format("ResourceName: {0}, FilePath: {1}", ResourceName, FilePath);
		}

		public RotateFlipType RotateFlipType { get; set; }
		public string FilePath { get; set; }

		/// <summary>
		/// При записи реплея содержимое будет сериализовано. Использовать с осторожностью.
		/// Лучше использовать <see cref="FromResource"/> 
		/// </summary>
		[Obsolete("При записи реплея содержимое будет сериализовано. Использовать с осторожностью")]
		public Image Image { get { return _image; } set { _image = value; } }

		public string ResourceName { get; set; }
		
		private Image _image;
	}
}