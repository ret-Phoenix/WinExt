using ScriptEngine.HostedScript.Library;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Drawing;
using System.Windows.Forms;


namespace WinExt
{
    /// <summary>
    /// Управление мышкой. Установить/Считать позицию. Кликнуть кнопкой.
    /// </summary>
    [ContextClass("Мышь")]
    public class Mouse : AutoContext<Mouse>
    {

        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;

        //This simulates a left mouse click
        [ContextMethod("ЛеваяКнопкаКлик")]
        public void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        //This simulates a left mouse click
        [ContextMethod("ПраваяКнопкаКлик")]
        public void RightMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0);
        }

        //This simulates a left mouse click
        [ContextMethod("СредняяКнопкаКлик")]
        public void MiddleMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_MIDDLEDOWN| MOUSEEVENTF_MIDDLEUP, xpos, ypos, 0, 0);
        }


        /// <summary>
        /// Установить позицию курсора
        /// </summary>
        /// <param name="posX">Позиция X</param>
        /// <param name="posY">Позиция Y</param>
        /// <returns>Булево - Удалось установить позицию курсора</returns>
        [ContextMethod("УстановитьПозициюКурсора")]
        public bool SetCursorPosition(int posX, int posY)
        {
            //Point cursorPosition = new Point(posX, posY);
            //Cursor.Position = cursorPosition;

            //return true;
            return SetCursorPos(posX, posY);
        }

        /// <summary>
        /// Получить позицию курсора
        /// </summary>
        /// <returns>Структура - Ключи: Верх, Лево</returns>
        [ContextMethod("ПолучитьПозициюКурсора")]
        public IValue GetCursorPosition()
        {

            StructureImpl strct = new StructureImpl();
            strct.Insert("Верх", ValueFactory.Create(Cursor.Position.Y));
            strct.Insert("Лево", ValueFactory.Create(Cursor.Position.X));
            FixedStructureImpl FixStruct = new FixedStructureImpl(strct);

            return FixStruct;
            
        }

        public override string ToString()
        {
            return "Мышь";
        }


        [ScriptConstructor]
        public static IRuntimeContextInstance Constructor()
        {
            return new Mouse();
        }
    }


}
