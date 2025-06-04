
using BitzData.Providers;
using Supabase;
using static Supabase.Gotrue.Constants;

namespace BitzData.Services
{
    /// <summary>
    /// This class is a singleton service for managing account-related operations in Bitz.
    /// </summary>
    public class BitzAccountService : GenericSupabaseService
    {
        private static BitzAccountService instance;
        private static Client supabase;

        private BitzAccountService() { }
        static BitzAccountService()
        {
            InitializeAsync();
        }
        private static void InitializeAsync()
        {
            supabase.Auth.AddStateChangedListener((auth, state) =>
            {
                switch (state)
                {
                    case AuthState.SignedIn:
                        if (auth is not null)
                        {
                        }
                        break;
                };
            });
        }

        public static BitzAccountService GetInstance()
        {
            instance ??= new BitzAccountService();
            return instance;
        }

        public static void RegisterAuthEventCallback(Supabase.Gotrue.Interfaces.IGotrueClient<Supabase.Gotrue.User, Supabase.Gotrue.Session>.AuthEventHandler eventHandler)
        {
            supabase.Auth.AddStateChangedListener(eventHandler);
        }
    }
}
