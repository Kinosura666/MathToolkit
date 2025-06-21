using System;
using System.Linq;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using MathCore.Extentions;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MathCore.Libraries.MatrixCore;
using MathCore.Models;

namespace Desktop
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadInitialMatrix(MatrixGridA);
            LoadInitialMatrix(MatrixGridB);
        }

        private void LoadInitialMatrix(System.Windows.Controls.DataGrid grid)
        {
            var initial = new double[,]
            {
                { 0.5, 1.2, 1.0, 0.9 },
                { 1.2, 2.0, 0.5, 1.2 },
                { 1.0, 0.5, 1.0, 1.0 },
                { 0.5, 1.2, 1.0, 2.2 }
            };

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
        private void MethodTree_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in MethodTree.Items)
            {
                if (item is TreeViewItem treeItem)
                    treeItem.IsExpanded = true;
            }
        }

        private void OnExecuteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MethodTree.SelectedItem is TreeViewItem selectedItem && selectedItem.Parent is TreeViewItem)
                {
                    string method = selectedItem.Header.ToString();
                    var A = ReadMatrixFromGrid(MatrixGridA);

                    switch (method)
                    {
                        case "Power Iteration":
                            {
                                var result = MatrixEigen.PowerIteration(A);

                                ResultText.Text =
                                    $"Power Iteration\n" +
                                    $"λ ≈ {result.Eigenvalue:F6}\n" +
                                    $"Iterations: {result.Iterations}\n" +
                                    $"Converged: {(result.Converged ? "Yes" : "No")}\n\n" +
                                    "Eigenvector:\n" +
                                    string.Join("\n", result.Eigenvector.Select(x => $"{x:F5}"));

                                break;
                            }

                        case "Inverse Iteration":
                            {
                                var result = MatrixEigen.InversePowerIteration(A);

                                ResultText.Text =
                                    $"Inverse Iteration\n" +
                                    $"λ ≈ {result.Eigenvalue:F6}\n" +
                                    $"Iterations: {result.Iterations}\n" +
                                    $"Converged: {(result.Converged ? "Yes" : "No")}\n\n" +
                                    "Eigenvector:\n" +
                                    string.Join("\n", result.Eigenvector.Select(x => $"{x:F5}"));

                                break;
                            }

                        case "Rayleigh Quotient Iteration":
                            {
                                var result = MatrixEigen.RayleighQuotientIteration(A);

                                ResultText.Text =
                                    $"Rayleigh Quotient Iteration\n" +
                                    $"λ ≈ {result.Eigenvalue:F6}\n" +
                                    $"Iterations: {result.Iterations}\n" +
                                    $"Converged: {(result.Converged ? "Yes" : "No")}\n\n" +
                                    "Eigenvector:\n" +
                                    string.Join("\n", result.Eigenvector.Select(x => $"{x:F5}"));

                                break;
                            }

                        case "Jacobi Method":
                            {
                                var result = MatrixEigen.JacobiEigenSolver(A);

                                ResultText.Text = "Jacobi Method\n";
                                ResultText.Text += $"Iterations: {result.Iterations}\n";
                                ResultText.Text += $"Converged: {(result.Converged ? "Yes" : "No")}\n\n";
                                ResultText.Text += "Eigenvalues and corresponding eigenvectors:\n";

                                for (int i = 0; i < result.Eigenvalues.Length; i++)
                                {
                                    ResultText.Text += $"λ{i + 1} ≈ {result.Eigenvalues[i]:F6}\n";
                                    ResultText.Text += "v = (";

                                    for (int j = 0; j < result.Eigenvectors.Length; j++)
                                    {
                                        ResultText.Text += $"{result.Eigenvectors[j][i]:F5}";
                                        if (j < result.Eigenvectors.Length - 1)
                                            ResultText.Text += "; ";
                                    }

                                    ResultText.Text += ")\n";
                                }

                                break;
                            }

                        case "QR Method":
                            {
                                var result = MatrixEigen.QREigenValues(A);

                                ResultText.Text = "QR Method\n";
                                ResultText.Text += $"Iterations: {result.Iterations}\n";
                                ResultText.Text += $"Converged: {(result.Converged ? "Yes" : "No")}\n\n";
                                ResultText.Text += "Eigenvalues:\n";

                                for (int i = 0; i < result.Eigenvalues.Length; i++)
                                {
                                    ResultText.Text += $"λ{i + 1} ≈ {result.Eigenvalues[i]:F6}\n";
                                }

                                break;
                            }

                        case "LR Method":
                            {
                                var result = MatrixEigen.LREigenValues(A);
                                ResultText.Text = "LR Method\n";
                                ResultText.Text += $"Iterations: {result.Iterations}\n";
                                ResultText.Text += $"Converged: {(result.Converged ? "Yes" : "No")}\n\n";
                                ResultText.Text += "Eigenvalues:\n";

                                for (int i = 0; i < result.Eigenvalues.Length; i++)
                                    ResultText.Text += $"λ{i + 1} ≈ {result.Eigenvalues[i]:F6}\n";

                                break;
                            }

                        case "Leverrier-Faddeev":
                            {
                                var result = MatrixPolynomial.LeverrierFaddeev(A);
                                var coeffs = result.Coefficients;
                                int degree = result.Degree;

                                ResultText.Text = "Leverrier-Faddeev\nCharacteristic polynomial:\n";

                                for (int i = 0; i < coeffs.Length; i++)
                                {
                                    int power = degree - i;
                                    ResultText.Text += $"a{power} = {coeffs[i]:F6}\n";
                                }

                                ResultText.Text += "\np(λ) = ";
                                for (int i = 0; i < coeffs.Length; i++)
                                {
                                    int power = degree - i;
                                    double coeff = coeffs[i];
                                    string sign = (coeff >= 0 && i > 0) ? " + " : (i > 0 ? " - " : "");
                                    ResultText.Text += $"{sign}{Math.Abs(coeff):F4}";

                                    if (power > 1)
                                        ResultText.Text += $"λ^{power}";
                                    else if (power == 1)
                                        ResultText.Text += "λ";
                                }

                                ResultText.Text += "\n";
                                break;
                            }

                        case "Krylov Method":
                            {
                                var result = MatrixPolynomial.KrylovCharacteristicPolynomial(A);
                                var coeffs = result.Coefficients;
                                int degree = result.Degree;

                                ResultText.Text = "Krylov\nCharacteristic polynomial:\n";

                                for (int i = 0; i < coeffs.Length; i++)
                                {
                                    int power = degree - i;
                                    ResultText.Text += $"a{power} = {coeffs[i]:F6}\n";
                                }

                                ResultText.Text += "\np(λ) = ";
                                for (int i = 0; i < coeffs.Length; i++)
                                {
                                    int power = degree - i;
                                    double coeff = coeffs[i];
                                    string sign = (coeff >= 0 && i > 0) ? " + " : (i > 0 ? " - " : "");
                                    ResultText.Text += $"{sign}{Math.Abs(coeff):F4}";

                                    if (power > 1)
                                        ResultText.Text += $"λ^{power}";
                                    else if (power == 1)
                                        ResultText.Text += "λ";
                                }

                                ResultText.Text += "\n";
                                break;
                            }

                        case "LU Decomposition":
                            {
                                try
                                {
                                    var (L, U) = MatrixDecompositions.LUDecomposition(A);

                                    ResultText.Text = "LU Decomposition:\n\n";
                                    ResultText.Text += "L (Lower Triangular):\n";
                                    ResultText.Text += L.ToFormattedString();

                                    ResultText.Text += "\nU (Upper Triangular):\n";
                                    ResultText.Text += U.ToFormattedString();
                                }
                                catch (Exception ex)
                                {
                                    ResultText.Text = $"LU decomposition failed: {ex.Message}";
                                }
                                break;
                            }

                        case "QR Decomposition":
                            {
                                try
                                {
                                    var (Q, R) = MatrixDecompositions.QRDecomposition(A);

                                    ResultText.Text = "QR Decomposition:\n\n";
                                    ResultText.Text += "Q (Orthogonal):\n";
                                    ResultText.Text += Q.ToFormattedString();

                                    ResultText.Text += "\nR (Upper Triangular):\n";
                                    ResultText.Text += R.ToFormattedString();
                                }
                                catch (Exception ex)
                                {
                                    ResultText.Text = $"QR decomposition failed: {ex.Message}";
                                }
                                break;
                            }

                        case "Cholesky Decomposition":
                            {
                                try
                                {
                                    var (L, LT) = MatrixDecompositions.CholeskyDecomposition(A);

                                    ResultText.Text = "Cholesky Decomposition:\n\n";
                                    ResultText.Text += "L (Lower Triangular):\n";
                                    ResultText.Text += L.ToFormattedString();

                                    ResultText.Text += "\nL^T (Transposed):\n";
                                    ResultText.Text += LT.ToFormattedString();
                                }
                                catch (Exception ex)
                                {
                                    ResultText.Text = $"Cholesky decomposition failed: {ex.Message}";
                                }
                                break;
                            }

                        case "SVD Decomposition":
                            {
                                var (U, S, VT) = MatrixDecompositions.SVD(A);

                                ResultText.Text = "SVD Decomposition\n";

                                ResultText.Text += "\nSingular values (diagonal of Σ):\n";
                                var singularValues = MatrixStats.GetSingularValues(A);
                                for (int i = 0; i < singularValues.Length; i++)
                                    ResultText.Text += $"σ{i + 1} ≈ {singularValues[i]:F6}\n";

                                ResultText.Text += "\nMatrix U:\n";
                                ResultText.Text += U.ToFormattedString();

                                ResultText.Text += "\nMatrix Σ:\n";
                                ResultText.Text += S.ToFormattedString();

                                ResultText.Text += "\nMatrix V^T:\n";
                                ResultText.Text += VT.ToFormattedString();

                                break;
                            }

                        case "Matrix Norms":
                            {
                                var frob = MatrixStats.FrobeniusNorm(A);
                                var norm1 = MatrixStats.OneNorm(A);
                                var normInf = MatrixStats.InfinityNorm(A);
                                var norm2 = MatrixStats.TwoNorm(A);

                                ResultText.Text = "Matrix norms:\n\n" +
                                                  $"‣ Frobenius (Euclid) norm ||A||_F  ≈ {frob:F5}\n" +
                                                  $"‣ 1-norm ||A||_1  ≈ {norm1:F5}\n" +
                                                  $"‣ ∞-norm ||A||_∞  ≈ {normInf:F5}\n" +
                                                  $"‣ 2-norm (spectral norm) ||A||_2  ≈ {norm2:F5}\n\n";
                                break;
                            }

                        case "Condition Number":
                            {
                                var cond = MatrixStats.ConditionNumber2(A);
                                ResultText.Text = $"Condition number \ncond₂(A) ≈ {cond:F5}";
                                break;
                            }

                        case "Gershgorin Discs":
                            {
                                var result = MatrixGershgorin.GershgorinDiscs(A);

                                ResultText.Text = "Gershgorin discs (center ± radius):\n\n";

                                for (int i = 0; i < result.Discs.Length; i++)
                                {
                                    var disc = result.Discs[i];
                                    ResultText.Text += $"D{i + 1}: {disc.Center:F5} ± {disc.Radius:F5}\n";
                                }

                                ResultText.Text += $"\nApproximate eigenvalue range: [{result.MinBound:F5}; {result.MaxBound:F5}]\n";
                                break;
                            }

                        case "Singular Values":
                            {
                                var singularValues = MatrixStats.GetSingularValues(A);

                                ResultText.Text = "Singular Values:\n";
                                for (int i = 0; i < singularValues.Length; i++)
                                    ResultText.Text += $"σ{i + 1} ≈ {singularValues[i]:F6}\n";
                                
                                break;
                            }

                        case "Pseudo-Inverse":
                            {
                                var pseudo = MatrixOperations.PseudoInverse(A);
                                ResultText.Text = $"Pseudo-Inverse:\n" + pseudo.ToFormattedString();
                                break;
                            }

                        case "Determinant":
                            {
                                var det = MatrixOperations.Determinant(A);
                                ResultText.Text = "Determinant\n";
                                ResultText.Text += $"det(A) ≈ {det:F6}";
                                break;
                            }

                        case "Inverse Matrix":
                            {
                                try
                                {
                                    var inv = MatrixOperations.Inverse(A);
                                    ResultText.Text = "Inverse Matrix\n";
                                    ResultText.Text += "A⁻¹ =\n" + inv.ToFormattedString();
                                }
                                catch (Exception ex)
                                {
                                    ResultText.Text = "Inverse Matrix\nMatrix is not invertible.\n" + ex.Message;
                                }
                                break;
                            }

                        case "Transpose Matrix":
                            {
                                var T = MatrixOperations.Transpose(A);
                                ResultText.Text = "Transpose Matrix\n";
                                ResultText.Text += "Aᵗ =\n" + T.ToFormattedString();
                                break;
                            }

                        case "Symmetrize Matrix":
                            {
                                try
                                {
                                    var sym = MatrixOperations.Symmetrize(A);
                                    ResultText.Text = "Symmetrized Matrix\n";
                                    ResultText.Text += "A_sym = (A + Aᵗ) / 2 =\n" + sym.ToFormattedString();
                                }
                                catch (Exception ex)
                                {
                                    ResultText.Text = "Symmetrization failed:\n" + ex.Message;
                                }
                                break;
                            }

                        case "Matrix Rank":
                            {
                                try
                                {
                                    int rank = MatrixOperations.Rank(A);
                                    ResultText.Text = "Matrix Rank\n";
                                    ResultText.Text += $"rank(A) = {rank}";
                                }
                                catch (Exception ex)
                                {
                                    ResultText.Text = "Rank calculation failed:\n" + ex.Message;
                                }
                                break;
                            }

                        default:
                            ResultText.Text = $"Method \"{method}\" not implemented.";
                            break;
                    }
                }
                else
                {
                    ResultText.Text = "Please choose a specific method.";
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Error: {ex.Message}";
            }
        }

        private MatrixModel ReadMatrixFromGrid(System.Windows.Controls.DataGrid grid)
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

        private void AdjustMatrixSize(System.Windows.Controls.DataGrid grid, int deltaRows, int deltaCols)
        {
            var view = (DataView)grid.ItemsSource;
            var table = view.ToTable();
            int rows = table.Rows.Count;
            int cols = table.Columns.Count;

            int newRows = Math.Max(1, rows + deltaRows);
            int newCols = Math.Max(1, cols + deltaCols);

            var newTable = new DataTable();

            for (int c = 0; c < newCols; c++)
                newTable.Columns.Add($"C{c + 1}", typeof(double));

            for (int r = 0; r < newRows; r++)
            {
                var row = newTable.NewRow();
                for (int c = 0; c < newCols; c++)
                {
                    double value = 0;
                    if (r < rows && c < cols)
                        value = Convert.ToDouble(table.Rows[r][c]);
                    row[c] = value;
                }
                newTable.Rows.Add(row);
            }

            grid.ItemsSource = newTable.DefaultView;
        }

        private void OnResizeClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var grid = tag.EndsWith("A") ? MatrixGridA : MatrixGridB;
                var delta = tag.StartsWith("+") ? 1 : -1;
                AdjustMatrixSize(grid, delta, delta);
            }
        }

        private void OnAddMatricesClick(object sender, RoutedEventArgs e)
        {

            var A = ReadMatrixFromGrid(MatrixGridA);
            var B = ReadMatrixFromGrid(MatrixGridB);
            if (A.Rows != B.Rows || A.Columns != B.Columns)
            {
                ResultText.Text = "Addition requires matrices of same size.";
                return;
            }

            var C = MatrixOperations.Add(A, B);
            ResultText.Text = "A + B:\n" + C.ToString();
        }

        private void OnSubtractMatricesClick(object sender, RoutedEventArgs e)
        {
            var A = ReadMatrixFromGrid(MatrixGridA);
            var B = ReadMatrixFromGrid(MatrixGridB);
            if (A.Rows != B.Rows || A.Columns != B.Columns)
            {
                ResultText.Text = "Subtraction requires matrices of same size.";
                return;
            }
            var C = MatrixOperations.Subtract(A, B);
            ResultText.Text = "A - B:\n" + C.ToString();
        }

        private void OnMultiplyMatricesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var A = ReadMatrixFromGrid(MatrixGridA);
                var B = ReadMatrixFromGrid(MatrixGridB);

                if (A.Columns != B.Rows)
                {
                    ResultText.Text = $"Multiplication not possible: A is {A.Rows}×{A.Columns}, B is {B.Rows}×{B.Columns}";
                    return;
                }

                var C = MatrixOperations.Multiply(A, B);
                ResultText.Text = "A × B:\n" + C.ToFormattedString();
            }
            catch (Exception ex)
            {
                ResultText.Text = "Error: " + ex.Message;
            }
        }

        private void OnTransposeClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button?.Tag?.ToString();
            var grid = tag == "A" ? MatrixGridA : MatrixGridB;
            var matrix = ReadMatrixFromGrid(grid);

            var result = MatrixOperations.Transpose(matrix);
            ResultText.Text = $"Transpose ({tag}):\n" + result.ToFormattedString();
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button?.Tag?.ToString();
            var grid = tag == "A" ? MatrixGridA : MatrixGridB;

            if (grid.ItemsSource is DataView view)
            {
                var table = view.ToTable();

                foreach (DataRow row in table.Rows)
                    for (int c = 0; c < table.Columns.Count; c++)
                        row[c] = 0.0;

                grid.ItemsSource = table.DefaultView;
            }
        }

        private void OnDetClick(object sender, RoutedEventArgs e)
        {
            var matrix = ReadMatrixFromGrid(GetMatrixGridFromTag(sender));
            ResultText.Text = $"det ≈ {MatrixOperations.Determinant(matrix):F6}";
        }

        private void OnInverseClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button?.Tag?.ToString();
            var grid = tag == "A" ? MatrixGridA : MatrixGridB;
            var matrix = ReadMatrixFromGrid(grid);

            try
            {
                var result = MatrixOperations.Inverse(matrix);
                ResultText.Text = $"Inverse ({tag}):\n" + result.ToFormattedString();
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Matrix {tag} is not invertible.\n{ex.Message}";
            }
        }

        private void OnPowerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var tag = button?.Tag?.ToString();

                var powerBox = tag == "A" ? PowerBoxA : PowerBoxB;
                var grid = tag == "A" ? MatrixGridA : MatrixGridB;

                if (!int.TryParse(powerBox.Text, out int k))
                {
                    ResultText.Text = $"Please enter a valid integer exponent for matrix {tag}.";
                    return;
                }

                var matrix = ReadMatrixFromGrid(grid);
                var result = MatrixOperations.Power(matrix, k);
                ResultText.Text = $"{tag}^{k} =\n" + result.ToFormattedString();
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Matrix power failed:\n{ex.Message}";
            }
        }

        private void OnThemeIconClick(object sender, RoutedEventArgs e)
        {
            bool isDark = ThemeToggle.IsChecked == true;
            string theme = isDark ? "DarkTheme.xaml" : "LightTheme.xaml";
            ThemeIcon.Text = isDark ? "🌙" : "☀";

            var existingTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme.xaml"));

            var newTheme = new ResourceDictionary
            {
                Source = new Uri($"/Desktop;component/Themes/{theme}", UriKind.Relative)
            };

            if (existingTheme != null)
            {
                int index = Application.Current.Resources.MergedDictionaries.IndexOf(existingTheme);
                Application.Current.Resources.MergedDictionaries[index] = newTheme;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(newTheme);
            }
        }


        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Maximized)
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = this.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
            }
            else if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        private DataGrid GetMatrixGridFromTag(object sender)
        {
            var button = sender as Button;
            string tag = button?.Tag?.ToString();
            return tag == "A" ? MatrixGridA : MatrixGridB;
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void MethodTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (MethodTree.SelectedItem is TreeViewItem selected)
            {
                var tag = selected.Tag?.ToString();

                MatrixPanel.Visibility = (tag == "matrix") ? Visibility.Visible : Visibility.Collapsed;
                SortingPanel.Visibility = (tag == "sorting") ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                MatrixPanel.Visibility = Visibility.Collapsed;
                SortingPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void OnGenerateArrayClick(object sender, RoutedEventArgs e)
        {
            ResultText.Clear();
            Random rnd = new Random();
            int[] array = Enumerable.Range(1, 10).Select(_ => rnd.Next(0, 100)).ToArray();
            SortInputBox.Text = string.Join(" ", array);
            ResultText.Text = "Generated array:\n" + SortInputBox.Text;
        }

        private void OnClearArrayClick(object sender, RoutedEventArgs e)
        {
            SortInputBox.Clear();
            ResultText.Clear();
        }

    }
}
