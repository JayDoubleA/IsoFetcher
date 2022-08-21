using IsoFetcher.Services.Interfaces;

namespace IsoFetcher.Services
{
    public class DummyLoggingService : ILoggingService
    {
        public void Log(string message)
        {
            // we're not actually logging, this is for demonstration purposes only!
        }
    }
}
