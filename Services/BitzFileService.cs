using BitzData.Models;
using Supabase.Gotrue;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace BitzData.Services
{
    class BitzFileService : GenericSupabaseService
    {
        private readonly string GameDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/YorokobiTeam/Bitz";
        // You can't instantiate this.
        private BitzFileService() { }
        public static async string GetActualPath(string objectId)
        {
            
        }
        public static async void DownloadObject(StorageObject @object, string relativePath, IProgress<float> progressCb)
        {
            // Sanity checks
            if (@object.ActualPath is null || @object.ActualPath == "") throw new InvalidDataException("The object was not found on remote server.");
            var path = Path.Combine(Constants.APPLICATION_DATA, relativePath);
            var bytes = await supabase.Storage.From(@object.Bucket).Download(@object.ActualPath, (obj, progress) =>
            {
                progressCb.Report(progress);
            });
            try
            {
                using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(bytes, 0, bytes.Length);
                @object.LocalPath = path;
            }
            catch (Exception)
            {
                throw new IOException("Couldn't write file to disk.");
            }
        }

        /// <summary>
        /// This function attempts to cache the storage object and
        /// </summary>
        /// <returns>
        ///     StorageObject - if the object was found in cache
        ///     null - if the object needs to be downloaded
        /// </returns>
        private static /*async*/ StorageObject? TryFromCache(string objectId)
        {
            return null;
        }

        public static async StorageObject GetStorageObject(string objectId)
        {
            
        }

        public static async File

    }

}
