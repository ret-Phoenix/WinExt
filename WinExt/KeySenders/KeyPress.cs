using ScottsUtils;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Windows.Forms;

namespace WinExt
{
    /// <summary>
    /// Класс для отправки нажатий клавиш, работа через WinAPI.
    /// </summary>
    [ContextClass("НажатиеКлавиш")]
    public class KeyPress : AutoContext<KeyPress>
    {
        [ScriptConstructor]
        public static IRuntimeContextInstance Constructor()
        {
            return new KeyPress();
        }

        #region Common

        [ContextMethod("ОчиститьМодификаторы")]
        public void ClearModifiers()
        {
            var kShift = (uint)WinAPI.KeyCode.Shift;
            var kAlt = (uint)WinAPI.KeyCode.Alt;
            var kCtrl = (uint)WinAPI.KeyCode.Control;
            var lWin = (uint)WinAPI.KeyCode.LWin;

            ScottsUtils.Keys.ClearModifiers(ref kShift);
            ScottsUtils.Keys.ClearModifiers(ref kAlt);
            ScottsUtils.Keys.ClearModifiers(ref kCtrl);
            ScottsUtils.Keys.ClearModifiers(ref lWin);
        }

        [ContextMethod("ПослатьНажатияКлавиш")]
        public void PressSendKeys(string keys)
        {
            ScottsUtils.Keys.PressSendKeys(keys);
        }

        #endregion

        #region PressKey

        [ContextMethod("НажатьCtrl")]
        public void PressCtrl()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.Control, true, false, false);
        }

        [ContextMethod("НажатьAlt")]
        public void PressAlt()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.Alt, true, false, false);
        }

        [ContextMethod("НажатьShift")]
        public void PressShift()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.Shift, true, false, false);
        }

        [ContextMethod("НажатьLWin")]
        public void PressLWin()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.LWin, true, false, false);
        }

        [ContextMethod("НажатьКонтекстноеМеню")]
        public void PressRWin()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.Apps, true, false, false);
        }

        #endregion

        #region UpKey

        [ContextMethod("ОтпуститьCtrl")]
        public void UpCtrl()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.Control, false, true, false);
        }

        [ContextMethod("ОтпуститьAlt")]
        public void UpAlt()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.Alt, false, true, false);
        }

        [ContextMethod("ОтпуститьShift")]
        public void UpShift()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.Shift, false, true, false);
        }

        [ContextMethod("ОтпуститьLWin")]
        public void UpLWin()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.LWin, false, true, false);
        }

        [ContextMethod("ОтпуститьКонтекстноеМеню")]
        public void UpRWin()
        {
            PressKeyVK(ScottsUtils.WinAPI.KeyCode.Apps, false, true, false);
        }

        #endregion

        #region ServiceMethods

        static public void PressKeyVK(ScottsUtils.WinAPI.KeyCode key)
        {
            PressKeyVK(key, true, true, false);
        }

        static public void PressKeyVK(ScottsUtils.WinAPI.KeyCode key, bool press, bool release, bool compatible)
        {

            uint scanCode = ScottsUtils.WinAPI.MapVirtualKey((uint)key, 0);
            uint extended = IsExtendedKey(key) ? (uint)ScottsUtils.WinAPI.KeybdEventFlag.ExtendedKey : 0;

            if (compatible)
            {
                extended = 0;
            }

            if (press)
            {
                ScottsUtils.WinAPI.keybd_event((byte)key, (byte)scanCode, (ScottsUtils.WinAPI.KeybdEventFlag)extended, 0);
            }

            if (release)
            {
                ScottsUtils.WinAPI.keybd_event((byte)key, (byte)scanCode, (ScottsUtils.WinAPI.KeybdEventFlag)(extended | (uint)ScottsUtils.WinAPI.KeybdEventFlag.KeyUp), 0);
            }
        }

        static public bool IsExtendedKey(ScottsUtils.WinAPI.KeyCode key)
        {
            switch (key)
            {
                case ScottsUtils.WinAPI.KeyCode.Left:
                case ScottsUtils.WinAPI.KeyCode.Up:
                case ScottsUtils.WinAPI.KeyCode.Right:
                case ScottsUtils.WinAPI.KeyCode.Down:
                case ScottsUtils.WinAPI.KeyCode.Prior:
                case ScottsUtils.WinAPI.KeyCode.Next:
                case ScottsUtils.WinAPI.KeyCode.End:
                case ScottsUtils.WinAPI.KeyCode.Home:
                case ScottsUtils.WinAPI.KeyCode.Insert:
                case ScottsUtils.WinAPI.KeyCode.Delete:
                case ScottsUtils.WinAPI.KeyCode.Divide:
                case ScottsUtils.WinAPI.KeyCode.NumLock:
                    return true;

                default:
                    return false;
            }
        }

        #endregion
    }
}
