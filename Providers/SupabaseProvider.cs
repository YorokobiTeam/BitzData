using BitzData.Models;
using Supabase;
using System.Net.Http.Json;

namespace BitzData.Providers
{
    public class SupabaseProvider
    {
        private static Client supabase;


        static SupabaseProvider()
        {
            // Static constructor to ensure the provider is initialized once
            InitializeAsync();
        }
        public async static void InitializeAsync()
        {
            // Get the Supabase URL and Key from the web server
            HttpClient hc = new()
            {
                BaseAddress = new Uri("http://api.maik.io.vn")
            };
            var response = await hc.GetFromJsonAsync<APIKeys>("/keys/bitz");
            if (response == null || response.SupabaseKey == null || response.SupabaseUrl == null)
            {
                throw new Exception("Failed to retrieve Supabase API keys from the server.");
            }

            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true,
                AutoRefreshToken = true,
            };
            supabase = new Supabase.Client(response.SupabaseUrl, response.SupabaseKey, options);
        }

        public static Client GetInstance() { 
            return supabase ?? throw new InvalidOperationException("SupabaseProvider has not been initialized.");
        }

    }
}
