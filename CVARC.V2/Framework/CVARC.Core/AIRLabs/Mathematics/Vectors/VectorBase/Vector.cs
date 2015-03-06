// Authors: Yuri Okulovsky, Dmitry Kononchuk

using System;

namespace AIRLab.Mathematics
{
    /// <summary>
    ///   A class for a read/write vector.
    /// </summary>
    [Serializable]
    public abstract class Vector : ReadOnlyVector
    {
        /// <inheritdoc />
        public override bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        ///   Inverse vector
        /// </summary>
        public void InverseSelf()
        {
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, -Elem(i));
        }

        /// <summary>
        ///   Copies a components from given vector to this
        /// </summary>
        public void CopyFrom(ReadOnlyVector original)
        {
            CheckLengths(this, original);
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, original.Elem(i));
        }

        /// <summary>
        ///   Sets all vectors components equal to given constant
        /// </summary>
        public void SetConstant(double constant)
        {
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, constant);
        }

        /// <inheritdoc />
        public override void Clear()
        {
            SetConstant(0);
        }

        #region Standart arithmetical Operations (Generated by TH)

        ///<summary>
        ///  Adds an argument to vector
        ///</summary>
        ///<exception cref="Exception">Throw exception if vector's lengthes are not equal</exception>
        public void AddTo(ReadOnlyVector arg)
        {
            CheckLengths(this, arg);
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, Elem(i) + arg.Elem(i));
        }

        ///<summary>
        ///  Subtract an argument from vector
        ///</summary>
        ///<exception cref="Exception">Throw exception if vector's lengthes are not equal</exception>
        public void SubtractTo(ReadOnlyVector arg)
        {
            CheckLengths(this, arg);
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, Elem(i) - arg.Elem(i));
        }

        ///<summary>
        ///  Multiply vector to argument componentwise
        ///</summary>
        ///<exception cref="Exception">Throw exception if vector's lengthes are not equal</exception>
        public void MultiplyComponentwiseTo(ReadOnlyVector arg)
        {
            CheckLengths(this, arg);
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, Elem(i)*arg.Elem(i));
        }

        ///<summary>
        ///  Divide vector to argument componentwise
        ///</summary>
        ///<exception cref="Exception">Throw exception if vector's lengthes are not equal</exception>
        ///<exception cref="Exception">Throws exception if divisor is zero</exception>
        public void DivideComponentwiseTo(ReadOnlyVector arg)
        {
            CheckLengths(this, arg);
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, Elem(i)/arg.Elem(i));
        }

        ///<summary>
        ///  Multiplies vector to scalar argument
        ///</summary>
        public void MultiplyTo(double arg)
        {
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, Elem(i)*arg);
        }

        ///<summary>
        ///  Divides vector to scalar argument
        ///</summary>
        public void DivideTo(double arg)
        {
            for (int i = Count - 1; i >= 0; --i)
                SetElem(i, Elem(i)/arg);
        }

        #endregion
    }
}