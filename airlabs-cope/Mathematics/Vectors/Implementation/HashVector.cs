// Authors: Yuri Okulovsky, Dmitry Kononchuk

using System;
using System.Collections;
using System.Collections.Generic;

namespace AIRLab.Mathematics
{
    /// <summary>
    ///   A vector, which is based on hash. One may set a default value (typically zero), and, if vector's element is equal to it, it is not actually stored.
    ///   This type of vector is to be used when there is a large zero-places in vectors
    /// </summary>
    [Serializable]
    public class HashVector : Vector
    {
        /// <summary>
        ///   A default value
        /// </summary>
        private readonly double def;

        /// <summary>
        ///   Hash of vector
        /// </summary>
        private readonly Dictionary<int, double> vector = new Dictionary<int, double>();

        /// <summary>
        ///   Creates a hash vector with default value def
        /// </summary>
        public HashVector(double def)
        {
            this.def = def;
            Elem = elem;
            SetElem = setElem;
        }

        /// <summary>
        ///   Creates a hash vector
        /// </summary>
        public HashVector()
            : this(0)
        {
        }

        #region Overriding

        /// <inheritdoc />
        public override int Count
        {
            get { return vector.Count; }
        }

        /// <inheritdoc />
        public override IEnumerator<double> GetEnumerator()
        {
            return new HashVectorEnumerator(vector.GetEnumerator());
        }

        private class HashVectorEnumerator : IEnumerator<double>
        {
            private readonly IEnumerator<KeyValuePair<int, double>> innerEnum;

            public HashVectorEnumerator(IEnumerator<KeyValuePair<int, double>> e)
            {
                innerEnum = e;
            }

            #region IEnumerator<double> Members

            public double Current
            {
                get { return innerEnum.Current.Value; }
            }

            public void Dispose()
            {
                innerEnum.Dispose();
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return innerEnum.MoveNext();
            }

            public void Reset()
            {
                innerEnum.Reset();
            }

            #endregion
        }

        #endregion

        /// <summary>
        ///   Local realisation of Elem delegate
        /// </summary>
        private double elem(int index)
        {
            double res;
            return vector.TryGetValue(index, out res) ? res : def;
        }

        /// <summary>
        ///   Local realisation of SetElem delegate
        /// </summary>
        private void setElem(int index, double value)
        {
            if (Math.Abs(value - def) > Geometry.Epsilon)
                vector[index] = value;
            else
                vector.Remove(index);
        }
    }
}