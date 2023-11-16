using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cSpeech
{
    public static class log
    {
        public static void Write(string msg, params object[] args)
        {
            Console.WriteLine(msg, args);
            Logged?.Invoke(null, new LogEventArgs() { Message = msg, Arguments = args });
        }

        public static event EventHandler<LogEventArgs> Logged;

        public class LogEventArgs : EventArgs
        {
            public string Message { get; set; }

            public object[] Arguments { get; set; }

            public string GetMessage()
            {
                return string.Format(Message, Arguments);
            }
        }

    }
}
