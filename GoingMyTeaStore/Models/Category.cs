using Newtonsoft.Json;

namespace GoingMyTeaStore.Models
{
    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        public string FullPathImageUrl => AppSettings.ApiUrl + ImageUrl;
    }
}
