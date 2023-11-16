using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharedTools
{
    public static class strHelp
    {
        public static string applyProps(string targetStr, object objSrc)
        {
            if (string.IsNullOrEmpty(targetStr)) return targetStr;
            var res = targetStr;

            var props = objSrc.GetType().GetProperties();
            props.ForEach(prop =>
            {
                if (prop.CanRead && !prop.PropertyType.IsArray)
                    res = res.Replace(
                        string.Format("%{0}%", prop.Name.ToUpper()), 
                        prop.GetValue(objSrc, null)?.ToString()
                        );
            });
            return res;
        }

        public static string[] Split(string args)
        {
            if (string.IsNullOrEmpty(args)) return null;

            return args.Split(',', ';', 'x', '-').Cast<string>().Select(s => s.Trim()).ToArray(); ;
        }
    }
}
