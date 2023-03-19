using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastruture.Excel;
using Template2.WPF.Events;

namespace Template2.WPF.ViewModels
{
    public class Sample009ViewModel : ViewModelBase
    {
        private ITaskMstExcelRepository _taskMstExcelRepository;


        public Sample009ViewModel(IEventAggregator eventAggregator)
            :this(eventAggregator, new TaskMstExcel())
        {
        }

        public Sample009ViewModel(IEventAggregator eventAggregator, ITaskMstExcelRepository taskMstExcel)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル009（Excel取り込み）");

            _taskMstExcelRepository = taskMstExcel;

            //// DelegateCommandメソッドを登録
            SelectExcelFileButton = new DelegateCommand(SelectExcelFileButtonExecute);
            ImportExcelButton = new DelegateCommand(ImportExcelButtonExecute);

            //// サンプル用の初期値
            ExcelFilePathText = @"C:\Repos\template2-wpf-cs\Fake\excel\TaskList.xlsx";
            ExcelSheetNameText = @"Sheet1";
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private string _excelFilePathText = String.Empty;
        public string ExcelFilePathText
        {
            get { return _excelFilePathText; }
            set { SetProperty(ref _excelFilePathText, value); }
        }

        private string _excelSheetNameText = String.Empty;
        public string ExcelSheetNameText
        {
            get { return _excelSheetNameText; }
            set { SetProperty(ref _excelSheetNameText, value); }
        }

        private ObservableCollection<TaskMstEntity> _taskMstEntities
            = new ObservableCollection<TaskMstEntity>();
        public ObservableCollection<TaskMstEntity> TaskMstEntities
        {
            get { return _taskMstEntities; }
            set { SetProperty(ref _taskMstEntities, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand SelectExcelFileButton { get; }
        private void SelectExcelFileButtonExecute()
        {
            //// ダイアログのインスタンスを生成
            var dialog = new OpenFileDialog();

            //// ファイルの種類を設定
            dialog.Filter = @"Excel ファイル|*.xls;*.xlsx;*.xlsm|テキストファイル (*.txt)|*.txt|全てのファイル (*.*)|*.*";

            //// ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {
                // 選択されたファイル名 (ファイルパス) を取得
                ExcelFilePathText = dialog.FileName;
            }
        }

        public DelegateCommand ImportExcelButton { get; }
        private void ImportExcelButtonExecute()
        {
            TaskMstEntities.Clear();

            foreach (var entity in _taskMstExcelRepository.GetExcelSheetDataToList(ExcelFilePathText, ExcelSheetNameText, true))
            {
                TaskMstEntities.Add(entity);
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
