using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample002ViewModel : BindableBase, INavigationAware
    {
        /// <summary>
        /// MainWindow
        /// </summary>
        private MainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// メッセージボックス
        /// </summary>
        private IMessageService _messageService;

        //// 外部接触Repository
        private IWorkingTimePlanMstRepository _workingTimePlanMstRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sample002ViewModel()
            : this(Factories.CreateWorkingTimePlanMst())
        {

        }

        public Sample002ViewModel(IWorkingTimePlanMstRepository workingTimePlanMstRepository)
        {
            //// メッセージボックス
            _messageService = new MessageService();

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workingTimePlanMstRepository = workingTimePlanMstRepository;

            //// DelegateCommandメソッドを登録

            //// Repositoryからデータ取得
            UpdateWorkingTimePlanMstEntities();

        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private ObservableCollection<WorkingTimePlanMstEntity> _workingTimePlanMstEntities;
        public ObservableCollection<WorkingTimePlanMstEntity> WorkingTimePlanMstEntities
        {
            get { return _workingTimePlanMstEntities; }
            set { SetProperty(ref _workingTimePlanMstEntities, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private void UpdateWorkingTimePlanMstEntities()
        {
            WorkingTimePlanMstEntities.Clear();

            foreach (var entity in _workingTimePlanMstRepository.GetData())
            {
                WorkingTimePlanMstEntities.Add(entity);
            }
        }


        #endregion
    }
}
