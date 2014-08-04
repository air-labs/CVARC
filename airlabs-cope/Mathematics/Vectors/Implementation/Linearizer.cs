// Authors: Yuri Okulovsky, Dmitry Kononchuk

using System;
using System.Collections;
using System.Collections.Generic;

namespace AIRLab.Mathematics
{
    /// <summary>
    ///   This class is used to concatenate vectors, and then indexing this concatenation as a signle vector
    /// </summary>
    [Serializable]
    public class Linearizer : Vector
    {
        #region Creation

        /// <summary>
        ///   By index in concatenated vector, gives an index of vector in set
        /// </summary>
        private readonly List<int> indexes = new List<int>();

        /// <summary>
        ///   By index in concatenated vector, gives an index of component in original vector
        /// </summary>
        private readonly List<int> offsets = new List<int>();

        /// <summary>
        ///   Set of vectors
        /// </summary>
        private readonly List<Vector> vectors = new List<Vector>();

        /// <summary>
        ///   Constructor
        /// </summary>
        public Linearizer()
        {
            Elem = elem;
            SetElem = setElem;
        }

        /// <summary>
        ///   Adds a vector to set
        /// </summary>
        public void PutIntoCollection(Vector v)
        {
            var L = v.Count;
            for (var i = 0; i < L; ++i)
            {
                indexes.Add(vectors.Count);
                offsets.Add(i);
            }
            vectors.Add(v);
        }

        /// <summary>
        ///   Adds a linearizer vector into set.
        ///   If vector is not linearizer, it will be added as in <see cref="PutIntoCollection" />
        ///   If argument V is linearizer, all vectors from it will be added. It increases performance, because all actions will be done directly, without using V.
        /// </summary>
        /// <param name="v"> </param>
        public void PutLinearizerIntoCollection(Vector v)
        {
            if (!(v is Linearizer)) PutIntoCollection(v);
            else
            {
                var g = (Linearizer) v;
                foreach (var t in g.vectors)
                    PutIntoCollection(t);
            }
        }

        /// <summary>
        ///   Clears all vectors from Linearizeer
        /// </summary>
        public void ClearCollection()
        {
            indexes.Clear();
            offsets.Clear();
            vectors.Clear();
        }

        /// <summary>
        ///   Local realisation of Elem delegate
        /// </summary>
        private double elem(int index)
        {
            int ind = indexes[index];
            int off = offsets[index];
            return (vectors[ind]).Elem(off);
        }

        /// <summary>
        ///   Local realisation of SetElem delegate
        /// </summary>
        private void setElem(int index, double value)
        {
            var ind = indexes[index];
            var off = offsets[index];
            (vectors[ind]).SetElem(off, value);
        }

        #endregion

        #region Implementation of vector's interface

        /// <inheritdoc />
        public override int Count
        {
            get { return indexes.Count; }
        }

        /// <inheritdoc />
        public override IEnumerator<double> GetEnumerator()
        {
            return new LinearizerEnumerator(this);
        }

        private struct LinearizerEnumerator : IEnumerator<double>
        {
            private readonly Linearizer owner;
            private int index;

            public LinearizerEnumerator(Linearizer l)
            {
                owner = l;
                index = 0;
            }

            #region IEnumerator<double> Members

            public double Current
            {
                get { return owner[index]; }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (index < owner.Count - 1)
                {
                    ++index;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                index = 0;
            }

            #endregion
        }

        #endregion
    }
}