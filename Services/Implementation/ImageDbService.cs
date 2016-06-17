
using System;
using System.Web.ModelBinding;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Enums;
using Services.Infrastructure;
using System.Collections.Generic;
using bigbus.checkout.data.Repositories.Implementation;

namespace Services.Implementation
{
    public class ImageDbService : BaseService, IImageDbService
    {
        private readonly IGenericDataRepository<Image> _imageRepository;
        private readonly IGenericDataRepository<ImageFolder> _folderRepository;
        private readonly IGenericDataRepository<ImageMetaData> _metaDataRepository;
        private readonly IGenericDataRepository<EcrOrderLineBarcode> _ecrBarcodeRepository;
        private readonly IImageService _imageService;
        private const string QrFolderName = "QRCodes";

        public static string QrImageNameFormat = "QR-{0}-{1}";

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
          
          
            //create image first
            var newImageMetaData = SaveImage(imageChartBytes);

            //*** persist barcode image physically


            //then create meta data
            if (newImageMetaData == null || newImageMetaData.ImageId == null)
            {
                //***Log(string.Format("Image Create failed ordernumber {0} metadata {1}"));
                return QrImageSaveStatus.ImageDataCreationFailed;
            }

            var imageName = string.Format(QrImageNameFormat, orderNumber, ticketId);
            newImageMetaData.AltText = imageName;
            newImageMetaData.ImageFolderId = qrFolder.Id;
            newImageMetaData.Tags = string.Empty;
            newImageMetaData.Name = imageName;
            newImageMetaData.DateCreated = DateTime.Now;
            newImageMetaData.Id = new Guid();

            _metaDataRepository.Add(newImageMetaData);


            if (barcode != null) return QrImageSaveStatus.Success;

            barcode = new EcrOrderLineBarcode
            {
                DateCreated = DateTime.Now,
                OrderNumber = orderNumber,
                TicketId = ticketId,
                ImageId = newImageMetaData.ImageId.Value
                //Add orderId here
            };

            _ecrBarcodeRepository.Add(barcode);

            return QrImageSaveStatus.Success;
        }

        public QrImageSaveStatus GenerateQrImage(int orderNumber, byte[] imageChartBytes, string micrositeId)
        {
            var folder = EnsureImageFolderExists(micrositeId);

            //check qrcodes folder, if it doesn't exist, create it.
            var qrFolder = EnsureImageFolderExistsWithParent(QrFolderName, folder.Id);

            //create image first
            var newImageMetaData = SaveImage(imageChartBytes);

            //*** persist barcode image physically

            //then create meta data
            if (newImageMetaData == null || newImageMetaData.ImageId == null)
            {
                Log(string.Format("Image Create failed ordernumber {0} metadata.", orderNumber));
                return QrImageSaveStatus.ImageDataCreationFailed;
            }

            var imageName = string.Format(QrImageNameFormat, orderNumber, "Ecr");
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

        public EcrOrderLineBarcode GetBarcodeByTicketAndOrder(string orderLineId)
        {
            return _ecrBarcodeRepository.GetSingle(x => 
                x.OrderLineId.Equals(orderLineId, StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual ImageMetaData GetImageMetaData(Guid imageGuid)
        {
            return _metaDataRepository.GetSingle(x => x.ImageId != null && x.ImageId.Equals(imageGuid));
        }

        public virtual ImageMetaData GetImageMetaDataByName(string name)
        {
            return _metaDataRepository.GetSingle(x => x.Name.Equals(name,StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual ImageMetaData GetImageMetaData(string imageId)
        {
            return _metaDataRepository.GetSingle(x => x.ImageId != null && x.ImageId.ToString().Equals(imageId));
        }

        public virtual string GetTicketImageUrl(string ticketImageId, string path)
        {
            if (string.IsNullOrEmpty(ticketImageId))
                return string.Empty;

            var imageMetaData = GetImageMetaData(ticketImageId);
            
            return
                imageMetaData != null ?
                    path + imageMetaData.ImageId + "." + imageMetaData.Type + "?h=102&w=124" :
                    string.Empty;
        }

        public virtual ImageMetaData GetMetaData(Guid metaDataId)
        {
            return _metaDataRepository.GetSingle(x => x.Id.Equals(metaDataId));
        }

        public virtual ImageMetaData GetMetaData(string metaDataId)
        {
            return _metaDataRepository.GetSingle(x => x.Id.ToString().Equals(metaDataId, StringComparison.CurrentCultureIgnoreCase));  
        }

        public Image GetImage(string imageId)
        {
            return _imageRepository.GetSingle(x => x.Id.ToString().Equals(imageId, StringComparison.CurrentCultureIgnoreCase));
        }

        public static Image RetrieveImageOnThefly(string imageId)
        {
            return (new GenericDataRepository<Image>()).GetSingle(x => x.Id.ToString().Equals(imageId, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
