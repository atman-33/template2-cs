using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastructure.Csv;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample008ViewModel : ViewModelBase
    {
        private ITaskMstCsvRepository _taskMstCsvRepository;

        public Sample008ViewModel(IEventAggregator eventAggregator)
            : this(eventAggregator, new TaskMstCsv())
        {
        }
        public Sample008ViewModel(IEventAggregator eventAggregator, ITaskMstCsvRepository taskMstCsvRepository)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル008（CSV取り込み、ListView並び替え）");

            _taskMstCsvRepository = taskMstCsvRepository;

            //// DelegateCommandメソッドを登録
            ImportCsvButton = new DelegateCommand(ImportCsvButtonExecute);
            MoveAllItemsButton = new DelegateCommand(MoveAllItemsButtonExecute);
            MoveSelectedItemButton = new DelegateCommand(MoveSelectedItemButtonExecute);
            RemoveSelectedItemButton = new DelegateCommand(RemoveSelectedItemButtonExecute);
            RemoveAllItemsButton = new DelegateCommand(RemoveAllItemsButtonExecute);

            //// サンプル用の初期値
            CsvFilePathText = @"C:\Repos\template2-wpf-cs\Fake\csv\TaskList.csv";
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private string _csvFilePathText = string.Empty;
        public string CsvFilePathText
        {
            get { return _csvFilePathText; }
            set { SetProperty(ref _csvFilePathText, value); }
        }

        private ObservableCollection<TaskMstEntity> _inputCsvListView = new ObservableCollection<TaskMstEntity>();
        public ObservableCollection<TaskMstEntity> InputCsvListView
        {
            get { return _inputCsvListView; }
            set{ SetProperty(ref _inputCsvListView, value); }
        }

        private TaskMstEntity _inputCsvListViewSelectedItem;
        public TaskMstEntity InputCsvListViewSelectedItem
        {
            get { return _inputCsvListViewSelectedItem; }
            set { SetProperty(ref _inputCsvListViewSelectedItem, value); }
        }

        private ObservableCollection<TaskMstEntity> _outputCsvListView = new ObservableCollection<TaskMstEntity>();
        public ObservableCollection<TaskMstEntity> OutputCsvListView
        {
            get { return _outputCsvListView; }
            set { SetProperty(ref _outputCsvListView, value); }
        }

        private TaskMstEntity _outputCsvListViewSelectedItem;
        public TaskMstEntity OutputCsvListViewSelectedItem
        {
            get { return _outputCsvListViewSelectedItem; }
            set { SetProperty(ref _outputCsvListViewSelectedItem, value); }
        }

        private int _outputCsvListViewSelectedIndex;
        public int OutputCsvListViewSelectedIndex
        {
            get { return _outputCsvListViewSelectedIndex; }
            set { SetProperty(ref _outputCsvListViewSelectedIndex, value); }
        }
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand ImportCsvButton { get; }
        private void ImportCsvButtonExecute()
        {
            foreach (var entity in _taskMstCsvRepository.GetData(CsvFilePathText))
            {
                InputCsvListView.Add(entity);
            }
        }

        public DelegateCommand MoveAllItemsButton { get; }
        private void MoveAllItemsButtonExecute()
        {
            foreach (var entity in InputCsvListView)
            {
                OutputCsvListView.Add(entity);
            }
        }

        public DelegateCommand MoveSelectedItemButton { get; }
        private void MoveSelectedItemButtonExecute()
        {
            OutputCsvListView.Add(InputCsvListViewSelectedItem);
        }

        public DelegateCommand RemoveSelectedItemButton { get; }
        private void RemoveSelectedItemButtonExecute()
        {
            OutputCsvListView.Remove(OutputCsvListViewSelectedItem);
        }

        public DelegateCommand RemoveAllItemsButton { get; }
        private void RemoveAllItemsButtonExecute()
        {
            OutputCsvListView.Clear();
        }
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Timer
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// ドラッグ&ドロップでListViewを並び替え
        /// </summary>
        public Action<int> OutputCsvListViewDropCallback { get { return OutputCsvListViewDropCallbackExecute; } }

        private void OutputCsvListViewDropCallbackExecute(int index)
        {
            if (index >= 0)
            {
                OutputCsvListView.Move(OutputCsvListViewSelectedIndex, (int)index);
            }
        }

        #endregion    
    }
}
