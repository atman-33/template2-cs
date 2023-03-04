using Prism.Commands;

namespace Template2.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
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
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand Sample001ViewButton { get; }

        private void Sample001ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample001ViewButton.Execute();
        }

        public DelegateCommand Sample002ViewButton { get; }

        private void Sample002ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample002ViewButton.Execute();
        }

        public DelegateCommand Sample003ViewButton { get; }

        private void Sample003ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample003ViewButton.Execute();
        }

        public DelegateCommand Sample004ViewButton { get; }

        private void Sample004ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample004ViewButton.Execute();
        }

        public DelegateCommand Sample005ViewButton { get; }

        private void Sample005ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample005ViewButton.Execute();
        }

        public DelegateCommand Sample006ViewButton { get; }
        private void Sample006ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample006ViewButton.Execute();
        }

        public DelegateCommand Sample007ViewButton { get; }
        private void Sample007ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample007ViewButton.Execute();
        }

        public DelegateCommand Sample008ViewButton { get; }
        private void Sample008ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample008ViewButton.Execute();
        }

        public DelegateCommand Sample009ViewButton { get; }
        private void Sample009ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample009ViewButton.Execute();
        }

        public DelegateCommand Sample010ViewButton { get; }
        private void Sample010ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample010ViewButton.Execute();
        }

        public DelegateCommand Sample011ViewButton { get; }
        private void Sample011ViewButtonExecute()
        {
            base._mainWindowViewModel.Sample011ViewButton.Execute();
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion
    }
}
