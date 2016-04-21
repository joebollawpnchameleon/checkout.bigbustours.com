using bigbus.checkout.data.Model;
using Common.Enums;

namespace Services.Infrastructure
{
    public interface IImageDbService
    {
        ImageFolder GetImageFolder(string folderName);

        QrImageSaveStatus GenerateQrImage(Order order, byte[] imageChartBytes, string micrositeId);
    }
}
