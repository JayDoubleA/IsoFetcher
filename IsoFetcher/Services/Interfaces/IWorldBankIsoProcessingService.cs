using IsoFetcher.Models;

namespace IsoFetcher.Services.Interfaces
{
    public interface IWorldBankIsoProcessingService
    {
        Task<WorldBankIsoResponseModel> ProcessIso(string isoInput);
    }
}
