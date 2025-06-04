
using BitzData.Models;

namespace BitzData.Contracts
{
    internal interface IFileService
    {
        /// <summary>
        /// This function attempts to get the storage object from the cache first, then validates the cache, and if needed, retrieves the storage object from Supabase.
        /// </summary>
        /// <param name="objectId">Duh?</param>
        /// <returns>The hydrated storage object</returns>
        public Task<StorageObject?> GetStorageObject(string objectId, string? bucket, IProgress<float>? progressCb);

        /// <summary>
        /// This function deletes the storage object and invalidates the cache.
        /// </summary>
        /// <param name="objectId"></param>
        void DeleteStorageObject(StorageObject objectId);

        /// <summary>
        /// Helper function to get the complete path on Supabase storage.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public string? GetActualPath(string objectId);

        /// <summary>
        /// Attempts to hydrate the given StorageObject
        /// </summary>
        /// <param name="object"></param>
        /// <param name="relativePath">The relative local path of object.</param>
        /// <param name="progressCb"></param>
        /// <returns></returns>
        public Task DownloadObject(StorageObject @object, string? relativePath, IProgress<float>? progressCb);


        internal void CacheObject(StorageObject @object);

        /// <summary>
        /// This function attempts to cache the storage object and returns the whole object if found.
        /// </summary>
        /// <returns>
        ///     A hydrated StorageObject - if the object was found in cache
        ///     <br></br
        ///     null - if the object needs to be downloaded
        /// </returns>
        /// <param name="objectId">The object ID to search for in cache</param>
        internal Task<StorageObject?> TryFromCache(string objectId);

    }
}
