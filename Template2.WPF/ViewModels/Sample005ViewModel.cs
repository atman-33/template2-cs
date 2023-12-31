using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using Template2.Domain.Modules.Objects.Composites;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample005ViewModel : ViewModelBase
    {
        //// 外部接触Repository
        private IWorkerGroupMstRepository _workerGroupMstRepository;
        private IWorkerMstRepository _workerMstRepository;

        public Sample005ViewModel(IEventAggregator eventAggregator)
            : this(eventAggregator, AbstractFactory.Create())
        {
        }

        public Sample005ViewModel(
            IEventAggregator eventAggregator,
            AbstractFactory factory)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル005（TreeViewと選択アイテムのバインド）");

            _workerGroupMstRepository = factory.CreateWorkerGroupMst();
            _workerMstRepository = factory.CreateWorkerMst();

            //// DelegateCommandメソッドを登録
            WorkerGroupTreeViewSelectedItemChanged = new DelegateCommand(WorkerGroupTreeViewSelectedItemChangedExecute);

            WorkerGroupTreeView = new ObservableCollection<OrganizationComponentBase>(
                OrganizationComponentBase.CreateOrganizationComponents(
                _workerGroupMstRepository.GetData(), _workerMstRepository.GetData()));
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private ObservableCollection<OrganizationComponentBase> _workerGroupTreeView
            = new ObservableCollection<OrganizationComponentBase>();
        public ObservableCollection<OrganizationComponentBase> WorkerGroupTreeView
        {
            get { return _workerGroupTreeView; }
            set { SetProperty(ref _workerGroupTreeView, value); }
        }

        private string _workerGroupCodeText;
        public string WorkerGroupCodeText
        {
            get { return _workerGroupCodeText; }
            set { SetProperty(ref _workerGroupCodeText, value); }
        }

        private string _workerCodeText;
        public string WorkerCodeText
        {
            get { return _workerCodeText; }
            set { SetProperty(ref _workerCodeText, value); }
        }

        private OrganizationComponentBase _workerGroupTreeViewSelectedItem;
        public OrganizationComponentBase WorkerGroupTreeViewSelectedItem
        {
            get { return _workerGroupTreeViewSelectedItem; }
            set { SetProperty(ref _workerGroupTreeViewSelectedItem, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand WorkerGroupTreeViewSelectedItemChanged { get; }

        private void WorkerGroupTreeViewSelectedItemChangedExecute()
        {
            if (WorkerGroupTreeViewSelectedItem == null)
            {
                return;
            }

            if (WorkerGroupTreeViewSelectedItem.GetType().Name == nameof(WorkerGroup))
            {
                //// ComponentがWorkerGroupの場合
                WorkerGroupCodeText = WorkerGroupTreeViewSelectedItem.Code;
                WorkerCodeText = string.Empty;
            }
            else
            {
                //// ComponentがWorkerの場合
                WorkerGroupCodeText = WorkerGroupTreeViewSelectedItem.WorkerGroupCode;
                WorkerCodeText = WorkerGroupTreeViewSelectedItem.Code;
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion
    }
}
