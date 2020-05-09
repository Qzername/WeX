using System;
using System.Collections.Generic;
using System.Text;
using AnimatedGif;
using System.Drawing;

namespace Gifs
{
    public static class GifProcessing
    {
        public static void CreateGif(params Image[] images)
        {
            using (var gif = AnimatedGif.AnimatedGif.Create("./Images/final.gif", 1000/images.Length))
            {
                foreach(Image x in images)
                    gif.AddFrame(x, delay: -1, quality: GifQuality.Bit8);
            }
        }
    }
}
