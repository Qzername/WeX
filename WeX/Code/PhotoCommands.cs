using ImageProcessor;
using ImageProcessor.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Photos
{
    public static class PhotoCommands
    {
        #region Fun
        public static void Lovemeter(Image img1, Image img2)
        {
            ImageFactory lm = PhotoProcessing.LoadFile("./Images/lovemeter.png");
            ImageLayer one, two;
            one = new ImageLayer();
            two = new ImageLayer();

            one.Image = img1;
            one.Size = new Size(100, 100);
            one.Position = new Point(13, 67);
            two.Image = img2;
            two.Size = new Size(100, 100);
            two.Position = new Point(117, 67);

            lm.Overlay(one);
            lm.Overlay(two);

            PhotoProcessing.SaveFile(lm, "./Images/final.png");
        }

        public static void Drake(Image img1, Image img2)
        {
            ImageFactory lm = PhotoProcessing.LoadFile("./Images/drake.png");
            ImageLayer one, two;
            one = new ImageLayer();
            two = new ImageLayer();

            one.Image = img1;
            one.Size = new Size(300, 300);
            one.Position = new Point(300, 0);
            two.Image = img2;
            two.Size = new Size(300, 300);
            two.Position = new Point(300, 300);

            lm.Overlay(one);
            lm.Overlay(two);

            PhotoProcessing.SaveFile(lm, "./Images/final.png");
        }

        public static void Picture(string name, Image img, Size size, Point point)
        {
            ImageFactory realimg = PhotoProcessing.LoadFile("./Images/"+name+".png");
            ImageFactory background = PhotoProcessing.LoadFile("./Images/"+name+"background.png");
            ImageLayer back = new ImageLayer();
            back.Image = realimg.Image;
            ImageLayer profile = new ImageLayer();
            profile.Image = img;
            profile.Size = size; //325 45
            profile.Position = point;

            background.Overlay(profile);
            background.Overlay(back);

            PhotoProcessing.SaveFile(background, "./Images/final.png");
        }

        public static void Tobecontinued(Image img)
        {
            ImageFactory tbc = PhotoProcessing.LoadFile("./Images/tbc.png");
            ImageFactory background = PhotoProcessing.LoadFile("./Images/tbcbackground.png");
            ImageLayer tobecontinued = new ImageLayer();
            tobecontinued.Image = tbc.Image;
            ImageLayer profile = new ImageLayer();
            profile.Image = img;
            profile.Size = new Size(300, 300); //325 45
            tobecontinued.Position = new Point(5, 247);

            background.Overlay(profile);
            background.Tint(Color.Yellow);
            background.Overlay(tobecontinued);

            PhotoProcessing.SaveFile(background, "./Images/final.png");
        }
        #endregion
    }
}
