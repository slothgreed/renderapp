using System;
using System.Windows.Threading;

namespace KI.Foundation.Core
{
    /// <summary>
    /// タイマー
    /// </summary>
    public class KITimer
    {
        /// <summary>
        /// タイマー
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        /// タイマーの初期化
        /// </summary>
        /// <param name="interval">間隔</param>
        /// <param name="timer">タイマーイベント</param>
        public KITimer(int interval, OnTimerEvent timer)
        {
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(interval);
            this.timer.Tick += new EventHandler(timer);
        }

        /// <summary>
        /// タイマーイベント
        /// </summary>
        /// <param name="source">イベント元</param>
        /// <param name="e">イベント</param>
        public delegate void OnTimerEvent(object source, EventArgs e);

        /// <summary>
        /// カウンター
        /// </summary>
        public int TimerCount { get; private set; }

        /// <summary>
        /// タイマースタート
        /// </summary>
        public void Start()
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
            }

            TimerCount = 0;
        }

        /// <summary>
        /// タイマーストップ
        /// </summary>
        public void Stop()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
        }
    }
}
