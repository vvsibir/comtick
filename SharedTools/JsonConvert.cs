using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SharedTools
{
    public static class JsonConvert
    {
        static Encoding enc = Encoding.UTF8;
        public static T DeserializeObject<T>(string json)
        {
            Type t = typeof(T);
            DataContractJsonSerializer jf = new DataContractJsonSerializer(t);
            MemoryStream ms = new MemoryStream(enc.GetBytes(json));
            var res = jf.ReadObject(ms);
            ms.Close();
            return (T)res;
        }

        public static string SerializeObject(object source)
        {
            var t = source.GetType();
            DataContractJsonSerializer jf = new DataContractJsonSerializer(t);

            MemoryStream ms = new MemoryStream();
            jf.WriteObject(ms, source);

            var res = enc.GetString(ms.ToArray());
            ms.Close();
            return res;
        }
    }
}
