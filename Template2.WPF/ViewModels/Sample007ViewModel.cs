using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.BackgroundWorkers;
using Template2.WPF.Events;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    /// <summary>
    /// DataGridの自動更新
    /// </summary>
    /// <remarks>
    /// DataGridを自動更新するタイマー処理は、Observerパターンで実装
    /// </remarks>
    public class Sample007ViewModel : ViewModelBase
    {
        private IWorkerMstRepository _workerMstRepository;

        public Sample007ViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
            : this(dialogService, eventAggregator, Factories.CreateWorkerMst())
        {
        }


        public Sample007ViewModel(
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IWorkerMstRepository workerMstRepository)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル007（DataGrid自動更新）");

            _workerMstRepository = workerMstRepository;

            //// DelegateCommandメソッドを登録
            AutoUpdateButtonChecked = new DelegateCommand(AutoUpdateButtonCheckedExecute);
            AutoUpdateButtonUnchecked = new DelegateCommand(AutoUpdateButtonUncheckedExecute);

            Sample002ViewButton = new DelegateCommand(Sample002ViewButtonExecute);

            //// テーブル更新
            UpdateWorkerMstEntities();

            //// Background側でタイマーStart
            Sample007ViewBackgroundWorker.Start();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //// 画面から離れる際に、タイマーアクションを除去
            Sample007ViewBackgroundWorker.Remove(UpdateWorkerMstEntities);
        }
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private string _updatedTimeLabel;
        public string UpdatedTimeLabel
        {
            get { return _updatedTimeLabel; }
            set { SetProperty(ref _updatedTimeLabel, value); }
        }

        private ObservableCollection<WorkerMstEntity> _workerMstEntities
            = new ObservableCollection<WorkerMstEntity>();
        public ObservableCollection<WorkerMstEntity> WorkerMstEntities
        {
            get { return _workerMstEntities; }
            set { SetProperty(ref _workerMstEntities, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// 自動更新ON時処理
        /// </summary>
        public DelegateCommand AutoUpdateButtonChecked { get; }

        private void AutoUpdateButtonCheckedExecute()
        {
            //// 定期実行メソッドをBackgroundWorkerに登録
            Sample007ViewBackgroundWorker.Add(UpdateWorkerMstEntities);
        }

        /// <summary>
        /// 自動更新OFF時の処理
        /// </summary>
        public DelegateCommand AutoUpdateButtonUnchecked { get; }

        private void AutoUpdateButtonUncheckedExecute()
        {
            Sample007ViewBackgroundWorker.Remove(UpdateWorkerMstEntities);
        }

        public DelegateCommand Sample002ViewButton { get; }

        private void Sample002ViewButtonExecute()
        {
            //// 画面遷移処理（ダイアログ）
            _dialogService.ShowDialog(nameof(Sample002View), null, null);
        }
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        private void UpdateWorkerMstEntities()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                {
                    WorkerMstEntities.Clear();

                    foreach (var entity in _workerMstRepository.GetData())
                    {
                        WorkerMstEntities.Add(entity);
                    }

                    UpdatedTimeLabel = DateTime.Now.ToString("HH:mm:ss");
                }
            }));
        }
        #endregion
    }
}
