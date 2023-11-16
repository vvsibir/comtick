using SharedTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ComTick
{
    class netClient
    {
        TcpClient _tcpclient;
        protected internal NetworkStream Stream { get; private set; }

        public netClient(TcpClient tcpclient)
        {
            _tcpclient = tcpclient;
        }
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }
        public void Process()
        {
            try
            {
                Stream = _tcpclient.GetStream();
                // получаем имя пользователя
                string message = GetMessage();

                if (message != cur.startArgs.PASSNET) return;
                Console.WriteLine(message);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage()?.Trim();
                        if(message?.ToUpper()=="EXIT")
                        {
                            log.Write("net: exit");
                            break;
                        }
                        log.Write("net: " + message);

                    }
                    catch(Exception ex)
                    {
                        log.Write("net: error exit " + ex.Message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                netServer.Remove(this);
            }
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (_tcpclient != null)
                _tcpclient.Close();
        }
    }
}
