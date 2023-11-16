using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ComTick
{
    public static class mouseImitation
    {
        //импортируем mouse_event(): 
        [DllImport("User32.dll")]
        static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

        //для удобства использования создаем перечисление с необходимыми флагами (константами), которые определяют действия мыши: 
        [Flags]
        enum MouseFlags
        {
            Move = 0x0001, LeftDown = 0x0002, LeftUp = 0x0004, RightDown = 0x0008,
            RightUp = 0x0010, Absolute = 0x8000
        };
        public static void clickLeft(Point location)
        {
            //и использование - клик левой примерно в центре экрана
            //(подробнее о координатах, передаваемых в mouse_event см. в MSDN): 
            //mouse_event(MouseFlags.Absolute | MouseFlags.Move, location.X, location.Y, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, location.X, location.Y, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, location.X, location.Y, 0, UIntPtr.Zero);
        }
        public static void clickRight(Point location)
        {
            //и использование - клик левой примерно в центре экрана
            //(подробнее о координатах, передаваемых в mouse_event см. в MSDN): 
            //mouse_event(MouseFlags.Absolute | MouseFlags.Move, location.X, location.Y, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.Absolute | MouseFlags.RightDown, location.X, location.Y, 0, UIntPtr.Zero);
            mouse_event(MouseFlags.Absolute | MouseFlags.RightUp, location.X, location.Y, 0, UIntPtr.Zero);
        }
        public static void downRight(Point location)
        {
            mouse_event(MouseFlags.Absolute | MouseFlags.RightDown, location.X, location.Y, 0, UIntPtr.Zero);
        }
        public static void upRight(Point location)
        {
            mouse_event(MouseFlags.Absolute | MouseFlags.RightUp, location.X, location.Y, 0, UIntPtr.Zero);
        }
        public static void downLeft(Point location)
        {
            mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, location.X, location.Y, 0, UIntPtr.Zero);
        }
        public static void upLeft(Point location)
        {
            mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, location.X, location.Y, 0, UIntPtr.Zero);
        }
    }
}
