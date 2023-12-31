using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Infrastructure.RestApi;
using Template2.WPF.Services;
using Template2.WPF.ViewModelEntities;

namespace Template2.WPF.ViewModels
{
    public class Sample012ViewModel : ViewModelBase
    {
        //// 外部接触Repository
        private IWorkerGroupMstRepositoryAsync _workerGroupMstRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sample012ViewModel(IEventAggregator eventAggregator)
            : this(eventAggregator, new MessageService(), new WorkerGroupMstRestApi())
        {
        }

        public Sample012ViewModel(
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IWorkerGroupMstRepositoryAsync workerGroupMstRepository)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル012（Web APIによるテーブル操作）");

            _messageService = messageService;

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workerGroupMstRepository = workerGroupMstRepository;

            //// DelegateCommandメソッドを登録
            WorkerGroupMstEntitiesSelectedCellsChanged = new DelegateCommand(WorkerGroupMstEntitiesSelectedCellsChangedExecute);
            NewButton = new DelegateCommand(NewButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);
            DeleteButton = new DelegateCommand(DeleteButtonExecute);

            //// Repositoryからデータ取得
            LoadWorkerGroupMstEntities();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private ObservableCollection<WorkerGroupMstViewModelEntity> _workerGroupMstEntities
            = new ObservableCollection<WorkerGroupMstViewModelEntity>();
        public ObservableCollection<WorkerGroupMstViewModelEntity> WorkerGroupMstEntities
        {
            get { return _workerGroupMstEntities; }
            set { SetProperty(ref _workerGroupMstEntities, value); }
        }

        private WorkerGroupMstViewModelEntity _workerGroupMstEntitiesSlectedItem;
        public WorkerGroupMstViewModelEntity WorkerGroupMstEntitiesSlectedItem
        {
            get { return _workerGroupMstEntitiesSlectedItem; }
            set { SetProperty(ref _workerGroupMstEntitiesSlectedItem, value); }
        }

        private string _workerGroupCodeText;
        public string WorkerGroupCodeText
        {
            get { return _workerGroupCodeText; }
            set { SetProperty(ref _workerGroupCodeText, value); }
        }

        private bool _workerGroupCodeIsEnabled = false;
        public bool WorkerGroupCodeIsEnabled
        {
            get { return _workerGroupCodeIsEnabled; }
            set { SetProperty(ref _workerGroupCodeIsEnabled, value); }
        }

        private string _workerGroupNameText;
        public string WorkerGroupNameText
        {
            get { return _workerGroupNameText; }
            set { SetProperty(ref _workerGroupNameText, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand WorkerGroupMstEntitiesSelectedCellsChanged { get; }
        private void WorkerGroupMstEntitiesSelectedCellsChangedExecute()
        {
            if (WorkerGroupMstEntitiesSlectedItem == null)
            {
                return;
            }

            WorkerGroupCodeText = WorkerGroupMstEntitiesSlectedItem.WorkerGroupCode;
            WorkerGroupNameText = WorkerGroupMstEntitiesSlectedItem.WorkerGroupName;

            WorkerGroupCodeIsEnabled = false;
        }

        public DelegateCommand NewButton { get; }
        private void NewButtonExecute()
        {
            WorkerGroupCodeIsEnabled = true;
            WorkerGroupCodeText = string.Empty;
            WorkerGroupNameText = string.Empty;
        }

        public DelegateCommand SaveButton { get; }
        private async void SaveButtonExecute()
        {
            Guard.IsNullOrEmpty(WorkerGroupNameText, "サンプル名称を入力してください。");

            if (_messageService.Question("保存しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            var entity = new WorkerGroupMstEntity(
                WorkerGroupCodeText,
                WorkerGroupNameText
                );

            await Task.Run(() => _workerGroupMstRepository.SaveAsync(entity));

            LoadWorkerGroupMstEntities();
        }

        public DelegateCommand DeleteButton { get; }
        private async void DeleteButtonExecute()
        {
            if (_messageService.Question("「" + WorkerGroupNameText + "」を削除しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            var entity = new WorkerGroupMstEntity(
                WorkerGroupCodeText,
                WorkerGroupNameText
                );

            await Task.Run(() => _workerGroupMstRepository.DeleteAsync(entity));

            LoadWorkerGroupMstEntities();
        }

        #endregion


        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private async void LoadWorkerGroupMstEntities()
        {
            WorkerGroupMstEntities.Clear();

            foreach (var entity in await _workerGroupMstRepository.GetDataAsync())
            {
                WorkerGroupMstEntities.Add(new WorkerGroupMstViewModelEntity(entity));
            }
        }

        #endregion
    }
}