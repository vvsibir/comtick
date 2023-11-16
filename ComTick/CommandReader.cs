using NoSleep;
using SharedTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComTick
{
    public class CommandReader
    {
        protected System.Timers.Timer tmr;
        public CommandReader()
        {
            init();
        }

        private void init()
        {
            tmr = new System.Timers.Timer();
            tmr.Interval = 1000;
            tmr.AutoReset = true;
            tmr.Elapsed += Tmr_Elapsed;
        }

        public void SetInterval(int i)
        {
            tmr.Interval = i;
        }
        public void Run()
        {
            OnTimerTick();
            tmr.Start();
        }
        /// <summary>
        /// нужен для того, чтобы при выходе/перезагрузке/выключении не запустился заново, т.е. стопим таймер перед выключением
        /// </summary>
        public bool Stopped { get; set; }
        internal void Stop()
        {
            tmr.Stop();
            Stopped = true;
            tmr.Dispose();
            tmr = null;
        }
        const EXECUTION_STATE ExecutionMode = 
            EXECUTION_STATE.ES_CONTINUOUS 
            //| EXECUTION_STATE.ES_DISPLAY_REQUIRED 
            | EXECUTION_STATE.ES_SYSTEM_REQUIRED 
            //| EXECUTION_STATE.ES_AWAYMODE_REQUIRED
            ;
        public bool NoSleep { get; set; } = true;
        private void Tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if(NoSleep) WinU.SetThreadExecutionState(ExecutionMode);
                tmr.Stop();
                OnTimerTick();
            }
            finally
            {
                if (!Stopped) tmr.Start();
            }
        }

        public virtual void OnTimerTick()
        {
            log.Write("tick");
        }
    }
}
