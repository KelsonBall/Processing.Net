using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExampleUserCode;
using Processing.Core;
using Processing.Samples.Wpf.Annotations;

namespace Processing.Samples.Wpf
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Canvas _exampleCanvas;
        public Canvas ExampleCanvas
        {
            get { return _exampleCanvas; }
            set
            {
                _exampleCanvas = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            ExampleCanvas = new MySketch();
        }


        #region Notify Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
