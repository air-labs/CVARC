namespace AIRLab.Mathematics
{
    public class Mechanics
    {
        /// <summary>
        ///   Получить путь материальной точки
        /// </summary>
        /// <param name="startVelocity"> Начальная скорость </param>
        /// <param name="acceleration"> Ускорение </param>
        /// <param name="time"> Время </param>
        /// <returns> Путь </returns>
        public static double GetDiffPathBySTandA(double startVelocity, double acceleration, double time)
        {
            return startVelocity*time + acceleration*time*time/2;
        }

        /// <summary>
        ///   Получить путь материальной точки
        /// </summary>
        /// <param name="startVelocity"> Начальная скорость </param>
        /// <param name="finishVelocity"> Конечная скорость </param>
        /// <param name="time"> Время </param>
        /// <returns> Путь </returns>
        public static double GetDiffPathBySTandSF(double startVelocity, double finishVelocity, double time)
        {
            return (startVelocity + finishVelocity)*time/2;
        }

        /// <summary>
        ///   Получить начальную скорость
        /// </summary>
        /// <param name="path"> Пройденное расстояние </param>
        /// <param name="finishVelocity"> Конечная скорость </param>
        /// <param name="time"> Время </param>
        /// <returns> Начальная скорость </returns>
        public static double GetStartSpeedByPathAndSF(double path, double finishVelocity, double time)
        {
            return 2*path/time - finishVelocity;
        }

        /// <summary>
        ///   Получить конечную скорость
        /// </summary>
        /// <param name="path"> Пройденное расстояние </param>
        /// <param name="startVelocity"> Начальная скорость </param>
        /// <param name="time"> Время </param>
        /// <returns> Конечная скорость </returns>
        public static double GetFinishSpeedByPathAndST(double path, double startVelocity, double time)
        {
            return 2*path/time - startVelocity;
        }

        /// <summary>
        ///   Получить конечную скорость
        /// </summary>
        /// <param name="startVelocity"> Начальная скорость </param>
        /// <param name="acceleration"> Ускорение </param>
        /// <param name="time"> Время </param>
        /// <returns> Конечная скорость </returns>
        public static double GetFinishSpeedBySTAndAc(double startVelocity, double acceleration, double time)
        {
            return startVelocity + acceleration*time;
        }

        /// <summary>
        ///   Получить ускорение материальной точки
        /// </summary>
        /// <param name="startVelocity"> Начальная скорость точки </param>
        /// <param name="finishVelocity"> Конечная скорость точки </param>
        /// <param name="time"> Время </param>
        /// <returns> Ускорение </returns>
        public static double GetAcceleration(double startVelocity, double finishVelocity, double time)
        {
            return (finishVelocity - startVelocity)/time;
        }

        public static double LinerySpeedToAngleSpeed(double speed, double r)
        {
            return speed/r;
        }

        public static double AngleSpeedToLinerySpeed(double w, double r)
        {
            return w*r;
        }
    }
}