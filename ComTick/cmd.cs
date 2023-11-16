using SharedTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComTick
{
    /// <summary>
    /// Набор выполняемых методов. 
    /// Выполняемым может только public и может содержать один string-аргумент, или не содержать аргументов вовсе
    /// </summary>
    public static class cmd
    {
        public static void Help()
        {
            List<string> res = new List<string>();

            log.Write(
                string.Join("\r\n",
                    typeof(cmd).GetMethods().Select(m => $"{m.Name}(" +
                    $"{(m.GetParameters()?.Count() > 0 ? m.GetParameters()[0].Name : null)}" +
                    $") - return {m.ReturnParameter?.ParameterType.Name}")
            ));
            GetLog();
        }

        public static void Update()
        {
            Update(cur.startArgs.getWithKeys(cur.startArgs.URLUpdate));
        }
        public static void Update(string url)
        {
            var args = string.Format("\"UrlUpdate:{0}\" \"targetFolder:{2}\" \"run:{1}\"", url, Environment.CommandLine, "DOWNLOAD");
            RunCommand("updatorurl", args);
            Exit();
        }
        public static void UpdateBat()
        {
            createAndRunUpdateBat(
                "update.bat",
                null,
                "start comtick.exe"
                );

        }
        public static void UpdateWBat()
        {
            createAndRunUpdateBat(
                "updatew.bat",
                null,
                "start wcomtick.exe"
                );
        }
        static void createAndRunUpdateBat(string batName, StringBuilder commonPart, params string[] addedRowsForUpdadeBat)
        {
            var sb = commonPart ?? new StringBuilder();
            cur.startArgs.UrlsToLoad?.ForEach((s) => sb.AppendLine(string.Format("UpdatorUrl UrlUpdate:{0} PageToLoad:dl.php?file= TargetFolder:DOWNLOAD", cur.startArgs.getWithKeys(s))));
            sb.AppendLine(@"copy DOWNLOAD\* * /y");
            addedRowsForUpdadeBat.ForEach(ss => sb.AppendLine(ss));
            var fname = new FileInfo(batName);
            File.WriteAllText(fname.FullName, sb.ToString());
            RunCommand(fname.FullName);
            Exit();
        }
        public static void run(string cmdLine)
        {
            commandParse.parseCommandLine(cmdLine, out string cmd, out string arg);
            RunCommand(cmd, arg);
        }
        static void RunCommand(string proc, string args = null)
        {
            Process pr = new Process();
            pr.StartInfo = new ProcessStartInfo(proc) { CreateNoWindow = true, UseShellExecute = false };
            log.WriteTime("run shell: {0}", proc);
            if (!string.IsNullOrEmpty(args))
            {
                log.WriteTime("arg shell: {0}", args);
                pr.StartInfo.Arguments = args;
            }
            pr.Start();
        }
        public static void GetProcesses(string pattern)
        {
            Process.GetProcesses().OrderBy(p=> p.ProcessName).ForEach(p => { string s = $"{p.ProcessName} [{p.Id}]"; log.Write(s); });
            GetLog();
        }
        /// <summary>
        /// kill process
        /// </summary>
        /// <param name="pattern">PID or name</param>
        public static void Kill(string pattern)
        {
            int pid;
            if(int.TryParse(pattern, out pid))
            {
                log.Write($"Kill process by ID {pid}:");
                Process.GetProcesses().Where(p => p.Id == pid).ForEach(p => { string s = $"{p.ProcessName} [{p.Id}] killed"; p.Kill(); log.Write(s); });
                return;
            }
            log.Write($"Kill processes by pattern '{pid}':");
            Regex r = new Regex(pattern);
            var pp = Process.GetProcesses().Where(p => r.IsMatch(p.ProcessName));
            pp.ForEach(p => { string s = $"{p.ProcessName} [{p.Id}] killed"; p.Kill(); log.Write(s); });
        }

        private static bool manualExit = false;
        public static bool ManualExit { get => manualExit; set => manualExit = value; }
        public static void Exit()
        {
            cur.cmdr.Stop();
            Exiting?.Invoke(null, null);
            if (!manualExit) Environment.Exit(0);
        }
        public static event EventHandler Exiting;

        public static void Reboot()
        {
            cur.cmdr.Stop();
            rebooter r = new rebooter();
            r.halt(true, true);
            Environment.Exit(0);
        }
        public static void PowerOff()
        {
            cur.cmdr.Stop();
            rebooter r = new rebooter();
            r.halt(false, true);
            Environment.Exit(0);
        }
        public static void Interval(string i)
        {
            cur.cmdr.SetInterval(Convert.ToInt32(i));
            log.WriteTime(string.Format("Set interval:{0}", i));
        }
        public static void VolMute()
        {
            cSpeech.soundManager.VolumeMute();
        }
        public static void VolUp()
        {
            cSpeech.soundManager.VolumeUp();
        }
        public static void VolDown()
        {
            cSpeech.soundManager.VolumeDown();
        }
        public static void Vol(string procent)
        {
            cSpeech.soundManager.VolumeDown();
        }

        public static void GetAudioDevices()
        {
            cSpeech.VolumeManager.GetDevCapsPlayback();
        }

        /// <summary>
        /// TODO: upload local log's files
        /// </summary>
        public static void GetLog()
        {
            Upload("wcomtick.log");
        }

        /// <summary>
        /// TODO: upload local log's files
        /// </summary>
        public static void ClearLog()
        {
            log.Clear();
            log.WriteTime("log cleared");
        }

        public static void Torrent(string torrent)
        {
            log.Write("download torrent: {0}", torrent);
            WebClient wc = new WebClient();
            wc.Proxy = htmlNet.getProxy();
            Uri uri = new Uri(torrent);
            string filename = null;
            if (uri.IsFile)
            {
                filename = System.IO.Path.GetFileName(uri.LocalPath);
            }
            else
            {
                filename = Guid.NewGuid() + ".torrent";
            }

            log.Write("torrent file: {0}", filename);
            var fi = new FileInfo(System.IO.Path.Combine("D:\\torrents\\", filename));
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();

            }
            wc.DownloadFile(torrent, fi.FullName);
            log.Write("file loaded: {0}", fi.FullName);
        }

        public static void ScreenShot()
        {
            ScreenShot("0"); // первый монитор
        }
        public static void ScreenShot(string num)
        {
            if (string.IsNullOrEmpty(cur.startArgs.UploadPath))
            {
                log.Write("{0} is null", nameof(cur.startArgs.UploadPath));
                return;
            }
            if (string.IsNullOrEmpty(cur.startArgs.UploadUrl))
            {
                log.Write("{0} is null", nameof(cur.startArgs.UploadUrl));
                return;
            }

            byte[] bytes = imageHelper.ImageToBytes(imageHelper.getImage(num), System.Drawing.Imaging.ImageFormat.Png);
            string filename = string.Format("scr_{0:yyyyMMdd_HHmmss}.png", DateTime.Now);
            string urlupload = cur.startArgs.getWithKeys(cur.startArgs.UploadUrl);

            htmlNet.Upload(bytes, filename, cur.startArgs.getWithKeys(cur.startArgs.UploadPath), urlupload);

        }
        /// <summary>
        /// делает скриншот всего экрана с масштабированием
        /// </summary>
        /// <param name="args">scale:4,format:png</param>
        public static void SCR(string args)
        {
            var scri = new imageHelper.scrInfo(args);
            var img = imageHelper.GetImageFullScreen(scri.Scale);
            uploadImage(img, scri.Format, "scr_{0:yyyyMMdd_HHmmss}.{1}", "upload screen: {0}");
        }
        static void uploadImage(System.Drawing.Image img, System.Drawing.Imaging.ImageFormat format, string filenameFormat, string logText)
        {
            var bytes = imageHelper.ImageToBytes(img, format);
            string filename = string.Format(filenameFormat, DateTime.Now, format);
            string urlupload = cur.startArgs.getWithKeys(cur.startArgs.UploadUrl);
            log.Write(logText, filename);
            htmlNet.Upload(bytes, filename, cur.startArgs.getWithKeys(cur.startArgs.UploadPath), urlupload);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale">во сколько раз уменьшить (0 и меньше - игнорируются)</param>
        /// <returns></returns>

        public static void Upload(string filename)
        {
            htmlNet.Upload(filename, cur.startArgs.getWithKeys(cur.startArgs.UploadPath), cur.startArgs.getWithKeys(cur.startArgs.UploadUrl));
        }

        public static void Dir(string dir)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            if(!d.Exists)
            {
                log.Write("'{0}' not exist", dir);
                return;
            }
            var ff = d.GetFiles();
            ff.ForEach(f => log.Write(f.Name));
        }
        public static void DirFull(string dir)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            if (!d.Exists)
            {
                log.Write("'{0}' not exist", dir);
                return;
            }
            d.GetDirectories().ForEach(d2 => log.Write(d2.FullName));
            d.GetFiles().ForEach(f => log.Write(f.FullName));
        }
        public static void Keys(string keys)
        {
            //if (Application.OpenForms.Count == 0) return;
            //var f = Application.OpenForms[0];

            keys.Split(' ').ForEach(k => keyImitation.ShortCut(k));
        }
        public static void Mouse(string args)
        {
            //if (Application.OpenForms.Count == 0) return;
            //var f = Application.OpenForms[0];

            var aa = args.Split(' ');

            var pp = aa[0].Split('x', ';', '-', ',');
            Point p = new Point(Convert.ToInt32(pp[0]), Convert.ToInt32(pp[1]));
            Cursor.Position = p;
            if (aa.Length > 1 && (aa[1].ToUpper().StartsWith("R")))
                mouseImitation.clickRight(p);
            else
                mouseImitation.clickLeft(p);

        }
        public static void Say(string args)
        {
            cSpeech.speech.Speak(args);
        }
        public static void SayAsync(string args)
        {
            cSpeech.speech.SpeakAsync(args);
        }

        public static void SerialPort(string args)
        {
            serialMaster.PORT = args;
        }
        public static void SerialBaude(string args)
        {
            serialMaster.BAUDE = Convert.ToInt32(args);
        }
        public static void SerialSend(string args)
        {
            serialMaster.Send(args);
        }
        public static void SerialDetect(string args)
        {
            var s = serialMaster.AutodetectArduinoPort(args);
            log.Write($"'{args}' detected: {s}");
        }

        public static void NoSleep (string args)
        {
            var b = bool.Parse(args);
            cur.cmdr.NoSleep = b;
        }

        public static void Cam()
        {
            var img = cam.shot();
            uploadImage(img, System.Drawing.Imaging.ImageFormat.Jpeg, "cam_{0:yyyyMMdd_HHmmss}.{1}", "upload camshot: {0}");

        }
        public static void Cam(string args)
        {
            log.Write($"set cam: {args}");
            cam.numCam = int.Parse(args);
            var img = cam.shot();
            uploadImage(img, System.Drawing.Imaging.ImageFormat.Jpeg, "cam_{0:yyyyMMdd_HHmmss}.{1}", "upload camshot: {0}");

        }

        /// <summary>
        /// запускает слушателя с "ipadress:port" для приема команд
        /// </summary>
        /// <param name="args"></param>
        public static void netcl(string args)
        {
            if(args.ToLower()=="stop")
            {
                netServer.StopAll();
                return;
            }

            var ss = args.Split(' ', ',', '/', ':').Select(s => s.Trim()).ToArray();
            netServer.StartNew(ss[0], ss[1]);
        }
    }
}
