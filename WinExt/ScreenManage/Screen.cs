using ScriptEngine.HostedScript.Library;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Drawing;
using System.Windows.Forms;


namespace WinExt.ScreenManage
{
    [ContextClass("Экран", "Sreen")]
    public class ScreenExt : AutoContext<ScreenExt>
    {

        /// <summary>
        /// Получить разрешение экрана
        /// </summary>
        /// <param name="SreenNumber">Число - Номер экрана, если ничего не задано - берет основной экран</param>
        /// <returns>ФиксированнаяСтруктура (Ширина, Высота)</returns>
        [ContextMethod("РазрешениеЭкрана")]
        public IValue ScreenResolution(int SreenNumber = 0)
        {

            Size resolution = System.Windows.Forms.Screen.AllScreens[SreenNumber].Bounds.Size;

            StructureImpl strct = new StructureImpl();
            strct.Insert("Ширина", ValueFactory.Create(resolution.Width));
            strct.Insert("Высота", ValueFactory.Create(resolution.Height));
            FixedStructureImpl FixStruct = new FixedStructureImpl(strct);

            return FixStruct;
        }

        /// <summary>
        /// Найти на экране фрагмент из файла
        /// </summary>
        /// <param name="fragmentFileName">Путь к файлу с искомым фрагментом</param>
        /// <returns>ФрагментЭкрана</returns>
        [ContextMethod("НайтиФрагмент", "FindFragment")]
        public IValue findFragment(string fragmentFileName)
        {

            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(printscreen as Image);

            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            Bitmap fragment = new Bitmap(Image.FromFile(fragmentFileName));

            SreenParserTemplate template = new SreenParserTemplate(fragment);
            var foundPoint = template.FindSubimage(printscreen, fragment);

            if (foundPoint == null)
            {
                return ValueFactory.Create();
            }

            ScreenFragment screenFragment = new ScreenFragment(foundPoint.Value.X, foundPoint.Value.Y, fragment.Height, fragment.Width);

            return screenFragment;

        }

        public override string ToString()
        {
            return "Экран";
        }
        
        [ScriptConstructor]
        public static IRuntimeContextInstance Constructor()
        {
            return new ScreenExt();
        }
    }
}
