using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Windows.Automation;

namespace WinExt.UIInteraction
{
    [ContextClass("РаботаСТекстом", "WorkWithText")]
    public class WorkWithText: AutoContext<WorkWithText>
    {
        private AutomationElement _curWnd;

        [ScriptConstructor]
        public static IRuntimeContextInstance Constructor()
        {
            return new WorkWithText();
        }

        [ContextMethod("ПолучитьТекстПоля")]
        public string GetModuleText()
        {
            var uia = new UIAuto();
            return uia.GetModuleText();
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
    }
}
