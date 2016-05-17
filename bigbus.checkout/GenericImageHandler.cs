
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.IO;
using Autofac;
using Autofac.Integration.Web;
using Mod = bigbus.checkout.data.Model;
using Services.Implementation;

namespace bigbus.checkout.Helpers
{
    public class GenericImageHandler : IHttpHandler
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly string CachePath = ConfigurationManager.AppSettings["FileUploadPath"];

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

            //first - get image requested
            var reqPath = context.Request.Path;
            var lookup = reqPath.Substring(reqPath.LastIndexOf("/", StringComparison.Ordinal) + 1, reqPath.Length - (reqPath.LastIndexOf("/", StringComparison.Ordinal) + 1));
            var id = lookup.Substring(0, lookup.LastIndexOf(".", StringComparison.Ordinal));

            var width = 0;
            var height = 0;
            var square = 0;
            var keepRatio = false;

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

            if (width > 0 || height > 0)
            {
                lookup = string.Format("{0}_{1}x{2}/{3}", "resized", width, height, lookup);
            }

            if (square > 0)
                lookup = string.Format("{0}_{1}/{2}", "squareCrop", square, lookup);

            if (id.Length < 2) //incase not using unique identifiers - create a special sub directory
                lookup = string.Concat("00", lookup);

            var findPath = string.Format("{0}/{1}/{2}", id.Substring(0, 1), id.Substring(1, 1), lookup);

            var fi = new FileInfo(context.Server.MapPath(findPath));

            #endregion

            #region response type (hidden cause its rubbish and i want to redo later)

            if (reqPath.EndsWith(".jpg"))
            {
                context.Response.ContentType = "image/jpeg";
            }
            else if (reqPath.EndsWith(".png"))
            {
                context.Response.ContentType = "image/png";
            }
            else if (reqPath.EndsWith(".gif"))
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
                var img = ImageDbService.RetrieveImageOnThefly(context.Request.QueryString["imageid"]);

                if (img != null)
                {
                    var stream = new System.IO.MemoryStream(img.Data);
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

                            int cx = (newi.Width / 2) - (square / 2);
                            int cy = (newi.Height / 2) - (square / 2);
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
                    catch { }

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