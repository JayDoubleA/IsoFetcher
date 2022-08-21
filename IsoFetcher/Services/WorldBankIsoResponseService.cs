using IsoFetcher.Models;
using IsoFetcher.Services.Interfaces;
using Newtonsoft.Json;
namespace IsoFetcher.Services
{
    public class WorldBankIsoResponseService : IWorldBankIsoResponseService
    {        
        public static string InvalidIsoResponse => @"[{""message"":[{""id"":""120"",""key"":""Invalid value"",""value"":""The provided parameter value is not valid""}]}]";

        public WorldBankIsoResponseModel? GetIsoResponseModel(string responseBody)
        {
            if (responseBody == InvalidIsoResponse)
            {
                return null;
            }

            var model = GetIsoResponseModelFromHorridJson(responseBody); // the API returns some seriously horrid JSON!

            return model;
        }

        // the json we get back from the API is horrible and not usable in its initial form, so we're going to have to
        // do some additional work to deserialise it
        // there is even a Stack Overflow post about this horrid json: https://stackoverflow.com/questions/36912178/cannot-deserialize-json-data
        public WorldBankIsoResponseModel GetIsoResponseModelFromHorridJson(string horridJson)
        {
            var model = new WorldBankIsoResponseModel();

            var cutOff = horridJson.IndexOf("},"); // split it at the end of the metadata object
            var metaJson = horridJson.Substring(1, cutOff);
            var meta = JsonConvert.DeserializeObject<WorldBankIsoResponseMetaModel>(metaJson);
            model.Meta = meta;

            var dataJson = horridJson.Remove(horridJson.Length - 1, 1).Substring(cutOff + 2);
            var data = JsonConvert.DeserializeObject<List<WorldBankIsoResponseDataModel>>(dataJson);
            model.Data = data;

            return model;
        }
    }
}
