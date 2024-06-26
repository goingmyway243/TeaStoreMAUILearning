﻿using Newtonsoft.Json;

namespace GoingMyTeaStore.Models
{
    public class ProductDetail
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        public string FullPathImageUrl => AppSettings.ApiUrl + ImageUrl;
    }
}
