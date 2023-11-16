using SharedTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ComTick
{
    public class netServer
    {
        public static string Host { get; set; } 
        public static int Port { get; set; }
        public static void SetNet(string host, string port)
        {
            Host = host;
            int.TryParse(port, out int p);
            Port = p;
        }

        static List<Thread> ths = new List<Thread>();

        public static void RunThread()
        {
            ThreadStart ts = new ThreadStart(serverProcess);

            Thread t = new Thread(ts);
            t.Start();
            ths.Add(t);
        }


        /// <summary>
        /// 1. сделать список клиентов и инфу по ним
        /// 2. параллельный поток, который смотрит TCP-клиента и его инфу 
        ///   2.2. если не авторизован и времени больше 3 минут - рубим
        ///   2.3. если молчит больше 5-10 минут - рубим
        /// 3. авторизацию клиента замутить только через пароль, зато длинный 
        /// </summary>
        static List<netClient> clients = new List<netClient>();

        static void serverProcess()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = Port;
                //IPAddress localAddr = IPAddress.Parse(Host);

                // TcpListener server = new TcpListener(port);
                if(Host?.ToUpper() == "ANY" || string.IsNullOrEmpty(Host?.Trim()))
                    server = new TcpListener(IPAddress.Any, port);
                if (!string.IsNullOrEmpty(Host))
                    server = new TcpListener(IPAddress.Parse(Host), port);
                else
                    server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    log.Write("TCPCl: Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient tcpclient = server.AcceptTcpClient();
                    log.Write("TCPCl: Connected!");

                    netClient clientObject = new netClient(tcpclient);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                    ths.Add(clientThread);
                }
            }
            catch (SocketException e)
            {
                log.Write("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        internal static void Remove(netClient netClient)
        {
            clients.Remove(netClient);
        }

        public static void StopAll()
        {
            ths.ForEach(t => t.Abort());
            clients.ToArray().ForEach(nc => { nc.Close(); Remove(nc); });
        }

        public static void StartNew(string host, string port)
        {
            SetNet(host, port);
            RunThread();
        }
    }
}

