using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ScottsUtils
{
    class SpecialCommands
    {
        public static IEnumerable<string> GetAll()
        {
            Type type = typeof(SpecialCommands);
            foreach (FieldInfo field in type.GetFields(
                BindingFlags.Static | BindingFlags.Public))
            {
                object ret = field.GetValue(null);
                if (ret is string)
                {
                    yield return (string)ret;
                }
            }
        }

        public const string ActiveApp = "activeapp";
        public const string AddVar = "addvar";
        public const string AltDown = "altdown";
        public const string AltUp = "altup";
        public const string BackSpace = "backspace";
        public const string Begin = "begin";
        public const string BkSp = "bksp";
        public const string BS = "bs";
        public const string CapsLock = "capslock";
        public const string CD = "cd";
        public const string Click = "click";
        public const string Clip = "clip";
        public const string CtrlDown = "ctrldown";
        public const string CtrlUp = "ctrlup";
        public const string Del = "del";
        public const string Delete = "delete";
        public const string Down = "down";
        public const string End = "end";
        public const string Enter = "enter";
        public const string Esc = "esc";
        public const string F1 = "f1";
        public const string F2 = "f2";
        public const string F3 = "f3";
        public const string F4 = "f4";
        public const string F5 = "f5";
        public const string F6 = "f6";
        public const string F7 = "f7";
        public const string F8 = "f8";
        public const string F9 = "f9";
        public const string F10 = "f10";
        public const string F11 = "f11";
        public const string F12 = "f12";
        public const string Hibernate = "hibernate";
        public const string Home = "home";
        public const string Ins = "ins";
        public const string Insert = "insert";
        public const string Launch = "launch";
        public const string LClick = "lclick";
        public const string LDown = "ldown";
        public const string Left = "left";
        public const string Loop = "loop";
        public const string LUp = "lup";
        public const string MoveCursor = "movecursor";
        public const string NumLock = "numlock";
        public const string OffsetCursor = "offsetcursor";
        public const string PasteScrub = "pastescrub";
        public const string PgDn = "pgdn";
        public const string PgUp = "pgup";
        public const string RClick = "rclick";
        public const string RDown = "rdown";
        public const string RestoreCursor = "restorecursor";
        public const string Right = "right";
        public const string RotateDisplay = "rotatedisplay";
        public const string RUp = "rup";
        public const string ScrollLock = "scrolllock";
        public const string SetVar = "setvar";
        public const string ShiftDown = "shiftdown";
        public const string ShiftUp = "shiftup";
        public const string Sleep = "sleep";
        public const string StoreCursor = "storecursor";
        public const string Suspend = "suspend";
        public const string SwitchTo = "switchto";
        public const string SysCommand = "syscommand";
        public const string Tab = "tab";
        public const string TypeVar = "typevar";
        public const string Up = "up";
        //public const string VLC = "vlc";

        public const string Other_CurlyOpen = "{";
        public const string Other_CurlyClose = "}";
        public const string Other_Plus = "+";
        public const string Other_Caret = "^";
        public const string Other_Percent = "%";
    }
}
