// Authors: Yuri Okulovsky, Dmitry Kononchuk

using System;
using System.Collections.Generic;
using AIRLab.Mathematics;

namespace AIRLab.Mathematics
{
    /// <summary>
    ///   A class of simple real vector. Its components have a double values.
    /// </summary>
    [Serializable]
    public class RealVector : Vector
    {
        /// <summary>
        ///   internal array of real numbers
        /// </summary>
        private readonly double[] vector;

        /// <summary>
        ///   Creates an array of real numbers with specified length
        /// </summary>
        public RealVector(int length)
        {
            vector = new double[length];
            Elem = elem;
            SetElem = setElem;
        }

        /// <summary>
        ///   Local realisation of Elem delegate
        /// </summary>
        private double elem(int index)
        {
            return vector[index];
        }

        /// <summary>
        ///   Local realisation of SetElem delegate
        /// </summary>
        private void setElem(int index, double value)
        {
            vector[index] = value;
        }

        /// <summary>
        ///   Creates a real vector from values.
        /// </summary>
        public static RealVector FromValues(params double[] values)
        {
            int i = values.Length;
            var v = new RealVector(i);
            values.CopyTo(v.vector, 0);
            return v;
        }

        #region Implementation of vector's interface

        /// <inheritdoc />
        public override int Count
        {
            get { return vector.Length; }
        }

        /// <inheritdoc />
        public override IEnumerator<double> GetEnumerator()
        {
            return new ArrayEnumerator<double>(vector);
        }

        #endregion
    }
}