using SharedTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace UpdatorUrl
{
    public class UpdateProcess
    {
        /// <summary>
        /// локальная папка для сохранения
        /// </summary>
        public static string FolderToSave = "DOWNLOAD";
        /// <summary>
        /// имя файла для получения списка файлов
        /// </summary>
        public static string FileListName = "files.php";

        StartUpRoutine _sa;
        public UpdateProcess() { }
        public UpdateProcess(StartUpRoutine sa)
        {
            _sa = sa;
        }

        public void Update()
        {
            if (!string.IsNullOrEmpty(_sa.URLJSON)) _sa = GetJSONFromURL(_sa);
            GetFromUrl(_sa);
            Run(_sa);

        }

        private StartUpRoutine GetJSONFromURL(StartUpRoutine sa)
        {
            string html = htmlNet.getHTML(sa.URLJSON);

            var res = JsonConvert.DeserializeObject<StartUpRoutine>(html);
            res.URLJSON = sa.URLJSON;
            return res;
        }

        public StartUpRoutine StartArguments { get => _sa; set => _sa = value; }

        public static void GetFromUrl(StartUpRoutine sa)
        {
            var html = getFileList(sa.UrlUpdate + "/" + (sa.FileListName ?? FileListName));
            if (html?.Contains("404") == true && html?.Contains("Error") == true)
            {
                log.Write(html.Replace("{}","{{").Replace("}","}}"));
                return;
            }
            var list = html.Trim().Replace("<br/>", "\n").Split('\n');
            foreach (var f in list)
            {
                if (string.IsNullOrEmpty(f)) continue;
                log.WriteTime("check file: {0}", f);
                var ff = f.Split('|');
                var fName = getFilePath(ff[0], sa);
                var d = ff[1].Trim().Replace("  ", " ").Split('.', ' ', ':');
                var dt = new DateTime(
                    year: Convert.ToInt32(d[0]),
                    month: Convert.ToInt32(d[1]),
                    day: Convert.ToInt32(d[2]),
                    hour: Convert.ToInt32(d[3]),
                    minute: Convert.ToInt32(d[4]),
                    second: Convert.ToInt32(d[5])
                    );

                FileInfo fi = new FileInfo(fName);
                if (!fi.Exists || fi.CreationTime != dt)
                {
                    log.WriteTime("need load {0}", fi.FullName);

                    if(DownLoadFile(sa, fi)) File.SetCreationTime(fi.FullName, dt);
                }
            }
        }

        public static bool DownLoadFile(StartUpRoutine sa, FileInfo f)
        {
            int iMax = 3;
            int iPause = 1000;
            bool res = false;
            var iTry = 1;
            while (!res && iTry<=iMax)
            {
                WebClient wc = new WebClient();
                if (!f.Directory.Exists) f.Directory.Create();
                try
                {
                    wc.Proxy = htmlNet.getProxy();
                    wc.UseDefaultCredentials = true;
                    wc.DownloadFile(sa.UrlUpdate + "/" + sa.PageToLoad + f.Name, f.FullName);
                    log.WriteTime("loaded {0}", f.FullName);
                    res = true;
                }
                catch (Exception ex)
                {
                    log.WriteTime("{0}/{1} ERR on get '{2}': {3}", iTry, iMax, f.Name, ex.Message);
                    Thread.Sleep(iPause);
                }
                iTry++;
            }

            return res;
        }

        public static string getFilePath(string name, StartUpRoutine sa)
        {
            var p = sa.TargetFolder ?? FolderToSave;
            return string.IsNullOrEmpty(p) ? name : Path.Combine(p, name);
        }

        public static string getFileList(string pathFileList)
        {
            log.Write("get list from {0}", pathFileList);
            string html = string.Empty;
            //string url = @"https://api.stackexchange.com/2.2/answers?order=desc&sort=activity&site=stackoverflow";
            html = htmlNet.getHTML(pathFileList);

            return html;
        }


        public void Run(StartUpRoutine sa)
        {
            if (string.IsNullOrEmpty(sa.Run)) return;
            var path = sa.Run;

            var i = path.IndexOf(' ');
            if (i > 0 && i < path.Length - 1)
            {
                Process pr = new Process();
                var p = path.Substring(0, i);
                var a = path.Substring(i + 1);
                log.WriteTime("run: {0}", p);
                log.WriteTime("arg: {0}", a);
                pr.StartInfo = new ProcessStartInfo(p, a);
                if(sa.ShowShell?.ToUpper()=="NO"
                    || sa.ShowShell?.ToUpper() == "FALSE"
                    || sa.ShowShell == "0")
                {
                    pr.StartInfo.UseShellExecute = false;
                    pr.StartInfo.CreateNoWindow = true;
                }
                pr.Start();
            }
            else
            {
                log.WriteTime("run: {0}", path);
                Process.Start(path);
            }
        }




    }
}
