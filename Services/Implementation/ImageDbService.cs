
using System;
using System.Web.ModelBinding;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Enums;
using Services.Infrastructure;
using System.Collections.Generic;

namespace Services.Implementation
{
    public class ImageDbService : IImageDbService
    {
        private readonly IGenericDataRepository<Image> _imageRepository;
        private readonly IGenericDataRepository<ImageFolder> _folderRepository;
        private readonly IGenericDataRepository<ImageMetaData> _metaDataRepository;
        private readonly IGenericDataRepository<EcrOrderLineBarcode> _ecrBarcodeRepository;
        private readonly IImageService _imageService;
        private const string QrFolderName = "QRCodes";
        private const string QrImageNameFormat = "QR-{0}-{1}";

        public ImageDbService(IImageService imageService, IGenericDataRepository<ImageFolder> folderRepository,
            IGenericDataRepository<Image> imageRepository, IGenericDataRepository<ImageMetaData> metaDataRepository,
            IGenericDataRepository<EcrOrderLineBarcode> ecrBarcodeRepository)
        {
            _imageRepository = imageRepository;
            _folderRepository = folderRepository;
            _metaDataRepository = metaDataRepository;
            _imageService = imageService;
            _ecrBarcodeRepository = ecrBarcodeRepository;
        }

        public ImageFolder GetImageFolder(string folderName)
        {
            return 
                _folderRepository.GetSingle(
                    x => x.FolderName.Equals(folderName, StringComparison.CurrentCultureIgnoreCase));  
        }

        public QrImageSaveStatus GenerateQrImage(int orderNumber, string ticketId, byte[] imageChartBytes, string micrositeId)
        {
            var folder =  EnsureImageFolderExists(micrositeId);

            //check qrcodes folder, if it doesn't exist, create it.
            var qrFolder = EnsureImageFolderExistsWithParent(QrFolderName, folder.Id);              

            //check if barcode exist.
            var barcode = _ecrBarcodeRepository.GetSingle(x => x.OrderNumber == orderNumber
                    && x.TicketId.Equals(ticketId));
            
            if(barcode == null)//create it
            {
                barcode =  new EcrOrderLineBarcode
                    {
                         DateCreated = DateTime.Now,
                         OrderNumber = orderNumber,
                         TicketId = ticketId                            
                    };

                _ecrBarcodeRepository.Add(barcode);

            }
          
            //create image first
            var newImageMetaData = SaveImage(imageChartBytes);

            //then create meta data
            if (newImageMetaData == null || newImageMetaData.ImageId == null)
            {
                //***Log(string.Format("Image Create failed ordernumber {0} metadata {1}"));
                return QrImageSaveStatus.ImageDataCreationFailed;
            }

            barcode.ImageId = newImageMetaData.ImageId.Value;

            var imageName = string.Format(QrImageNameFormat, orderNumber, ticketId);
            newImageMetaData.AltText = imageName;
            newImageMetaData.ImageFolderId = qrFolder.Id;
            newImageMetaData.Tags = string.Empty;
            newImageMetaData.Name = imageName;
            newImageMetaData.DateCreated = DateTime.Now;
            newImageMetaData.Id = new Guid();

            _metaDataRepository.Add(newImageMetaData);

            return QrImageSaveStatus.Success;
        }
                
        public ImageMetaData SaveImage(byte[] originalBytes)
        {
            var realImage = _imageService.GetImageFromBytes(originalBytes);
            var imageExtension = _imageService.GetImageExtension(realImage);

            var dbImage = new bigbus.checkout.data.Model.Image
            {
                Data = originalBytes,
                Type = imageExtension
            };

            _imageRepository.Add(dbImage);

            var imageData = new ImageMetaData
            {
                ImageId = dbImage.Id,
                Width = realImage.Width,
                Height = realImage.Height,
                Type = imageExtension
            };

            return imageData;
        }

        private ImageFolder EnsureImageFolderExists(string folderName)
        {
            var folder = GetImageFolder(folderName);

            if (folder == null)
            {
                folder = new ImageFolder
                {
                    FolderName = folderName
                };

                _folderRepository.Add(folder);
            }
            return folder;
        }

        private ImageFolder EnsureImageFolderExistsWithParent(string folderName, Guid parentFolderId)
        {
            var folder = _folderRepository.GetSingle(
                    x => x.FolderName.Equals(folderName, StringComparison.CurrentCultureIgnoreCase) &&
                         x.ParentFolderId != null && x.ParentFolderId.Equals(parentFolderId));

            if (folder == null)
            {
                folder = new ImageFolder
                {
                    FolderName = folderName,
                    ParentFolderId = parentFolderId
                };

                _folderRepository.Add(folder);
            }
            return folder;
        }
       
        public IList<EcrOrderLineBarcode> GetOrderEcrBarcodes(int orderNumber)
        {
           return _ecrBarcodeRepository.GetList(x => x.OrderNumber == orderNumber);
        }

    }
}
