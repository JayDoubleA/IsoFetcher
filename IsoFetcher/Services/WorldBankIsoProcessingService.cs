using IsoFetcher.Enums;
using IsoFetcher.Models;
using IsoFetcher.Services.Interfaces;

namespace IsoFetcher.Services
{
    public class WorldBankIsoProcessingService : IWorldBankIsoProcessingService
    {
        private readonly IWorldBankIsoResponseService _responseService;
        private readonly IWorldBankIsoRequestClientService _clientService;
        private readonly ILoggingService _dummyLoggingService;

        public WorldBankIsoProcessingService(
            IWorldBankIsoResponseService responseService, 
            IWorldBankIsoRequestClientService clientService, 
            ILoggingService dummyLoggingService
            )
        {
            _responseService = responseService;
            _clientService = clientService;
            _dummyLoggingService = dummyLoggingService;
        }

        public async Task<WorldBankIsoResponseModel> ProcessIso(string isoInput)
        {
            var responseModel = new WorldBankIsoResponseModel();

            try
            {
                if (_clientService.IsIsoCodeValid(isoInput) == false)
                {
                    responseModel.ProcessingStatus = Status.FailedValidation;
                }
                else
                {
                    var isoResponseBody = await _clientService.SendIsoRequest(isoInput);

                    if (isoResponseBody == null)
                    {
                        responseModel.ProcessingStatus = Status.SomethingOdd;
                        _dummyLoggingService.Log("A very odd thing happened!"); // we would log the very odd thing in a production app
                    }
                    else
                    {
                        var responseModelFromRequest = _responseService.GetIsoResponseModel(isoResponseBody);

                        if (responseModelFromRequest == null)
                        {
                            responseModel.ProcessingStatus = Status.InvalidValueResponse;
                        }
                        else if (responseModelFromRequest.Data != null && responseModelFromRequest.Data.Any())
                        {
                            responseModel.Data = responseModelFromRequest.Data;
                            responseModel.Meta = responseModelFromRequest.Meta;
                            responseModel.ProcessingStatus = Status.SuccessResponse;
                        }
                        else
                        {
                            responseModel.ProcessingStatus = Status.SomethingOdd;
                            _dummyLoggingService.Log("A very odd thing happened!"); // we would log the very odd thing in a production app
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                responseModel.ProcessingStatus = Status.Error;
                _dummyLoggingService.Log(ex.Message); // we would log the error in a production app
            }

            return responseModel;
        }
    }
}
