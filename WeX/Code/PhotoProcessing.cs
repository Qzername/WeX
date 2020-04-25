using System;
using System.Drawing;
using System.IO;
using Discord.Net;
using ImageProcessor;
using ImageProcessor.Imaging;

namespace Photos
{
    public static class PhotoProcessing
    {
        public static ImageFactory LoadFile(string path)
        {
            byte[] photoBytes = File.ReadAllBytes(path);

            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                ImageFactory imageFactory = new ImageFactory(preserveExifData: true);
                imageFactory.Load(inStream);
                return imageFactory;
            }
        }

        public static void SaveFile(ImageFactory pic, string path)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                pic.Save(path);
            }
        }
    }
}
