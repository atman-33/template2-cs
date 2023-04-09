using Prism.Commands;
using Prism.Events;
using Template2.WPF.Events;

namespace Template2.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            //// DelegateCommandメソッドを登録
            Sample001ViewButton = new DelegateCommand(Sample001ViewButtonExecute);
            Sample002ViewButton = new DelegateCommand(Sample002ViewButtonExecute);
            Sample003ViewButton = new DelegateCommand(Sample003ViewButtonExecute);
            Sample004ViewButton = new DelegateCommand(Sample004ViewButtonExecute);
            Sample005ViewButton = new DelegateCommand(Sample005ViewButtonExecute);
            Sample006ViewButton = new DelegateCommand(Sample006ViewButtonExecute);
            Sample007ViewButton = new DelegateCommand(Sample007ViewButtonExecute);
            Sample008ViewButton = new DelegateCommand(Sample008ViewButtonExecute);
            Sample009ViewButton = new DelegateCommand(Sample009ViewButtonExecute);
            Sample010ViewButton = new DelegateCommand(Sample010ViewButtonExecute);
            Sample011ViewButton = new DelegateCommand(Sample011ViewButtonExecute);
            Sample012ViewButton = new DelegateCommand(Sample012ViewButtonExecute);
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand Sample001ViewButton { get; }

        private void Sample001ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample001ViewButton));
        }

        public DelegateCommand Sample002ViewButton { get; }

        private void Sample002ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample002ViewButton));
        }

        public DelegateCommand Sample003ViewButton { get; }

        private void Sample003ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample003ViewButton));
        }

        public DelegateCommand Sample004ViewButton { get; }

        private void Sample004ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample004ViewButton));
        }

        public DelegateCommand Sample005ViewButton { get; }

        private void Sample005ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample005ViewButton));
        }

        public DelegateCommand Sample006ViewButton { get; }
        private void Sample006ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample006ViewButton));
        }

        public DelegateCommand Sample007ViewButton { get; }
        private void Sample007ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample007ViewButton));
        }

        public DelegateCommand Sample008ViewButton { get; }
        private void Sample008ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample008ViewButton));
        }

        public DelegateCommand Sample009ViewButton { get; }
        private void Sample009ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample009ViewButton));
        }

        public DelegateCommand Sample010ViewButton { get; }
        private void Sample010ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample010ViewButton));
        }

        public DelegateCommand Sample011ViewButton { get; }
        private void Sample011ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample011ViewButton));
        }

        public DelegateCommand Sample012ViewButton { get; }
        private void Sample012ViewButtonExecute()
        {
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Publish(nameof(MainWindowViewModel.Sample012ViewButton));
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion
    }
}
