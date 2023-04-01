using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Template2.Domain;

namespace Template2.WPF.BackgroundWorkers
{
    internal static class Sample007ViewBackgroundWorker
    {
        /// <summary>
        /// タイマー
        /// </summary>
        private static Timer _timer;

        /// <summary>
        /// 処理中の時True
        /// </summary>
        /// <remarks>
        /// フラグのコメントは、どの状態でTrueか書くと分かり易い
        /// </remarks>
        private static bool _isWork = false;

        /// <summary>
        /// バックグラウンドの実行処理（アクション）
        /// </summary>
        /// <remarks>
        /// 引数を利用する際はジェネリック型を指定する。
        /// 例）Action<bool>
        /// </remarks>
        private static event Action _backgroundAction;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Sample007ViewBackgroundWorker()
        {
            _timer = new Timer(Callback);
        }

        /// <summary>
        /// スタート
        /// </summary>
        internal static void Start()
        {
            //// 値は設定ファイル等から読み込む方が良い
            _timer.Change(0, Shared.TimerPeriod);
            Debug.WriteLine("タイマーStart");
        }

        /// <summary>
        /// ストップ
        /// </summary>
        internal static void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            Debug.WriteLine("タイマーStop");
        }

        /// <summary>
        /// アクションに追加
        /// </summary>
        public static void Add(Action action)
        {
            bool contains = false;
            if (_backgroundAction != null)
            {
                contains = _backgroundAction.GetInvocationList().Contains(action);
            }

            if (!contains)
            {
                _backgroundAction += action;
            }
        }

        /// <summary>
        /// アクションから除去
        /// </summary>
        public static void Remove(Action action)
        {
            _backgroundAction -= action;
        }

        /// <summary>
        /// コールバック（指定時間間隔に実行）
        /// </summary>
        /// <param name="o">オブジェクト</param>
        private static void Callback(object o)
        {
            //Debug.WriteLine("Callback : _isWork => " + _isWork.ToString());

            if (_isWork)
            {
                return;
            }

            try
            {
                _isWork = true;

                Debug.WriteLine("Callbck : " + (DateTime.Now).ToString("HH:mm:ss"));

                //// 登録Actionを実行
                _backgroundAction?.Invoke();
            }
            finally
            {
                _isWork = false;
            }
        }
    }
}
