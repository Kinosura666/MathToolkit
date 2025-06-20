﻿using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Runtime.CompilerServices;
using System.Data;

namespace MathCore.Extentions
{
    public static class MatrixExtensions
    {
        public static Matrix<double> ToMathNet(this Models.Matrix matrix)
        {
            return DenseMatrix.OfArray(matrix.Data);
        }

        public static Models.Matrix ToCore(this Matrix<double> matrix)
        {
            return new Models.Matrix(matrix.ToArray());
        }

        public static DataTable ToDataTable(this MathCore.Models.Matrix matrix)
        {
            var table = new DataTable();
            for (int j = 0; j < matrix.Columns; j++)
                table.Columns.Add($"C{j + 1}", typeof(double));

            for (int i = 0; i < matrix.Rows; i++)
            {
                var row = table.NewRow();
                for (int j = 0; j < matrix.Columns; j++)
                    row[j] = matrix.Data[i, j];
                table.Rows.Add(row);
            }

            return table;
        }

        public static string ToFormattedString(this MathCore.Models.Matrix matrix)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                    sb.Append($"{matrix.Data[i, j],8:F5}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

    }
}
