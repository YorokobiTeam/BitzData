using BitzData.Models;
using BitzData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitzData.Telemetry
{
    class TelemetryService : GenericSupabaseService
    {
        public static void RecordException(Exception e)
        {
            supabase.From<ExceptionLog>().Insert(new ExceptionLog { LogText = e.StackTrace ?? e.Message });
        }
    }
}
