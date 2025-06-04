using BitzData.Providers;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitzData.Services
{
    public abstract class GenericSupabaseService
    {
        internal static Client supabase { get; private set; }
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
