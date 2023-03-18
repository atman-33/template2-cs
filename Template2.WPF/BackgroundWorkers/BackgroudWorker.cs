using System;
using System.Diagnostics;
using System.Threading;
using Template2.Domain;
using Template2.Domain.StaticValues;
using Template2.Infrastructure;

namespace Template2.WPF.BackgroundWorkers
{
    internal static class BackgroudWorker
    {
        /// <summary>
        /// タイマー
        /// </summary>
        private static Timer _timer;

        /// <summary>
        /// 処理中の時True
        /// </summary>
        private static bool _isWork = false;    //// フラグのコメントは、どの状態でTrueか書くと分かり易い

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static BackgroudWorker()
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
        /// コールバック
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

                //// 処理を記述

                //// 時刻を更新
                Shared.Sample007ViewUpdatedTime = DateTime.Now;
                Debug.WriteLine("Callbck : " + Shared.Sample007ViewUpdatedTime.ToString("HH:mm:ss"));

                //// データを更新
                var repository = Factories.CreateWorkerMst();
                StaticWorkerMst.Update(repository);
            }
            finally
            {
                _isWork = false;
            }
        }
    }
}
