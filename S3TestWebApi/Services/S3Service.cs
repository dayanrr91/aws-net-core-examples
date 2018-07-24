using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using S3TestWebApi.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace S3TestWebApi.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;

        // You need to manually create the file in this path or change it yourself
        private const string FilePath = "C:\\Users\\userName\\Documents\\workspace\\amazon\\test.txt";

        private const string UploadWithKeyName = "UploadWithKeyName";
        private const string FileStreamUpload = "FileStreamUpload";
        private const string AdvancedUpload = "AdvancedUpload";

        // You need to manually create this directory in your OS, or change the path
        private const string PathAndFileName = "C:\\S3Temp\\";

        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }

        /// <summary>
        ///     Create a bucket, sending the Bucket Name
        /// </summary>
        /// <param name="bucketName">Bucket Name (string)</param>
        /// <returns></returns>
        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                // Check if bucket exists, then create it
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    var response = await _client.PutBucketAsync(putBucketRequest);

                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }
            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }

            // Catch other errors
            catch (Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }

            return new S3Response
            {
                Message = "Something went wrong",
                Status = HttpStatusCode.InternalServerError
            };
        }

        /// <summary>
        ///     Deleting a bucket, sending the Bucket Name
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <returns></returns>
        public async Task<S3Response> DeleteBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName))
                {
                    // Delete the bucket and return the response
                    var response = await _client.DeleteBucketAsync(bucketName);
                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }
            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }

            // Catch other errors
            catch (Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }

            return new S3Response
            {
                Message = "Something went wrong",
                Status = HttpStatusCode.InternalServerError
            };
        }

        /// <summary>
        ///     Upload a file to an S3, here four files are uploaded in four different ways
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <returns></returns>
        public async Task<S3Response> UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);

                // Option 1 (Upload an existing file in your computer to the S3)
                await fileTransferUtility.UploadAsync(FilePath, bucketName);

                // Option2 (Upload and create the file in the process)
                await fileTransferUtility.UploadAsync(FilePath, bucketName, UploadWithKeyName);

                // Option 3 (Upload and create the file in the process)
                using (var fileToUpload = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, FileStreamUpload);
                }

                // Option 4 (Upload and create the file in the process)
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = FilePath,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456,
                    Key = AdvancedUpload,
                    CannedACL = S3CannedACL.NoACL
                };
                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }

            // Catch other errors
            catch (Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }

            return new S3Response
            {
                Message = "File uploaded Successfully",
                Status = HttpStatusCode.OK
            };
        }

        /// <summary>
        ///     Get a file from S3
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <param name="keyName">Key name of the bucket (File Name)</param>
        /// <returns></returns>
        public async Task<S3Response> GetObjectFromS3Async(string bucketName, string keyName)
        {
            if (string.IsNullOrEmpty(keyName)) keyName = "test.txt";

            try
            {
                // Build the request with the bucket name and the keyName (name of the file)
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };
                string responseBody;

                using (var response = await _client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var title = response.Metadata["x-amz-meta-title"];
                    var contentType = response.Headers["Content-Type"];

                    Console.WriteLine($"Object meta, Title: {title}");
                    Console.WriteLine($"Content type, Title: {contentType}");
                    responseBody = reader.ReadToEnd();
                }
                
                var createText = responseBody;
                File.WriteAllText(PathAndFileName + keyName, createText);

            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }

            // Catch other errors
            catch (Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }

            return new S3Response
            {
                Message = "Success",
                Status = HttpStatusCode.OK
            };
        }

        /// <summary>
        ///     Delete a file from an S3
        /// </summary>
        /// <param name="bucketName">Bucket where file is stored</param>
        /// <param name="keyName">Key name of the file</param>
        /// <returns></returns>
        public async Task<S3Response> DeleteObjectFromS3Async(string bucketName, string keyName)
        {
            if (string.IsNullOrEmpty(keyName)) keyName = "test.txt";
            
            try
            {
                // Build the request with the bucket name and the keyName (name of the file)
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                await _client.GetObjectAsync(request);

                await _client.DeleteObjectAsync(bucketName, keyName);

                return new S3Response
                {
                    Message = "The file was successfully deleted",
                    Status = HttpStatusCode.OK
                };
            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }

            // Catch other errors
            catch (Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
