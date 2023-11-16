using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace cSpeech
{
    public class VolumeManager
    {
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public struct WAVEOUTCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            private string szPname;
            public uint dwFormats;
            public short wChannels;
            public short wReserved;
            public uint dwSupport;

            public override string ToString()
            {
                return string.Format("wMid:{0}|wPid:{1}|vDriverVersion:{2}|'szPname:{3}'|dwFormats:{4}|wChannels:{5}|wReserved:{6}|dwSupport:{7}", new object[] { wMid, wPid, vDriverVersion, szPname, dwFormats, wChannels, wReserved, dwSupport });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public struct WAVEINCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public uint dwFormats;
            public short wChannels;
            public short wReserved;
            //public int dwSupport;

            public override string ToString()
            {
                return string.Format("wMid:{0}|wPid:{1}|vDriverVersion:{2}|'szPname:{3}'|dwFormats:{4}|wChannels:{5}|wReserved:{6}", new object[] { wMid, wPid, vDriverVersion, szPname, dwFormats, wChannels, wReserved });
            }
        }

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveInGetDevCaps(int hwo, ref WAVEINCAPS pwic, /*uint*/ int cbwic);

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint waveInGetNumDevs();

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutGetDevCaps(int hwo, ref WAVEOUTCAPS pwoc, /*uint*/ int cbwoc);

        [DllImport("winmm.dll", SetLastError = true)]

        static extern uint waveOutGetNumDevs();
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int waveOutGetVolume(IntPtr hwo, ref int dwVolume);
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int waveOutSetVolume(IntPtr device, int volume);

        //[DllImport("coredll.dll", SetLastError = true)]
        //internal static extern int waveOutSetVolume(IntPtr device, int volume);
        //[DllImport("coredll.dll", SetLastError = true)]
        //internal static extern int waveOutGetVolume(IntPtr device, ref int volume);

        /// <summary>
        /// возвращает набор устройств для воспроизведения
        /// </summary>
        /// <returns></returns>
        public static WAVEOUTCAPS[] GetDevCapsPlayback()
        {
            uint waveOutDevicesCount = waveOutGetNumDevs();
            if (waveOutDevicesCount > 0)
            {
                WAVEOUTCAPS[] list = new WAVEOUTCAPS[waveOutDevicesCount];
                for (int uDeviceID = 0; uDeviceID < waveOutDevicesCount; uDeviceID++)
                {
                    WAVEOUTCAPS waveOutCaps = new WAVEOUTCAPS();
                    waveOutGetDevCaps(uDeviceID, ref waveOutCaps, Marshal.SizeOf(typeof(WAVEOUTCAPS)));
                    //System.Diagnostics.Debug.WriteLine("\n" + waveOutCaps.ToString());
                    //Console.WriteLine("\n" + waveOutCaps.ToString());
                    log.Write("\n" + waveOutCaps.ToString());
                    list[uDeviceID] = waveOutCaps;
                }
                return list;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// возвращает набор устройств для записи
        /// </summary>
        /// <returns></returns>
        public static WAVEINCAPS[] GetDevCapsRecording()
        {
            uint waveInDevicesCount = waveInGetNumDevs();
            if (waveInDevicesCount > 0)
            {
                WAVEINCAPS[] list = new WAVEINCAPS[waveInDevicesCount];
                for (int uDeviceID = 0; uDeviceID < waveInDevicesCount; uDeviceID++)
                {
                    WAVEINCAPS waveInCaps = new WAVEINCAPS();
                    waveInGetDevCaps(uDeviceID, ref waveInCaps, Marshal.SizeOf(typeof(WAVEINCAPS)));
                    System.Diagnostics.Debug.WriteLine("\n" + waveInCaps.ToString());
                    list[uDeviceID] = waveInCaps;
                }
                return list;
            }
            else
            {
                return null;
            }
        }
        //---------------- https://www.pinvoke.net/default.aspx/coredll.waveoutgetvolume

        public enum Volumes : int
        {

            OFF = 0,
            LOW = 858993459,
            NORMAL = 1717986918,
            MEDIUM = -1717986919,
            HIGH = -858993460,
            VERY_HIGH = -1
        }

        public static Volumes Volume
        {

            get
            {
                int v = (int)0; 
                waveOutGetVolume(IntPtr.Zero, ref v);
                switch (v)
                {
                    case (int)Volumes.OFF: return Volumes.OFF;
                    case (int)Volumes.LOW: return Volumes.LOW;
                    case (int)Volumes.NORMAL: return Volumes.NORMAL;
                    case (int)Volumes.MEDIUM: return Volumes.MEDIUM;
                    case (int)Volumes.HIGH: return Volumes.HIGH;
                    case (int)Volumes.VERY_HIGH: return Volumes.VERY_HIGH;
                    default: return Volumes.OFF;
                }
            }
            set {
                waveOutSetVolume(IntPtr.Zero, (int)value); }
        }

        public static void GetAudioDevices()
        {
            ManagementObjectSearcher mo = new ManagementObjectSearcher("select * from Win32_SoundDevice");

            foreach (ManagementObject soundDevice in mo.Get())
            {
                Console.WriteLine(soundDevice.GetPropertyValue("DeviceId"));
                Console.WriteLine(soundDevice.GetPropertyValue("Manufacturer"));
                // etc                       
            }
        }
    }
}

