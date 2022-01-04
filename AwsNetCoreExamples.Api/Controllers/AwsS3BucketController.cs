using AwsNetCoreExamples.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AwsNetCoreExamples.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AwsS3Bucket")]
    public class AwsS3BucketController : Controller
    {
        private readonly IAwsS3Service _service;

        public AwsS3BucketController(IAwsS3Service service)
        {
            _service = service;
        }

        [HttpPost("CreateBucket/{bucketName}")]
        public async Task<IActionResult> CreateBucket(string bucketName)
        {
            var response = await _service.CreateBucketAsync(bucketName);

            return Ok(response);
        }

        [HttpDelete("DeleteBucket/{bucketName}")]
        public async Task<IActionResult> DeleteBucket([FromRoute] string bucketName)
        {
            var response = await _service.DeleteBucketAsync(bucketName);

            return Ok(response);
        }

        [HttpPost("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFileToS3(string bucketName)
        {
            await _service.UploadFileAsync(bucketName);

            return Ok();
        }

        [HttpGet("GetFile/{bucketName}/{keyName}")]
        public async Task<IActionResult> GetObjectFromS3(string bucketName, string keyName = null)
        {
            await _service.GetObjectFromS3Async(bucketName, keyName);

            return Ok();
        }

        [HttpDelete("DeleteFile/{bucketName}/{keyName}")]
        public async Task<IActionResult> DeleteObjectFromS3(string bucketName, string keyName = null)
        {
            await _service.DeleteObjectFromS3Async(bucketName, keyName);

            return Ok();
        }
    }
}
