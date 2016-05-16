using System.Drawing.Printing;
using System.Web;
using TuesPechkin;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class PdfClientRenderer : IClientRenderService
    {
        private readonly IConverter _converter;

        public PdfClientRenderer()
        {
            _converter =
                new ThreadSafeConverter(
                    new RemotingToolset<PdfToolset>(new Win64EmbeddedDeployment(new TempFolderDeployment())));
        }

        public virtual byte[] GetBytesFromUrl(string url, string documentTitle)
        {
            var uri = string.IsNullOrWhiteSpace(url) ? "http://www.bigbustours.com/international/home.html" : url;

            // fully qualify the url
            if (!uri.ToLower().StartsWith("http://") && !uri.ToLower().StartsWith("https://"))
            {
                var context = HttpContext.Current;

                if (context != null)
                {
                    var rUrl = context.Request.Url;
                    var hostPart = string.Format("{0}://{1}:{2}", rUrl.Scheme, rUrl.Host, rUrl.Port);
                    if (!uri.StartsWith("/")) hostPart += "/";
                    uri = hostPart + uri;
                }
            }

            var document = new HtmlToPdfDocument
            {
                GlobalSettings =
                {
                    ProduceOutline = true,
                    DocumentTitle = documentTitle,
                    PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
                    Margins =
                    {
                        Top = 20,
                        Right = 20,
                        Bottom = 20,
                        Left = 15
                    }
                },
                Objects = {
                    new ObjectSettings { PageUrl = uri }
                }
            };
            
            return _converter.Convert(document);
        }
    }
}
