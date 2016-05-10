
using System;
using System.Web.ModelBinding;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Common.Enums;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class ImageDbService : IImageDbService
    {
        private readonly IGenericDataRepository<Image> _imageRepository;
        private readonly IGenericDataRepository<ImageFolder> _folderRepository;
        private readonly IGenericDataRepository<ImageMetaData> _metaDataRepository;
        private readonly IImageService _imageService;
        private const string QrFolderName = "QRCodes";

        public ImageDbService(IImageService imageService, IGenericDataRepository<ImageFolder> folderRepository,
            IGenericDataRepository<Image> imageRepository, IGenericDataRepository<ImageMetaData> metaDataRepository)
        {
            _imageRepository = imageRepository;
            _folderRepository = folderRepository;
            _metaDataRepository = metaDataRepository;
            _imageService = imageService;
        }

        public ImageFolder GetImageFolder(string folderName)
        {
            return 
                _folderRepository.GetSingle(
                    x => x.FolderName.Equals(folderName, StringComparison.CurrentCultureIgnoreCase));  
        }

        public QrImageSaveStatus GenerateQrImage(Order order, byte[] imageChartBytes, string micrositeId)
        {
            var folder =
                _folderRepository.GetSingle(
                    x => x.FolderName.Equals(micrositeId, StringComparison.CurrentCultureIgnoreCase));

            //check microsite folder. If folder doesn't exist, create it.
            if (folder == null)
            {
                folder = new ImageFolder
                {
                   FolderName = micrositeId
                };

                _folderRepository.Add(folder);
            }
            
            //check qrcodes folder, if it doesn't exist, create it.
            var qrFolder =
                _folderRepository.GetSingle(
                    x => x.FolderName.Equals(QrFolderName, StringComparison.CurrentCultureIgnoreCase) &&
                         x.ParentFolderId != null && x.ParentFolderId.Equals(folder.Id));

            if (qrFolder == null)
            {
                qrFolder = new ImageFolder
                {
                    FolderName = QrFolderName,
                    ParentFolderId = folder.Id
                };

                _folderRepository.Add(qrFolder);
            }

            //check image meta data
            var imageName = string.Format("QRCODE For {0}", order.OrderNumber);
            
            //var imageData =
            //    _metaDataRepository.GetSingle(x => x.Name != null && x.Name.Equals(imageName, StringComparison.CurrentCultureIgnoreCase)
            //        && x.ImageFolderId != null && x.ImageFolderId.Value.Equals(qrFolder.Id));
            //if (imageData != null) return QrImageSaveStatus.ImageDataExist;

            //create image first
            var newImageMetaData = SaveImage(imageChartBytes);

            //then create meta data
            if (newImageMetaData == null) return QrImageSaveStatus.ImageDataCreationFailed;

            newImageMetaData.AltText = imageName;
            newImageMetaData.ImageFolderId = qrFolder.Id;
            newImageMetaData.Tags = string.Empty;
            newImageMetaData.Name = imageName;
            newImageMetaData.AltText = imageName;
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


    }
}
