using Microsoft.AspNetCore.Mvc;
using S3TestWebApi.Services;
using System.Threading.Tasks;

namespace S3TestWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/S3Bucket")]
    public class S3BucketController : Controller
    {
        private readonly IS3Service _service;

        public S3BucketController(IS3Service service)
        {
            _service = service;
        }

        [HttpPost("CreateBucket/{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
            var response = await _service.CreateBucketAsync(bucketName);

            return Ok(response);
        }

        [HttpPost("DeleteBucket/{bucketName}")]
        public async Task<IActionResult> DeleteBucket([FromRoute] string bucketName)
        {
            var response = await _service.DeleteBucketAsync(bucketName);

            return Ok(response);
        }

        [HttpPost("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFileToS3([FromRoute] string bucketName)
        {
            await _service.UploadFileAsync(bucketName);

            return Ok();
        }

        [HttpPost("GetFile/{bucketName}/{keyName}")]
        public async Task<IActionResult> GetObjectFromS3([FromRoute] string bucketName, string keyName = null)
        {
            await _service.GetObjectFromS3Async(bucketName, keyName);

            return Ok();
        }

        [HttpPost("DeleteFile/{bucketName}/{keyName}")]
        public async Task<IActionResult> DeleteObjectFromS3([FromRoute] string bucketName, string keyName = null)
        {
            await _service.DeleteObjectFromS3Async(bucketName, keyName);

            return Ok();
        }
    }
}