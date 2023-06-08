using Amazon.S3;
using Amazon.S3.Model;

namespace MvcConciertosEGPB.Services
{
    public class ServiceStorageS3
    {
        private string BucketName;
        private IAmazonS3 ClientS3;
        public ServiceStorageS3(IConfiguration configuration, IAmazonS3 clients3)
        {
            this.BucketName = configuration.GetValue<string>("AWS:BucketName");
            this.ClientS3 = clients3;
        }
        public async Task<bool> UploadFileAsync(string filenName, Stream stream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = filenName,
                BucketName = this.BucketName
            };
            PutObjectResponse response = await this.ClientS3.PutObjectAsync(request);
            if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
