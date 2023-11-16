using SharedTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ComTick
{
    public class CommandURLReader : CommandReader
    {
        public string URLCmd { get; set; }
        public int Interval { get => (int)tmr.Interval; set => tmr.Interval = value; }

        public override void OnTimerTick()
        {
            //base.OnTimerTick();

            try
            {
                var cmds = getCommands();

                if (!string.IsNullOrEmpty(cmds?.Trim()))
                {
                    cmdRun.runCommands(cmds);
                }
            }
            catch(Exception ex)
            {
                log.WriteTime("Error: {0}", ex.Message);
            }
        }



        private string getCommands()
        {
            return htmlNet.getHTML(cur.startArgs.getWithKeys(URLCmd));
        }
    }
}
