using Prism.Commands;
using Prism.Regions;
using System;
using Template2.Domain.Modules.Helpers;

namespace Template2.WPF.ViewModels
{
    public class Sample010ViewModel : ViewModelBase
    {
        public Sample010ViewModel()
        {
            PythonExecuteButton = new DelegateCommand(PythonExecuteButtonExecute);


            //// サンプル用の初期値
            PythonFilePathText = @"C:\Repos\template2-cs\Template2.Domain\Modules\Python\test.py";
            PythonArgumentText = "5 2";
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private string _pythonFilePathText;
        public string PythonFilePathText
        {
            get { return _pythonFilePathText; }
            set { SetProperty(ref _pythonFilePathText, value); }
        }

        private string _pythonArgumentText;
        public string PythonArgumentText
        {
            get { return _pythonArgumentText; }
            set { SetProperty(ref _pythonArgumentText, value); }
        }

        private string _pythonResultText;
        public string PythonResultText
        {
            get { return _pythonResultText; }
            set { SetProperty(ref _pythonResultText, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand PythonExecuteButton { get; }

        private void PythonExecuteButtonExecute()
        {
            PythonResultText = String.Empty;

            foreach (string line in PythonHelper.PythonCall(PythonFilePathText, PythonArgumentText))
            {
                PythonResultText += line;
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            _mainWindowViewModel.ViewOutline = "> サンプル010（Python実行）";
        }
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Timer
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion
    }
}
