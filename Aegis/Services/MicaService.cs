using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Aegis.Services
{
    public static class MicaService
    {
        public static void EnableMica(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;

            int mica = 1; // Mica
            DwmSetWindowAttribute(
                hwnd,
                DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                ref mica,
                sizeof(int));
        }

        private enum DWMWINDOWATTRIBUTE
        {
            DWMWA_SYSTEMBACKDROP_TYPE = 38
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE attribute,
            ref int pvAttribute,
            int cbAttribute);
    }
}
