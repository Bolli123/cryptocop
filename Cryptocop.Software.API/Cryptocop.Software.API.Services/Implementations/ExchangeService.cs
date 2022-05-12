using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;


namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        private string _url = "https://data.messari.io/api/";
        static readonly HttpClient client = new HttpClient();
        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)   
        {
            var rawData = await client.GetAsync($"{_url}v1/markets?page={pageNumber}");
            var markets = Helpers.HttpResponseMessageExtensions.DeserializeJsonToList<ExchangeDto>(rawData, false).Result;
            return new Envelope<ExchangeDto>(pageNumber, markets);
        }
    }
}