using ScottsUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace WinExt.UIInteraction
{
    class UIAuto
    {
        public TreeNode node;

        public string GetModuleText()
        {
            AutomationElement ae = AutomationElement.FocusedElement;
            var valText = ae.GetCurrentPropertyValue(ValuePattern.ValueProperty);
            return valText.ToString();
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
