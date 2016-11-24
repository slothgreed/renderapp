using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using OpenTK;
namespace RenderApp.Utility
{

    public class RATimer
    {
        public delegate void OnTimerEvent(object source, EventArgs e);

        DispatcherTimer m_Timer;
        private int m_TimerCount;
        public int Count { get { return m_TimerCount; } }
        public int AngleCount { get { return m_TimerCount % 360; } }
        public float RadianCount { get { return (float)MathHelper.DegreesToRadians(m_TimerCount % 360); } }
        public bool IsEnable
        {
            get
            {
                if (m_Timer == null)
                {
                    return false;
                }
                return m_Timer.IsEnabled;

            }
        }
        /// <summary>
        /// タイマーの初期化
        /// </summary>
        public RATimer(int interval, OnTimerEvent timer)
        {
            m_Timer = new DispatcherTimer();
            m_Timer.Interval = TimeSpan.FromMilliseconds(32);
            m_Timer.Tick += new EventHandler(timer);
        }
        /// <summary>
        /// タイマースタート
        /// </summary>
        public void Start()
        {
            if(!m_Timer.IsEnabled)
            {
                m_Timer.Start();
            }
            m_TimerCount = 0;
        }
        /// <summary>
        /// タイマーストップ
        /// </summary>
        public void Stop()
        {
            if(m_Timer.IsEnabled)
            {
                m_Timer.Stop();
            }
        }

        public void AddTimer()
        {
            m_TimerCount++;
        }

    }
}
