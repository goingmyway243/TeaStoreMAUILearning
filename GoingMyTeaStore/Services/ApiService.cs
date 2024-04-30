using GoingMyTeaStore.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GoingMyTeaStore.Services
{
    public static class ApiService
    {
        public static async Task<bool> RegisterUser(string name, string email, string phone, string password)
        {
            var register = new Register()
            {
                Name = name,
                Email = email,
                Phone = phone,
                Password = password
            };

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/register", content);

            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> Login(string email, string password)
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/login", content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Token>(jsonResult);

            Preferences.Set("accesstoken", result.AccessToken);
            Preferences.Set("userid", result.UserId);
            Preferences.Set("username", result.UserName);

            return true;
        }

        public static async Task<ProfileImage> GetUserProfileImage()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/users/profileimage");

            return JsonConvert.DeserializeObject<ProfileImage>(response);
        }

        public static async Task<bool> UploadUserImage(byte[] imageArray)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(imageArray), "image", "image.jpg" }
            };

            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/uploadphoto", content);

            return response.IsSuccessStatusCode;
        }

        public static async Task<List<Category>> GetCatergories()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.GetAsync(AppSettings.ApiUrl + "api/categories");

            if (response.IsSuccessStatusCode)
            {
                var strContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Category>>(strContent);
            }

            return new List<Category>();
        }

        public static async Task<List<Product>> GetProducts(string productType, string categoryId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.GetAsync(AppSettings.ApiUrl + $"api/products/productType={productType}&categoryId={categoryId}");

            if (response.IsSuccessStatusCode)
            {
                var strContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(strContent);
            }

            return new List<Product>();
        }

        public static async Task<ProductDetail> GetProductDetail(int productId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + $"api/products/" + productId);
            return JsonConvert.DeserializeObject<ProductDetail>(response);
        }

        public static async Task<bool> AddItemsInCart(ShoppingCart shoppingCart)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(shoppingCart);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/shoppingcartitems", content);

            return response.IsSuccessStatusCode;
        }

        public static async Task<List<ShoppingCartItem>> GetShoppingCartItems(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/shoppingcartitems/" + userId);
            return JsonConvert.DeserializeObject<List<ShoppingCartItem>>(response);
        }

        public static async Task<bool> UpdateCartQuantity(int productId, string action)
        {
            var httpClient = new HttpClient();
            var content = new StringContent(string.Empty);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.PutAsync(AppSettings.ApiUrl + $"api/shoppingcartitems?productId={productId}&action={action}", content);

            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> PlaceOrder(Order order)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/orders", content);

            return response.IsSuccessStatusCode;
        }

        public static async Task<List<OrderByUser>> GetOrdersByUser(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/orders/ordersbyuser/" + userId);
            return JsonConvert.DeserializeObject<List<OrderByUser>>(response);
        }

        public static async Task<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/orders/orderdetails/" + orderId);
            return JsonConvert.DeserializeObject<List<OrderDetail>>(response);
        }
    }
}
