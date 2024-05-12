using Newtonsoft.Json;

namespace GoingMyTeaStore.Models
{
    public class ProfileImage
    {
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        public string FullPathImageUrl => string.IsNullOrEmpty(ImageUrl) ? "uploadphoto.png" : AppSettings.ApiUrl + ImageUrl;
    }
}
