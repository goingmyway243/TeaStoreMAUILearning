using Newtonsoft.Json;

namespace GoingMyTeaStore.Models
{
    public class OrderByUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("orderTotal")]
        public int OrderTotal { get; set; }

        [JsonProperty("orderPlaced")]
        public DateTime OrderPlaced { get; set; }
    }
}
