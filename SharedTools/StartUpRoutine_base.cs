using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharedTools
{
    [DataContract]
    public class StartUpRoutine_base
    {
        public StartUpRoutine_base(string[] args)
        {
            Init(args);
        }
        public StartUpRoutine_base()
        {
        }
        public void Init(string[] args)
        {
            IEnumerable<System.Reflection.PropertyInfo> pp = PropHelper.GetPropsForApply(this.GetType());

            foreach (var p in pp)
            {
                var arg = args.FirstOrDefault(a => a.ToUpper().StartsWith(p.Name.ToUpper() + ":"));
                if (!string.IsNullOrEmpty(arg))
                {
                    p.SetValue(this, arg.Remove(0, p.Name.Length + 1), null);
                }
            }
        }

        public string getWithKeys(string str)
        {
            return strHelp.applyProps(str, this);
        }
    }
}
