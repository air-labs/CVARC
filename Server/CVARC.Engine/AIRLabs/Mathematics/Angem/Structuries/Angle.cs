using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace AIRLab.Mathematics
{
    /// <summary>
    ///   This class represents angle, which have radian and gradus forms. This class is immtable.
    /// </summary>
    [Serializable]
    public struct Angle
    {

        /// <summary>
        ///   Creates angle from radian
        /// </summary>
        private Angle(double grad)
        {
            Grad = grad;
        }

        /// <summary>
        ///   Gets gradii representation of angle
        /// </summary>
        public readonly double Grad;

        /// <summary>
        ///   Gets radian representation of angle
        /// </summary>
        public double Radian
        {
            get { return Grad*Math.PI/180; }
        }

        public static Angle Zero
        {
            get { return new Angle(0); }
        }

        public static Angle Pi
        {
            get { return new Angle(180); }
        }

        public static Angle HalfPi
        {
            get { return new Angle(90); }
        }

        /// <summary>
        ///   Make the angle inside [-180,180] interval
        /// </summary>
        public Angle Simplify180()
        {
            double r = Grad;
            while (r < -180) r += 360;
            while (r > 180) r -= 360;
            return new Angle(r);
        }

        /// <summary>
        ///   Make the angle inside [0,360] interval
        /// </summary>
        public Angle Simplify360()
        {
            double r = Grad;
            while (r < 0) r += 360;
            while (r > 360) r -= 360;
            return new Angle(r);
        }


        /// <summary>
        ///   Creates angle from gradii representation
        /// </summary>
        public static Angle FromGrad(double grad)
        {
            return new Angle(grad);
        }

        /// <summary>
        ///   Creates angle from radian representation
        /// </summary>
        public static Angle FromRad(double rad)
        {
            return new Angle(rad*180.0/Math.PI);
        }

        ///<inheritdoc />
        public override string ToString()
        {
            return Grad + "G";
        }

        public static Angle Abs(Angle angle)
        {
            return FromGrad(Math.Abs(angle.Grad));
        }

        #region Arithmetic

        public static Angle operator +(Angle v1, Angle v2)
        {
            return new Angle(v1.Grad + v2.Grad);
        }

        public static Angle operator -(Angle v1, Angle v2)
        {
            return new Angle(v1.Grad - v2.Grad);
        }

        public static Angle operator -(Angle v)
        {
            return new Angle(-v.Grad);
        }

        public static Angle operator *(Angle v1, double v2)
        {
            return new Angle(v1.Grad * v2);
        }

        public static Angle operator *(double v2, Angle v1)
        {
            return v1*v2;
        }

        public static Angle operator /(Angle v1, double v2)
        {
            return new Angle(v1.Grad / v2);
        }


        public static double operator /(Angle v1, Angle v2)
        {
            return v1.Grad/v2.Grad;
        }

        /// <summary>
        ///   Adds specified gradii to angle
        /// </summary>
        public Angle AddGrad(double grad)
        {
            return new Angle(Grad + grad);
        }

        /// <summary>
        ///   Adds specified radians to angle
        /// </summary>
        public Angle AddRad(double rad)
        {
            return new Angle(Grad)+Angle.FromRad(rad);
        }

        #endregion

        #region Comparison

        public static bool operator <(Angle a, Angle b)
        {
            return a.Grad < b.Grad;
        }

        public static bool operator >(Angle a, Angle b)
        {
            return a.Grad > b.Grad;
        }

        public static bool operator <=(Angle a, Angle b)
        {
            return a.Grad <= b.Grad;
        }

        public static bool operator >=(Angle a, Angle b)
        {
            return a.Grad >= b.Grad;
        }

        #endregion

		#region JsonConverter

	    public class Converter : JavaScriptConverter
		{
			public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
			{
				if (dictionary == null)
					throw new ArgumentNullException("dictionary");
				var angle = Angle.FromGrad(Convert.ToDouble(dictionary["Grad"]));
				return angle;
			}

			public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
			{
				var angle = (Angle)obj;
				var result = new Dictionary<string, object>();
				result["Grad"] = angle.Grad;

				return result;
			}

			public override IEnumerable<Type> SupportedTypes
			{
				get { return new[] { typeof(Angle) };}
			}
		}
		#endregion
	}
}