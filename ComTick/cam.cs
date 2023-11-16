using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;

namespace ComTick
{
    public static class cam
    {
        private static FilterInfoCollection videoDevices;

        public static int numCam { get; set; } = 0;
        public static object resolution { get; set; }

        static cam()
        {
            init();
        }
        public static void init()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                {
                    throw new Exception("No cam devices");
                }

                for (int i = 1, n = videoDevices.Count; i <= n; i++)
                {
                    var vd = videoDevices[i - 1];
                    string cameraName = i + " : " + vd.Name;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("No camera found, err: ", ex);
            }
        }

        public static string[] GetCams()
        {
            return videoDevices.Cast<FilterInfo>().Select(f => f.Name).ToArray();
        }

        public static System.Drawing.Image shot()
        {
            try
            {
                VideoCaptureDevice vs = new VideoCaptureDevice(videoDevices[numCam].MonikerString);
                vs.NewFrame += Vs_NewFrame_OneShot;
                vs.DesiredFrameRate = 10;
                vs.Start();
                vs.WaitForStop();
                var res = img;
                img = null;
                return res;
            }
            catch(Exception ex)
            {
                return img = Resources.res.errorPixel;
            }

        }
        static System.Drawing.Image img;
        private static void Vs_NewFrame_OneShot(object sender, NewFrameEventArgs e)
        {
            try
            {
                var vs = (sender as VideoCaptureDevice);
                vs.NewFrame -= Vs_NewFrame_OneShot;
                img = e.Frame;
            }
            catch(Exception ex)
            {
                img = Resources.res.errorPixel;
                SharedTools.log.logError_full(ex);
            }
        }
    }
}
