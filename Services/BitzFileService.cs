using BitzData.Contracts;
using BitzData.Models;
using BitzData.Telemetry;
using System.Text.Json;

namespace BitzData.Services
{
    /// <summary>
    /// This class manages IO and file related operations in Bitz.
    /// </summary>
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


        public async Task DownloadObject(StorageObject @object, string relativeDir = "/cache/files", IProgress<float>? progressCb = null)
        {
            // Sanity checks
            if (@object.ActualPath is null || @object.ActualPath == "") throw new InvalidDataException("The object was not found on remote server.");
            var path = Path.Combine(Constants.APPLICATION_DATA, relativeDir ?? "", $"{@object.ObjectId}.{Utilities.GetExtension(@object.ActualPath)}");
            var bytes = await supabase.Storage.From(@object.Bucket).Download(@object.ActualPath, (obj, progress) =>
            {
                progressCb?.Report(progress);
            });
            try
            {
                using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(bytes, 0, bytes.Length);
                @object.LocalPath = path;
                CacheObject(@object, relativeDir);
            }
            catch (Exception e)
            {
                TelemetryService.RecordException(e);
                throw;
            }
        }


        public async Task<StorageObject?> TryFromCache(string objectId)
        {
            var metadataPath = Path.Combine(Constants.CACHE_METADATA, $"{objectId}.bmeta");
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

                // Verify hashes match
                if (localMetadata.MD5.Equals(serverMetadata.MD5))
                {
                    var filePath = Path.Combine(Constants.APPLICATION_DATA, localMetadata.RelativeDirectory, $"{serverMetadata.Id}.{Utilities.GetExtension(serverMetadata.Name)}");
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


        public async void CacheObject(StorageObject @object, string relativeDir = "/cache/files")
        {
            try
            {
                // Sanity checks
                if (@object.ActualPath is null) throw new InvalidDataException("The object does not exist");
                if (@object.LocalPath is null) throw new InvalidDataException("The object is not hydrated");
                if (!File.Exists(@object.LocalPath)) throw new FileNotFoundException("The object does not actually exist in disk.");

                // Writes the metadata to disk
                var metadataPath = Path.Combine(Constants.CACHE_METADATA, @object.ObjectId + ".bmeta");
                var serverMetadataJson = supabase.Rpc("get_object_metadata", new Dictionary<string, string>
                {
                    {"object_id", @object.ObjectId }
                }).Result.Content ?? throw new Exception("Couldn't retrieve information about object on server.");

                var serverMetadata = (JsonSerializer.Deserialize<StorageObjectMetadata>(serverMetadataJson)) ?? throw new Exception("Server responded with invalid data");
                serverMetadata.RelativeDirectory = relativeDir;
                File.WriteAllText(
                    metadataPath,
                    JsonSerializer.Serialize(serverMetadata)
                );
            }
            catch (Exception e)
            {
                TelemetryService.RecordException(e);
                return;
            }
        }

        /// <summary>
        /// Retrieves a <see cref="StorageObject"/> by its ID, optionally downloading it from storage if not found in the local cache.
        /// </summary>
        /// <param name="objectId">The unique identifier of the object to retrieve.</param>
        /// <param name="relativeDir">
        /// The relative directory where the object should be downloaded to. Defaults to <c>"/cache/files"</c>.
        /// </param>
        /// <param name="bucket">
        /// The name of the storage bucket. Defaults to <c>"bitz-files"</c>.
        /// </param>
        /// <param name="progressCb">
        /// An optional callback to report download progress, where the value is a float between 0 and 1.
        /// </param>
        /// <returns>
        /// A <see cref="StorageObject"/> instance if found or successfully downloaded; otherwise, <c>null</c>.
        /// </returns>
        /// <remarks>
        /// If the object is found in cache, it is returned immediately without downloading. If not, the method
        /// attempts to download the object from the specified bucket and directory. Any exceptions during download
        /// are caught and recorded via <see cref="TelemetryService.RecordException(Exception)"/>.
        /// </remarks>
        public async Task<StorageObject?> GetStorageObject(string objectId, string relativeDir = "/cache/files", string bucket = "bitz-files", IProgress<float> progressCb = null)
        {
            StorageObject? fromCache = await TryFromCache(objectId);
            if (fromCache is not null) return fromCache;

            try
            {
                var @object = new StorageObject(objectId)
                {
                    Bucket = bucket ?? "bitz-files"
                };
                await DownloadObject(@object, relativeDir, progressCb);
                return @object;
            }
            catch (Exception e)
            {
                TelemetryService.RecordException(e);
                return null;
            }
        }

        public void DeleteStorageObject(StorageObject @object)
        {
            File.Delete(Path.Combine(Constants.CACHE_METADATA, $"{@object.ObjectId}.bmeta"));
            File.Delete(Path.Combine(Constants.CACHE_FILES, $"{@object.ObjectId}.{Utilities.GetExtension(@object.ActualPath)}"));

        }


    }

}
