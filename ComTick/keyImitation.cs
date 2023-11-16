using SharedTools;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ComTick
{
    public static class keyImitation
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public static void KeyDown(Keys key)
        {
            keybd_event((byte)key, 0, 0, 0);
        }
        public static void KeyUp(Keys key)
        {
            keybd_event((byte)key, 0, 0x2, 0);
        }

        public static void KeyClick(Keys key)
        {
            KeyDown(key);
            Thread.Sleep(10);
            KeyUp(key);
        }

        public static void ShortCut(string keys)
        {
            Keys result = System.Windows.Forms.Keys.None;
            if (string.IsNullOrEmpty(keys)) return;
            //return result;
            string[] kk = keys.Split(new char[] { '+' });
            foreach (string k in kk)
            {
                string key = k.Trim();
                if (string.IsNullOrEmpty(k)) continue;
                Keys kTmp;
                if (!Enum.TryParse(k, true, out kTmp))
                {
                    log.Write("Ошибка при пасинге Key: не распознан {0} в наборе {1}", k, keys);
                }
                result = result | kTmp;
            }
            keyImitation.KeyClick(result);
        }

    }
}
