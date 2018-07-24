using S3TestWebApi.Models;
using System.Threading.Tasks;

namespace S3TestWebApi.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);

        Task<S3Response> DeleteBucketAsync(string bucketName);

        Task<S3Response> UploadFileAsync(string bucketName);

        Task<S3Response> GetObjectFromS3Async(string bucketName, string keyName);

        Task<S3Response> DeleteObjectFromS3Async(string bucketName, string keyName);
    }
}