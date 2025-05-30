using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitzData.Models
{
    [Table("map")]
    class BitzMap
    {
        [PrimaryKey("map_id")]
        public string Id { get; set; }

        [Column("music_file_id")]
        public string MusicFileId { get; set; }

        [Column("background_file_id")]
        public string BackgroundFileId { get; set; }

        [Column("cover_file_id")]
        public string CoverFileId { get; set; }

        [Column("map_author_id")]
        public string MapAuthorId { get; set; }

        [Column("song_name")]
        public string SongName { get; set; }

        [Column("song_author")]
        public string SongAuthor { get; set; }

        [Column("difficulty")]
        public string Difficulty { get; set; }

    }
}
