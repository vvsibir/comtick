using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComTick
{
    public static class commandParse
    {
        /// <summary>
        /// без учета кавычек
        /// </summary>
        /// <param name="cmdLine"></param>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void parseCommandLine(string cmdLine, out string cmd, out string args)
        {
            cmdLine = cmdLine.Trim();
            cmd = string.Empty;
            args = string.Empty;
            var i = cmdLine.IndexOf(' ');
            if (i > -1)
            {
                cmd = cmdLine.Substring(0, i);
                args = cmdLine.Substring(i).Trim();
            }
            else
            {
                cmd = cmdLine;
            }
        }

        /// <summary>
        /// с кавычками
        /// </summary>
        /// <param name="cmdLine"></param>
        /// <param name="cmd"></param>
        /// <param name="arg"></param>
        public static void parse(string cmdLine, out string cmd, out string arg)
        {
            cmdLine = cmdLine.Trim();
            var iQuote = cmdLine.IndexOf('\"');
            var iSpace = cmdLine.IndexOf(' ');
            if (iSpace < 0)
            {
                cmd = cmdLine;
                arg = null;
                return;
            }
            if (iQuote > iSpace)
            {
                cmd = cmdLine.Substring(0, iSpace);
                arg = cmdLine.Substring(0, iSpace);
                return;
            }
            var iQuote2 = cmdLine.IndexOf('\"', iQuote + 1);
            
            cmd = cmdLine.Substring(iQuote, iQuote2 + 1 - iQuote);
            if (iQuote2 == cmdLine.Last())
            {
                arg = null;
            }
            else
            {
                arg = cmdLine.Substring(iQuote2 + 1).Trim();
            }
        }
    }
}
