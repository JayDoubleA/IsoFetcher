namespace IsoFetcher.Services.Interfaces
{
    public interface IWorldBankIsoRequestClientService
    {
        Task<string?> SendIsoRequest(string isoString);
        bool IsIsoCodeValid(string isoString);
    }
}
