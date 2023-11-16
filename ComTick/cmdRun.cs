using SharedTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComTick
{
    public static class cmdRun
    {
        public static void runCommands(string cmds)
        {
            log.WriteTime("Get commands:\n{0}", cmds);
            var cmdList = cmds.Split('\n', '\r');
            foreach (string s in cmdList)
            {
                var cmd = s.Replace("<br>", "").Trim();
                if (string.IsNullOrEmpty(cmd)) continue;
                log.WriteTime("run '{0}'", cmd);
                try
                {
                    run(cmd);
                }
                catch (Exception ex)
                {
                    log.logError_full(ex);
                }
            }
        }


        public static void run(string cmdLine)
        {
            string cmd, args;
            parseCommandLine(cmdLine, out cmd, out args);

            var a = CommandRepository.Get(cmd);
            var a2 = CommandRepository.GetWithArg(cmd);
            if (a == null && a2 == null)
            {
                log.Write("{0} {1} is null", DateTime.Now, cmd);
                return;
            }
            if (string.IsNullOrEmpty(args) && a != null)
            {
                log.Write($"run no args: {cmd}");
                a();
            }
            else if (a2 != null)
            {
                if (!string.IsNullOrEmpty(args))
                {
                    log.Write($"run with args: {cmd}({args})");
                    a2(args);
                }
                else
                {
                    log.Write($"run with null args: {cmd}(null)");
                    a2(null);
                }
            }

        }

        public static void parseCommandLine(string cmdLine, out string cmd, out string args)
        {
            commandParse.parseCommandLine(cmdLine, out cmd, out args);
            log.Write("cmd: {0}", cmd);
            log.Write("arg: {0}", args);
        }
    }
}
