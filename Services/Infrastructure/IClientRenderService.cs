

namespace Services.Infrastructure
{
    public interface IClientRenderService
    {
        byte[] GetBytesFromUrl(string url, string documentTitle);
    }
}
