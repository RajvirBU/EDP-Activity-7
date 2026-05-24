using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NetAndBrewWinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }

        public static Image MakeCircularImage(Image img)
        {
            // Determine square dimensions based on shortest side to avoid stretching
            int minSize = Math.Min(img.Width, img.Height);
            Bitmap bmp = new Bitmap(minSize, minSize);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Scale and center the image to fill the circle properly
                Rectangle destRect = new Rectangle(0, 0, minSize, minSize);
                Brush brush = new TextureBrush(img, new Rectangle(
                    (img.Width - minSize) / 2, 
                    (img.Height - minSize) / 2, 
                    minSize, minSize));
                
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(destRect);
                g.FillPath(brush, path);
            }
            return bmp;
        }
    }
}
