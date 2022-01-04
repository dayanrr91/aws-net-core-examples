using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AwsNetCoreExamples.Services.Models
{
    public class AwsS3Response
    {
        public HttpStatusCode Status { get; set; }

        public string Message { get; set; }
    }
}
