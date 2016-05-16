using bigbus.checkout.data.Model;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace Services.Infrastructure
{
    public interface IImageDbService
    {
        ImageFolder GetImageFolder(string folderName);

        QrImageSaveStatus GenerateQrImage(int orderNumber, string ticketId, byte[] imageChartBytes, string micrositeId);

        IList<EcrOrderLineBarcode> GetOrderEcrBarcodes(int orderNumber);

        ImageMetaData GetImageMetaData(Guid guid);

        string GetTicketImageUrl(string ticketImageId);

        Image GetImage(string imageId);
    }
}
