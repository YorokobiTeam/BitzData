﻿
using Supabase.Gotrue;

namespace BitzData.Services
{
    /// <summary>
    /// This class is a singleton service for managing account-related operations in Bitz.
    /// </summary>
    public class BitzAccountService : GenericSupabaseService
    {
        private static BitzAccountService instance;



        private BitzAccountService() { }

        static BitzAccountService()
        {
            instance ??= new BitzAccountService();
            Initialize();
        }
        private static void Initialize()
        {

        }

        public async Task<Session?> Login(string email, string password) => await supabase.Auth.SignInWithPassword(email, password);

        public async Task Logout() => await supabase.Auth.SignOut();

        public static BitzAccountService GetInstance() => instance;


        /// <summary>
        /// </summary>
        /// <param name="eventHandler"></param>
        public void RegisterAuthEventCallback(Supabase.Gotrue.Interfaces.IGotrueClient<Supabase.Gotrue.User, Supabase.Gotrue.Session>.AuthEventHandler eventHandler) => supabase.Auth.AddStateChangedListener(eventHandler);
    }
}
