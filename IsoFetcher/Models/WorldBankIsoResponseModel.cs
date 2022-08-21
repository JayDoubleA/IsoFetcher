using IsoFetcher.Enums;
using Newtonsoft.Json;

namespace IsoFetcher.Models
{
    public class WorldBankIsoResponseModel
    {
        public WorldBankIsoResponseMetaModel Meta { get; set; }
        public List<WorldBankIsoResponseDataModel> Data { get; set; }
        public Status ProcessingStatus { get; set; }

        public WorldBankIsoResponseModel()
        {
            ProcessingStatus = Status.NotStarted;
            Meta = new WorldBankIsoResponseMetaModel();
            Data = new List<WorldBankIsoResponseDataModel>();
        }
    }

    public class WorldBankIsoResponseMetaModel
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("per_page")]
        public string? PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }


    public class WorldBankIsoResponseDataModel
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("iso2Code")]
        public string? Iso2Code { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("region")] 
        public WorldBankIsoResponseDataProperty? Region { get; set; }

        [JsonProperty("adminregion")] 
        public WorldBankIsoResponseDataProperty? Adminregion { get; set; }

        [JsonProperty("incomeLevel")] 
        public WorldBankIsoResponseDataProperty? IncomeLevel { get; set; }
        
        [JsonProperty("lendingType")] 
        public WorldBankIsoResponseDataProperty? LendingType { get; set; }

        [JsonProperty("capitalCity")] 
        public string? CapitalCity { get; set; }

        [JsonProperty("longitude")] 
        public string? Longitude { get; set; }

        [JsonProperty("latitude")] 
        public string? Latitude { get; set; }
    }

    public class WorldBankIsoResponseDataProperty
    {
        [JsonProperty("id")] 
        public string? Id { get; set; }

        [JsonProperty("iso2code")] 
        public string? Iso2code { get; set; }

        [JsonProperty("value")] 
        public string? Value { get; set; }
    }

}
