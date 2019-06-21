using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AreaCodeCheckerUWP
{

    class WindowsFunctions
    {
        #region system constants
        private static readonly int HWND_TOP = 0;
        private static readonly int HWND_BOTTOM = 1;
        private static readonly int HWND_TOPMOST = -1;
        private static readonly int HWND_NOTOPMOST = -2;

        private static readonly uint SWP_NOSIZE = 0x0001;
        private static readonly uint SWP_NOMOVE = 0x0002;

        private static readonly uint ERROR_SUCCESS = 0x0;

        private static readonly uint LWA_ALPHA = 0x00000002;

        private static readonly byte AC_SRC_OVER = 0x0;
        private static readonly byte AC_SRC_ALPHA = 0x1;
        #endregion

        [DllImport("Kernel32.dll", EntryPoint = "GetCurrentProcessId")]
        static extern UInt32 GetCurrentProcessId();

        #region extern functions.
        // calls user32.dll SetWindowPos function to set the window position of a given window
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        // need to get the display context of the window so that we can grab the background color, and then apply transparency with SetLayeredWindowAttributes
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private static extern IntPtr GetDC(IntPtr hwnd);

        // gets the current background color of the device context
        [DllImport("gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint GetBkColor(IntPtr hdc);

        // get the last system error as an unsigned 32-bit integer 
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern uint GetLastError();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UpdateLayeredWindow(
            IntPtr hWnd,
            out IntPtr hdcDst,
            ref CStructures.POINT pptDst,
            ref CStructures.SIZE psize,
            IntPtr hdcSrc,
            ref CStructures.POINT pptSrc,
            uint crKey,
            ref CStructures.BLENDFUNCTION pblend,
            uint dwFlags
            );
        #endregion

        #region public static functions
        //public static bool SetWindowTopmost()
        //{
        //    var p = GetCurrentProcessId();
        //    IntPtr handle = p.MainWindowHandle;

        //    return SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        //}

        //public static bool SetWindowNotTopmost()
        //{
        //    Process p = Process.GetCurrentProcess();
        //    IntPtr handle = p.MainWindowHandle;

        //    return SetWindowPos(handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        //}

        //public static bool SetWindowTransparency(int pct)
        //{
        //    byte alpha = 0;//(byte)Math.Floor((decimal)(255 * ((decimal)pct / 100))); // use math to convert the percentage to an alpha value
        //    IntPtr dstContext;
        //    Process p = Process.GetCurrentProcess();
        //    IntPtr windowHandle = p.MainWindowHandle;
        //    IntPtr deviceContext = GetDC(windowHandle);
        //    uint bkColor = GetBkColor(deviceContext);
        //    CStructures.BLENDFUNCTION blendFunction =
        //        new CStructures.BLENDFUNCTION
        //        {
        //            BlendOp = AC_SRC_OVER,
        //            BlendFlags = 0,
        //            SourceConstantAlpha = alpha,
        //            AlphaFormat = 0//AC_SRC_ALPHA
        //        };
        //    CStructures.POINT point = new CStructures.POINT
        //    {
        //        x = 0,
        //        y = 0
        //    };
        //    CStructures.POINT point2 = new CStructures.POINT
        //    {
        //        x = 0,
        //        y = 0
        //    };
        //    CStructures.SIZE size = new CStructures.SIZE
        //    {
        //        cx = 0,
        //        cy = 0
        //    };



        //    bool ret = UpdateLayeredWindow(
        //        windowHandle,
        //        out dstContext,
        //        ref point,
        //        ref size,
        //        deviceContext,
        //        ref point2,
        //        0,
        //        ref blendFunction,
        //        LWA_ALPHA);
        //    //bool ret = SetLayeredWindowAttributes(windowHandle, bkColor, alpha, LWA_ALPHA);
        //    uint err = GetLastError();
        //    //IntPtr i = Marshal.ReadIntPtr(deviceContext);
        //    if (windowHandle != null)
        //    {
        //        windowHandle = IntPtr.Zero;
        //    }
        //    p.Close();

        //    return ret;
        //}

        #endregion
    }

}