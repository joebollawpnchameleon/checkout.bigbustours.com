
using System.Net;
using Services.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;

namespace Services.Implementation
{
    public class ImageService : IImageService
    {
        public virtual byte[] DownloadImageFromUrl(string url)
        {
            var webClient = new WebClient();
            return webClient.DownloadData(url);
        }

        public virtual Image GetImageFromBytes(byte[] originalBytes)
        {
            return Image.FromStream(new System.IO.MemoryStream(originalBytes));
        }

        public virtual string GetImageExtension(Image image)
        {
            var bmpFormat = image.RawFormat;
            var strFormat = "jpg";

            if (bmpFormat.Equals(ImageFormat.Bmp)) strFormat = "bmp";
            else if (bmpFormat.Equals(ImageFormat.Emf)) strFormat = "emf";
            else if (bmpFormat.Equals(ImageFormat.Exif)) strFormat = "exif";
            else if (bmpFormat.Equals(ImageFormat.Gif)) strFormat = "gif";
            else if (bmpFormat.Equals(ImageFormat.Icon)) strFormat = "icon";
            else if (bmpFormat.Equals(ImageFormat.Jpeg)) strFormat = "jpg";
            else if (bmpFormat.Equals(ImageFormat.MemoryBmp)) strFormat = "MemoryBMP";
            else if (bmpFormat.Equals(ImageFormat.Png)) strFormat = "png";
            else if (bmpFormat.Equals(ImageFormat.Tiff)) strFormat = "tiff";
            else if (bmpFormat.Equals(ImageFormat.Wmf)) strFormat = "wmf";

            return strFormat;
        }
    }
}
