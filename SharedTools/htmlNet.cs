using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SharedTools
{
    public static class htmlNet
    {
        public static string getHTML(string pathFileList)
        {
            string html;
            string url = pathFileList;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            request.Proxy = getProxy();
            request.UseDefaultCredentials = true;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }

        public static IWebProxy getProxy()
        {
            var res = WebRequest.GetSystemWebProxy();
            res.Credentials = CredentialCache.DefaultCredentials;

            return res;
        }

        public static void Upload(string filename, string filepath, string urlupload)
        {
            FileInfo fi = new FileInfo(filename);
            if(!fi.Exists)
            {
                log.Write("file not found: {0}", fi.FullName);
                return;
            }
            var bytes = File.ReadAllBytes(fi.FullName);

            Upload(bytes, fi.Name, filepath, urlupload);
        }
        /// <summary>
        /// заливает файл на url
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        /// <param name="filepath"></param>
        /// <param name="urlupload"></param>
        public static void Upload(byte[] content, string filename, string filepath, string urlupload)
        {
            HttpContent stringContent_pathfile = new StringContent(filepath); // путь файла на удаленном сервере
            HttpContent bytesContent = new ByteArrayContent(content); // файл
            // Now create a client handler which uses that proxy
            var httpClientHandler = new HttpClientHandler() { Proxy = getProxy()};
            using (var client = new HttpClient(httpClientHandler))
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(bytesContent, "filename", filename);
                formData.Add(stringContent_pathfile, "path");
                var responseTask = client.PostAsync(urlupload, formData);
                responseTask.Wait();
                var response = responseTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                var responseContentTask = response.Content.ReadAsStreamAsync();
                responseContentTask.Wait();

                StreamReader reader = new StreamReader(responseContentTask.Result);
                string text = reader.ReadToEnd();
                if (text.Contains("success")) log.WriteTime("OK");
                else log.WriteTime(text);

            }
        }

    }
}
