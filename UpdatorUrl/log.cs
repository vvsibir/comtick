using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdatorUrl
{
    public static class log
    {
        public static void Write(string msg)
        {
            Console.WriteLine(msg);
            Logged?.Invoke(null, new LogEventArgs() { Message = msg });
        }
        public static void Write(string msg, params string[] args)
        {
            Console.WriteLine(msg, args);
            Logged?.Invoke(null, new LogEventArgs() { Message = msg, Arguments = args });
        }
        public static void WriteTime(string msg, params object[] args)
        {
            Console.WriteLine("{0} {1}", DateTime.Now, string.Format(msg, args));
            Logged?.Invoke(null, new LogEventArgs() { Message = string.Format("{0} {1}", DateTime.Now, string.Format(msg, args)) });
        }

        public static event EventHandler<LogEventArgs> Logged;

        public class LogEventArgs : EventArgs
        {
            public string Message { get; set; }

            public string[] Arguments { get; set; }

            public string GetMessage()
            {
                return string.Format(Message, Arguments);
            }
        }

    }
}
