using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using AwsNetCoreExamples.Services.Interfaces;
using AwsNetCoreExamples.Services.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AwsNetCoreExamples.Services.Services
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly IAmazonS3 _awsS3Client;

        // You need to manually create the file in this path or change it yourself
        private const string FilePathToUpload = "C:\\Test-Files\\test.txt";

        // You need to manually create this directory in your OS, or change the path
        private const string FilePathToDownload = "C:\\Test-Files\\";

        public AwsS3Service(IAmazonS3 awsS3Client)
        {
            _awsS3Client = awsS3Client;
        }

        /// <summary>
        ///     Create a bucket sending the Name
        /// </summary>
        /// <param name="bucketName">Bucket Name (string)</param>
        /// <returns></returns>
        public async Task<AwsS3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                var existBucket = await AmazonS3Util.DoesS3BucketExistV2Async(_awsS3Client, bucketName);

                if (existBucket)
                    return new AwsS3Response { Message = "The bucket already exists", Status = HttpStatusCode.Conflict };

                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                var response = await _awsS3Client.PutBucketAsync(putBucketRequest);

                return new AwsS3Response { Message = $"Bucket with name {bucketName} was successfully created", Status = response.HttpStatusCode };
            }
            catch (AmazonS3Exception e)
            {
                return new AwsS3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new AwsS3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        ///     Deleting a bucket sending the Name
        /// </summary>
        /// <param name="bucketName">Bucket name to delete</param>
        /// <returns></returns>
        public async Task<AwsS3Response> DeleteBucketAsync(string bucketName)
        {
            try
            {
                var existBucket = await AmazonS3Util.DoesS3BucketExistV2Async(_awsS3Client, bucketName);

                if (!existBucket)
                    return new AwsS3Response { Message = $"Bucket with name {bucketName} was not found", Status = HttpStatusCode.NotFound };

                var response = await _awsS3Client.DeleteBucketAsync(bucketName);

                return new AwsS3Response { Message = response.ResponseMetadata.RequestId, Status = response.HttpStatusCode };
            }
            catch (AmazonS3Exception e)
            {
                return new AwsS3Response { Message = e.Message, Status = e.StatusCode };
            }
            catch (Exception e)
            {
                return new AwsS3Response { Message = e.Message, Status = HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        ///     Upload a file to an S3, here four files are uploaded in four different ways
        /// </summary>
        /// <param name="bucketName">Upload a file to a bucket</param>
        /// <returns></returns>
        public async Task<AwsS3Response> UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_awsS3Client);

                // Option 1 (Upload an existing file in your computer to the S3)
                await fileTransferUtility.UploadAsync(FilePathToUpload, bucketName);

                // Option2 (Upload and create the file in the process)
                //await fileTransferUtility.UploadAsync(FilePath, bucketName, UploadWithKeyName);

                // Option 3 (Upload and create the file in the process)
                //using (var fileToUpload = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                //{
                //    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, FileStreamUpload);
                //}

                // Option 4 (Upload and create the file in the process)
                //var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                //{
                //    BucketName = bucketName,
                //    FilePath = FilePath,
                //    StorageClass = S3StorageClass.Standard,
                //    PartSize = 6291456,
                //    Key = AdvancedUpload,
                //    CannedACL = S3CannedACL.NoACL
                //};
                //fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                //fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                //await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception e)
            {
                return new AwsS3Response { Message = e.Message, Status = e.StatusCode };
            }
            catch (Exception e)
            {
                return new AwsS3Response { Message = e.Message, Status = HttpStatusCode.InternalServerError };
            }

            return new AwsS3Response { Message = "File uploaded Successfully", Status = HttpStatusCode.OK };
        }

        /// <summary>
        ///     Get a file from S3
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <param name="keyName">Key name of the file (file name including extension)</param>
        /// <returns></returns>
        public async Task<AwsS3Response> GetObjectFromS3Async(string bucketName, string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
                keyName = "test.txt";

            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };
                string responseBody;

                using (var response = await _awsS3Client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var title = response.Metadata["x-amz-meta-title"];
                    var contentType = response.Headers["Content-Type"];
                    responseBody = reader.ReadToEnd();
                }

                var createText = responseBody;
                File.WriteAllText(FilePathToDownload + keyName, createText);
            }
            catch (AmazonS3Exception e)
            {
                return new AwsS3Response { Message = e.Message, Status = e.StatusCode };
            }
            catch (Exception e)
            {
                return new AwsS3Response { Message = e.Message, Status = HttpStatusCode.InternalServerError };
            }
            return new AwsS3Response { Message = "Success", Status = HttpStatusCode.OK };
        }

        /// <summary>
        ///     Delete a file from an S3 bucket
        /// </summary>
        /// <param name="bucketName">Bucket where file is stored</param>
        /// <param name="keyName">Key name of the file (file name including extension)</param>
        /// <returns></returns>
        public async Task<AwsS3Response> DeleteObjectFromS3Async(string bucketName, string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
                keyName = "test.txt";

            try
            {
                var request = new GetObjectRequest { BucketName = bucketName, Key = keyName };

                var response = await _awsS3Client.GetObjectAsync(request);

                if (response == null || response.HttpStatusCode != HttpStatusCode.OK)
                    return new AwsS3Response { Message = "Error getting the object from the bucket", Status = HttpStatusCode.NotFound };

                await _awsS3Client.DeleteObjectAsync(bucketName, keyName);

                return new AwsS3Response { Message = "The file was successfully deleted", Status = HttpStatusCode.OK };
            }
            catch (AmazonS3Exception e)
            {
                return new AwsS3Response { Message = e.Message, Status = e.StatusCode };
            }
            catch (Exception e)
            {
                return new AwsS3Response { Message = e.Message, Status = HttpStatusCode.InternalServerError };
            }
        }
    }
}
