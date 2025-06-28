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
using MathCore.Libraries.SortingCore;
using System.Text;
using MathCore.Common;
using MathCore.Models.SortResults;
using System.Globalization;
using MathCore.Interfaces;
using Desktop.Services;
using Desktop.ViewModels;
using ControlzEx.Standard;

namespace Desktop
{
    public partial class MainWindow : MetroWindow
    {
        private string _lastMatrixTag;

        public MainWindow()
        {
            InitializeComponent();

            var mainVM = new MainViewModel();

            var sortVM = new SortViewModel
            {
                SetResultText = text => mainVM.Result = text
            };

            var matrixVM = new MatrixViewModel
            {
                GetMatrixA = () => MatrixService.ReadMatrixFromGrid(MatrixGridA),
                GetMatrixB = () => MatrixService.ReadMatrixFromGrid(MatrixGridB),
                GetMatrixTag = () => _lastMatrixTag,
                GetMatrixPower = () =>
                {
                    var box = _lastMatrixTag == "B" ? PowerBoxB : PowerBoxA;
                    return int.TryParse(box.Text, out int k) ? k : 1;
                },
                SetResultText = text => mainVM.Result = text,
                SetMatrixGrid = (model, tag) =>
                {
                    if (tag == "A")
                        MatrixService.UpdateMatrixGrid(model, MatrixGridA);
                    else
                        MatrixService.UpdateMatrixGrid(model, MatrixGridB);
                }
            };

            mainVM.SortViewModel = sortVM;
            mainVM.MatrixViewModel = matrixVM;

            this.DataContext = mainVM;

            MatrixService.LoadInitialMatrix(MatrixGridA);
            MatrixService.LoadInitialMatrix(MatrixGridB);
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
                if (DataContext is not MainViewModel mainVM)
                    return;

                var sortVM = mainVM.SortViewModel;

                if (MethodTree.SelectedItem is TreeViewItem selectedItem && selectedItem.Parent is TreeViewItem)
                {
                    string method = selectedItem.Header.ToString() ?? "";
                    string tag = selectedItem.Tag?.ToString() ?? "";

                    if (tag == "sorting")
                    {
                        bool success = SortingExecutorService.TryExecuteSort(method, sortVM);
                        sortVM.SetResultText = text => mainVM.Result = text;
                        return;
                    }

                    var A = MatrixService.ReadMatrixFromGrid(MatrixGridA);
                    var B = MatrixService.ReadMatrixFromGrid(MatrixGridB);

                    MatrixModel input = _lastMatrixTag == "B" ? B : A;
                    MatrixExecutorService.SetActiveMatrixTag(_lastMatrixTag);

                    if (MatrixExecutorService.TryExecute(method, A, B, out string result))
                    {
                        mainVM.Result = result;
                    }
                    else
                    {
                        mainVM.Result = $"Method \"{method}\" not implemented.";
                    }
                }
                else
                {
                    mainVM.Result = "Please choose a specific method.";
                }
            }
            catch (Exception ex)
            {
                if (DataContext is MainViewModel vm)
                    vm.Result = $"Error: {ex.Message}";
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

    }
}
