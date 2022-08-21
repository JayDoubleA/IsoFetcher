using IsoFetcher.Models;

namespace IsoFetcher.Services.Interfaces
{
    public interface IWorldBankIsoResponseService
    {
        WorldBankIsoResponseModel? GetIsoResponseModel(string responseBody);
        WorldBankIsoResponseModel GetIsoResponseModelFromHorridJson(string horridJson);        
    }
}