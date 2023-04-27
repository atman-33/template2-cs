using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        /// <summary>
        /// 編集中はtrue（データ未保存の状態）
        /// </summary>
        private static bool IsEdited = false;

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

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// 編集時に呼び出すメソッド
        /// </summary>
        public void OnEdit()
        {
            IsEdited = true;
        }

        /// <summary>
        /// 編集完了時に呼び出すメソッド
        /// </summary>
        public void OnEditCompleted()
        {
            IsEdited = false;
        }

        /// <summary>
        /// 画面遷移してよい場合にtrueを返す
        /// </summary>
        /// <returns></returns>
        public bool canNavigate()
        {
            if (IsEdited)
            {
                if (_messageService == null)
                {
                    _messageService = new MessageService();
                }

                if (_messageService.Warning("編集中の内容が保存されていません。画面を移動しますか？") == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }

            IsEdited = false;
            return true;
        }

        #endregion
    }
}
