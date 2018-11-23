using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottsUtils
{
    public static class WndInfo
    {
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return WinAPI.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
            else
            {
                return new IntPtr(WinAPI.SetWindowLong(hWnd, nIndex, dwNewLong.ToInt32()));
            }
        }

        public static string GetClassName(IntPtr target)
        {
            StringBuilder sb = new StringBuilder(250);
            WinAPI.GetClassName(target, sb, 250);
            return sb.ToString();
        }

        public static string GetWindowText(IntPtr target)
        {
            int len = WinAPI.GetWindowTextLength(target) + 1;
            StringBuilder sb = new StringBuilder(len);
            WinAPI.GetWindowText(target, sb, len);
            return sb.ToString();
        }
    }
}
