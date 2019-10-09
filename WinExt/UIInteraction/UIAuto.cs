using ScottsUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.Windows.Forms;

namespace WinExt.UIInteraction
{
    class UIAuto
    {
        public string GetModuleText()
        {
            AutomationElement ae = AutomationElement.FocusedElement;
            var valText = ae.GetCurrentPropertyValue(ValuePattern.ValueProperty);
            return valText.ToString();
        }

        public string GetSelectedText()
        {
            AutomationElement ae = AutomationElement.FocusedElement;

            TextPattern tp = ae.GetCurrentPattern(TextPattern.Pattern) as TextPattern;

            if (tp == null)
            {
                return "";
            }

            TextPatternRange[] trs;

            if (tp.SupportedTextSelection == SupportedTextSelection.None)
            {
                return "";
            }
            else
            {
                trs = tp.GetSelection();
                return trs[0].GetText(-1);
            }
        }


        public AutomationElement GetCurrentWindow()
        {
            return AutomationElement.FocusedElement;
        }

        public void SetCurrentWindow(AutomationElement wnd)
        {
            wnd.SetFocus();
        }

   
    }
}
