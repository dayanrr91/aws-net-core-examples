using AwsNetCoreExamples.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AwsNetCoreExamples.Services.Interfaces
{
    public interface IAwsS3Service
    {
        Task<AwsS3Response> CreateBucketAsync(string bucketName);

        Task<AwsS3Response> DeleteBucketAsync(string bucketName);

        Task<AwsS3Response> UploadFileAsync(string bucketName);

        Task<AwsS3Response> GetObjectFromS3Async(string bucketName, string keyName);

        Task<AwsS3Response> DeleteObjectFromS3Async(string bucketName, string keyName);
    }
}
