using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private string _url = "https://data.messari.io/api/";
        private string cryptoQuery = "?fields=id,symbol,name,slug,metrics/market_data/price_usd,profile/general/overview/project_details";
        private IEnumerable<string> allowedCurrencies = new List<string>() {"BTC", "ETH", "USDT", "XMR"};
        static readonly HttpClient client = new HttpClient();
        public async Task<IEnumerable<CryptocurrencyDto>> GetAvailableCryptocurrencies()
        {
            var currencies = await Helpers.HttpResponseMessageExtensions.DeserializeJsonToList<CryptocurrencyDto>(client.GetAsync($"{_url}v2/assets{cryptoQuery}").Result, true);
            var toRet = new List<CryptocurrencyDto>();
            foreach (var currency in currencies)
            {
                if (allowedCurrencies.Contains(currency.Symbol))
                {
                    toRet.Add(currency);
                }
            }
            return toRet;
        }
        
        public async Task<CryptocurrencyDto> GetCryptocurrency(string productIdentifier)
        {
            var currency = await Helpers.HttpResponseMessageExtensions.DeserializeJsonToObject<CryptocurrencyDto>(client.GetAsync($"{_url}v1/assets/{productIdentifier}/metrics?fields=id,symbol,name,slug,market_data/price_usd").Result, true);
            if (currency.Symbol == null)
            {
                return null;
            }
            if (allowedCurrencies.Contains(currency.Symbol))
            {
                return currency;
            }
            return null;
        }

    }
}