using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using Template2.Domain;
using Template2.Domain.Entities;
using Template2.Domain.StaticValues;
using Template2.WPF.BackgroundWorkers;
using Template2.WPF.Events;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    public class Sample007ViewModel : ViewModelBase
    {
        /// <summary>
        /// タイマー
        /// </summary>
        private DispatcherTimer _timer;

        public Sample007ViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル007（DataGrid自動更新）");

            //// タイマー処理の初期化
            InitializeTimer();

            //// DelegateCommandメソッドを登録
            AutoUpdateButtonChecked = new DelegateCommand(AutoUpdateButtonCheckedExecute);
            AutoUpdateButtonUnchecked = new DelegateCommand(AutoUpdateButtonUncheckedExecute);

            Sample002ViewButton = new DelegateCommand(Sample002ViewButtonExecute);
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private string _updatedTimeLabel = Shared.UpdatedTime.ToString("HH:mm:ss");
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

        public DelegateCommand AutoUpdateButtonChecked { get; }

        private void AutoUpdateButtonCheckedExecute()
        {
            //// テーブル更新
            updateWorkerMstEntities();

            //// 描画とデータ更新を、それぞれのスレッドで処理させている。
            //// 例）②Background側でDataGridのソースを5秒毎に更新し、①ViewModel側で1秒毎に描画を更新

            //// ①ViewModel側で描画処理Start
            _timer.Start();

            //// ②Background側でデータ処理Start
            BackgroudWorker.Start();
        }

        //AutoUpdateButtonUnhecked
        public DelegateCommand AutoUpdateButtonUnchecked { get; }

        private void AutoUpdateButtonUncheckedExecute()
        {
            //// ①ViewModel側で描画処理Stop
            _timer.Stop();

            //// ②Background側でデータ処理Stop
            BackgroudWorker.Stop();
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
        private void updateWorkerMstEntities()
        {
            WorkerMstEntities.Clear();

            foreach (var entity in StaticWorkerMst.Entities)
            {
                WorkerMstEntities.Add(entity);
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Timer
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        private void InitializeTimer()
        {
            //// 優先順位を指定してタイマのインスタンスを生成
            _timer = new DispatcherTimer(DispatcherPriority.Background);

            //// インターバルを設定
            _timer.Interval = new TimeSpan(0, 0, 1);

            //// タイマメソッドを設定
            _timer.Tick += (e, s) => { TimerExecute(); };
        }

        private void TimerExecute()
        {
            UpdatedTimeLabel = Shared.UpdatedTime.ToString("HH:mm:ss");
            updateWorkerMstEntities();
        }
        #endregion

    }
}
