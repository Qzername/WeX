using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Database
{
    public static class HTTPrequest
    {
        public static Image RequestImage(string link)
        {
            System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(link);
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.Timeout = 30000;

            System.Net.WebResponse webResponse = webRequest.GetResponse();

            System.IO.Stream stream = webResponse.GetResponseStream();
            Image final = System.Drawing.Image.FromStream(stream);
            webResponse.Close();
            return final;
        }
    }
}
