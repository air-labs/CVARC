namespace CVARC.Graphics
{
	/// <summary>
	/// Камера - класс, который возвращает три Transform,
	/// необходимых, чтобы определить точку обзора
	/// </summary>
	/// <typeparam name="T">Тип Transform</typeparam>
	public interface ICamera<out T>
	{
		T ViewTransform { get; }
		T WorldTransform { get; }
		T ProjectionTransform { get; }

		/// <summary>
		/// Соотношение сторон экрана
		/// </summary>
		double AspectRatio { get; set; }
	}
}