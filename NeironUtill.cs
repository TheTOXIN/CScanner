using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CScanner
{
    class NeironUtill
    {
        public static void clearImage(PictureBox pictureBox)
        {
            pictureBox.Image = (Image)new Bitmap(pictureBox.Width, pictureBox.Height);
        }

        public static int[,] getArray(Bitmap image, bool messahe = false)
        {
            int[,] res = new int[image.Width, image.Height];

            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    int color = (image.GetPixel(i, j).R +
                                    image.GetPixel(i, j).G +
                                        image.GetPixel(i, j).B) / 3;
                    res[i, j] = color > 0 ? 1 : 0;
                }
            }

            return res;
        }

        public static Image drawLitera(Image bmp, String l)
        {
            Font font = new Font("Arial", 40f);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                SizeF size = g.MeasureString(l, font);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawString(l, font, new SolidBrush(Color.Black), Point.Empty);
            }

            return bmp;
        }

        public static Bitmap getBitmap(int[,] arr)
        {
            Bitmap bitmap = new Bitmap(arr.GetLength(0), arr.GetLength(1));

            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                    if (arr[i, j] == 0)
                        bitmap.SetPixel(i, j, Color.White);
                    else
                        bitmap.SetPixel(i, j, Color.Black);

            return bitmap;
        }

        public static int[,] laodArray(int[,] source, int[,] res)
        {
            for (int i = 0; i < res.GetLength(0); i++)
                for (int j = 0; j < res.GetLength(1); j++)
                    res[i, j] = 0;

            double pX = (double)res.GetLength(0) / (double)source.GetLength(0);
            double pY = (double)res.GetLength(1) / (double)source.GetLength(1);

            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    int posX = (int)(i * pX);
                    int posY = (int)(j * pY);

                    if (res[posX, posY] == 0)
                        res[posX, posY] = source[i, j];
                }
            }

            return res;
        }

        public static Bitmap getMemory(Neiron n)
        {
            double[,] w = n.weight;
            Bitmap b = new Bitmap(w.GetLength(0), w.GetLength(1));

            for (int i = 0; i < w.GetLength(0); i++)
            {
                for (int j = 0; j < w.GetLength(1); j++)
                {
                    Color c;

                    if (w[i, j] <= 0)
                        c = Color.White;
                    else
                        c = Color.Black;

                    c = Color.FromArgb((int)(255 * w[i, j]), Color.Black);
                    b.SetPixel(i, j, c);
                }
            }

            return b;
        }

        public static int[,] cutImage(Bitmap b, Point max)
        {
            int x1 = 0;
            int y1 = 0;
            int x2 = max.X;
            int y2 = max.Y;

            for (int y = 0; y < b.Height && y1 == 0; y++)
                for (int x = 0; x < b.Width && y1 == 0; x++)
                    if (b.GetPixel(x, y).ToArgb() != 0)
                        y1 = y;

            for (int y = b.Height - 1; y >= 0 && y2 == max.Y; y--)
                for (int x = 0; x < b.Width && y2 == max.Y; x++)
                    if (b.GetPixel(x, y).ToArgb() != 0) 
                        y2 = y;

            for (int x = 0; x < b.Width && x1 == 0; x++)
                for (int y = 0; y < b.Height && x1 == 0; y++)
                    if (b.GetPixel(x, y).ToArgb() != 0)
                        x1 = x;

            for (int x = b.Width - 1; x >= 0 && x2 == max.X; x--)
                for (int y = 0; y < b.Height && x2 == max.X; y++)
                    if (b.GetPixel(x, y).ToArgb() != 0)
                        x2 = x;

            if (x1 == 0 && y1 == 0 && x2 == max.X && y2 == max.Y) 
                return null;

            int size = x2 - x1 > y2 - y1 ? x2 - x1 + 1 : y2 - y1 + 1;
            int dx = y2 - y1 > x2 - x1 ? ((y2 - y1) - (x2 - x1)) / 2 : 0;
            int dy = y2 - y1 < x2 - x1 ? ((x2 - x1) - (y2 - y1)) / 2 : 0;

            int[,] res = new int[size, size];

            for (int x = 0; x < res.GetLength(0); x++)
            {
                for (int y = 0; y < res.GetLength(1); y++)
                {
                    int pX = x + x1 - dx;
                    int pY = y + y1 - dy;

                    if (pX < 0 || pX >= max.X || pY < 0 || pY >= max.Y)
                        res[x, y] = 0;
                    else
                        res[x, y] = b.GetPixel(x + x1 - dx, y + y1 - dy).ToArgb() == 0 ? 0 : 1;
                }
            }

            return res;
        }
    }
}
