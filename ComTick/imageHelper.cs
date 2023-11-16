using SharedTools;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ComTick
{
    public static class imageHelper
    {
        public static System.Drawing.Image getImage(string num)
        {
            int i;
            if (!int.TryParse(num, out i) || i > System.Windows.Forms.Screen.AllScreens.Length) i = 0;
            log.Write("get from screen {0} [requested num: {1}]", i, num);
            var bounds = System.Windows.Forms.Screen.AllScreens[i].Bounds;
            Bitmap printscreen = new Bitmap(bounds.Width, bounds.Height);
            Graphics graphics = Graphics.FromImage(printscreen as System.Drawing.Image);
            graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, printscreen.Size);

            return printscreen;

        }
        public class scrInfo
        {
            private string args;

            public scrInfo(string args)
            {
                this.args = args;
                var ss = strHelp.Split(args);
                if (ss != null)
                {
                    for (int i = 0; i < ss.Length; i++)
                    {
                        var s = ss[i];
                        if (s.Contains(':'))
                        {
                            var aa = s.Split(':');
                            setParam(aa[0], aa[1]);
                        }
                        else setParam(i, s);
                    }
                }
            }

            public float Scale { get; set; } = 4;
            public System.Drawing.Imaging.ImageFormat Format { get; set; } = System.Drawing.Imaging.ImageFormat.Jpeg;

            internal void setParam(string name, string value)
            {
                switch (name.ToLower())
                {
                    case "scale":
                        SetScale(value);
                        break;
                    case "format":
                        SetFormat(value);
                        break;
                }

            }
            internal void setParam(int num, string value)
            {
                switch (num)
                {
                    case 0:
                        SetScale(value);
                        break;
                    case 1:
                        SetFormat(value);
                        break;
                }

            }

            private void SetScale(string value)
            {
                float f;
                if (float.TryParse(value, out f)) Scale = f;
            }

            private void SetFormat(string value)
            {
                if (value.ToLower() == "jpg")
                    Format = System.Drawing.Imaging.ImageFormat.Jpeg;
                else
                    Format = (System.Drawing.Imaging.ImageFormat)Enum.Parse(typeof(System.Drawing.Imaging.ImageFormat), value, true);
            }
        }

        internal static AForge.Imaging.Image ToJPG(Bitmap frame)
        {
            throw new NotImplementedException();
        }

        public static System.Drawing.Image GetImageFullScreen(float scale = 4)
        {
            int x, y, width, height;
            x = Screen.AllScreens.Select(s => s.Bounds.X).Min();
            y = Screen.AllScreens.Select(s => s.Bounds.Y).Min();
            width = Screen.AllScreens.Select(s => s.Bounds.X + s.Bounds.Width).Max();
            height = Screen.AllScreens.Select(s => s.Bounds.Y + s.Bounds.Height).Max();
            int ww = width - x, hh = height - y;
            var fullScreen = new Bitmap(ww, hh);
            Graphics graphics = Graphics.FromImage(fullScreen as System.Drawing.Image);
            for (int i = 0; i < Screen.AllScreens.Count(); i++)
            {
                var scr = Screen.AllScreens[i];
                graphics.CopyFromScreen(scr.Bounds.X, scr.Bounds.Y, scr.Bounds.X - x, scr.Bounds.Y - y, scr.Bounds.Size);
            }

            return scaleImage(fullScreen, scale);
        }

        private static System.Drawing.Image scaleImage(Bitmap image, float scale)
        {
            int width = (int)(image.Width / scale);
            int height = (int)(image.Height / scale);
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static byte[] ImageToBytes(System.Drawing.Image img, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] res;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, format);
                res = ms.GetBuffer();
            }
            return res;
        }
    }
}
