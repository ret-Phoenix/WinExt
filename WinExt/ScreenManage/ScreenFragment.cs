using ScriptEngine.HostedScript.Library;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Drawing;
using System.Windows.Forms;


namespace WinExt.ScreenManage
{
    [ContextClass("ФрагментЭкрана", "ScreenFragment")]
    public class ScreenFragment : AutoContext<ScreenFragment>
    {

        private int top;
        private int left;
        private int height;
        private int width;


        [ContextProperty("Верх", "Top")]
        public int Top
        {
            get { return this.top; }
        }

        [ContextProperty("Лево", "Left")]
        public int Left
        {
            get { return this.left; }
        }

        [ContextProperty("Высота", "Height")]
        public int Height
        {
            get { return this.height; }
        }

        [ContextProperty("Ширина", "Width")]
        public int Width
        {
            get { return this.width; }
        }

        public ScreenFragment(int posX, int posY, int height, int width)
        {
            this.top = posY;
            this.left = posX;
            this.height = height;
            this.width = width;
        }

        public override string ToString()
        {
            return "ФрагментЭкрана";
        }
        
    }
}
