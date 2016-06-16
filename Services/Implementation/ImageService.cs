
using System.Net;
using Services.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Drawing.Drawing2D;
using System.IO;

namespace Services.Implementation
{
    public class ImageService : BaseService, IImageService
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="destSize"></param>
        /// <returns></returns>
        public static Image ResizeImage(Image img, Size destSize)
        {
            try
            {

                Image oThumbNail = new Bitmap(destSize.Width, destSize.Height, img.PixelFormat);
                Graphics oGraphic;
                try
                {
                    oGraphic = Graphics.FromImage(oThumbNail);
                }
                catch
                {
                    //for images with indexed pixel format
                    //draw the old bitmaps contents to the new bitmap
                    //thank you internet !!

                    Bitmap newbm = new Bitmap(destSize.Width, destSize.Height);
                    oGraphic = Graphics.FromImage(newbm);
                    oGraphic.DrawImage(oThumbNail, new Rectangle(0, 0, newbm.Width, newbm.Height), 0,
                        0, oThumbNail.Width, oThumbNail.Height, GraphicsUnit.Pixel);
                    oThumbNail = newbm;

                }

                oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //Rectangle oRectangle = new Rectangle(0, 0, destSize.Width, destSize.Height);
                Rectangle oRectangle = new Rectangle(-1, -1, destSize.Width + 2, destSize.Height + 2);
                oGraphic.DrawImage(img, oRectangle);

                oGraphic.Dispose();
                return oThumbNail;

            }
            catch
            {
                //log ?
                return null;
            }

        }

        public static Image CropImage(Image img, int x, int y, int width, int height)
        {
            Rectangle cropArea = new Rectangle(x, y, width, height);
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="toHeight"></param>
        /// <returns></returns>
        public static Image ScaleImageToHeight(Image img, int toHeight)
        {
            try
            {
                double destWidth = Math.Floor(((float)toHeight / (float)img.Height) * img.Width);
                return ResizeImage(img, new Size((int)destWidth, toHeight));
            }
            catch
            {
                //log ?
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="toWidth"></param>
        /// <returns></returns>
        public static Image ScaleImageToWidth(Image img, int toWidth)
        {
            try
            {
                int destHeight = (int)Math.Floor(((float)toWidth / (float)img.Width) * img.Height);
                return ResizeImage(img, new Size((int)toWidth, destHeight));
            }
            catch
            {
                //log ?
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="imgSize"></param>
        /// <returns></returns>
        public static Image ScaleImageToFixedSize(Image img, Size imgSize)
        {
            try
            {

                int sourceWidth = img.Width;
                int sourceHeight = img.Height;
                int destX = 0;
                int destY = 0;
                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)imgSize.Width / (float)sourceWidth);
                nPercentH = ((float)imgSize.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                    destX = Convert.ToInt16((imgSize.Width - (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = System.Convert.ToInt16((imgSize.Height - (sourceHeight * nPercent)) / 2);
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(imgSize.Width, imgSize.Height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.Clear(Color.White);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //grPhoto.DrawImage(img, new Rectangle(destX,destY,destWidth,destHeight), new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
                //GraphicsUnit.Pixel);
                grPhoto.DrawImage(img, new Rectangle(-1, -1, destWidth + 2, destHeight + 2), new Rectangle(0, 0, sourceWidth, sourceHeight), GraphicsUnit.Pixel);


                grPhoto.Dispose();
                return bmPhoto;

            }
            catch
            {
                //log ?
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="image"></param>
        public static void SaveTheImage(string filepath, System.Drawing.Image image)
        {

            if (image.RawFormat.Equals(ImageFormat.Jpeg))
            {

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo codec = null;
                for (int i = 0; i < codecs.Length; i++)
                {
                    if (codecs[i].MimeType.ToLower() == "image/jpeg")
                        codec = codecs[i];
                }

                if (codec != null)
                {

                    Encoder encoderInstance = Encoder.Quality;
                    EncoderParameters encoderParametersInstance = new EncoderParameters(2);
                    EncoderParameter encoderParameterInstance = new EncoderParameter(encoderInstance, 80L);
                    encoderParametersInstance.Param[0] = encoderParameterInstance;
                    encoderInstance = Encoder.ColorDepth;
                    encoderParameterInstance = new EncoderParameter(encoderInstance, 24L);
                    encoderParametersInstance.Param[1] = encoderParameterInstance;

                    image.Save(filepath, codec, encoderParametersInstance);
                }
                else
                {
                    image.Save(filepath);
                }
            }
            else
            {
                image.Save(filepath);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ImageFormat LookupImageFormatFromExtension(string filename)
        {
            filename = filename.ToLower();

            if (filename.IndexOf(".gif", StringComparison.Ordinal) > 0)
            {
                return ImageFormat.Gif;
            }
            else if (filename.IndexOf(".png", StringComparison.Ordinal) > 0)
            {
                return ImageFormat.Png;
            }
            else if (filename.IndexOf(".jpg", StringComparison.Ordinal) > 0)
            {
                return ImageFormat.Jpeg;
            }
            else if (filename.IndexOf(".jpeg", StringComparison.Ordinal) > 0)
            {
                return ImageFormat.Jpeg;
            }
            else if (filename.IndexOf(".bmp", StringComparison.Ordinal) > 0)
            {
                return ImageFormat.Bmp;
            }

            return ImageFormat.Jpeg;

        }

        public bool DoesBarCodeImageExist(string barcode, string fullPath)
        {
            var fi = new FileInfo(fullPath);

            if (fi.Exists)return true;

            try
            {
                var generatedBarcodeBitmap = new Bitmap(105, 65);                

                //create a new graphic using the bitmap
                var barcodeGraphic = Graphics.FromImage(generatedBarcodeBitmap);

                //ensure that the background is white
                barcodeGraphic.Clear(Color.White);
                //draw the barcode image on the graphic
                barcodeGraphic.DrawImage(
                       Zen.Barcode.BarcodeDrawFactory.CodeEan13WithChecksum.Draw(barcode.Substring(0, 12), 40), 5, 5);
               
                //barcodeGraphic.DrawImage(
                //    Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum.Draw(barcode.Substring(0, 12), 40), 5, 5);
                barcodeGraphic.Flush();


                generatedBarcodeBitmap.Save(fi.FullName, ImageFormat.Jpeg);
                generatedBarcodeBitmap.Dispose();
                return true;
            }
            catch(Exception ex)
            {
                LoggerService.LogItem("Barcode generation failed for: " + fullPath + " : " + ex.Message);
                return false;
            }
        }
    }
}
