using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        //// 画面遷移用
        private IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            //// 画面遷移用（ナビゲーション）
            _regionManager = regionManager;

            //// 初期画面
            //// ex. _regionManager.RegisterViewWithRegion("ContentRegion", nameof(HomeView));

            //// DelegateCommandメソッドを登録
            Sample001ViewButton = new DelegateCommand(Sample001ViewButtonExecute);
            Sample002ViewButton = new DelegateCommand(Sample002ViewButtonExecute);
            Sample003ViewButton = new DelegateCommand(Sample003ViewButtonExecute);
            Sample004ViewButton = new DelegateCommand(Sample004ViewButtonExecute);
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// タイトル
        /// </summary>
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// 画面の概要
        /// </summary>
        private string _viewOutline;
        public string ViewOutline
        {
            get { return _viewOutline; }
            set { SetProperty(ref _viewOutline, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand Sample001ViewButton { get; }

        private void Sample001ViewButtonExecute()
        {
            //// パラメータ渡し
            var p = new NavigationParameters();
            p.Add("MainWindow", this);

            _regionManager.RequestNavigate("ContentRegion", nameof(Sample001View), p);
        }

        public DelegateCommand Sample002ViewButton { get; }

        private void Sample002ViewButtonExecute()
        {
            //// パラメータ渡し
            var p = new NavigationParameters();
            p.Add("MainWindow", this);

            _regionManager.RequestNavigate("ContentRegion", nameof(Sample002View), p);
        }

        public DelegateCommand Sample003ViewButton { get; }

        private void Sample003ViewButtonExecute()
        {
            //// パラメータ渡し
            var p = new NavigationParameters();
            p.Add("MainWindow", this);

            _regionManager.RequestNavigate("ContentRegion", nameof(Sample003View), p);
        }

        public DelegateCommand Sample004ViewButton { get; }

        private void Sample004ViewButtonExecute()
        {
            //// パラメータ渡し
            var p = new NavigationParameters();
            p.Add("MainWindow", this);

            _regionManager.RequestNavigate("ContentRegion", nameof(Sample004View), p);
        }


        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

    }
}
