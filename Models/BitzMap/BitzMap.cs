using Supabase.Postgrest.Attributes;
using System.Text.Json.Serialization;


namespace BitzData.Models.BitzMap
{
    [Table("map")]
    class BitzMap
    {
        [PrimaryKey("map_id"), JsonPropertyName("map_id")]
        public string Id { get; set; }

        [PrimaryKey("music_file_id"), JsonPropertyName("music_file_id")]
        public string MusicFileId { get; set; }

        [PrimaryKey("background_file_id"), JsonPropertyName("background_file_id")]
        public string BackgroundFileId { get; set; }

        [Column("cover_file_id"), JsonPropertyName("cover_file_id")]
        public string CoverFileId { get; set; }

        [Column("map_author_id"), JsonPropertyName("map_author_id")]
        public string MapAuthorId { get; set; }

        [Column("song_name"), JsonPropertyName("song_name")]
        public string SongName { get; set; }

        [Column("song_author"), JsonPropertyName("song_author")]
        public string SongAuthor { get; set; }

        [Column("difficulty"), JsonPropertyName("song_author")]
        public string Difficulty { get; set; }

        [JsonPropertyName("is_offline")]
        public bool IsOffline { get; set; }
    }
}
