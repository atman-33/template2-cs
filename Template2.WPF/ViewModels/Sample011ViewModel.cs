using Prism.Events;
using System.Collections.ObjectModel;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Events;

namespace Template2.WPF.ViewModels
{
    public class Sample011ViewModel : ViewModelBase
    {
        private IWorkerGroupMstRepository _sampleMstRepository;

        public Sample011ViewModel(IEventAggregator eventAggregator)
            : this(eventAggregator, Factories.CreateWorkerGroupMst())
        {
        }

        public Sample011ViewModel(IEventAggregator eventAggregator, IWorkerGroupMstRepository workerGroupMstRepository)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル011（ドラッグ＆ドロップでDataGrid並び替え）");

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _sampleMstRepository = workerGroupMstRepository;

            //// Repositoryからデータ取得
            UpdateWorkerGroupMstEntities();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        private ObservableCollection<Sample011ViewModelWorkerGroupMst> _workerGroupMstEntities
            = new ObservableCollection<Sample011ViewModelWorkerGroupMst>();
        public ObservableCollection<Sample011ViewModelWorkerGroupMst> WorkerGroupMstEntities
        {
            get { return _workerGroupMstEntities; }
            set { SetProperty(ref _workerGroupMstEntities, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Timer
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        private void UpdateWorkerGroupMstEntities()
        {
            WorkerGroupMstEntities.Clear();

            foreach (var entity in _sampleMstRepository.GetData())
            {
                WorkerGroupMstEntities.Add(new Sample011ViewModelWorkerGroupMst(entity));
            }
        }

        #endregion
    }
}
