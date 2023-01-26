using Prism.Mvvm;
using Prism.Regions;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class ViewModelBase : BindableBase, INavigationAware
    {
        /// <summary>
        /// MainWindowViewModel
        /// </summary>
        protected MainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// リージョンマネージャー（リージョン画面遷移に必要）
        /// </summary>
        protected IRegionManager _regionManager;

        /// <summary>
        /// メッセージボックス
        /// </summary>
        protected IMessageService _messageService;

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
            //// MainWindowViewModelを受け取り
            _mainWindowViewModel = navigationContext.Parameters.GetValue<MainWindowViewModel>(nameof(MainWindowViewModel));
        }

        #endregion
    }
}
