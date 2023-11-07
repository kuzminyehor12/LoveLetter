using LoveLetter.Core.Entities;
using LoveLetter.UI.Properties;
using System.Drawing.Drawing2D;

namespace LoveLetter.UI.Infrastructure
{
    public class ImageUtils
    {
        public static Image RotateImage(Image img, float rotationAngle)
        {
            var bmp = new Bitmap(img.Width, img.Height);
            using var gfx = Graphics.FromImage(bmp);
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            gfx.RotateTransform(rotationAngle);
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.DrawImage(img, new Point(0, 0));
            return bmp;
        }

        public static Image? GetImage(Card card) => 
            Resources.ResourceManager.GetObject(card.CardType.ToString()) as Bitmap;
    }
}
