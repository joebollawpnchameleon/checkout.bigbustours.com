using System;
using System.Configuration;
using System.IO;
using System.Web;
using Services.Implementation;

namespace bigbus.checkout
{
    public class BarCodeImageHandler : IHttpHandler
    {
        /// <summary>
        ///
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public virtual void ProcessRequest(HttpContext context)
        {

            #region path structure

            var width = 0;
            var height = 0;
            var square = 0;
            var keepRatio = false;
            var imageid = context.Request.QueryString["imageid"];
            var imageExtension = context.Request.QueryString["extension"];
            var microsite = context.Request.QueryString["micrositeid"];
            var rootFolder = ConfigurationManager.AppSettings["BarCodeDir"];

            try
            {
                if (!string.IsNullOrWhiteSpace(context.Request.QueryString["w"]))
                {
                    width = Convert.ToInt32(context.Request.QueryString["w"]);
                }

                if (!string.IsNullOrWhiteSpace(context.Request.QueryString["h"]))
                {
                    height = Convert.ToInt32(context.Request.QueryString["h"]);
                }

                if (!string.IsNullOrWhiteSpace(context.Request.QueryString["r"]))
                {
                    keepRatio = true;
                }

                if (!string.IsNullOrWhiteSpace(context.Request.QueryString["s"]))
                {
                    square = Convert.ToInt32(context.Request.QueryString["s"]);
                }

                if (width > 2000) width = 0;
                if (height > 2000) height = 0;
                if (square > 2000) square = 0;
            }
            catch
            {
                //ignore
            }
          
            var findPath = string.Format("~{0}{1}/{2}", rootFolder, microsite, imageid + "." + imageExtension);

            var fi = new FileInfo(context.Server.MapPath(findPath));

            #endregion

            #region response type (hidden cause its rubbish and i want to redo later)

            if (imageExtension.Equals("jpg", StringComparison.CurrentCultureIgnoreCase))
            {
                context.Response.ContentType = "image/jpeg";
            }
            else if (imageExtension.Equals("png", StringComparison.CurrentCultureIgnoreCase))
            {
                context.Response.ContentType = "image/png";
            }
            else if (imageExtension.Equals("gif", StringComparison.CurrentCultureIgnoreCase))
            {
                context.Response.ContentType = "image/gif";
            }

            #endregion

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));

            context.Response.BufferOutput = false;

            if (!fi.Exists)
            {
                //get it!
                if (imageid != null)
                {
                    var image = ImageDbService.RetrieveImageOnThefly(imageid);
                    var stream = new System.IO.MemoryStream(image.Data);
                    var newstream = new MemoryStream();

                    try
                    {
                        //save locally
                        var di = Directory.CreateDirectory(fi.DirectoryName);

                        System.Drawing.Image i = System.Drawing.Image.FromStream(stream);
                        System.Drawing.Image newi = null;
                        if (width > 0 && height > 0 && (width != i.Width || height != i.Height))
                        {
                            if (keepRatio)
                                newi = ImageService.ScaleImageToFixedSize(i, new System.Drawing.Size(width, height));
                            else
                                newi = ImageService.ResizeImage(i, new System.Drawing.Size(width, height));

                            newi.Save(newstream, i.RawFormat);
                        }
                        else if (width > 0 && width != i.Width)
                        {
                            newi = ImageService.ScaleImageToWidth(i, width);
                            newi.Save(newstream, i.RawFormat);
                        }
                        else if (height > 0 && height != i.Height)
                        {
                            newi = ImageService.ScaleImageToHeight(i, height);
                            newi.Save(newstream, i.RawFormat);
                        }
                        else if (square > 0)
                        {
                            if (i.Width >= i.Height)
                                newi = ImageService.ScaleImageToHeight(i, square);
                            else
                                newi = ImageService.ScaleImageToWidth(i, square);

                            int cx = (newi.Width/2) - (square/2);
                            int cy = (newi.Height/2) - (square/2);
                            newi = ImageService.CropImage(newi, cx, cy, square, square);
                            newi.Save(newstream, i.RawFormat);
                        }
                        else
                        {
                            newi = i;
                            newstream = stream;
                        }

                        newi.Save(fi.FullName, i.RawFormat);
                        newi.Dispose();
                        i.Dispose();
                    }
                    catch
                    {
                        //ignore
                    }

                    if (newstream != null)
                    {
                        newstream.Position = 0;
                        const int buffersize = 1024 * 16;
                        byte[] buffer = new byte[buffersize];

                        int count = newstream.Read(buffer, 0, buffersize);
                        while (count > 0)
                        {
                            context.Response.OutputStream.Write(buffer, 0, count);
                            count = newstream.Read(buffer, 0, buffersize);
                        }

                        newstream.Close();
                        newstream.Dispose();
                    }

                    stream.Close();
                    stream.Dispose();
                }

            }
            else
            {
                //send it !
                context.Response.WriteFile(context.Server.MapPath(findPath));
            }

            context.Response.End();
        }


    }

}


