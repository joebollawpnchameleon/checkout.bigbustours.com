using System;
using System.Web;

namespace bigbus.checkout.Helpers
{
    /// <summary>
    /// Summary description for QCodeImageHandler
    /// </summary>
    public class QrCodeImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                HttpResponse r = context.Response;
                r.ContentType = "image/png";
                //
                // Write the requested image
                //
                string file = context.Request.QueryString["file"];

                if (file == "logo")
                {
                    r.WriteFile(@"C:\Projects\checkout.bigbus.com\bigbus.checkout\Content\Images\Design\Logo-other.png");
                }
                else
                {
                    r.WriteFile("Flower1.png");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}


