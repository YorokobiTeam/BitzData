﻿using Supabase.Postgrest.Attributes;

namespace BitzData.Models
{
    [Table("player_info")]
    class BitzPlayerInfo
    {
        [PrimaryKey("player_id")]
        public string Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("pfp_file_id")]
        public string ProfileImageFileId { get; set; }
        [Column("experience")]
        public int Experience { get; set; }
    }
}
