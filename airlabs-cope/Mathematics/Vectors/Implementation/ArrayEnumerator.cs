// Author: Dmitry Kononchuk

using System;
using System.Collections;
using System.Collections.Generic;

namespace AIRLab.Mathematics
{
    /// <summary>
    ///   Class realizing <see cref="IEnumerator{T}" /> for <see cref="Array" /> of <typeparamref name="T" />.
    /// </summary>
    public struct ArrayEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] array;
        private int i;

        /// <summary>
        ///   Constructor.
        /// </summary>
        public ArrayEnumerator(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException();
            this.array = array;
            i = -1;
        }

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            //    throw new NotSupportedException();
        }

        #endregion

        #region Implementation of IEnumerator

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (i < array.Length)
            {
                ++i;
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public void Reset()
        {
            i = -1;
        }

        /// <inheritdoc />
        public T Current
        {
            get
            {
                if (i < array.Length)
                    return array[i];
                return default(T);
            }
        }

        /// <inheritdoc />
        object IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion
    }
}