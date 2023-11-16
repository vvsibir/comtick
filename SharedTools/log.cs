using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SharedTools
{
    public static class log
    {
        public static void logError_full(Exception ex)
        {
            log.WriteTime("Error: {0}", ex.Message);
            if (ex.InnerException != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("! Full errors messages stack:");
                var exx = ex;
                while (exx != null)
                {
                    sb.AppendLine(exx.Message);
                    exx = exx.InnerException;
                }
                log.Write(sb.ToString());
            }
        }
        public static void Write(string msg)
        {
            Console.WriteLine(msg);
            logHtml(msg);
            Logged?.Invoke(null, new LogEventArgs() { Message = msg });
        }
        public static void Write(string msg, params object[] args)
        {
            Console.WriteLine(msg, args);
            logHtml(msg, args);
            Logged?.Invoke(null, new LogEventArgs() { Message = msg, Arguments = args });
        }
        public static void WriteTime(string msg, params object[] args)
        {
            Console.WriteLine("{0} {1}", DateTime.Now, string.Format(msg, args));
            logHtml("{0} {1}", DateTime.Now, string.Format(msg, args));
            Logged?.Invoke(null, new LogEventArgs() { Message = string.Format("{0} {1}", DateTime.Now, string.Format(msg, args)) });
        }

        public static string URLLog { get; set; }
        static void logHtml(string msg, params object[] args)
        {
            try
            {
                if (string.IsNullOrEmpty(URLLog)) return;
                var request = (HttpWebRequest)WebRequest.Create(URLLog);

                var postData = "msg=" + string.Format(msg, args);
                //postData += "&thing2=world";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("HTMLLog error: {0}", ex.Message));
                //throw;
            }
        }

        public static event EventHandler<LogEventArgs> Logged;
        public static event EventHandler<EventArgs> ClearLog;

        public class LogEventArgs : EventArgs
        {
            public string Message { get; set; }

            public object[] Arguments { get; set; }

            public string GetMessage()
            {

                return Arguments == null ? Message : string.Format(Message, Arguments);
            }
            public string GetMessageTime()
            {

                return Arguments == null ?
                    string.Format("{0} {1}", DateTime.Now, Message) :
                    string.Format("{0} {1}", DateTime.Now, string.Format(Message, Arguments));
            }
        }

        public static void Clear()
        {
            ClearLog?.Invoke(null, null);
        }
    }
}
