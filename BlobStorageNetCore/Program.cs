using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorageNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var ConnectionA = "DefaultEndpointsProtocol=https;AccountName=<accountnameA>;AccountKey=<keyA>;EndpointSuffix=core.windows.net";
            var ConnectionB = "DefaultEndpointsProtocol=https;AccountName=<accountnameB>;AccountKey=<keyB>;EndpointSuffix=core.windows.net";

            BlobStuff(ConnectionA);
            BlobStuff(ConnectionB);
        }

        private static void BlobStuff(string connectionString)
        {                  
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            Console.WriteLine($"Connecting to Blob Storage Account:\n\t { blobServiceClient.AccountName }\n");

            string containerName = "blobfromapp";
            
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();

            string fileName = "myFile" + Guid.NewGuid().ToString() + ".txt";
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine($"Uploading to Blob Storage:\n\t { blobClient.Uri }\n");

            using(MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("Prestifilippo rulez")))
            {
                blobClient.Upload(ms);
            }

            foreach (BlobItem item in containerClient.GetBlobs())
                Console.WriteLine(item.Name);

            Console.WriteLine($"Downloading blob");

            BlobDownloadInfo download = blobClient.Download();

            Console.WriteLine($"Complete");
        }
    }
}
