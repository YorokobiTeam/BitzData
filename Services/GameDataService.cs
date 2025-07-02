using BitzData.Contracts;
using BitzData.Models.BitzMap;
using BitzData.Models.GameData;

namespace BitzData.Services
{
    public class GameDataService : GenericSupabaseService, IGameDataService
    {
        /// <summary>
        /// This class manages IO and file related operations in Bitz.
        /// </summary>
        // Singleton boilerplate
        internal static GameDataService Instance { get; private set; }

        public static GameDataService GetInstance()
        {
            if (Instance is null)
            {
                Initialize();
            }
            return Instance!;
        }

        private static void Initialize() => Instance ??= new GameDataService();


        // You can't instantiate this.
        private GameDataService() { }


        public async Task<BitzMap?> GetBitzMapAsync(string id)
        {
            return (await supabase.From<BitzMap>().Select("*").Filter("map_id", Supabase.Postgrest.Constants.Operator.Equals, id).Get()).Models[0];
        }

        public async Task<List<BitzMap>> QueryMapsAsync(string query = "")
        {
            return (await supabase.From<BitzMap>().Select("*").Where(x => x.SongName.Contains(query)).Get()).Models;
        }

        public async Task<PlayerInfo?> GetPlayerInfoAsync(Guid id)
        {
            return (await supabase.From<PlayerInfo>().Select("*").Where(x => x.PlayerId == id).Get()).Models[0];
        }

        public async Task UpsertBeatmap(BitzMap map)
        {
            await supabase.From<BitzMap>().Upsert(map);
        }

        public async Task UpdatePlayerInfo(PlayerInfo info)
        {
            await supabase.From<PlayerInfo>().Where(x => x.PlayerId == info.PlayerId).Update(info);
        }

        public async void DeleteBeatmap(Guid map_id)
        {
            await supabase.From<BitzMap>().Where(x => x.Id == map_id).Delete();
        }

    }
}
