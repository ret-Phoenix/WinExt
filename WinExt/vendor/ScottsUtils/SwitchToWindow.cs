using System;
using System.Collections.Generic;
using System.Text;

namespace ScottsUtils
{
    static class SwitchToWindow
    {
        static string m_partialCaption;

        static public bool Switch(string partialCaption)
        {
            m_partialCaption = partialCaption.ToLower();
            return WinAPI.EnumWindows(EnumWindowsProc, IntPtr.Zero);
        }

        static bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam)
        {
            if (WinAPI.IsWindowVisible(hWnd))
            {
                string caption = WndInfo.GetWindowText(hWnd).ToLower();
                if (caption.Contains(m_partialCaption))
                {
                    WinAPI.SetForegroundWindow(hWnd);
                    return false;
                }
            }

            return true;
        }
    }
}
