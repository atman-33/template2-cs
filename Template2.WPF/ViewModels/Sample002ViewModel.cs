using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Collections;
using Template2.WPF.Services;
using Template2.WPF.ViewModelEntities;

namespace Template2.WPF.ViewModels
{
    public class Sample002ViewModel : ViewModelBase, IDialogAware   //// 別ViewModelからダイアログ表示させるためIDialogAware実装
    {
        //// 外部接触Repository
        private IWorkerMstRepository _workerMstRepository;
        private IWorkerGroupMstRepository _workerGroupMstRepository;

        public Sample002ViewModel(IEventAggregator eventAggregator)
            : this(eventAggregator, 
                  new MessageService(), 
                  Factories.CreateWorkerMst(), 
                  Factories.CreateWorkerGroupMst())
        {
        }

        public Sample002ViewModel(
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IWorkerMstRepository workerMstRepository, 
            IWorkerGroupMstRepository workerGroupMstRepository)
        {
            _eventAggregator = eventAggregator;
            
            //// Navigation表示の時のみ表示するように変更
            //_eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル002（DataGridを直接編集）");

            _messageService = messageService;

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workerMstRepository = workerMstRepository;
            _workerGroupMstRepository = workerGroupMstRepository;

            //// Repositoryからデータ取得
            _workerMstCollection = new WorkerMstCollection(workerMstRepository);
            _workerMstCollection.LoadData();

            _workerGroupMstSelectList = new WorkerGroupMstCollection(workerGroupMstRepository);
            _workerGroupMstSelectList.LoadData();

            //// DBテーブルにレコードが存在しない場合は、空レコードを追加
            if (_workerMstCollection.Count == 0)
            {
                _workerMstCollection.AddNewItem();
            }

        }

        public string Title => "Sample002View";

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル002（DataGridを直接編集）");
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private WorkerMstCollection _workerMstCollection;
        public WorkerMstCollection WorkerMstCollection
        {
            get { return _workerMstCollection; }
            set { SetProperty(ref _workerMstCollection, value); }
        }

        private WorkerMstViewModelEntity _workerMstCollectionSlectedItem;
        public WorkerMstViewModelEntity WorkerMstCollectionSlectedItem
        {
            get { return _workerMstCollectionSlectedItem; }
            set { SetProperty(ref _workerMstCollectionSlectedItem, value); }
        }

        private string _workerCodeText;
        public string WorkerCodeText
        {
            get { return _workerCodeText; }
            set { SetProperty(ref _workerCodeText, value); }
        }

        private WorkerGroupMstCollection _workerGroupMstSelectList;

        public WorkerGroupMstCollection WorkerGroupMstSelectList
        {
            get { return _workerGroupMstSelectList; }
            set { SetProperty(ref _workerGroupMstSelectList, value); }
        }

        private Visibility _workerNameVisibility = Visibility.Visible;

        public Visibility WorkerNameVisibility
        {
            get { return _workerNameVisibility; }
            set { SetProperty(ref _workerNameVisibility, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand AddRowButton =>
            new DelegateCommand(() =>
            {
                WorkerMstCollection.AddNewItem();
            });

        public DelegateCommand DeleteRowButton =>
            new DelegateCommand(() =>
            {
                if (WorkerMstCollectionSlectedItem == null)
                {
                    return;
                }

                WorkerMstCollection.DeleteItem(WorkerMstCollectionSlectedItem);

            });

        public DelegateCommand SaveButton =>
            new DelegateCommand(() =>
            {
                if (_messageService.Question("保存しますか？") != MessageBoxResult.OK)
                {
                    return;
                }

                //// DBテーブルの中身を全て削除
                foreach (var entity in _workerMstRepository.GetData())
                {
                    _workerMstRepository.Delete(entity);
                }

                //// 画面に設定した内容をDBテーブルに保存
                foreach (var viewModelEntity in WorkerMstCollection)
                {
                    var entity = new WorkerMstEntity(
                        viewModelEntity.Entity.WorkerCode.Value,
                        viewModelEntity.Entity.WorkerName.Value,
                        viewModelEntity.Entity.WorkerGroupCode.Value);
                    _workerMstRepository.Save(entity);
                }

                _messageService.ShowDialog("保存しました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
            });

        public DelegateCommand ChangeWorkerNameVisibilityButton =>
            new DelegateCommand(() =>
            {
                if (WorkerNameVisibility == Visibility.Visible)
                {
                    WorkerNameVisibility = Visibility.Collapsed;
                }
                else
                {
                    WorkerNameVisibility = Visibility.Visible;
                }

            });

        public DelegateCommand WorkerMstCollectionSelectedCellsChanged =>
            new DelegateCommand(() =>
            {
                if (WorkerMstCollectionSlectedItem == null)
                {
                    return;
                }

                WorkerCodeText = WorkerMstCollectionSlectedItem.WorkerCode;
            });

        public DelegateCommand WorkerMstCollectionCurrentCellChanged =>
            new DelegateCommand(() =>
            {
                if (WorkerMstCollectionSlectedItem == null)
                {
                    return;
                }

                WorkerCodeText = WorkerMstCollectionSlectedItem.WorkerCode;
            });

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 


        #endregion

    }
}
