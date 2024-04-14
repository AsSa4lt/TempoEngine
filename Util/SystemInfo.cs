using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 


namespace TempoEngine.Util {
    internal partial class SystemInfo {
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

        [LibraryImport("user32.dll")]
        private static partial IntPtr GetDC(IntPtr hWnd);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [LibraryImport("gdi32.dll")]
        private static partial int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
}
