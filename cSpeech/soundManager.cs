using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace cSpeech
{
    public static class soundManager
    {
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        static Form mf { get { return Application.OpenForms[0]; } }
        public static void VolumeUp()
        {
            var c = mf;
            SendMessageW(c.Handle, WM_APPCOMMAND, c.Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
        }

        public static void VolumeMute()
        {
            var c = mf;
            SendMessageW(c.Handle, WM_APPCOMMAND, c.Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
        public static void VolumeDown()
        {
            var c = mf;
            SendMessageW(c.Handle, WM_APPCOMMAND, c.Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, String lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        public static void btnMute_Click(object sender, EventArgs e)//Выключение-включение звука
        {
            var c = (sender as Control);
            SendMessageW(c.Handle, WM_APPCOMMAND, c.Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        public static void btnDecVol_Click(object sender, EventArgs e)//Убавление громкости
        {
            var c = (sender as Control);            
            SendMessageW(c.Handle, WM_APPCOMMAND, c.Handle,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        public static void btnIncVol_Click(object sender, EventArgs e)//Прибавление звука
        {
            var c = (sender as Control);
            SendMessageW(c.Handle, WM_APPCOMMAND, c.Handle,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }
    }
}
