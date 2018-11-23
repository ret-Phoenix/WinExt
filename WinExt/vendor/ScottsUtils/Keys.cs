using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace ScottsUtils
{
    static class Keys
    {
        static KeysState s_State = new KeysState();

        static void Hibernate(bool suspend)
        {
            IntPtr hProcess = WinAPI.GetCurrentProcess();
            IntPtr hToken = IntPtr.Zero;
            if (!WinAPI.OpenProcessToken(hProcess, WinAPI.TOKEN_ADJUST_PRIVILEGES, out hToken))
            {
                return;
            }

            WinAPI.LUID luid;
            if (!WinAPI.LookupPrivilegeValue(null, WinAPI.SE_SHUTDOWN_NAME, out luid))
            {
                return;
            }

            WinAPI.TOKEN_PRIVILEGES newState;
            newState.PrivilegeCount = 1;
            newState.Luid = luid;
            newState.Attributes = WinAPI.SE_PRIVILEGE_ENABLED;
            if (!WinAPI.AdjustTokenPrivileges(hToken, false, ref newState, 0, 0, 0))
            {
                return;
            }

            WinAPI.SetSystemPowerState(suspend, false);
        }

        public static void PressKey(char key, bool hold, bool release)
        {
            uint vk = (uint)WinAPI.VkKeyScan(key);

            if (vk == 0)
            {
                return;
            }

            uint scanCode = WinAPI.MapVirtualKey(vk & 0xFF, 0);
            uint extended = IsExtendedKey((WinAPI.KeyCode)(vk & 0xFF)) ? (uint)WinAPI.KeybdEventFlag.ExtendedKey : 0;

            bool shift = (vk & 0x100) != 0;
            bool ctrl = (vk & 0x200) != 0;
            bool alt = (vk & 0x400) != 0;

            vk = vk & 0xFF;

            if (!release)
            {
                if (shift)
                {
                    PressKeyVK(WinAPI.KeyCode.Shift, true, false, false);
                }
                if (ctrl)
                {
                    PressKeyVK(WinAPI.KeyCode.Control, true, false, false);
                }
                if (alt)
                {
                    PressKeyVK(WinAPI.KeyCode.Alt, true, false, false);
                }

                WinAPI.keybd_event((byte)vk, (byte)scanCode, (WinAPI.KeybdEventFlag)extended, 0);
            }

            if (!hold)
            {
                WinAPI.keybd_event((byte)vk, (byte)scanCode, (WinAPI.KeybdEventFlag)(extended | (uint)WinAPI.KeybdEventFlag.KeyUp), 0);

                if (shift)
                {
                    PressKeyVK(WinAPI.KeyCode.Shift, false, true, false);
                }
                if (ctrl)
                {
                    PressKeyVK(WinAPI.KeyCode.Control, false, true, false);
                }
                if (alt)
                {
                    PressKeyVK(WinAPI.KeyCode.Alt, false, true, false);
                }
            }
        }

        static public void PressString(string toPress)
        {
            PressString(toPress, false);
        }

        static public void PressString(string toPress, bool doEvents)
        {
            foreach (char key in toPress)
            {
                PressKey(key, false, false);
                Thread.Sleep(20);
                if (doEvents)
                {
                    Application.DoEvents();
                }
            }
        }

        static public void PressKeyVK(WinAPI.KeyCode key)
        {
            PressKeyVK(key, true, true, false);
        }

        static public void PressKeyVK(WinAPI.KeyCode key, bool press, bool release, bool compatible)
        {
            if (key > WinAPI.KeyCode.MouseFirst)
            {
                switch (key)
                {
                    case WinAPI.KeyCode.MouseLClick:
                        WinAPI.mouse_event(WinAPI.MouseEventFlag.LeftDown, 0, 0, 0, 0);
                        WinAPI.mouse_event(WinAPI.MouseEventFlag.LeftUp, 0, 0, 0, 0);
                        break;
                    case WinAPI.KeyCode.MouseLDown:
                        WinAPI.mouse_event(WinAPI.MouseEventFlag.LeftDown, 0, 0, 0, 0);
                        break;
                    case WinAPI.KeyCode.MouseLUp:
                        WinAPI.mouse_event(WinAPI.MouseEventFlag.LeftUp, 0, 0, 0, 0);
                        break;
                    case WinAPI.KeyCode.MouseRClick:
                        WinAPI.mouse_event(WinAPI.MouseEventFlag.RightDown, 0, 0, 0, 0);
                        WinAPI.mouse_event(WinAPI.MouseEventFlag.RightUp, 0, 0, 0, 0);
                        break;
                    case WinAPI.KeyCode.MouseRDown:
                        WinAPI.mouse_event(WinAPI.MouseEventFlag.RightDown, 0, 0, 0, 0);
                        break;
                    case WinAPI.KeyCode.MouseRUp:
                        WinAPI.mouse_event(WinAPI.MouseEventFlag.RightUp, 0, 0, 0, 0);
                        break;
                }
                return;
            }

            uint scanCode = WinAPI.MapVirtualKey((uint)key, 0);
            uint extended = IsExtendedKey(key) ? (uint)WinAPI.KeybdEventFlag.ExtendedKey : 0;

            if (compatible)
            {
                extended = 0;
            }

            if (press)
            {
                WinAPI.keybd_event((byte)key, (byte)scanCode, (WinAPI.KeybdEventFlag)extended, 0);
            }

            if (release)
            {
                WinAPI.keybd_event((byte)key, (byte)scanCode, (WinAPI.KeybdEventFlag)(extended | (uint)WinAPI.KeybdEventFlag.KeyUp), 0);
            }
        }

        static public bool IsExtendedKey(WinAPI.KeyCode key)
        {
            switch (key)
            {
                case WinAPI.KeyCode.Left:
                case WinAPI.KeyCode.Up:
                case WinAPI.KeyCode.Right:
                case WinAPI.KeyCode.Down:
                case WinAPI.KeyCode.Prior:
                case WinAPI.KeyCode.Next:
                case WinAPI.KeyCode.End:
                case WinAPI.KeyCode.Home:
                case WinAPI.KeyCode.Insert:
                case WinAPI.KeyCode.Delete:
                case WinAPI.KeyCode.Divide:
                case WinAPI.KeyCode.NumLock:
                    return true;

                default:
                    return false;
            }
        }

        static public void GetLockStatus(ref bool capsLock, ref bool numLock, ref bool scrollLock)
        {
            capsLock = WinAPI.GetKeyState((uint)WinAPI.KeyCode.Capital) != 0;
            numLock = WinAPI.GetKeyState((uint)WinAPI.KeyCode.NumLock) != 0;
            scrollLock = WinAPI.GetKeyState((uint)WinAPI.KeyCode.Scroll) != 0;
        }

        static public void ClearModifiers(ref uint modifiers)
        {
            if ((modifiers & (uint)WinAPI.ModifyCode.Alt) != 0)
            {
                PressKeyVK(WinAPI.KeyCode.Alt, false, true, false);
                PressKeyVK(WinAPI.KeyCode.Alt, false, true, true);
            }

            if ((modifiers & (uint)WinAPI.ModifyCode.Control) != 0)
            {
                PressKeyVK(WinAPI.KeyCode.Control, false, true, false);
                PressKeyVK(WinAPI.KeyCode.Control, false, true, true);
            }

            if ((modifiers & (uint)WinAPI.ModifyCode.Shift) != 0)
            {
                PressKeyVK(WinAPI.KeyCode.Shift, false, true, false);
                PressKeyVK(WinAPI.KeyCode.Shift, false, true, true);
            }

            if ((modifiers & (uint)WinAPI.ModifyCode.WinKey) != 0)
            {
                PressKeyVK(WinAPI.KeyCode.RWin, false, true, false);
                PressKeyVK(WinAPI.KeyCode.LWin, false, true, true);
                PressKeyVK(WinAPI.KeyCode.RWin, false, true, false);
                PressKeyVK(WinAPI.KeyCode.LWin, false, true, true);
            }

            modifiers = 0;
        }

        static public void PressSendKeys(string keys)
        {
            PressSendKeys(keys, 0);
        }

        static public void PressSendKeys(string keys, uint modifiers)
        {
            s_State.Reset();

            List<WinAPI.KeyCode> modify = new List<WinAPI.KeyCode>();
            KeyState state = new KeyState();

            for (int pos = 0; pos < keys.Length; pos++)
            {
                Regex re;
                Match m;
                bool handled = false;

                if (!handled)
                {
                    re = new Regex("^([\\^\\%\\+]+)\\(([^\\)]+)\\)");
                    m = re.Match(keys.Substring(pos));
                    if (m.Success)
                    {
                        ClearModifiers(ref modifiers);
                        handled = true;

                        foreach (char cur in m.Groups[1].ToString())
                        {
                            switch (cur)
                            {
                                case '+':
                                    PressKeyVK(WinAPI.KeyCode.Shift, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Shift);
                                    break;
                                case '%':
                                    PressKeyVK(WinAPI.KeyCode.Alt, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Alt);
                                    break;
                                case '^':
                                    PressKeyVK(WinAPI.KeyCode.Control, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Control);
                                    break;
                            }
                        }
                        PressSendKeys(m.Groups[2].ToString(), modifiers);
                        foreach (WinAPI.KeyCode vk in modify)
                        {
                            PressKeyVK(vk, false, true, false);
                        }
                        modify.Clear();

                        pos += m.Groups[1].ToString().Length + m.Groups[2].ToString().Length + 1;
                    }
                }

                if (!handled)
                {
                    re = new Regex("^([\\^\\%\\+]+)\\{([^\\}]+)\\}");
                    m = re.Match(keys.Substring(pos));
                    if (m.Success)
                    {
                        ClearModifiers(ref modifiers);
                        handled = true;

                        foreach (char cur in m.Groups[1].ToString())
                        {
                            switch (cur)
                            {
                                case '+':
                                    PressKeyVK(WinAPI.KeyCode.Shift, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Shift);
                                    break;
                                case '%':
                                    PressKeyVK(WinAPI.KeyCode.Alt, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Alt);
                                    break;
                                case '^':
                                    PressKeyVK(WinAPI.KeyCode.Control, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Control);
                                    break;
                            }
                        }
                        PressSendKeys("{" + m.Groups[2].ToString() + "}", 0);
                        foreach (WinAPI.KeyCode vk in modify)
                        {
                            PressKeyVK(vk, false, true, false);
                        }
                        modify.Clear();

                        pos += m.Groups[1].ToString().Length + m.Groups[2].ToString().Length + 1;
                    }
                }

                if (!handled)
                {
                    re = new Regex("^([\\^\\%\\+]+)(.)");
                    m = re.Match(keys.Substring(pos));
                    if (m.Success)
                    {
                        ClearModifiers(ref modifiers);
                        handled = true;

                        foreach (char cur in m.Groups[1].ToString())
                        {
                            switch (cur)
                            {
                                case '+':
                                    PressKeyVK(WinAPI.KeyCode.Shift, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Shift);
                                    break;
                                case '%':
                                    PressKeyVK(WinAPI.KeyCode.Alt, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Alt);
                                    break;
                                case '^':
                                    PressKeyVK(WinAPI.KeyCode.Control, true, false, false);
                                    modify.Add(WinAPI.KeyCode.Control);
                                    break;
                            }
                        }
                        PressSendKeys(m.Groups[2].ToString(), 0);
                        foreach (WinAPI.KeyCode vk in modify)
                        {
                            PressKeyVK(vk, false, true, false);
                        }
                        modify.Clear();

                        pos += m.Groups[1].ToString().Length;
                    }
                }

                if (!handled)
                {
                    if (keys[pos] == '~')
                    {
                        ClearModifiers(ref modifiers);
                        handled = true;

                        PressKeyVK(WinAPI.KeyCode.Return);
                    }
                }

                if (!handled)
                {
                    if (keys.Length - pos >= 3)
                    {
                        if (keys.Substring(pos, 3) == "{}}")
                        {
                            ClearModifiers(ref modifiers);
                            handled = true;

                            PressString("}");

                            pos += 2;
                        }
                    }
                }

                if (!handled)
                {
                    if (keys.Length - pos >= 3)
                    {
                        if (keys.Substring(pos, 3) == "{{}")
                        {
                            ClearModifiers(ref modifiers);
                            handled = true;

                            PressString("{");

                            pos += 2;
                        }
                    }
                }

                if (!handled)
                {
                    re = new Regex("^\\{([^\\{]+)\\}");
                    m = re.Match(keys.Substring(pos));
                    if (m.Success)
                    {
                        handled = true;

                        int nextPos = pos;

                        HandleSpecial(state, m.Groups[1].ToString(), ref modifiers, ref nextPos);

                        if (nextPos != pos)
                        {
                            pos = nextPos - 1;
                        }
                        else
                        {
                            pos += m.Groups[1].ToString().Length + 1;
                        }
                    }
                }

                if (!handled)
                {
                    ClearModifiers(ref modifiers);
                    PressString(keys[pos].ToString());
                }
            }
        }

        static void HandleSpecial(KeyState state, string special, ref uint modifiers, ref int nextPos)
        {
            int count = 1;
            string other = "";

            Regex re = new Regex("^([^ ]+) *<([0-9]+)>$");
            Match m = re.Match(special);
            if (m.Success)
            {
                special = m.Groups[1].ToString();
                count = int.Parse(m.Groups[2].ToString());
            }

            re = new Regex("^([^ ]+) (.+)$");
            m = re.Match(special);
            if (m.Success)
            {
                special = m.Groups[1].ToString();
                other = m.Groups[2].ToString();
            }

            for (int i = 0; i < count; i++)
            {
                switch (special.ToLower())
                {
                    case SpecialCommands.Begin:
                        s_State.StartLoop(nextPos);
                        break;
                    case SpecialCommands.Loop:
                        {
                            int targetPos = 0;
                            if (s_State.IterateLoop(ReadNum(other), ref targetPos))
                            {
                                nextPos = targetPos;
                            }
                        }
                        break;
                    case SpecialCommands.AddVar:
                        s_State.Variable += ReadNum(other);
                        break;
                    case SpecialCommands.SetVar:
                        s_State.Variable = ReadNum(other);
                        break;
                    case SpecialCommands.TypeVar:
                        PressString(s_State.Variable.ToString());
                        break;
                    case SpecialCommands.ActiveApp:
                        ActiveApp.HandleCode(other);
                        break;
                    case SpecialCommands.RotateDisplay:
                        SpecialRotateDisplay(other);
                        break;
                    case SpecialCommands.PasteScrub:
                        ClearModifiers(ref modifiers);
                        SpecialPasteScrub();
                        break;
                    case SpecialCommands.Hibernate:
                        Hibernate(false);
                        break;
                    case SpecialCommands.Suspend:
                        Hibernate(true);
                        break;
                    case SpecialCommands.Launch:
                        SpecialLaunch(other);
                        i = count;
                        break;
                    case SpecialCommands.Clip:
                        ClearModifiers(ref modifiers);
                        SpecialClip();
                        i = count;
                        break;
                    case SpecialCommands.SysCommand:
                        SpecialSysCommand(other);
                        i = count;
                        break;
                    case SpecialCommands.Sleep:
                        Thread.Sleep(ReadNum(other));
                        break;
                    case SpecialCommands.StoreCursor:
                        SpecialStoreCursor(state);
                        break;
                    case SpecialCommands.RestoreCursor:
                        SpecialRestoreCursor(state);
                        break;
                    case SpecialCommands.MoveCursor:
                        SpecialMoveCursor(other);
                        break;
                    case SpecialCommands.OffsetCursor:
                        SpecialOffsetCursor(other);
                        i = count;
                        break;
                    //case SpecialCommands.VLC:
                    //    SpecialVLC(other);
                    //    i = count;
                    //    break;
                    case SpecialCommands.CD:
                        SpecialCD(other);
                        break;
                    case SpecialCommands.AltDown:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Alt, true, false, false);
                        break;
                    case SpecialCommands.AltUp:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Alt, false, true, false);
                        break;
                    case SpecialCommands.CtrlDown:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Control, true, false, false);
                        break;
                    case SpecialCommands.CtrlUp:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Control, false, true, false);
                        break;
                    case SpecialCommands.ShiftDown:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Shift, true, false, false);
                        break;
                    case SpecialCommands.ShiftUp:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Shift, false, true, false);
                        break;
                    case SpecialCommands.SwitchTo:
                        SwitchToWindow.Switch(other);
                        i = count;
                        break;
                    case SpecialCommands.BackSpace:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Backspace);
                        break;
                    case SpecialCommands.BkSp:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Backspace);
                        break;
                    case SpecialCommands.BS:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Backspace);
                        break;
                    case SpecialCommands.CapsLock:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Capital);
                        break;
                    case SpecialCommands.Del:
                    case SpecialCommands.Delete:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Delete);
                        break;
                    case SpecialCommands.Down:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Down);
                        break;
                    case SpecialCommands.End:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.End);
                        break;
                    case SpecialCommands.Enter:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Return);
                        break;
                    case SpecialCommands.Esc:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Escape);
                        break;
                    case SpecialCommands.F1:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F1);
                        break;
                    case SpecialCommands.F2:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F2);
                        break;
                    case SpecialCommands.F3:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F3);
                        break;
                    case SpecialCommands.F4:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F4);
                        break;
                    case SpecialCommands.F5:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F5);
                        break;
                    case SpecialCommands.F6:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F6);
                        break;
                    case SpecialCommands.F7:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F7);
                        break;
                    case SpecialCommands.F8:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F8);
                        break;
                    case SpecialCommands.F9:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F9);
                        break;
                    case SpecialCommands.F10:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F10);
                        break;
                    case SpecialCommands.F11:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F11);
                        break;
                    case SpecialCommands.F12:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.F12);
                        break;
                    case SpecialCommands.Home:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Home);
                        break;
                    case SpecialCommands.Ins:
                    case SpecialCommands.Insert:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Insert);
                        break;
                    case SpecialCommands.Click:
                    case SpecialCommands.LClick:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.MouseLClick);
                        break;
                    case SpecialCommands.LDown:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.MouseLDown);
                        break;
                    case SpecialCommands.Left:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Left);
                        break;
                    case SpecialCommands.LUp:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.MouseLUp);
                        break;
                    case SpecialCommands.NumLock:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.NumLock);
                        break;
                    case SpecialCommands.PgDn:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.PageDown);
                        break;
                    case SpecialCommands.PgUp:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.PageUp);
                        break;
                    case SpecialCommands.RClick:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.MouseRClick);
                        break;
                    case SpecialCommands.RDown:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.MouseRDown);
                        break;
                    case SpecialCommands.Right:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Right);
                        break;
                    case SpecialCommands.RUp:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.MouseRUp);
                        break;
                    case SpecialCommands.ScrollLock:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Scroll);
                        break;
                    case SpecialCommands.Tab:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Tab);
                        break;
                    case SpecialCommands.Up:
                        ClearModifiers(ref modifiers);
                        PressKeyVK(WinAPI.KeyCode.Up);
                        break;
                    default:
                        ClearModifiers(ref modifiers);
                        PressString(special);
                        break;
                }
            }
        }

        static int ReadNum(string other)
        {
            if (other.StartsWith("["))
            {
                other = other.Substring(1);
            }

            if (other.EndsWith("]"))
            {
                other = other.Substring(0, other.Length - 1);
            }

            int ret = 0;

            if (!int.TryParse(other, out ret))
            {
                ret = 0;
            }

            return ret;
        }

        static void SpecialCD(string other)
        {
            switch (other.ToLower())
            {
                case "open":
                    WinAPI.mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
                    break;
                case "close":
                    WinAPI.mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero);
                    break;
            }
        }

        //static void SpecialVLC(string other)
        //{
        //    string[] split = other.Split(',');
        //    if (split.Length == 2)
        //    {
        //        int port = int.Parse(split[0]);
        //        VLC.SendCommand(port, split[1]);
        //    }
        //}

        static void SpecialOffsetCursor(string other)
        {
            string[] split = other.Split(',');
            if (split.Length == 2)
            {
                ScottsUtils.WinAPI.POINT cur;
                WinAPI.GetCursorPos(out cur);

                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);

                WinAPI.SetCursorPos(cur.X + x, cur.Y + y);
            }
        }

        static void SpecialMoveCursor(string other)
        {
            string[] split = other.Split(',');
            if (split.Length == 2)
            {
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);

                WinAPI.SetCursorPos(x, y);
            }
        }

        static void SpecialStoreCursor(KeyState state)
        {
            WinAPI.GetCursorPos(out state.Point);
        }

        static void SpecialRestoreCursor(KeyState state)
        {
            WinAPI.SetCursorPos(state.Point.X, state.Point.Y);
        }

        static void SpecialSysCommand(string other)
        {
            switch (other.ToLower())
            {
                case "close":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_CLOSE, 0);
                    break;
                case "hscroll":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_HSCROLL, 0);
                    break;
                case "maximize":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_MAXIMIZE, 0);
                    break;
                case "minimize":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_MINIMIZE, 0);
                    break;
                case "move":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_MOVE, 0);
                    break;
                case "nextwindow":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_NEXTWINDOW, 0);
                    break;
                case "prevwindow":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_PREVWINDOW, 0);
                    break;
                case "restore":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_RESTORE, 0);
                    break;
                case "screensave":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_SCREENSAVE, 0);
                    break;
                case "size":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_SIZE, 0);
                    break;
                case "tasklist":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_TASKLIST, 0);
                    break;
                case "vscroll":
                    WinAPI.PostMessage(WinAPI.GetForegroundWindow(), WinAPI.WindowMessage.WM_SYSCOMMAND, WinAPI.SysCommand.SC_VSCROLL, 0);
                    break;
            }
        }

        static void SpecialClip()
        {
            try
            {
                PressString(Clipboard.GetText());
            }
            catch (Exception)
            {
                // Nothing
            }
        }

        static void SpecialLaunch(string other)
        {
            try
            {
                StringBuilder prog = new StringBuilder();
                StringBuilder arg = new StringBuilder();
                bool toProg = true;
                bool quote = false;

                foreach (char cur in other)
                {
                    if (toProg)
                    {
                        if (cur == '"')
                        {
                            quote = !quote;
                        }
                        if (!quote)
                        {
                            if (cur == ' ')
                            {
                                toProg = false;
                            }
                        }
                        if (toProg)
                        {
                            prog.Append(cur);
                        }
                    }
                    else
                    {
                        arg.Append(cur);
                    }
                }
                System.Diagnostics.Process.Start(prog.ToString(), arg.ToString());
            }
            catch (Exception)
            {
                // Nothing
            }
        }

        static void SpecialPasteScrub()
        {
            try
            {
                string temp = Clipboard.GetText();
                Clipboard.Clear();
                temp = temp.Replace("\r\n", "\n");
                temp = temp.Replace("\r", "\n");
                temp = temp.Replace("\n", "\r\n");
                Clipboard.SetText(temp);
                PressSendKeys("^v", 0);
            }
            catch (Exception)
            {
                // Nothing
            }
        }

        static void SpecialRotateDisplay(string other)
        {
            try
            {
                uint target = WinAPI.DMDO_DEFAULT;

                switch (other)
                {
                    case "90d":
                        target = WinAPI.DMDO_90;
                        break;
                    case "180d":
                        target = WinAPI.DMDO_180;
                        break;
                    case "270d":
                        target = WinAPI.DMDO_270;
                        break;
                }

                WinAPI.DEVMODE dm = new WinAPI.DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(dm);
                WinAPI.EnumDisplaySettings(null, WinAPI.ENUM_CURRENT_SETTINGS, ref dm);

                if (other == "")
                {
                    switch (dm.dmDisplayOrientation)
                    {
                        case WinAPI.DMDO_DEFAULT:
                            target = WinAPI.DMDO_270;
                            break;
                        case WinAPI.DMDO_270:
                            target = WinAPI.DMDO_180;
                            break;
                        case WinAPI.DMDO_180:
                            target = WinAPI.DMDO_90;
                            break;
                        case WinAPI.DMDO_90:
                            target = WinAPI.DMDO_DEFAULT;
                            break;
                    }
                }

                bool swap = false;
                if ((target == WinAPI.DMDO_DEFAULT || target == WinAPI.DMDO_180) &&
                    (dm.dmDisplayOrientation == WinAPI.DMDO_90 || dm.dmDisplayOrientation == WinAPI.DMDO_270))
                {
                    swap = true;
                }
                else if ((target == WinAPI.DMDO_90 || target == WinAPI.DMDO_270) &&
                         (dm.dmDisplayOrientation == WinAPI.DMDO_DEFAULT || dm.dmDisplayOrientation == WinAPI.DMDO_180))
                {
                    swap = true;
                }

                if (swap)
                {
                    uint dwTemp = 0;
                    dwTemp = dm.dmPelsWidth;
                    dm.dmPelsWidth = dm.dmPelsHeight;
                    dm.dmPelsHeight = dwTemp;
                }
                dm.dmDisplayOrientation = target;

                dm.dmFields = WinAPI.DM_DISPLAYORIENTATION |
                    WinAPI.DM_PELSWIDTH | WinAPI.DM_PELSHEIGHT;

                WinAPI.ChangeDisplaySettings(ref dm, 0);
            }
            catch (Exception)
            {
                // Nothing
            }
        }

        public static string VerifySpecial(string text)
        {
            Regex re = new Regex("\\{(.[^\\}]*)\\}");
            Match m = re.Match(text);

            List<string> all = new List<string>(SpecialCommands.GetAll());

            while (m != null && m.Success)
            {
                string cmd = m.Groups[1].Value;
                cmd = Regex.Replace(cmd, " .*$", "");
                cmd = cmd.ToLower();

                if (!all.Contains(cmd))
                {
                    return cmd;
                }

                m = m.NextMatch();
            }

            return null;
        }
    }
}
