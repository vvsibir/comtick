using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;

namespace ComTick
{
    public static class serialMaster
    {
        public static string PORT { get; set; }
        public static int BAUDE { get; set; } = 9600;

        public static void Send(string s)
        {
            using (SerialPort sp = new SerialPort(PORT, BAUDE))
            {
                sp.WriteLine(s);
                sp.Close();
            }
            
        }
        public static void aa()
        {
            SerialPort.GetPortNames();
        }
        public static string AutodetectArduinoPort(string a)
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            string res = null;
            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc?.ToUpper().Contains(a) == true)
                    {
                        res = deviceId;
                    }
                }
            }
            catch (ManagementException e)
            {
                
                /* Do Nothing */
            }

            return res;
        }

        // Helper function to handle regex search
        static string regex(string pattern, string text)
        {
            Regex re = new Regex(pattern);
            Match m = re.Match(text);
            if (m.Success)
            {
                return m.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SerialFromMain()
        {
            SharedTools.log.Write("Init serial master");
            // Use WMI to get info
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\"");

            // Search all serial ports
            foreach (ManagementObject queryObj in searcher.Get())
            {
                // Parse the data
                if (null != queryObj["Name"])
                {
                    SharedTools.log.Write("Port = " + regex(@"(\(COM\d+\))", queryObj["Name"].ToString()));
                }
                //PNPDeviceID = USB\VID_1A86&PID_7523\5&1A63D808&0&2
                if (null != queryObj["PNPDeviceID"])
                {
                    SharedTools.log.Write("VID = " + regex("VID_([0-9a-fA-F]+)", queryObj["PNPDeviceID"].ToString()));
                    SharedTools.log.Write("PID = " + regex("PID_([0-9a-fA-F]+)", queryObj["PNPDeviceID"].ToString()));
                }
            }
            SharedTools.log.Write("Done");
            
        }
    }
}
