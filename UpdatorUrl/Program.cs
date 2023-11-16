using SharedTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdatorUrl
{
    class Program
    {
        static StartUpRoutine sa;
        static void Main(string[] args)
        {
            if(args.Count() == 0 || args.Contains("/?") == true)
            {
                var names = PropHelper.GetPropsForApply(typeof(StartUpRoutine)).Select(m => m.Name).ToArray();
                Console.Write("Attributes:\n{0}", string.Join("\n", names));
                return;
            }

            sa = new StartUpRoutine();
            sa.Init(args);
            log.URLLog = sa.URLLog;

            if (sa.OnlyJson?.ToUpper() == "TRUE")
            {
                var s = JsonConvert.SerializeObject(sa);

                log.Write(s);
                Console.ReadKey();
                return;
            }
            (new UpdateProcess(sa)).Update();
            //Console.ReadKey();
        }




    }
}
