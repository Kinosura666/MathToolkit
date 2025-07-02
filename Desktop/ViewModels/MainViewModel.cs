using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Desktop.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public SortViewModel SortViewModel { get; set; }
        public MatrixViewModel MatrixViewModel { get; set; }

        private string _result;
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
