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
        public static void Computer(Image img)
        {
            ImageFactory realimg = PhotoProcessing.LoadFile("./Images/Computer.png");
            ImageFactory background = PhotoProcessing.LoadFile("./Images/background.png");
            ImageLayer back = new ImageLayer();
            back.Image = realimg.Image;
            ImageLayer profile = new ImageLayer();
            profile.Image = img;
            profile.Size = new Size(235, 235); //325 45
            profile.Position = new Point(325, 20);

            background.Overlay(profile);
            background.Overlay(back);

            PhotoProcessing.SaveFile(background, "./Images/final.png");
        }

        public static void Toilet(Image img)
        {
            ImageFactory realimg = PhotoProcessing.LoadFile("./Images/toilet.png");
            ImageFactory background = PhotoProcessing.LoadFile("./Images/backgroundtoilet.png");
            ImageLayer back = new ImageLayer();
            back.Image = realimg.Image;
            ImageLayer profile = new ImageLayer();
            profile.Image = img;
            profile.Size = new Size(50, 50); //325 45
            profile.Position = new Point(120, 65);

            background.Overlay(profile);
            background.Overlay(back);

            PhotoProcessing.SaveFile(background, "./Images/final.png");
        }

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

        public static void Pickle(Image img)
        {
            ImageFactory realimg = PhotoProcessing.LoadFile("./Images/pickle.png");
            ImageFactory background = PhotoProcessing.LoadFile("./Images/picklebackground.png");
            ImageLayer back = new ImageLayer();
            back.Image = realimg.Image;
            ImageLayer profile = new ImageLayer();
            profile.Image = img;
            profile.Size = new Size(250, 250); //325 45
            profile.Position = new Point(404, 142);

            background.Overlay(profile);
            background.Overlay(back);

            PhotoProcessing.SaveFile(background, "./Images/final.png");
        }
        #endregion
    }
}
