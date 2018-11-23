using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace WinExt.ScreenManage
{
    class SreenParserTemplate
    {
        List<ColorPoint> FeaturePoints = new List<ColorPoint>();

        public SreenParserTemplate(Bitmap template)
        {
            var colors = new HashSet<Color>();
            using (var wr = new ImageWrapper(template))
                foreach (var p in wr)
                {
                    var c = wr[p];

                    if (!colors.Contains(c))
                    {
                        colors.Add(c);
                        FeaturePoints.Add(new ColorPoint { Color = c, Location = p });
                    }
                }
        }

        //ищем фрагмент в большом изображении
        public Point? Find(Bitmap source)
        {
            using (var wr = new ImageWrapper(source))
                foreach (var p in wr)
                {
                    var c = wr[p];
                    if (c == FeaturePoints[0].Color)
                    {
                        var offset = new Point(p.X - FeaturePoints[0].Location.X, p.Y - FeaturePoints[0].Location.Y);
                        //проверяем все особые точки
                        foreach (var fp in FeaturePoints)
                        {
                            var pp = fp.Location;
                            pp.Offset(offset);
                            if (wr[pp] != fp.Color)
                                goto next;
                        }
                        return offset;
                    }
                next:;
                }

            return null;
        }

        public Point? FindSubimage(Bitmap image, Bitmap subimage)
        {
            if (image.PixelFormat != PixelFormat.Format32bppArgb ||
                subimage.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("Invalid pixel format");

            unsafe
            {
                BitmapData data1 = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var pBase1 = (byte*)data1.Scan0;

                BitmapData data2 = subimage.LockBits(
                    new Rectangle(0, 0, subimage.Width, subimage.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var pBase2 = (byte*)data2.Scan0;

                for (int j = 0; j < image.Height; j++)
                {
                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int jj = 0; jj < subimage.Height; jj++)
                            for (int ii = 0; ii < subimage.Width; ii++)
                            {
                                byte* pixel1 = pBase1 + (j + jj) * data1.Stride + ((i + ii) << 2);
                                byte* pixel2 = pBase2 + jj * data2.Stride + (ii << 2);

                                if (*pixel1 != *pixel2 ||
                                    *(pixel1 + 1) != *(pixel2 + 1) ||
                                    *(pixel1 + 2) != *(pixel2 + 2) ||
                                    *(pixel1 + 3) != *(pixel2 + 3))
                                {
                                    goto next;
                                }
                            }

                        return new Point(i, j);
                    next:
                        continue;
                    }
                }

                subimage.UnlockBits(data2);
                image.UnlockBits(data1);
            }

            return null;
        }


        //public static Boolean Picture_In(Bitmap pic)
        //{
        //    int a = 0;
        //    Boolean bol = false;
        //    Pixel p = new Pixel();
        //    Point point = new Point(0, 0);
        //    Bitmap screen = p.GetScreen();
        //    while (a++ < (SystemInformation.PrimaryMonitorSize.Width - (pic.Width - 1)) * (SystemInformation.PrimaryMonitorSize.Height - (pic.Height - 1)))
        //    {
        //        point = Pixel_In(0, point.Y, SystemInformation.PrimaryMonitorSize.Width - pic.Width, SystemInformation.PrimaryMonitorSize.Height - pic.Height, pic.GetPixel(0, 0));

        //        if (point != new Point(0, 0))
        //        {
        //            bol = true;
        //            for (int i = 0; i < pic.Height; i++)
        //            {
        //                for (int n = 0; n < pic.Width; n++)
        //                {
        //                    if (screen.GetPixel(point.X + n, point.Y + i) == pic.GetPixel(n, i))
        //                    {
        //                        bol &= true;
        //                    }
        //                    else
        //                    {
        //                        bol = false;
        //                        screen.SetPixel(point.X, point.Y, Color.Blue);
        //                        break;
        //                    }
        //                }
        //                if (!bol) break;
        //            }

        //        }
        //        if (point == new Point(0, 0)) break;
        //        if (bol) return bol;
        //    }

        //    return bol;
        //}


    }

    struct ColorPoint
    {
        public Point Location;
        public Color Color;
    }
}
