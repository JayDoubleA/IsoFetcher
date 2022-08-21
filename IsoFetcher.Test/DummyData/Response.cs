namespace IsoFetcher.Test.DummyData
{
    public static class Response
    {
        public static string ValidReseponse => @"[{""page"":1,""pages"":1,""per_page"":""50"",""total"":1},[{""id"":""NLD"",""iso2Code"":""NL"",""name"":""Netherlands"",""region"":{""id"":""ECS"",""iso2code"":""Z7"",""value"":""Europe & Central Asia""},""adminregion"":{""id"":"""",""iso2code"":"""",""value"":""""},""incomeLevel"":{""id"":""HIC"",""iso2code"":""XD"",""value"":""High income""},""lendingType"":{""id"":""LNX"",""iso2code"":""XX"",""value"":""Not classified""},""capitalCity"":""Amsterdam"",""longitude"":""4.89095"",""latitude"":""52.3738""}]]";
        public static string InvalidResponse => @"[{""message"":[{""id"":""120"",""key"":""Invalid value"",""value"":""The provided parameter value is not valid""}]}]";
    }
}
