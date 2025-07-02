using MathCore.Libraries.MatrixCore;
using MathCore.Models;
using System;
using System.Data;
using System.Windows.Controls;

namespace Desktop.Services
{
    public static class MatrixService
    {
        public static MatrixModel ReadMatrixFromGrid(DataGrid grid)
        {
            var view = (DataView)grid.ItemsSource;
            var table = view.ToTable();
            int rows = table.Rows.Count;
            int cols = table.Columns.Count;

            var data = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    data[i, j] = Convert.ToDouble(table.Rows[i][j]);

            return new MatrixModel(data);
        }

        public static void LoadInitialMatrix(DataGrid grid, double[,]? initial = null)
        {
            if (initial == null)
            {
                initial = new double[,]
                {
                    { 0.5, 1.2, 1.0, 0.9 },
                    { 1.2, 2.0, 0.5, 1.2 },
                    { 1.0, 0.5, 1.0, 1.0 },
                    { 0.5, 1.2, 1.0, 2.2 }
                };
            }

            int rows = initial.GetLength(0);
            int cols = initial.GetLength(1);
            var table = new DataTable();

            for (int c = 0; c < cols; c++)
                table.Columns.Add($"C{c + 1}", typeof(double));

            for (int r = 0; r < rows; r++)
            {
                var row = table.NewRow();
                for (int c = 0; c < cols; c++)
                    row[c] = initial[r, c];
                table.Rows.Add(row);
            }

            grid.ItemsSource = table.DefaultView;
        }

        public static void UpdateMatrixGrid(MatrixModel model, DataGrid grid)
        {
            int rows = model.Rows;
            int cols = model.Columns;
            var table = new DataTable();

            for (int c = 0; c < cols; c++)
                table.Columns.Add($"C{c + 1}", typeof(double));

            for (int r = 0; r < rows; r++)
            {
                var row = table.NewRow();
                for (int c = 0; c < cols; c++)
                    row[c] = model.Data[r, c];
                table.Rows.Add(row);
            }

            grid.ItemsSource = table.DefaultView;
        }

    }
}
