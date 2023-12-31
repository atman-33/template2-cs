using Prism.Commands;
using Prism.Events;
using System;
using Template2.Domain.Modules.Helpers;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample010ViewModel : ViewModelBase
    {
        public Sample010ViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル010（Python実行）");

            PythonExecuteButton = new DelegateCommand(PythonExecuteButtonExecute);

            //// サンプル用の初期値
            PythonFilePathText = @"C:\Repos\template2-wpf-cs\Template2.Domain\Modules\Python\test.py";
            PythonArgumentText = "5 2";
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
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
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand PythonExecuteButton { get; }

        private void PythonExecuteButtonExecute()
        {
            PythonResultText = string.Empty;

            foreach (string line in PythonHelper.PythonCall(PythonFilePathText, PythonArgumentText))
            {
                PythonResultText += line;
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Timer
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

    }
}
