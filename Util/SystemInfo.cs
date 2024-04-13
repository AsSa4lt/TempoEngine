using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 


namespace TempoEngine.Util {
    internal class SystemInfo {
        public static int GetRefreshRate() {
            // Get the handle to the device context (DC) for the primary screen
            IntPtr hdc = GetDC(IntPtr.Zero); // Passing IntPtr.Zero gets the DC for the entire screen
            if (hdc == IntPtr.Zero) {
                return 0; // Return 0 if we fail to get the DC
            }

            // Get the vertical refresh rate
            int refreshRate = GetDeviceCaps(hdc, VERTREFRESH);

            // Release the device context
            ReleaseDC(IntPtr.Zero, hdc);

            return refreshRate;
        }




        const int VERTREFRESH = 116;

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
}
