// Authors: Yuri Okulovsky, Dmitry Kononchuk

using System;
using System.Collections.Generic;

namespace AIRLab.Mathematics
{
    /// <summary>
    ///   Represents a class of "Locked" vector. It is a non-abstract class, which wrap some vector and allow only getters to its elements, not setters. Use this class to be sure that components of vector will not be changed.
    /// </summary>
    [Serializable]
    public class LockedVector : ReadOnlyVector
    {
        /// <summary>
        ///   Underlying vector's object
        /// </summary>
        private readonly ReadOnlyVector vector;

        /// <summary>
        ///   Creates an instance of LockedVector over given vector
        /// </summary>
        public LockedVector(ReadOnlyVector vector)
        {
            this.vector = vector;
            Elem = vector.Elem;
        }

        /// <summary>
        ///   Returs a vector's length
        /// </summary>
        public override int Count
        {
            get { return vector.Count; }
        }

        /// <summary>
        ///   Implicity conversion vector's object to locked object
        /// </summary>
        public static implicit operator LockedVector(Vector vector)
        {
            return new LockedVector(vector);
        }

        /// <summary>
        ///   Returns pseudo-unlocked vector
        /// </summary>
        public UnlockedVector Unlock()
        {
            return new UnlockedVector(this);
        }

        /// <inheritdoc />
        public override IEnumerator<double> GetEnumerator()
        {
            return vector.GetEnumerator();
        }
    }
}