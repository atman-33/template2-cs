using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        /// <summary>
        /// リージョンマネージャー（リージョン画面遷移に必要）
        /// </summary>
        protected IRegionManager _regionManager;

        /// <summary>
        /// ダイアログサービス（ダイアログ画面遷移に必要）
        /// </summary>
        protected IDialogService _dialogService;

        /// <summary>
        /// イベントアグリゲーター
        /// </summary>
        protected IEventAggregator _eventAggregator;

        /// <summary>
        /// メッセージボックス
        /// </summary>
        protected IMessageService _messageService;

        /// <summary>
        /// ViewModel破棄に伴いメモリ開放する際はfalse
        /// </summary>
        public bool KeepAlive { get; set; } = false;

        public ViewModelBase()
        {
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //// RegionMemberLifetime(KeepAlive = false)でViewModelを破棄するため、こちらはTrue
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
