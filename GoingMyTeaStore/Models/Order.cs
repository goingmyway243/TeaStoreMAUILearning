using Newtonsoft.Json;

namespace GoingMyTeaStore.Models
{
    public class Order
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("orderTotal")]
        public int OrderTotal { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }
    }
}
