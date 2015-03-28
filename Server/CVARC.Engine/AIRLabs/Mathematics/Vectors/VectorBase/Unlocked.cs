#region

using System;
using System.Collections.Generic;

#endregion

namespace AIRLab.Mathematics
{
    /// <summary>
    ///   A class of pseudo-unlocked vector. It allows to see to a LockedVector as to Vector, but setter always throws exception
    /// </summary>
    public class UnlockedVector : Vector
    {
        /// <summary>
        ///   Underlying locked vector
        /// </summary>
        private readonly LockedVector locked;

        /// <summary>
        ///   Creates unlocked vector around given locked vector
        /// </summary>
        public UnlockedVector(LockedVector locked)
        {
            this.locked = locked;
            SetElem = setElem;
            Elem = locked.Elem;
        }

        /// <summary>
        ///   Returns a length of vector
        /// </summary>
        public override int Count
        {
            get { return locked.Count; }
        }

        /// <inheritdoc />
        public override IEnumerator<double> GetEnumerator()
        {
            return locked.GetEnumerator();
        }

        private static void setElem(int index, double value)
        {
            throw new InvalidOperationException("Cannot set element in unlocked vector");
        }
    }
}