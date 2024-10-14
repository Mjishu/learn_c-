using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Intro
{
    class Program
    {
        private static async Task<dynamic> getCity(string city)
        {
            ApiService apiService = new ApiService();
            string url = $"http://api.weatherapi.com/v1/current.json?key=bd6ea40dff0c4acc8cc150209242107&q={city}";
            dynamic data = await apiService.GetDataAsync(url);

            if (data.ValueKind != JsonValueKind.Undefined && data.ValueKind != JsonValueKind.Null)
            {
                return data;
            }
            return null;
        }
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter your name");
            string username = Console.ReadLine();

            Console.WriteLine("Your username is " + username);
            Console.WriteLine("Do you want to continue? (y)es or (n)o");

            string toContinue = Console.ReadLine();
            if (toContinue == "n" || toContinue == "no")
            {
                Console.WriteLine("See you next time " + username);
                return;
            }

            Console.WriteLine("Which city are you looking for?");
            string city = Console.ReadLine();
            var data = await getCity(city);

            if (data != null)
            {
                Console.WriteLine($"City: {data.location.name}\nRegion: {data.location.region}\nCountry: {data.location.country}");
                Console.WriteLine($"Temperature in F: {data.current.temp_f}, C: {data.current.temp_c}");
            }
            else
            {
                Console.WriteLine("City data not found.");
            }
            return;
        }
    }
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<dynamic> GetDataAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<dynamic>(responseData) ?? throw new InvalidOperationException("Deserialization returned null");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return default;
            }
        }
    }

}