using System;
using System.Windows.Input;
using MathCore.Models;
using MathCore.Extentions;
using Desktop.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MathCore.Libraries.MatrixCore.DefaultLogic;

namespace Desktop.ViewModels
{
    public class MatrixViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Func<MatrixModel>? GetMatrixA { get; set; }
        public Func<MatrixModel>? GetMatrixB { get; set; }
        public Func<string>? GetMatrixTag { get; set; }
        public Func<int>? GetMatrixPower { get; set; }

        public Action<string>? SetResultText { get; set; }
        public Action<MatrixModel, string>? SetMatrixGrid { get; set; }

        public ICommand AddMatricesCommand { get; }
        public ICommand SubtractMatricesCommand { get; }
        public ICommand MultiplyMatricesCommand { get; }
        public ICommand TransposeMatrixCommand { get; }
        public ICommand DeterminantMatrixCommand { get; }
        public ICommand InverseMatrixCommand { get; }
        public ICommand PowerMatrixCommand { get; }
        public ICommand ClearMatrixCommand { get; }
        public ICommand ResizeMatrixCommand { get; }

        public MatrixViewModel()
        {
            AddMatricesCommand = new RelayCommand(_ => Add());
            SubtractMatricesCommand = new RelayCommand(_ => Subtract());
            MultiplyMatricesCommand = new RelayCommand(_ => Multiply());
            TransposeMatrixCommand = new RelayCommand(tag => Transpose(tag));
            DeterminantMatrixCommand = new RelayCommand(tag => Determinant(tag));
            InverseMatrixCommand = new RelayCommand(tag => Inverse(tag));
            PowerMatrixCommand = new RelayCommand(tag => Power(tag));
            ClearMatrixCommand = new RelayCommand(tag => Clear(tag));
            ResizeMatrixCommand = new RelayCommand(param => Resize(param?.ToString()));
        }

        public void Add()
        {
            var A = GetMatrixA?.Invoke();
            var B = GetMatrixB?.Invoke();
            if (A == null || B == null)
            {
                SetResultText?.Invoke("Matrix read error.");
                return;
            }

            if (A.Rows != B.Rows || A.Columns != B.Columns)
            {
                SetResultText?.Invoke("Addition requires matrices of same size.");
                return;
            }

            var C = MatrixOperations.Add(A, B);
            SetResultText?.Invoke("A + B:\n" + C.ToFormattedString());
        }

        public void Subtract()
        {
            var A = GetMatrixA?.Invoke();
            var B = GetMatrixB?.Invoke();
            if (A == null || B == null)
            {
                SetResultText?.Invoke("Matrix read error.");
                return;
            }

            if (A.Rows != B.Rows || A.Columns != B.Columns)
            {
                SetResultText?.Invoke("Subtraction requires matrices of same size.");
                return;
            }

            var C = MatrixOperations.Subtract(A, B);
            SetResultText?.Invoke("A - B:\n" + C.ToFormattedString());
        }

        public void Multiply()
        {
            var A = GetMatrixA?.Invoke();
            var B = GetMatrixB?.Invoke();
            if (A == null || B == null)
            {
                SetResultText?.Invoke("Matrix read error.");
                return;
            }

            if (A.Columns != B.Rows)
            {
                SetResultText?.Invoke($"Multiplication not possible: A is {A.Rows}×{A.Columns}, B is {B.Rows}×{B.Columns}");
                return;
            }

            var C = MatrixOperations.Multiply(A, B);
            SetResultText?.Invoke("A × B:\n" + C.ToFormattedString());
        }

        public void Transpose(object? tagObj)
        {
            string tag = tagObj?.ToString() ?? GetMatrixTag?.Invoke();
            if (tag == null) return;

            var matrix = tag == "A" ? GetMatrixA?.Invoke() : GetMatrixB?.Invoke();
            if (matrix == null) return;

            var transposed = MatrixOperations.Transpose(matrix);
            SetResultText?.Invoke($"Transpose ({tag}):\n" + transposed.ToFormattedString());
        }

        public void Determinant(object? tagObj)
        {
            string tag = tagObj?.ToString() ?? GetMatrixTag?.Invoke();
            if (tag == null) return;

            var matrix = tag == "A" ? GetMatrixA?.Invoke() : GetMatrixB?.Invoke();
            var det = MatrixOperations.Determinant(matrix);
            SetResultText?.Invoke($"det({tag}) ≈ {det:F6}");
        }

        public void Inverse(object? tagObj)
        {
            string tag = tagObj?.ToString() ?? GetMatrixTag?.Invoke();
            if (tag == null) return;

            try
            {
                var matrix = tag == "A" ? GetMatrixA?.Invoke() : GetMatrixB?.Invoke();
                var inv = MatrixOperations.Inverse(matrix);
                SetResultText?.Invoke($"Inverse ({tag}):\n" + inv.ToFormattedString());
            }
            catch (Exception ex)
            {
                SetResultText?.Invoke($"Matrix {tag} is not invertible.\n{ex.Message}");
            }
        }

        public void Power(object? tagObj)
        {
            string tag = tagObj?.ToString() ?? GetMatrixTag?.Invoke();
            if (tag == null) return;

            try
            {
                var matrix = tag == "A" ? GetMatrixA?.Invoke() : GetMatrixB?.Invoke();
                int k = GetMatrixPower?.Invoke() ?? 1;
                var result = MatrixOperations.Power(matrix, k);
                SetResultText?.Invoke($"{tag}^{k} =\n" + result.ToFormattedString());
            }
            catch (Exception ex)
            {
                SetResultText?.Invoke($"Matrix power failed:\n{ex.Message}");
            }
        }

        public void Clear(object? tagObj)
        {
            string tag = tagObj?.ToString() ?? GetMatrixTag?.Invoke();
            if (tag == null)
            {
                SetResultText?.Invoke("Matrix tag not defined.");
                return;
            }

            var matrix = tag == "A" ? GetMatrixA?.Invoke() : GetMatrixB?.Invoke();
            if (matrix == null) return;

            var cleared = new MatrixModel(new double[matrix.Rows, matrix.Columns]);
            SetMatrixGrid?.Invoke(cleared, tag);
            SetResultText?.Invoke($"Matrix {tag} cleared.");
        }

        private void Resize(string? tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                SetResultText?.Invoke("Resize tag is missing.");
                return;
            }

            string matrixId = tag[^1..];
            int delta = tag.StartsWith("+") ? 1 : -1;

            var matrix = matrixId == "A" ? GetMatrixA?.Invoke() : GetMatrixB?.Invoke();
            if (matrix == null)
            {
                SetResultText?.Invoke($"Matrix {matrixId} not found.");
                return;
            }

            int oldRows = matrix.Rows;
            int oldCols = matrix.Columns;
            int newRows = Math.Max(1, oldRows + delta);
            int newCols = Math.Max(1, oldCols + delta);

            var data = new double[newRows, newCols];

            for (int i = 0; i < Math.Min(oldRows, newRows); i++)
                for (int j = 0; j < Math.Min(oldCols, newCols); j++)
                    data[i, j] = matrix.Data[i, j];

            var resizedMatrix = new MatrixModel(data);
            SetMatrixGrid?.Invoke(resizedMatrix, matrixId);
            SetResultText?.Invoke($"Matrix {matrixId} resized to {newRows}×{newCols}.");
        }
    }
}
