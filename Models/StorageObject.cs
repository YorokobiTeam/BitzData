using BitzData.Services;
using System.Diagnostics.CodeAnalysis;

namespace BitzData.Models
{

    public class StorageObject
    {
        public string Bucket { get; set; } = "bitz-files";

        public string ObjectId
        {
            get; private set;
        }

        public string ActualPath { get; set; }

        [AllowNull]
        public string localPath = null;

        public string LocalPath
        {
            get
            {
                if (localPath is null)
                {
                    throw new Exception("Do not initialize this object outside of BitzFileService.");
                }
                return localPath;
            }
            set
            {
                localPath = value;
            }
        }

        public StorageObject(string objectId)
        {
            // Sanity check
            if (objectId is null || objectId is "")
                throw new InvalidDataException("Empty object id is not permitted.");

            ObjectId = objectId;
            ActualPath = BitzFileService.GetInstance().GetActualPath(objectId) ?? throw new Exception("File not found on remote server.");
        }

    }

    public class ProgressCallbackArgs(string initialMessage)
    {
        /// <summary>
        /// This represents the progress and goes from 0-1 (inclusive)
        /// </summary>
        public float CurrentProgress { get; set; }
        public string CallMessage { get; set; } = initialMessage;
    }
}
