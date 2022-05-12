using System;
using Newtonsoft.Json;

namespace Cryptocop.Software.API.Models.DTOs
{
    public class ExchangeDto
    {
        public string Id {get; set;}
        [JsonProperty("exchange_name")]
        public string Name {get; set;}
        [JsonProperty("exchange_slug")]
        public string Slug {get; set;}
        [JsonProperty("base_asset_symbol")]
        public string AssetSymbol {get; set;}
        [JsonProperty("price_usd")]
        public Nullable<float> PriceInUsd {get; set;}
        [JsonProperty("last_trade_at")]
        public Nullable<DateTime> LastTrade {get; set;}
    }
}