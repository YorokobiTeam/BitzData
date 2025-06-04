using BitzData.Contracts;
using BitzData.Models;
using BitzData.Telemetry;
using System.Text.Json;

namespace BitzData.Services
{
    class BitzFileService : GenericSupabaseService, IFileService
    {
        // Singleton boilerplate
        internal static BitzFileService Instance { get; private set; }
        public static BitzFileService GetInstance()
        {
            if (Instance is null)
            {
                Initialize();
            }
            return Instance!;
        }
        private static void Initialize() => Instance = new BitzFileService();


        // You can't instantiate this.
        private BitzFileService() { }

        public string? GetActualPath(string objectId) => supabase.Rpc<string>("get_actual_path", new Dictionary<string, string>
        {
            {"object_id", objectId }
        }).Result;


        public async Task DownloadObject(StorageObject @object, string? relativePath, IProgress<float>? progressCb)
        {
            // Sanity checks
            if (@object.ActualPath is null || @object.ActualPath == "") throw new InvalidDataException("The object was not found on remote server.");
            var path = Path.Combine(Constants.APPLICATION_DATA, relativePath ?? "");
            var bytes = await supabase.Storage.From(@object.Bucket).Download(@object.ActualPath, (obj, progress) =>
            {
                progressCb?.Report(progress);
            });
            try
            {
                using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(bytes, 0, bytes.Length);
                @object.LocalPath = path;
            }
            catch (Exception e)
            {
                TelemetryService.RecordException(e);
                throw;
            }
        }


        public async Task<StorageObject?> TryFromCache(string objectId)
        {
            var metadataPath = Path.Combine(Constants.APPLICATION_DATA, "cache", "metadata", objectId + ".bmeta");
            // Checks and attempts to deserialize metadata file
            try
            {
                StorageObjectMetadata localMetadata = JsonSerializer
                    .Deserialize<StorageObjectMetadata>(
                    File.ReadAllText(metadataPath)
                   ) ?? throw new FileNotFoundException("Caching metadata was not found on the file system.");

                // Gets server side metadata
                StorageObjectMetadata serverMetadata = await supabase.Rpc<StorageObjectMetadata>("get_object_metadata", new Dictionary<string, string>
               {
                   {"object_id", objectId}
               }) ?? throw new FileNotFoundException("Caching metadata was not found on the remote host.");
                if (localMetadata.MD5.Equals(serverMetadata.MD5))
                {
                    var filePath = Path.Combine(Constants.APPLICATION_DATA, "cache", "files", localMetadata.Name);
                    // Checks if file actually exists or not
                    if (!File.Exists(filePath))
                    {
                        File.Delete(metadataPath);
                        throw new FileNotFoundException("File does not exist on disk");
                    }
                    return new StorageObject(objectId)
                    {
                        LocalPath = filePath
                    };
                }
                return null;
            }
            catch (Exception e)
            {
                TelemetryService.RecordException(e);
                return null;
            }
        }


        public async void CacheObject(StorageObject @object)
        {
            try
            {
                // Sanity checks
                if (@object.ActualPath is null) throw new InvalidDataException("The object does not exist");
                if (@object.LocalPath is null) throw new InvalidDataException("The object is not hydrated");
                if (!File.Exists(@object.LocalPath)) throw new FileNotFoundException("The object does not actually exist in disk.");
                var metadataPath = Path.Combine(Constants.APPLICATION_DATA, "cache", "metadata", @object.ObjectId + ".bmeta");
                File.WriteAllText(
                    metadataPath,
                    (await supabase.Rpc(
                        "get_object_metadata",
                        new Dictionary<string, string> { { "object_id", @object.ObjectId } }))
                        .Content
                    );
            }
            catch (Exception e)
            {
                TelemetryService.RecordException(e);
                return;
            }
        }

        public async Task<StorageObject?> GetStorageObject(string objectId, string? bucket, IProgress<float>? progressCb)
        {
            StorageObject? fromCache = await TryFromCache(objectId);
            if (fromCache is not null) return fromCache;

            try
            {
                var @object = new StorageObject(objectId)
                {
                    Bucket = bucket ?? "bitz-files"
                };
                await DownloadObject(@object, "/cache/files", progressCb);
                CacheObject(@object);
                return @object;
            }
            catch (Exception e)
            {
                TelemetryService.RecordException(e);
                return null;
            }
        }



    }

}
