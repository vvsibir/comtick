using SharedTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComTick
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("/?") == true)
            {
                // если попросили хелп получаем список возможных свойств
                var names = PropHelper.GetPropsForApply(typeof(StartUpRoutine)).Select(m => m.Name).ToArray(); 
                // и вывождим в лог
                log.Write("Attributes:\n{0}", string.Join("\n", names));
                return;
            }

            try
            {
                cur.startArgs = new StartUpRoutine(args);
            }
            catch (Exception ex)
            {
                log.Write("Error on config:\n" + ex.Message);
                return;
            }

            if (singleRun()) return;

            try
            {
                cur.cmdr = new CommandURLReader() { URLCmd = cur.startArgs.getWithKeys(cur.startArgs.URLCmd), Interval = Convert.ToInt32(cur.startArgs.Interval ?? "1000") };
            }
            catch(Exception ex)
            {
                log.Write("Error on init:\n" + ex.Message);
                return;
            }

            cur.cmdr.Run();

            Console.WriteLine("Press any key to exit");
            //cmd.UpdateBat();
            Console.ReadKey();
        }

        private static bool singleRun()
        {
            if (cur.startArgs.OnlyJson?.ToUpper() == "TRUE")
            {
                var s = JsonConvert.SerializeObject(cur.startArgs);

                log.Write(s);
                Console.ReadKey();
                return true; ;
            }

            if (cur.startArgs.ArduinInfo?.ToUpper() == "TRUE")
            {
                serialMaster.SerialFromMain();
                Console.ReadKey();
                return true; 
            }

            return false;
        }
    }
}
