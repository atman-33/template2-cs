using Prism.Commands;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using System.Windows;
using Template2.Infrastructure;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private NavigationParameters _parameters = new NavigationParameters();

        public MainWindowViewModel(IRegionManager regionManager)
        {
            //// 画面遷移用（ナビゲーション）
            _regionManager = regionManager;

            //// 初期画面
            //// ex. _regionManager.RegisterViewWithRegion("ContentRegion", nameof(HomeView));

            //// DelegateCommandメソッドを登録
            WindowContentRendered = new DelegateCommand(WindowContentRenderedExecute);

            ExitButton = new DelegateCommand(ExitButtonExecute);

            HomeViewButton = new DelegateCommand(HomeViewButtonExecute);
            Sample001ViewButton = new DelegateCommand(Sample001ViewButtonExecute);
            Sample002ViewButton = new DelegateCommand(Sample002ViewButtonExecute);
            Sample003ViewButton = new DelegateCommand(Sample003ViewButtonExecute);
            Sample004ViewButton = new DelegateCommand(Sample004ViewButtonExecute);
            Sample005ViewButton = new DelegateCommand(Sample005ViewButtonExecute);
            Sample006ViewButton = new DelegateCommand(Sample006ViewButtonExecute);
            Sample007ViewButton = new DelegateCommand(Sample007ViewButtonExecute);
            Sample008ViewButton = new DelegateCommand(Sample008ViewButtonExecute);
            Sample009ViewButton = new DelegateCommand(Sample009ViewButtonExecute);
            Sample010ViewButton = new DelegateCommand(Sample010ViewButtonExecute);
            Sample011ViewButton = new DelegateCommand(Sample011ViewButtonExecute);

            //// 自身をパラメータに格納
            _parameters.Add(nameof(MainWindowViewModel), this);
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// ローディング中表示のVisibility
        /// </summary>
        private Visibility _loadingBarVisibility = Visibility.Visible;
        public Visibility LoadingBarVisibility
        {
            get { return _loadingBarVisibility; }
            set { SetProperty(ref _loadingBarVisibility, value); }
        }

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

        private bool _dbConnectionIsChecked = false;
        public bool DBConnectionIsChecked
        {
            get { return _dbConnectionIsChecked; }
            set { SetProperty(ref _dbConnectionIsChecked, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand WindowContentRendered { get; }

        private async void WindowContentRenderedExecute()
        {
            //// ※注意：App.xaml.cs内のDispatcherUnhandledExceptionでは、
            //// コンストラクタ内の例外処理はキャッチできない。
            //// そのため、コンストラクタ内のDB接続処理はContentRenderedイベントで処理し、
            //// DB接続エラーをキャッチする方が良い。

            await Task.Run(() => DBConnectionCheck());
            await Task.Delay(1000);

            _regionManager.RequestNavigate("ContentRegion", nameof(HomeView), _parameters);
        }

        public DelegateCommand ExitButton { get; }
        private void ExitButtonExecute()
        {
            Application.Current.Shutdown();
        }

        public DelegateCommand HomeViewButton { get; }
        private void HomeViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(HomeView), _parameters);
        }

        public DelegateCommand Sample001ViewButton { get; }

        private void Sample001ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample001View), _parameters);
        }

        public DelegateCommand Sample002ViewButton { get; }

        private void Sample002ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample002View), _parameters);
        }

        public DelegateCommand Sample003ViewButton { get; }

        private void Sample003ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample003View), _parameters);
        }

        public DelegateCommand Sample004ViewButton { get; }

        private void Sample004ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample004PageListView), _parameters);
        }
        public DelegateCommand Sample005ViewButton { get; }

        private void Sample005ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample005View), _parameters);
        }

        public DelegateCommand Sample006ViewButton { get; }
        private void Sample006ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample006View), _parameters);
        }

        public DelegateCommand Sample007ViewButton { get; }
        private void Sample007ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample007View), _parameters);
        }

        public DelegateCommand Sample008ViewButton { get; }
        private void Sample008ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample008View), _parameters);
        }

        public DelegateCommand Sample009ViewButton { get; }
        private void Sample009ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample009View), _parameters);
        }

        public DelegateCommand Sample010ViewButton { get; }
        private void Sample010ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample010View), _parameters);
        }

        public DelegateCommand Sample011ViewButton { get; }
        private void Sample011ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample011View), _parameters);
        }


        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private void DBConnectionCheck()
        {
            try
            {
                //// DB接続確認
                Factories.Open();
                DBConnectionIsChecked = true;
                LoadingBarVisibility = Visibility.Collapsed;
            }
            catch (Exception e)
            {
                DBConnectionIsChecked = false;

                throw new Exception(e.Message, e);
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion
    }
}
