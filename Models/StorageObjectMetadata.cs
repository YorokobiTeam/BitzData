
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzData.Models
{

    class StorageObjectMetadata
    {
        public int UpdatedAt { get; set; }
        public string MD5 { get; set; }
        public StorageObjectType Type { get; set; }
        public string Id { get; set; }
        public int CreatedAt { get; set; }
    }

    enum StorageObjectType
    {
        Audio,
        Map,
        Image,
        Other
    }
}
