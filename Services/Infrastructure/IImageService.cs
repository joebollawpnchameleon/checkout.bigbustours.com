using System.Drawing;

namespace Services.Infrastructure
{
    public interface IImageService
    {
        byte[] DownloadImageFromUrl(string url);

       Image GetImageFromBytes(byte[] originalBytes);

        string GetImageExtension(Image image);

        bool DoesBarCodeImageExist(string barcode, string fullPath);
    }
}
