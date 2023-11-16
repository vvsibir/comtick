using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComTick
{
    static class CommandRepository
    {
        public static Dictionary<string, Action> Actions = new Dictionary<string, Action>()
        {
            //{ "update", cmd.Update }
            //,{ "exit", cmd.Exit }
            //,{ "reboot", cmd.Reboot }
            //,{ "poweroff", cmd.PowerOff }
        };
        public static Dictionary<string, Action<string>> ActionsWithArg = new Dictionary<string, Action<string>>()
        {
            //{ "update", cmd.Update }
        };
        public static Action Get(string name)
        {
            var acmd = getOf(typeof(cmd), name);
            return acmd ?? Actions.FirstOrDefault(a => a.Key.ToUpper() == name?.ToUpper()).Value;
        }

        private static Action getOf(Type type, string name)
        {
            var m = type.GetMethods().Where(t => t.Name.ToUpper() == name.ToUpper() && t.GetParameters().Count() == 0).FirstOrDefault();
            return m == null ? (Action)null : () => { m.Invoke(null, null); };
        }

        public static Action<string> GetWithArg(string name)
        {
            var acmd = getOf_withArg(typeof(cmd), name);
            return acmd ?? ActionsWithArg.FirstOrDefault(a => a.Key.ToUpper() == name?.ToUpper()).Value;
        }
        private static Action<string> getOf_withArg(Type type, string name)
        {
            var m = type
                .GetMethods()
                .Where(
                    t => t.Name.ToUpper() == name.ToUpper() 
                    && t.GetParameters().Count() == 1 
                    && t.GetParameters()[0].ParameterType == typeof(string))
                .FirstOrDefault();
            return m == null ? (Action<string>)null : (s) => { m.Invoke(null, new object[] { s }); };
        }
    }
}
