using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BitzData.Models
{
    [Table("exception_logs")]
    class ExceptionLog : BaseModel
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("log_text")]
        public string LogText { get; set; }
        [Column("time")]
        public string Time { get; set; }

    }
}
