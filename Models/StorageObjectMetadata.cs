
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzData.Models
{

    class StorageObjectMetadata
    {
        [JsonPropertyName("e_updated_at")]
        public int UpdatedAt { get; set; }

        [JsonPropertyName("md5")]
        public string MD5 { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("e_created_at")]
        public int CreatedAt { get; set; }

        [JsonPropertyName("relative_dir")]
        public string RelativeDirectory { get; set; }



    }
}
