using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Threading.Tasks;
using System.Windows;
using Template2.Infrastructure;
using Template2.WPF.Events;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// MainWindowViewのContentRegion名称。他ViewModelから参照可能なようにStatic
        /// </summary>
        public static string MainWindowContentRegionName = "ContentRegion";

        public MainWindowViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            base._regionManager = regionManager;
            base._eventAggregator = eventAggregator;

            //// 初期画面 => WindowContentRendered後に遷移するためコメントアウト
            //_regionManager.RegisterViewWithRegion(_contentRegionName, nameof(HomeView));

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
            Sample012ViewButton = new DelegateCommand(Sample012ViewButtonExecute);

            //// EventAggregatorメソッドを登録
            _eventAggregator.GetEvent<MainWindowCallMethodEvent>().Subscribe(HandleCallMethodEvent);
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Subscribe(HandleSetSubTitleEvent);
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private readonly string _contentRegionName = MainWindowContentRegionName;
        public string ContentRegionName
        {
            get { return _contentRegionName; }
        }

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
        /// サブタイトル
        /// </summary>
        private string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set { SetProperty(ref _subTitle, value); }
        }

        private bool _dbConnectionIsChecked = false;
        public bool DBConnectionIsChecked
        {
            get { return _dbConnectionIsChecked; }
            set { SetProperty(ref _dbConnectionIsChecked, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand WindowContentRendered { get; }

        private async void WindowContentRenderedExecute()
        {
            //// ※注意：App.xaml.cs内のDispatcherUnhandledExceptionでは、
            //// コンストラクタ内の例外処理はキャッチできない。
            //// そのため、コンストラクタ内のDB接続処理はContentRenderedイベントで処理し、
            //// DB接続エラーをキャッチする方が良い。

            await Task.Delay(500);
            await Task.Run(() => DBConnectionCheck());

            _regionManager.RequestNavigate(_contentRegionName, nameof(HomeView));
        }

        public DelegateCommand ExitButton { get; }
        private void ExitButtonExecute()
        {
            Application.Current.Shutdown();
        }

        public DelegateCommand HomeViewButton { get; }
        private void HomeViewButtonExecute()
        {
            if(!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(HomeView));
        }

        public DelegateCommand Sample001ViewButton { get; }

        private void Sample001ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample001View));
        }

        public DelegateCommand Sample002ViewButton { get; }

        private void Sample002ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample002View));
        }

        public DelegateCommand Sample003ViewButton { get; }

        private void Sample003ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample003View));
        }

        public DelegateCommand Sample004ViewButton { get; }

        private void Sample004ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample004PageListView));
        }
        public DelegateCommand Sample005ViewButton { get; }

        private void Sample005ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample005View));
        }

        public DelegateCommand Sample006ViewButton { get; }
        private void Sample006ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample006View));
        }

        public DelegateCommand Sample007ViewButton { get; }
        private void Sample007ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample007View));
        }

        public DelegateCommand Sample008ViewButton { get; }
        private void Sample008ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample008View));
        }

        public DelegateCommand Sample009ViewButton { get; }
        private void Sample009ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample009View));
        }

        public DelegateCommand Sample010ViewButton { get; }
        private void Sample010ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample010View));
        }

        public DelegateCommand Sample011ViewButton { get; }
        private void Sample011ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample011View));
        }

        public DelegateCommand Sample012ViewButton { get; }
        private void Sample012ViewButtonExecute()
        {
            if (!canNavigate())
            {
                return;
            }

            _regionManager.RequestNavigate(_contentRegionName, nameof(Sample012View));
        }
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
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

        /// <summary>
        /// MainWindowEventがPublishされた際の処理
        /// </summary>
        /// <param name="delegateCommandName"></param>
        private void HandleCallMethodEvent(string delegateCommandName)
        {
            Type type = this.GetType();

            var property = type.GetProperty(delegateCommandName);
            var getter = property.GetGetMethod();
            var command = getter.Invoke(this, null) as DelegateCommand;

            command.Execute();
        }

        /// <summary>
        /// サブタイトルをセット
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void HandleSetSubTitleEvent(string subTitle)
        {
            SubTitle = subTitle;
        }

        #endregion
    }
}
