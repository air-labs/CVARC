using CVARC.Graphics;

namespace CVARC.Basic.Sensors
{
	/// <summary>
	/// 
	/// </summary>
	public interface ISensor<out T> where T : ISensorData
	{
	    void Init(Robot robot, World wrld, DrawerFactory factory);
		/// <summary>
		/// Метод производит измерение и возвращает данные, снятые с сенсора.
		/// </summary>
		/// <returns></returns>
		T Measure();
	}
}
