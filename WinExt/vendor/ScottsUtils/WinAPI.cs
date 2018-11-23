using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottsUtils
{
    public static class WinAPI
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, UInt32 BufferLength, /* ref TOKEN_PRIVILEGES */ UInt32 PreviousState, /* IntPtr */ UInt32 ReturnLength);
        [DllImport("kernel32.dll")]
        public static extern bool SetSystemPowerState(bool fSuspend, bool fForce);
        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, KeybdEventFlag dwFlags, uint dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern void mouse_event(MouseEventFlag dwFlags, int dx, int dy, uint dwData, uint dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern short GetKeyState(uint nVirtKey);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("winmm.dll")]
        public static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, WindowMessage Msg, SysCommand wParam, uint lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, WindowMessage Msg, uint wParam, uint lParam);
        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string deviceName, uint modeNum, ref DEVMODE devMode);
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, uint dwflags);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public const uint TOKEN_ADJUST_PRIVILEGES = 0x20;
        public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        public const uint SE_PRIVILEGE_ENABLED = 0x2;
        public const uint SW_MAXIMIZE = 3;
        public const uint SW_MINIMIZE = 6;
        public const uint SW_RESTORE = 9;
        public const int GWL_STYLE = -16;
        public const uint SWP_NOZORDER = 0x4;
        public const uint DMDO_180 = 2;
        public const uint DMDO_270 = 3;
        public const uint DMDO_90 = 1;
        public const uint DMDO_DEFAULT = 0;
        public const uint ENUM_CURRENT_SETTINGS = 0xFFFFFFFF;
        public const uint DM_DISPLAYORIENTATION = 0x80;
        public const uint DM_PELSHEIGHT = 0x100000;
        public const uint DM_PELSWIDTH = 0x80000;

        public enum KeyCode : uint
        {
            None = 0x0,
            LButton = 0x1,
            RButton = 0x2,
            Cancel = 0x3,
            MButton = 0x4,
            XButton1 = 0x5,
            XButton2 = 0x6,
            Backspace = 0x8,
            Tab = 0x9,
            Clear = 0xC,
            Return = 0xD,
            Shift = 0x10,
            Control = 0x11,
            Menu = 0x12,
            Alt = 0x12,
            Pause = 0x13,
            Capital = 0x14,
            Kana = 0x15,
            Hangeul = 0x15,
            Hangul = 0x15,
            Junja = 0x17,
            Final = 0x18,
            Hanja = 0x19,
            Kanji = 0x19,
            Escape = 0x1B,
            Convert = 0x1C,
            Nonconvert = 0x1D,
            Accept = 0x1E,
            ModeChange = 0x1F,
            Space = 0x20,
            Prior = 0x21,
            PageUp = 0x21,
            Next = 0x22,
            PageDown = 0x22,
            End = 0x23,
            Home = 0x24,
            Left = 0x25,
            Up = 0x26,
            Right = 0x27,
            Down = 0x28,
            Select = 0x29,
            Print = 0x2A,
            Execute = 0x2B,
            Snapshot = 0x2C,
            Insert = 0x2D,
            Delete = 0x2E,
            Help = 0x2F,
            Num0 = 0x30,
            Num1 = 0x31,
            Num2 = 0x32,
            Num3 = 0x33,
            Num4 = 0x34,
            Num5 = 0x35,
            Num6 = 0x36,
            Num7 = 0x37,
            Num8 = 0x38,
            Num9 = 0x39,
            LetA = 0x41,
            LetB = 0x42,
            LetC = 0x43,
            LetD = 0x44,
            LetE = 0x45,
            LetF = 0x46,
            LetG = 0x47,
            LetH = 0x48,
            LetI = 0x49,
            LetJ = 0x4A,
            LetK = 0x4B,
            LetL = 0x4C,
            LetM = 0x4D,
            LetN = 0x4E,
            LetO = 0x4F,
            LetP = 0x50,
            LetQ = 0x51,
            LetR = 0x52,
            LetS = 0x53,
            LetT = 0x54,
            LetU = 0x55,
            LetV = 0x56,
            LetW = 0x57,
            LetX = 0x58,
            LetY = 0x59,
            LetZ = 0x5A,
            LWin = 0x5B,
            RWin = 0x5C,
            Apps = 0x5D,
            Sleep = 0x5F,
            NumPad0 = 0x60,
            NumPad1 = 0x61,
            NumPad2 = 0x62,
            NumPad3 = 0x63,
            NumPad4 = 0x64,
            NumPad5 = 0x65,
            NumPad6 = 0x66,
            NumPad7 = 0x67,
            NumPad8 = 0x68,
            NumPad9 = 0x69,
            Multiply = 0x6A,
            Add = 0x6B,
            Separator = 0x6C,
            Subtract = 0x6D,
            Decimal = 0x6E,
            Divide = 0x6F,
            F1 = 0x70,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            F13 = 0x7C,
            F14 = 0x7D,
            F15 = 0x7E,
            F16 = 0x7F,
            F17 = 0x80,
            F18 = 0x81,
            F19 = 0x82,
            F20 = 0x83,
            F21 = 0x84,
            F22 = 0x85,
            F23 = 0x86,
            F24 = 0x87,
            NumLock = 0x90,
            Scroll = 0x91,
            Equal = 0x92,
            Jisho = 0x92,
            Masshou = 0x93,
            Touroku = 0x94,
            Loya = 0x95,
            Roya = 0x96,
            LShift = 0xA0,
            RShift = 0xA1,
            LControl = 0xA2,
            RControl = 0xA3,
            LMenu = 0xA4,
            RMenu = 0xA5,
            BrowserBack = 0xA6,
            BrowserForward = 0xA7,
            BrowserRefresh = 0xA8,
            BrowserStop = 0xA9,
            BrowserSearch = 0xAA,
            BrowserFavorites = 0xAB,
            BrowserHome = 0xAC,
            VolumeMute = 0xAD,
            VolumeDown = 0xAE,
            VolumeUp = 0xAF,
            MediaNextTrack = 0xB0,
            MediaPrevTrack = 0xB1,
            MediaStop = 0xB2,
            MediaPlayPause = 0xB3,
            LaunchMail = 0xB4,
            LaunchMediaSelect = 0xB5,
            LaunchApp1 = 0xB6,
            LaunchApp2 = 0xB7,
            Oem1 = 0xBA,
            OemPlus = 0xBB,
            OemComma = 0xBC,
            OemMinus = 0xBD,
            OemPeriod = 0xBE,
            Oem2 = 0xBF,
            Oem3 = 0xC0,
            Oem4 = 0xDB,
            Oem5 = 0xDC,
            Oem6 = 0xDD,
            Oem7 = 0xDE,
            Oem8 = 0xDF,
            OemAx = 0xE1,
            Oem102 = 0xE2,
            IcoHelp = 0xE3,
            Ico00 = 0xE4,
            ProcessKey = 0xE5,
            IcoClear = 0xE6,
            Packet = 0xE7,
            OemReset = 0xE9,
            OemJump = 0xEA,
            OemPa1 = 0xEB,
            OemPa2 = 0xEC,
            OemPa3 = 0xED,
            OemWsctrl = 0xEE,
            OemCusel = 0xEF,
            OemAttn = 0xF0,
            OemFinish = 0xF1,
            OemCopy = 0xF2,
            OemAuto = 0xF3,
            OemEnlw = 0xF4,
            OemBackTab = 0xF5,
            Attention = 0xF6,
            CurrentSelection = 0xF7,
            ExtraSelection = 0xF8,
            EndOfFile = 0xF9,
            Play = 0xFA,
            Zoom = 0xFB,
            NoName = 0xFC,
            Pa1 = 0xFD,
            OemClear = 0xFE,
            MouseFirst = 0x100,
            MouseLClick = 0x101,
            MouseRClick = 0x102,
            MouseLDown = 0x103,
            MouseLUp = 0x104,
            MouseRDown = 0x105,
            MouseRUp = 0x106,
        }
        public enum KeybdEventFlag : uint
        {
            ExtendedKey = 0x1,
            KeyUp = 0x2,
        }
        public enum MouseEventFlag : uint
        {
            LeftDown = 0x2,
            LeftUp = 0x4,
            MiddleDown = 0x20,
            MiddleUp = 0x40,
            Move = 0x1,
            Absolute = 0x8000,
            RightDown = 0x8,
            RightUp = 0x10
        }
        public enum ModifyCode : uint
        {
            Alt = 0x1,
            Control = 0x2,
            Shift = 0x4,
            WinKey = 0x8,
        }
        [Flags]
        public enum WindowStyle : uint
        {
            WS_ACTIVECAPTION = 0x1,
            WS_BORDER = 0x800000,
            WS_CAPTION = 0xC00000,
            WS_CHILD = 0x40000000,
            WS_CLIPCHILDREN = 0x2000000,
            WS_CLIPSIBLINGS = 0x4000000,
            WS_DISABLED = 0x8000000,
            WS_DLGFRAME = 0x400000,
            WS_EX_ACCEPTFILES = 0x10,
            WS_EX_APPWINDOW = 0x40000,
            WS_EX_CLIENTEDGE = 0x200,
            WS_EX_COMPOSITED = 0x2000000,
            WS_EX_CONTEXTHELP = 0x400,
            WS_EX_CONTROLPARENT = 0x10000,
            WS_EX_DLGMODALFRAME = 0x1,
            WS_EX_LAYERED = 0x80000,
            WS_EX_LAYOUTRTL = 0x400000,
            WS_EX_LEFT = 0x0,
            WS_EX_LEFTSCROLLBAR = 0x4000,
            WS_EX_LTRREADING = 0x0,
            WS_EX_MDICHILD = 0x40,
            WS_EX_NOACTIVATE = 0x8000000,
            WS_EX_NOINHERITLAYOUT = 0x100000,
            WS_EX_NOPARENTNOTIFY = 0x4,
            WS_EX_RIGHT = 0x1000,
            WS_EX_RIGHTSCROLLBAR = 0x0,
            WS_EX_RTLREADING = 0x2000,
            WS_EX_STATICEDGE = 0x20000,
            WS_EX_TOOLWINDOW = 0x80,
            WS_EX_TOPMOST = 0x8,
            WS_EX_TRANSPARENT = 0x20,
            WS_EX_WINDOWEDGE = 0x100,
            WS_GROUP = 0x20000,
            WS_HSCROLL = 0x100000,
            WS_MAXIMIZE = 0x1000000,
            WS_MAXIMIZEBOX = 0x10000,
            WS_MINIMIZE = 0x20000000,
            WS_MINIMIZEBOX = 0x20000,
            WS_OVERLAPPED = 0x0,
            WS_POPUP = 0x80000000,
            WS_SYSMENU = 0x80000,
            WS_TABSTOP = 0x10000,
            WS_THICKFRAME = 0x40000,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x200000,
        }
        public enum WindowMessage : uint
        {
            EM_AUTOURLDETECT = 1115,
            EM_CANPASTE = 1074,
            EM_CANREDO = 1109,
            EM_CANUNDO = 198,
            EM_CHARFROMPOS = 215,
            EM_DISPLAYBAND = 1075,
            EM_EMPTYUNDOBUFFER = 205,
            EM_EXGETSEL = 1076,
            EM_EXLIMITTEXT = 1077,
            EM_EXLINEFROMCHAR = 1078,
            EM_EXSETSEL = 1079,
            EM_FINDTEXT = 1080,
            EM_FINDTEXTEX = 1103,
            EM_FINDTEXTEXW = 1148,
            EM_FINDTEXTW = 1147,
            EM_FINDWORDBREAK = 1100,
            EM_FMTLINES = 200,
            EM_FORMATRANGE = 1081,
            EM_GETAUTOURLDETECT = 1116,
            EM_GETBIDIOPTIONS = 1225,
            EM_GETCHARFORMAT = 1082,
            EM_GETCTFMODEBIAS = 1261,
            EM_GETCTFOPENSTATUS = 1264,
            EM_GETCUEBANNER = 5378,
            EM_GETEDITSTYLE = 1229,
            EM_GETEVENTMASK = 1083,
            EM_GETFIRSTVISIBLELINE = 206,
            EM_GETHANDLE = 189,
            EM_GETHYPHENATEINFO = 1254,
            EM_GETIMECOLOR = 1129,
            EM_GETIMECOMPMODE = 1146,
            EM_GETIMECOMPTEXT = 1266,
            EM_GETIMEMODEBIAS = 1151,
            EM_GETIMEOPTIONS = 1131,
            EM_GETIMEPROPERTY = 1268,
            EM_GETIMESTATUS = 217,
            EM_GETLANGOPTIONS = 1145,
            EM_GETLIMITTEXT = 213,
            EM_GETLINE = 196,
            EM_GETLINECOUNT = 186,
            EM_GETMARGINS = 212,
            EM_GETMODIFY = 184,
            EM_GETOLEINTERFACE = 1084,
            EM_GETOPTIONS = 1102,
            EM_GETPAGEROTATE = 1259,
            EM_GETPARAFORMAT = 1085,
            EM_GETPASSWORDCHAR = 210,
            EM_GETPUNCTUATION = 1125,
            EM_GETRECT = 178,
            EM_GETREDONAME = 1111,
            EM_GETSCROLLPOS = 1245,
            EM_GETSEL = 176,
            EM_GETSELTEXT = 1086,
            EM_GETTEXTEX = 1118,
            EM_GETTEXTLENGTHEX = 1119,
            EM_GETTEXTMODE = 1114,
            EM_GETTEXTRANGE = 1099,
            EM_GETTHUMB = 190,
            EM_GETTYPOGRAPHYOPTIONS = 1227,
            EM_GETUNDONAME = 1110,
            EM_GETWORDBREAKPROC = 209,
            EM_GETWORDBREAKPROCEX = 1104,
            EM_GETWORDWRAPMODE = 1127,
            EM_GETZOOM = 1248,
            EM_HIDEBALLOONTIP = 5380,
            EM_HIDESELECTION = 1087,
            EM_ISIME = 1267,
            EM_LIMITTEXT = 197,
            EM_LINEFROMCHAR = 201,
            EM_LINEINDEX = 187,
            EM_LINELENGTH = 193,
            EM_LINESCROLL = 182,
            EM_PASTESPECIAL = 1088,
            EM_POSFROMCHAR = 214,
            EM_RECONVERSION = 1149,
            EM_REDO = 1108,
            EM_REPLACESEL = 194,
            EM_REQUESTRESIZE = 1089,
            EM_SCROLL = 181,
            EM_SCROLLCARET = 183,
            EM_SELECTIONTYPE = 1090,
            EM_SETBIDIOPTIONS = 1224,
            EM_SETBKGNDCOLOR = 1091,
            EM_SETCHARFORMAT = 1092,
            EM_SETCTFMODEBIAS = 1262,
            EM_SETCTFOPENSTATUS = 1265,
            EM_SETCUEBANNER = 5377,
            EM_SETEDITSTYLE = 1228,
            EM_SETEVENTMASK = 1093,
            EM_SETFONTSIZE = 1247,
            EM_SETHANDLE = 188,
            EM_SETHYPHENATEINFO = 1255,
            EM_SETIMECOLOR = 1128,
            EM_SETIMEMODEBIAS = 1150,
            EM_SETIMEOPTIONS = 1130,
            EM_SETIMESTATUS = 216,
            EM_SETLANGOPTIONS = 1144,
            EM_SETLIMITTEXT = 197,
            EM_SETMARGINS = 211,
            EM_SETMODIFY = 185,
            EM_SETOLECALLBACK = 1094,
            EM_SETOPTIONS = 1101,
            EM_SETPAGEROTATE = 1260,
            EM_SETPALETTE = 1117,
            EM_SETPARAFORMAT = 1095,
            EM_SETPASSWORDCHAR = 204,
            EM_SETPUNCTUATION = 1124,
            EM_SETREADONLY = 207,
            EM_SETRECT = 179,
            EM_SETRECTNP = 180,
            EM_SETSCROLLPOS = 1246,
            EM_SETSEL = 177,
            EM_SETTABSTOPS = 203,
            EM_SETTARGETDEVICE = 1096,
            EM_SETTEXTEX = 1121,
            EM_SETTEXTMODE = 1113,
            EM_SETTYPOGRAPHYOPTIONS = 1226,
            EM_SETUNDOLIMIT = 1106,
            EM_SETWORDBREAKPROC = 208,
            EM_SETWORDBREAKPROCEX = 1105,
            EM_SETWORDWRAPMODE = 1126,
            EM_SETZOOM = 1249,
            EM_SHOWBALLOONTIP = 5379,
            EM_SHOWSCROLLBAR = 1120,
            EM_STOPGROUPTYPING = 1112,
            EM_STREAMIN = 1097,
            EM_STREAMOUT = 1098,
            EM_UNDO = 199,
            WM_ACTIVATE = 0x6,
            WM_ACTIVATEAPP = 0x1C,
            WM_AFXFIRST = 0x360,
            WM_AFXLAST = 0x37F,
            WM_APP = 0x8000,
            WM_APPCOMMAND = 0x319,
            WM_ASKCBFORMATNAME = 0x30C,
            WM_CANCELJOURNAL = 0x4B,
            WM_CANCELMODE = 0x1F,
            WM_CAPTURECHANGED = 0x215,
            WM_CHANGECBCHAIN = 0x30D,
            WM_CHANGEUISTATE = 0x127,
            WM_CHAR = 0x102,
            WM_CHARTOITEM = 0x2F,
            WM_CHILDACTIVATE = 0x22,
            WM_CLEAR = 0x303,
            WM_CLIPBOARDUPDATE = 0x31D,
            WM_CLOSE = 0x10,
            WM_CL_INTERLACED420 = 0,
            WM_CL_PROGRESSIVE420 = 1,
            WM_CODEC_ONEPASS_CBR = 1,
            WM_CODEC_ONEPASS_VBR = 2,
            WM_CODEC_TWOPASS_CBR = 4,
            WM_CODEC_TWOPASS_VBR_PEAKCONSTRAINED = 16,
            WM_CODEC_TWOPASS_VBR_UNCONSTRAINED = 8,
            WM_COMMAND = 0x111,
            WM_COMMNOTIFY = 0x44,
            WM_COMPACTING = 0x41,
            WM_COMPAREITEM = 0x39,
            WM_CONTEXTMENU = 0x7B,
            WM_CONVERTREQUEST = 0x10A,
            WM_CONVERTREQUESTEX = 0x109,
            WM_CONVERTRESULT = 0x10B,
            WM_COPY = 0x301,
            WM_COPYDATA = 0x4A,
            WM_CREATE = 0x1,
            WM_CTLCOLOR = 0x19,
            WM_CTLCOLORBTN = 0x135,
            WM_CTLCOLORDLG = 0x136,
            WM_CTLCOLOREDIT = 0x133,
            WM_CTLCOLORLISTBOX = 0x134,
            WM_CTLCOLORMSGBOX = 0x132,
            WM_CTLCOLORSCROLLBAR = 0x137,
            WM_CTLCOLORSTATIC = 0x138,
            WM_CT_BOTTOM_FIELD_FIRST = 0x20,
            WM_CT_INTERLACED = 0x80,
            WM_CT_REPEAT_FIRST_FIELD = 0x10,
            WM_CT_TOP_FIELD_FIRST = 0x40,
            WM_CUT = 0x300,
            WM_DDE_FIRST = 0x3E0,
            WM_DEADCHAR = 0x103,
            WM_DELETEITEM = 0x2D,
            WM_DESTROY = 0x2,
            WM_DESTROYCLIPBOARD = 0x307,
            WM_DEVICECHANGE = 0x219,
            WM_DEVMODECHANGE = 0x1B,
            WM_DISPLAYCHANGE = 0x7E,
            WM_DRAWCLIPBOARD = 0x308,
            WM_DRAWITEM = 0x2B,
            WM_DROPFILES = 0x233,
            WM_DWMCOLORIZATIONCOLORCHANGED = 0x320,
            WM_DWMCOMPOSITIONCHANGED = 0x31E,
            WM_DWMNCRENDERINGCHANGED = 0x31F,
            WM_DWMWINDOWMAXIMIZEDCHANGE = 0x321,
            WM_ENABLE = 0xA,
            WM_ENDSESSION = 0x16,
            WM_ENTERIDLE = 0x121,
            WM_ENTERMENULOOP = 0x211,
            WM_ENTERSIZEMOVE = 0x231,
            WM_ERASEBKGND = 0x14,
            WM_EXITMENULOOP = 0x212,
            WM_EXITSIZEMOVE = 0x232,
            WM_FONTCHANGE = 0x1D,
            WM_GETDLGCODE = 0x87,
            WM_GETFONT = 0x31,
            WM_GETHOTKEY = 0x33,
            WM_GETICON = 0x7F,
            WM_GETMINMAXINFO = 0x24,
            WM_GETOBJECT = 0x3D,
            WM_GETTEXT = 0xD,
            WM_GETTEXTLENGTH = 0xE,
            WM_GETTITLEBARINFOEX = 0x33F,
            WM_HANDHELDFIRST = 0x358,
            WM_HANDHELDLAST = 0x35F,
            WM_HELP = 0x53,
            WM_HOTKEY = 0x312,
            WM_HSCROLL = 0x114,
            WM_HSCROLLCLIPBOARD = 0x30E,
            WM_ICONERASEBKGND = 0x27,
            WM_IMEKEYDOWN = 0x290,
            WM_IMEKEYUP = 0x291,
            WM_IME_CHAR = 0x286,
            WM_IME_COMPOSITION = 0x10F,
            WM_IME_COMPOSITIONFULL = 0x284,
            WM_IME_CONTROL = 0x283,
            WM_IME_ENDCOMPOSITION = 0x10E,
            WM_IME_KEYDOWN = 0x290,
            WM_IME_KEYLAST = 0x10F,
            WM_IME_KEYUP = 0x291,
            WM_IME_NOTIFY = 0x282,
            WM_IME_REPORT = 0x280,
            WM_IME_REQUEST = 0x288,
            WM_IME_SELECT = 0x285,
            WM_IME_SETCONTEXT = 0x281,
            WM_IME_STARTCOMPOSITION = 0x10D,
            WM_INITDIALOG = 0x110,
            WM_INITMENU = 0x116,
            WM_INITMENUPOPUP = 0x117,
            WM_INPUT = 0xFF,
            WM_INPUTLANGCHANGE = 0x51,
            WM_INPUTLANGCHANGEREQUEST = 0x50,
            WM_INPUT_DEVICE_CHANGE = 0xFE,
            WM_INTERIM = 0x10C,
            WM_KEYDOWN = 0x100,
            WM_KEYFIRST = 0x100,
            WM_KEYLAST = 0x109,
            WM_KEYUP = 0x101,
            WM_KILLFOCUS = 0x8,
            WM_LBUTTONDBLCLK = 0x203,
            WM_LBUTTONDOWN = 0x201,
            WM_LBUTTONUP = 0x202,
            WM_MAX_STREAMS = 0x3f,
            WM_MAX_VIDEO_STREAMS = 0x3f,
            WM_MBUTTONDBLCLK = 0x209,
            WM_MBUTTONDOWN = 0x207,
            WM_MBUTTONUP = 0x208,
            WM_MDIACTIVATE = 0x222,
            WM_MDICASCADE = 0x227,
            WM_MDICREATE = 0x220,
            WM_MDIDESTROY = 0x221,
            WM_MDIGETACTIVE = 0x229,
            WM_MDIICONARRANGE = 0x228,
            WM_MDIMAXIMIZE = 0x225,
            WM_MDINEXT = 0x224,
            WM_MDIREFRESHMENU = 0x234,
            WM_MDIRESTORE = 0x223,
            WM_MDISETMENU = 0x230,
            WM_MDITILE = 0x226,
            WM_MEASUREITEM = 0x2C,
            WM_MENUCHAR = 0x120,
            WM_MENUCOMMAND = 0x126,
            WM_MENUDRAG = 0x123,
            WM_MENUGETOBJECT = 0x124,
            WM_MENURBUTTONUP = 0x122,
            WM_MENUSELECT = 0x11F,
            WM_MOUSEACTIVATE = 0x21,
            WM_MOUSEFIRST = 0x200,
            WM_MOUSEHOVER = 0x2A1,
            WM_MOUSEHWHEEL = 0x20E,
            WM_MOUSELAST = 0x20E,
            WM_MOUSELEAVE = 0x2A3,
            WM_MOUSEMOVE = 0x200,
            WM_MOUSEWHEEL = 0x20A,
            WM_MOVE = 0x3,
            WM_MOVING = 0x216,
            WM_NCACTIVATE = 0x86,
            WM_NCCALCSIZE = 0x83,
            WM_NCCREATE = 0x81,
            WM_NCDESTROY = 0x82,
            WM_NCHITTEST = 0x84,
            WM_NCLBUTTONDBLCLK = 0xA3,
            WM_NCLBUTTONDOWN = 0xA1,
            WM_NCLBUTTONUP = 0xA2,
            WM_NCMBUTTONDBLCLK = 0xA9,
            WM_NCMBUTTONDOWN = 0xA7,
            WM_NCMBUTTONUP = 0xA8,
            WM_NCMOUSEHOVER = 0x2A0,
            WM_NCMOUSELEAVE = 0x2A2,
            WM_NCMOUSEMOVE = 0xA0,
            WM_NCPAINT = 0x85,
            WM_NCRBUTTONDBLCLK = 0xA6,
            WM_NCRBUTTONDOWN = 0xA4,
            WM_NCRBUTTONUP = 0xA5,
            WM_NCXBUTTONDBLCLK = 0xAD,
            WM_NCXBUTTONDOWN = 0xAB,
            WM_NCXBUTTONUP = 0xAC,
            WM_NEXTDLGCTL = 0x28,
            WM_NEXTMENU = 0x213,
            WM_NOTIFY = 0x4E,
            WM_NOTIFYFORMAT = 0x55,
            WM_NULL = 0x0,
            WM_PAINT = 0xF,
            WM_PAINTCLIPBOARD = 0x309,
            WM_PAINTICON = 0x26,
            WM_PALETTECHANGED = 0x311,
            WM_PALETTEISCHANGING = 0x310,
            WM_PARENTNOTIFY = 0x210,
            WM_PASTE = 0x302,
            WM_PENWINFIRST = 0x380,
            WM_PENWINLAST = 0x38F,
            WM_POWER = 0x48,
            WM_POWERBROADCAST = 0x218,
            WM_PRINT = 0x317,
            WM_PRINTCLIENT = 0x318,
            WM_QUERYDRAGICON = 0x37,
            WM_QUERYENDSESSION = 0x11,
            WM_QUERYNEWPALETTE = 0x30F,
            WM_QUERYOPEN = 0x13,
            WM_QUERYUISTATE = 0x129,
            WM_QUEUESYNC = 0x23,
            WM_QUIT = 0x12,
            WM_RASDIALEVENT = 0xCCCD,
            WM_RBUTTONDBLCLK = 0x206,
            WM_RBUTTONDOWN = 0x204,
            WM_RBUTTONUP = 0x205,
            WM_RENDERALLFORMATS = 0x306,
            WM_RENDERFORMAT = 0x305,
            WM_SETCURSOR = 0x20,
            WM_SETFOCUS = 0x7,
            WM_SETFONT = 0x30,
            WM_SETHOTKEY = 0x32,
            WM_SETICON = 0x80,
            WM_SETREDRAW = 0xB,
            WM_SETTEXT = 0xC,
            WM_SHOWWINDOW = 0x18,
            WM_SIZE = 0x5,
            WM_SIZECLIPBOARD = 0x30B,
            WM_SIZING = 0x214,
            WM_SPOOLERSTATUS = 0x2A,
            WM_STYLECHANGED = 0x7D,
            WM_STYLECHANGING = 0x7C,
            WM_SYNCPAINT = 0x88,
            WM_SYSCHAR = 0x106,
            WM_SYSCOLORCHANGE = 0x15,
            WM_SYSCOMMAND = 0x112,
            WM_SYSDEADCHAR = 0x107,
            WM_SYSKEYDOWN = 0x104,
            WM_SYSKEYUP = 0x105,
            WM_TABLET_DEFBASE = 0x2C0,
            WM_TABLET_FIRST = 0x2c0,
            WM_TABLET_LAST = 0x2df,
            WM_TABLET_MAXOFFSET = 0x20,
            WM_TCARD = 0x52,
            WM_THEMECHANGED = 0x31A,
            WM_TIMECHANGE = 0x1E,
            WM_TIMER = 0x113,
            WM_UNDO = 0x304,
            WM_UNICHAR = 0x109,
            WM_UNINITMENUPOPUP = 0x125,
            WM_UPDATEUISTATE = 0x128,
            WM_USER = 0x400,
            WM_USERCHANGED = 0x54,
            WM_VKEYTOITEM = 0x2E,
            WM_VSCROLL = 0x115,
            WM_VSCROLLCLIPBOARD = 0x30A,
            WM_WINDOWPOSCHANGED = 0x47,
            WM_WINDOWPOSCHANGING = 0x46,
            WM_WININICHANGE = 0x1A,
            WM_WNT_CONVERTREQUESTEX = 0x109,
            WM_WTSSESSION_CHANGE = 0x2B1,
            WM_XBUTTONDBLCLK = 0x20D,
            WM_XBUTTONDOWN = 0x20B,
            WM_XBUTTONUP = 0x20C,
        }
        public enum SysCommand : uint
        {
            SC_SIZE = 0xF000,
            SC_MOVE = 0xF010,
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_NEXTWINDOW = 0xF040,
            SC_PREVWINDOW = 0xF050,
            SC_CLOSE = 0xF060,
            SC_VSCROLL = 0xF070,
            SC_HSCROLL = 0xF080,
            SC_MOUSEMENU = 0xF090,
            SC_KEYMENU = 0xF100,
            SC_ARRANGE = 0xF110,
            SC_RESTORE = 0xF120,
            SC_TASKLIST = 0xF130,
            SC_SCREENSAVE = 0xF140,
            SC_HOTKEY = 0xF150,
            SC_DEFAULT = 0xF160,
            SC_MONITORPOWER = 0xF170,
            SC_CONTEXTHELP = 0xF180,
            SC_SEPARATOR = 0xF00F,
            SCF_ISSECURE = 0x1,
            SC_ICON = SC_MINIMIZE,
            SC_ZOOM = SC_MAXIMIZE,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            public UInt32 LowPart;
            public Int32 HighPart;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            public UInt32 PrivilegeCount;
            public LUID Luid;
            public UInt32 Attributes;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public override int GetHashCode()
            {
                return (Left.GetHashCode() ^
                    Right.GetHashCode() ^
                    Top.GetHashCode() ^
                    Bottom.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                {
                    return (
                        ((RECT)obj).Left == Left &&
                        ((RECT)obj).Right == Right &&
                        ((RECT)obj).Top == Top &&
                        ((RECT)obj).Bottom == Bottom);
                }
                else if (obj is Rectangle)
                {
                    return (
                        ((Rectangle)obj).Left == Left &&
                        ((Rectangle)obj).Right == Right &&
                        ((Rectangle)obj).Top == Top &&
                        ((Rectangle)obj).Bottom == Bottom);
                }
                else
                {
                    return base.Equals(obj);
                }
            }

            public int left;
            public int top;
            public int right;
            public int bottom;

            public int Left
            {
                get
                {
                    return left;
                }
                set
                {
                    left = value;
                }
            }

            public int Right
            {
                get
                {
                    return right;
                }
                set
                {
                    right = value;
                }
            }

            public int Bottom
            {
                get
                {
                    return bottom;
                }
                set
                {
                    bottom = value;
                }
            }

            public int Top
            {
                get
                {
                    return top;
                }
                set
                {
                    top = value;
                }
            }

            public void SetFrom(Control ctrl)
            {
                left = ctrl.Left;
                top = ctrl.Top;
                right = ctrl.Right;
                bottom = ctrl.Bottom;
            }

            public RECT(Rectangle rt)
            {
                this.left = rt.Left;
                this.top = rt.Top;
                this.bottom = rt.Bottom;
                this.right = rt.Right;
            }

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public static implicit operator RECT(Control ctrl)
            {
                return new RECT(ctrl.Left, ctrl.Top, ctrl.Right, ctrl.Bottom);
            }

            public static implicit operator Rectangle(RECT value)
            {
                return new Rectangle(value.Left, value.Top, value.Width, value.Height);
            }

            public static implicit operator RECT(Rectangle value)
            {
                return new RECT(value.Left, value.Top, value.Right, value.Bottom);
            }

            public int Height
            {
                get
                {
                    return bottom - top;
                }
            }

            public int Width
            {
                get
                {
                    return right - left;
                }
            }

            public Size Size
            {
                get
                {
                    return new Size(Width, Height);
                }
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            public const int CCHDEVICENAME = 32;
            public const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public uint dmFields;

            /*
            public short dmOrientation;
            public short dmPaperSize;
            public short dmPaperLength;
            public short dmPaperWidth;

            public short dmScale;
            public short dmCopies;
            public short dmDefaultSource;
            public short dmPrintQuality;
            */

            public uint dmStructPositionX;
            public uint dmStructPositionY;
            public uint dmDisplayOrientation;
            public uint dmDisplayFixedOutput;

            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            public short dmLogPixels;
            public short dmBitsPerPel;
            public uint dmPelsWidth;
            public uint dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;

            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;

            public int dmPositionX;
            public int dmPositionY;
        }
    }
}
