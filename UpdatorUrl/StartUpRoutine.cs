using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UpdatorUrl
{
    [DataContract]
    public class StartUpRoutine : SharedTools.StartUpRoutine_base
    {
        [DataMember]
        public string URLJSON { get; set; }
        [DataMember]
        public string UrlUpdate { get; set; }
        /// <summary>
        /// запускает после 
        /// </summary>
        [DataMember]
        public string Run { get; set; }
        /// <summary>
        /// страница для получения списка файлов
        /// </summary>
        [DataMember]
        public string FileListName { get; internal set; }
        [DataMember]
        public string TargetFolder { get; set; }
        /// <summary>
        /// страница для загрузки файлов, если стандартный метод (например, ехе-шники нельзя скачать с хостинга)
        /// </summary>
        [DataMember]        
        public string PageToLoad { get; set; }
        [DataMember]
        public string ShowShell { get; set; }
        /// <summary>
        /// выводит в консоль JSON-файл
        /// </summary>
        [DataMember]
        public string OnlyJson { get; internal set; }
        [DataMember]
        public string URLLog { get; internal set; }

    }
}
