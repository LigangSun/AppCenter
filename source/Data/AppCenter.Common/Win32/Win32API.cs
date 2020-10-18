using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SoonLearning.AppCenter.Win32
{
    public class Win32API
    {
        #region Win32 API

        public const int SW_SHOW = 5;

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;

        public const int WS_SYSMENU = 0x00080000;
        public const int WS_MINIMIZEBOX = 0x00020000;

        public const int WM_USER = 0x0400;
        public const int SC_SIZE = 0xF000;
        public const int WM_SYSCOMMAND = 0x112;

        public const int GWL_STYLE = (-16);

        [DllImport("User32.dll")]
        public extern static int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("User32.dll")]
        public extern static int SetWindowPos(IntPtr hWnd,
                                            IntPtr hWndInsertAfter,
                                            int X,
                                            int Y,
                                            int cx,
                                            int cy,
                                            uint uFlags);

        [DllImport("User32.dll")]
        public extern static IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public extern static int SendMessage(
                                IntPtr hWnd,
                                int Msg,
                                int wParam,
                                int lParam);

        [DllImport("User32.dll")]
        public extern static IntPtr FindWindow(string className, string windowName);

        [DllImport("User32.dll")]
        public extern static int SetWindowLong(IntPtr hWnd, int index, UInt32 flags);

        [DllImport("User32.dll")]
        public extern static UInt32 GetWindowLong(IntPtr hWnd, int index);

        #endregion
    }
}
