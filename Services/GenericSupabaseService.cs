using BitzData.Providers;
using Supabase;
namespace BitzData.Services
{
    public abstract class GenericSupabaseService
    {
        public static Client supabase { get; private set; }
        static GenericSupabaseService()
        {
            // Static constructor to ensure the service is initialized once
            InitializeAsync();
        }
        private static void InitializeAsync()
        {
            supabase = SupabaseProvider.GetInstance();
            if (supabase is null)
            {
                throw new Exception("SupabaseProvider was not initialized properly.");
            }
        }

    }
}
