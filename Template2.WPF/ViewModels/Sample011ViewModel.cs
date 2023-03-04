using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastructure;

namespace Template2.WPF.ViewModels
{
    public class Sample011ViewModel : ViewModelBase
    {
        private IWorkerGroupMstRepository _sampleMstRepository;

        public Sample011ViewModel()
            : this(Factories.CreateWorkerGroupMst())
        {

        }

        public Sample011ViewModel(IWorkerGroupMstRepository workerGroupMstRepository)
        {
            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _sampleMstRepository = workerGroupMstRepository;

            //// Repositoryからデータ取得
            UpdateWorkerGroupMstEntities();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        private ObservableCollection<WorkerGroupMstEntity> _workerGroupMstEntities
            = new ObservableCollection<WorkerGroupMstEntity>();
        public ObservableCollection<WorkerGroupMstEntity> WorkerGroupMstEntities
        {
            get { return _workerGroupMstEntities; }
            set { SetProperty(ref _workerGroupMstEntities, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        private void UpdateWorkerGroupMstEntities()
        {
            WorkerGroupMstEntities.Clear();

            foreach (var entity in _sampleMstRepository.GetData())
            {
                WorkerGroupMstEntities.Add(entity);
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Timer
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion
    }
}
