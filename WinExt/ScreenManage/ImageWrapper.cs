using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinExt.ScreenManage
{
    public class ImageWrapper : IDisposable, IEnumerable<Point>
    {
        /// <summary>
        /// Ширина изображения
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Высота изображения
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// Цвет по-умолачнию (используется при выходе координат за пределы изображения)
        /// </summary>
        public Color DefaultColor { get; set; }

        private byte[] data;//буфер исходного изображения
        private int stride;
        private BitmapData bmpData;
        private Bitmap bmp;

        /// <summary>
        /// Создание обертки поверх bitmap.
        /// </summary>
        /// <param name="copySourceToOutput">Копирует исходное изображение в выходной буфер</param>
        public ImageWrapper(Bitmap bmp, bool copySourceToOutput = false)
        {
            Width = bmp.Width;
            Height = bmp.Height;
            this.bmp = bmp;

            bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            stride = bmpData.Stride;

            data = new byte[stride * Height];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, data, 0, data.Length);
        }

        /// <summary>
        /// Возвращает пиксел из исходнго изображения.
        /// Либо заносит пиксел в выходной буфер.
        /// </summary>
        public Color this[int x, int y]
        {
            get
            {
                var i = GetIndex(x, y);
                return i < 0 ? DefaultColor : Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]);
            }
        }

        /// <summary>
        /// Возвращает пиксел из исходнго изображения.
        /// Либо заносит пиксел в выходной буфер.
        /// </summary>
        public Color this[Point p]
        {
            get { return this[p.X, p.Y]; }
        }

        int GetIndex(int x, int y)
        {
            return (x < 0 || x >= Width || y < 0 || y >= Height) ? -1 : x * 4 + y * stride;
        }

        /// <summary>
        /// Заносит в bitmap выходной буфер и снимает лок.
        /// Этот метод обязателен к исполнению (либо явно, лмбо через using)
        /// </summary>
        public void Dispose()
        {
            bmp.UnlockBits(bmpData);
        }

        /// <summary>
        /// Перечисление всех точек изображения
        /// </summary>
        public IEnumerator<Point> GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                yield return new Point(x, y);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
