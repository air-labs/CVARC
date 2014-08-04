using System;
using System.Globalization;

namespace AIRLab.Mathematics
{
    [Serializable]
    public class Matrix : ICloneable
    {
        public readonly int ColumnCount;
        public readonly int RowCount;
        private readonly double[,] matrix;

        public double this[int row, int column]
        {
            get { return matrix[row, column]; }
            set { matrix[row, column] = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            var newMatrix = new Matrix(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }
            return newMatrix;
        }

        #endregion

        public void Print()
        {
            Console.WriteLine();
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                    Console.Write(matrix[i, j].ToString(CultureInfo.InvariantCulture) + "\t");
                Console.WriteLine();
            }
        }

        public override String ToString()
        {
            String str = "";
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                    str += (matrix[i, j].ToString(CultureInfo.InvariantCulture) + "\t");
                str += "\n";
            }
            return str;
        }

        /// <summary>
        ///   translate matrix to row echelon(переводит матрицу в ступенчатый вид)
        /// </summary>
        /// <returns> returns matrix and amount of swappings </returns>
        public Tuple<Matrix, int> ToRowEchelon()
        {
            int swapings = 0;
            var matrix1 = new double[RowCount][];
            for (int i = 0; i < RowCount; i++)
            {
                matrix1[i] = new double[ColumnCount];
                for (int j = 0; j < ColumnCount; j++)
                {
                    matrix1[i][j] = this[i, j];
                }
            }

            int activeColumn = 0;

            for (int buildedRows = 0; buildedRows < RowCount && activeColumn < ColumnCount; buildedRows++)
            {
                int lastElement = -1;
                for (; activeColumn < ColumnCount && lastElement < 0; activeColumn++)
                {
                    for (int row = buildedRows; row < RowCount && lastElement < 0; row++)
                    {
                        if (Math.Abs(matrix1[row][activeColumn]) > double.Epsilon)
                        {
                            lastElement = row;
                        }
                    }
                    if (lastElement >= 0)
                    {
                        if (buildedRows != lastElement)
                        {
                            swapings++;
                            double[] swapElement = matrix1[buildedRows];
                            matrix1[buildedRows] = matrix1[lastElement];
                            matrix1[lastElement] = swapElement;
                        }

                        for (int i = buildedRows + 1; i < RowCount; i++)
                        {
                            if (Math.Abs(matrix1[i][activeColumn]) > double.Epsilon)
                            {
                                double koef = matrix1[i][activeColumn]/matrix1[buildedRows][activeColumn];
                                for (int j = activeColumn; j < ColumnCount; j++)
                                {
                                    matrix1[i][j] -= matrix1[buildedRows][j]*koef;
                                }
                            }
                        }
                    }
                }
            }

            var newMatrix = new Matrix(RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    newMatrix[i, j] = matrix1[i][j];
                }
            }
            return new Tuple<Matrix, int>(newMatrix, swapings);
        }

        public double GetDeterminant()
        {
            if (ColumnCount != RowCount)
                throw new Exception("Матрица должна быть квадратной.");
            Tuple<Matrix, int> triangleMatrix = ToRowEchelon();

            double determinant = ((triangleMatrix.Item2 & 1) == 1 ? -1 : 1);
            for (int i = 0; i < RowCount; i++)
            {
                determinant *= triangleMatrix.Item1[i, i];
            }
            return determinant;
        }

        public double Minor(int x, int y, int size)
        {
            if (x + size > ColumnCount || y + size > RowCount || y < 0 || x < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var newMatrix = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    newMatrix[i, j] = this[x + i, y + j];
                }
            }
            return newMatrix.GetDeterminant();
        }

        #region constructors

        public Matrix(double[,] matrix)
        {
            RowCount = matrix.GetUpperBound(0) + 1;
            ColumnCount = matrix.GetUpperBound(1) + 1;
            this.matrix = matrix;
        }

        public Matrix(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            matrix = new double[rowCount,columnCount];
        }

        public static Matrix IdentityMatrix(int width)
        {
            var matrix = new Matrix(width, width);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < width; j++)
                    if (i == j)
                        matrix[i, j] = 1;
                    else
                        matrix[i, j] = 0;
            return matrix;
        }

        #endregion

        #region operations

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount)
                throw new ArgumentException("Sizes of matrices are not equal");
            var c = new Matrix(a.RowCount, a.ColumnCount);
            for (int row = 0; row < c.RowCount; row++)
                for (int column = 0; column < c.ColumnCount; column++)
                    c[row, column] = a[row, column] + b[row, column];
            return c;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount)
                throw new ArgumentException("Sizes of matrices are not equal");
            var c = new Matrix(a.RowCount, a.ColumnCount);
            for (int row = 0; row < c.RowCount; row++)
                for (int column = 0; column < c.ColumnCount; column++)
                    c[row, column] = a[row, column] - b[row, column];
            return c;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.ColumnCount != b.RowCount)
                throw new ArgumentException("Matrices are not fit to multiply");
            var c = new Matrix(a.RowCount, b.ColumnCount);
            for (int row = 0; row < c.RowCount; row++)
                for (int column = 0; column < c.ColumnCount; column++)
                    for (int k = 0; k < a.ColumnCount; k++)
                        c[row, column] += a[row, k]*b[k, column];
            return c;
        }

        public static Matrix operator /(Matrix matrix, double value)
        {
            var newMatrix = new Matrix(matrix.RowCount, matrix.ColumnCount);
            for (int row = 0; row < matrix.RowCount; row++)
                for (int column = 0; column < matrix.ColumnCount; column++)
                    newMatrix[row, column] = matrix[row, column]/value;
            return newMatrix;
        }

        public static Matrix operator *(Matrix matrix, double value)
        {
            var newMatrix = new Matrix(matrix.RowCount, matrix.ColumnCount);
            for (int row = 0; row < matrix.RowCount; row++)
                for (int column = 0; column < matrix.ColumnCount; column++)
                    newMatrix[row, column] = matrix[row, column]*value;
            return newMatrix;
        }

        public static Matrix operator *(double value, Matrix matrix)
        {
            var newMatrix = new Matrix(matrix.RowCount, matrix.ColumnCount);
            for (int row = 0; row < matrix.RowCount; row++)
                for (int column = 0; column < matrix.ColumnCount; column++)
                    newMatrix[row, column] = matrix[row, column]*value;
            return newMatrix;
        }

        #endregion
    }
}