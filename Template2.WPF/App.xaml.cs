using Prism.Ioc;
using System.Windows;
using System.Windows.Threading;
using Template2.Domain.Exceptions;
using Template2.WPF.ViewModels;
using Template2.WPF.Views;

namespace Template2.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// ログ
        /// </summary>
        private static log4net.ILog _logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            //// 例外処理をキャッチする処理
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.Error(e.Exception.Message, e.Exception);    //// ログ出力

            MessageBoxImage icon = MessageBoxImage.Error;
            string caption = "エラー";
            var exceptionBase = e.Exception as ExceptionBase;   //// 型が異なる場合はnullが返る
            if (exceptionBase != null)
            {
                if (exceptionBase.Kind == ExceptionBase.ExceptionKind.Info)
                {
                    icon = MessageBoxImage.Information;
                    caption = "情報";
                }
                else if (exceptionBase.Kind == ExceptionBase.ExceptionKind.Warning)
                {
                    icon = MessageBoxImage.Warning;
                    caption = "警告";
                }
            }
            MessageBox.Show(e.Exception.Message, caption, MessageBoxButton.OK, icon);

            e.Handled = true;   //// true:アプリケーションが落ちない
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //// ナビゲーション画面 ※パラメータを引き渡す場合は、ViewModel に INavigationAware 実装が必要
            //// ex. containerRegistry.RegisterForNavigation<XXXView>();
            
            containerRegistry.RegisterForNavigation<HomeView>();
            containerRegistry.RegisterForNavigation<Sample001View>();
            containerRegistry.RegisterForNavigation<Sample002View>();
            containerRegistry.RegisterForNavigation<Sample003View>();
            containerRegistry.RegisterForNavigation<Sample004PageListView>();
            containerRegistry.RegisterForNavigation<Sample004PageEditingView>();
            containerRegistry.RegisterForNavigation<Sample004PagePreviewView>();
            containerRegistry.RegisterForNavigation<Sample005View>();
            containerRegistry.RegisterForNavigation<Sample006View>();
            containerRegistry.RegisterForNavigation<Sample007View>();
            containerRegistry.RegisterForNavigation<Sample008View>();
            containerRegistry.RegisterForNavigation<Sample009View>();

            //// ダイアログ画面（別画面に表示） ※ViewModel に IDialogAware 実装が必要
            //// ex. containerRegistry.RegisterDialog<XXXView, XXXViewModel>();

            containerRegistry.RegisterDialog<Sample002View, Sample002ViewModel>();
            containerRegistry.RegisterDialog<Sample004PageEditingView, Sample004PageEditingViewModel>();

        }
    }
}
