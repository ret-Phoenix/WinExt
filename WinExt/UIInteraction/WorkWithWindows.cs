using ScottsUtils;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System;
using System.Windows.Automation;
using System.Windows.Forms;


namespace WinExt.UIInteraction
{
    [ContextClass("РаботаСОкнами", "WorkWithWindows")]
    public class WorkWithWindows : AutoContext<WorkWithWindows>
    {
        private AutomationElement _curWnd;

        [ScriptConstructor]
        public static IRuntimeContextInstance Constructor()
        {
            return new WorkWithWindows();
        }

        [ContextMethod("ЗапомнитьТекущееОкно")]
        public void GetLinkToCurWindow()
        {
            var uia = new UIAuto();
            _curWnd = uia.GetCurrentWindow();
        }

        [ContextMethod("АктивироватьЗапомненноеОкно")]
        public void WndActivate()
        {
            var uia = new UIAuto();
            uia.SetCurrentWindow(_curWnd);
        }

        [ContextMethod("АктивироватьОкноПоЗаголовку", "SwitchToWinByTitle")]
        public bool SwitchToWin(string WinTitle)
        {

            return ScottsUtils.SwitchToWindow.Switch(WinTitle);
        }
    }
}
