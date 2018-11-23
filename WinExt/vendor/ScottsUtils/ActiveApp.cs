using System;
using System.Collections.Generic;
using System.Text;
using ScottsUtils;
using System.Drawing;
using System.Windows.Forms;

namespace ScottsUtils
{
    static class ActiveApp
    {
        static LinkedList<IntPtr> m_knownWnds = new LinkedList<IntPtr>();
        static Dictionary<IntPtr, Rectangle> m_savedRects = new Dictionary<IntPtr, Rectangle>();

        static bool MinimizeOtherWindows(IntPtr hWnd, IntPtr lParam)
        {
            if (hWnd != lParam)
            {
                if (WinAPI.IsWindowVisible(hWnd) && CanWindowMinimize(hWnd))
                {
                    WinAPI.ShowWindow(hWnd, (int)WinAPI.SW_MINIMIZE);
                }
            }

            return true;
        }

        public static void HandleCode(string other)
        {
            IntPtr hWnd = WinAPI.GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
            {
                return;
            }

            if (WinAPI.IsIconic(hWnd))
            {
                return;
            }

            WinAPI.RECT rt;
            WinAPI.GetWindowRect(hWnd, out rt);

            Screen curScreen = Screen.FromRectangle(rt);

            if (!rt.Equals(ScreensLeft(curScreen)) &&
                !rt.Equals(ScreensRight(curScreen)) &&
                !WinAPI.IsZoomed(hWnd))
            {
                if (!m_savedRects.ContainsKey(hWnd))
                {
                    m_knownWnds.AddLast(hWnd);
                    m_savedRects.Add(hWnd, rt);
                    while (m_knownWnds.Count > 50)
                    {
                        m_savedRects.Remove(m_knownWnds.First.Value);
                        m_knownWnds.RemoveFirst();
                    }
                }
                else
                {
                    m_savedRects[hWnd] = rt;
                }
            }

            switch (other.ToLower())
            {
                case "left":
                    if (!CanWindowSize(hWnd) || !WinAPI.IsWindowVisible(hWnd))
                    {
                        return;
                    }

                    if (WinAPI.IsZoomed(hWnd))
                    {
                        WinAPI.ShowWindow(hWnd, (int)WinAPI.SW_RESTORE);
                    }

                    if (rt.Equals(ScreensLeft(curScreen)))
                    {
                        MoveWindow(hWnd,
                            ScreensRight(OffScreen(curScreen, -1, 0)));
                    }
                    else if (rt.Equals(ScreensRight(curScreen)))
                    {
                        if (m_savedRects.ContainsKey(hWnd))
                        {
                            Screen last = Screen.FromRectangle(m_savedRects[hWnd]);
                            MoveWindow(hWnd,
                                new Rectangle(
                                    m_savedRects[hWnd].X - last.WorkingArea.X + curScreen.WorkingArea.X,
                                    m_savedRects[hWnd].Y - last.WorkingArea.Y + curScreen.WorkingArea.Y,
                                    m_savedRects[hWnd].Width,
                                    m_savedRects[hWnd].Height));
                        }
                        else
                        {
                            MoveWindow(hWnd,
                                new Rectangle(
                                    (int)(curScreen.WorkingArea.X + curScreen.WorkingArea.Width * 0.1),
                                    (int)(curScreen.WorkingArea.Y + curScreen.WorkingArea.Height * 0.1),
                                    (int)(curScreen.WorkingArea.Width * 0.8),
                                    (int)(curScreen.WorkingArea.Height * 0.8)));
                        }
                    }
                    else
                    {
                        MoveWindow(hWnd, ScreensLeft(curScreen));
                    }
                    break;
                case "right":
                    if (!CanWindowSize(hWnd) || !WinAPI.IsWindowVisible(hWnd))
                    {
                        return;
                    }

                    if (WinAPI.IsZoomed(hWnd))
                    {
                        WinAPI.ShowWindow(hWnd, (int)WinAPI.SW_RESTORE);
                    }

                    if (rt.Equals(ScreensRight(curScreen)))
                    {
                        MoveWindow(hWnd,
                            ScreensLeft(OffScreen(curScreen, 1, 0)));
                    }
                    else if (rt.Equals(ScreensLeft(curScreen)))
                    {
                        if (m_savedRects.ContainsKey(hWnd))
                        {
                            Screen last = Screen.FromRectangle(m_savedRects[hWnd]);
                            MoveWindow(hWnd,
                                new Rectangle(
                                    m_savedRects[hWnd].X - last.WorkingArea.X + curScreen.WorkingArea.X,
                                    m_savedRects[hWnd].Y - last.WorkingArea.Y + curScreen.WorkingArea.Y,
                                    m_savedRects[hWnd].Width,
                                    m_savedRects[hWnd].Height));
                        }
                        else
                        {
                            MoveWindow(hWnd,
                                new Rectangle(
                                    (int)(curScreen.WorkingArea.X + curScreen.WorkingArea.Width * 0.1),
                                    (int)(curScreen.WorkingArea.Y + curScreen.WorkingArea.Height * 0.1),
                                    (int)(curScreen.WorkingArea.Width * 0.8),
                                    (int)(curScreen.WorkingArea.Height * 0.8)));
                        }
                    }
                    else
                    {
                        MoveWindow(hWnd, ScreensRight(curScreen));
                    }
                    break;

                case "monleft":
                    MonWindow(curScreen, rt, hWnd, -1, 0);
                    break;
                case "monright":
                    MonWindow(curScreen, rt, hWnd, 1, 0);
                    break;
                case "monup":
                    MonWindow(curScreen, rt, hWnd, 0, -1);
                    break;
                case "mondown":
                    MonWindow(curScreen, rt, hWnd, 0, 1);
                    break;

                case "minother":
                    WinAPI.EnumWindows(MinimizeOtherWindows, hWnd);
                    break;

                case "stretch":
                    if (!CanWindowSize(hWnd) || !WinAPI.IsWindowVisible(hWnd))
                    {
                        return;
                    }

                    StretchWindow(curScreen, rt, hWnd);
                    break;

                case "min":
                    if (!CanWindowMinimize(hWnd) || !WinAPI.IsWindowVisible(hWnd))
                    {
                        return;
                    }

                    if (WinAPI.IsZoomed(hWnd))
                    {
                        WinAPI.ShowWindow(hWnd, (int)WinAPI.SW_RESTORE);
                    }
                    else
                    {
                        WinAPI.ShowWindow(hWnd, (int)WinAPI.SW_MINIMIZE);
                    }
                    break;

                case "max":
                    if (!CanWindowZoom(hWnd) || !WinAPI.IsWindowVisible(hWnd))
                    {
                        return;
                    }

                    if (!WinAPI.IsZoomed(hWnd))
                    {
                        WinAPI.ShowWindow(hWnd, (int)WinAPI.SW_MAXIMIZE);
                    }
                    break;
            }
        }

        static void StretchWindow(Screen curScreen, Rectangle rt, IntPtr hWnd)
        {
            MoveWindow(hWnd,
                new Rectangle(
                    rt.Left, curScreen.WorkingArea.Top, 
                    rt.Width, curScreen.WorkingArea.Height));
        }

        static bool CanWindowSize(IntPtr hWnd)
        {
            int dwStyle = WinAPI.GetWindowLong(hWnd, WinAPI.GWL_STYLE);
            if ((dwStyle & (int)WinAPI.WindowStyle.WS_THICKFRAME) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static bool CanWindowMinimize(IntPtr hWnd)
        {
            int dwStyle = WinAPI.GetWindowLong(hWnd, WinAPI.GWL_STYLE);
            if ((dwStyle & (int)WinAPI.WindowStyle.WS_MINIMIZEBOX) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static bool CanWindowZoom(IntPtr hWnd)
        {
            int dwStyle = WinAPI.GetWindowLong(hWnd, WinAPI.GWL_STYLE);
            if ((dwStyle & (int)WinAPI.WindowStyle.WS_MAXIMIZEBOX) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static void MonWindow(Screen curScreen, WinAPI.RECT rt, IntPtr hWnd, int x, int y)
        {
            if (!WinAPI.IsWindowVisible(hWnd))
            {
                return;
            }

            bool wasZoomed = false;
            if (WinAPI.IsZoomed(hWnd))
            {
                wasZoomed = true;
                WinAPI.ShowWindow(hWnd, (int)WinAPI.SW_RESTORE);
            }

            Screen screen = OffScreen(curScreen, x, y);
            MoveWindow(hWnd,
                new Rectangle(
                    rt.left - curScreen.WorkingArea.Left + screen.WorkingArea.Left,
                    rt.top - curScreen.WorkingArea.Top + screen.WorkingArea.Top,
                    rt.Width, rt.Height));

            if (wasZoomed)
            {
                WinAPI.ShowWindow(hWnd, (int)WinAPI.SW_MAXIMIZE);
            }
        }

        static void MoveWindow(IntPtr hWnd, Rectangle temp)
        {
            WinAPI.SetWindowPos(hWnd, IntPtr.Zero,
                temp.X, temp.Y, temp.Width, temp.Height,
                WinAPI.SWP_NOZORDER);
        }

        static Screen OffScreen(Screen from, int offx, int offy)
        {
            int offset = offx + offy;
            List<Screen> screens = new List<Screen>();

            foreach (Screen to in Screen.AllScreens)
            {
                if (offy == 0)
                {
                    if (to.Bounds.Top < (from.Bounds.Top + from.Bounds.Bottom) / 2 &&
                        to.Bounds.Bottom > (from.Bounds.Top + from.Bounds.Bottom) / 2)
                    {
                        screens.Add(to);
                    }
                }
                else
                {
                    if (to.Bounds.Left < (from.Bounds.Left + from.Bounds.Right) / 2 &&
                        to.Bounds.Right > (from.Bounds.Left + from.Bounds.Right) / 2)
                    {
                        screens.Add(to);
                    }
                }
            }

            if (offy == 0)
            {
                screens.Sort(delegate(Screen a, Screen b)
                {
                    return (((a.Bounds.Left + a.Bounds.Right) / 2).CompareTo(
                        ((b.Bounds.Left + b.Bounds.Right) / 2)));
                });
            }
            else
            {
                screens.Sort(delegate(Screen a, Screen b)
                {
                    return (((a.Bounds.Top + a.Bounds.Bottom) / 2).CompareTo(
                        ((b.Bounds.Top + b.Bounds.Bottom) / 2)));
                });
            }

            int index = 0;
            for (int i = 0; i < screens.Count; i++)
            {
                if (screens[i].Bounds == from.Bounds)
                {
                    index = i;
                }
            }
            index += offset;
            if (index >= screens.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = screens.Count - 1;
            }

            return screens[index];
        }

        static Rectangle ScreensRight(Screen curScreen)
        {
            Rectangle right = new Rectangle(
                curScreen.WorkingArea.Right - curScreen.WorkingArea.Width / 2, curScreen.WorkingArea.Y,
                curScreen.WorkingArea.Width / 2, curScreen.WorkingArea.Height);
            return right;
        }

        static Rectangle ScreensLeft(Screen curScreen)
        {
            Rectangle left = new Rectangle(
                curScreen.WorkingArea.X, curScreen.WorkingArea.Y,
                curScreen.WorkingArea.Width / 2, curScreen.WorkingArea.Height);
            return left;
        }
    }
}
