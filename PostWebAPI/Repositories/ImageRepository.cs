using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI.Repositories
{
    public class ImageRepository
    {
        private string container;
        private string connectionString;
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient blobContainerClient;

        public ImageRepository(string container, string connectionString)
        {
            this.container = container;
            this.connectionString = connectionString;
            blobServiceClient = new BlobServiceClient(this.connectionString);
            blobContainerClient = blobServiceClient.GetBlobContainerClient(this.container);

        }

        internal void Add(string fileName, Stream stream)
        {
            blobContainerClient.UploadBlob(fileName, stream);
        }
    }
}
