using SharedTools;
using System.IO;
using System.Runtime.Serialization;

namespace ComTick
{
    [DataContract]
    public class StartUpRoutine : SharedTools.StartUpRoutine_base
    {
        /// <summary>
        /// ключ, разделяющий клиентов
        /// </summary>
        [DataMember]
        public string KEYCMD { get; set; }
        /// <summary>
        /// хост подключения
        /// </summary>
        [DataMember]
        public string HOST { get; set; }
        [DataMember]
        public string URLCmd { get; set; }
        [DataMember]
        public string URLUpdate { get; set; }
        [DataMember]
        public string URLLog { get; set; }
        [DataMember]
        public string Interval { get; set; }
        [DataMember]
        public string Config { get => _config; set => _config = value; }
        string _config = "comtick.json";
        [DataMember]
        public string[] UrlsToLoad { get => _urls; set => _urls = value; }
        string[] _urls;// = new string[] { @"http://xoxoxoesy.esy.es/cmdComTickCommon/updatecfg.json", @"http://xoxoxoesy.esy.es/%p%/updatecfg.json" };
        /// <summary>
        /// выводит в консоль JSON-файл
        /// </summary>
        [DataMember]
        public string OnlyJson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string UploadUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string UploadPath { get; set; }
        /// <summary>
        /// ищет ардуин в консоль
        /// </summary>
        public string ArduinInfo { get; set; }

        [DataMember]
        public string PASSNET { get; set; }


        public StartUpRoutine(string[] args) : base(args)
        {
            if (File.Exists(Config))
            {
                var cfg = JsonConvert.DeserializeObject<StartUpRoutine>(File.ReadAllText(Config));
                
                PropHelper.CopyProperties(this, cfg); // сначала перезаписываем значения из файла на значения, пришедшие из командной строки
                PropHelper.CopyProperties(cfg, this); // теперь в this заливаем итоговые значения
            }
        }
    }
}
