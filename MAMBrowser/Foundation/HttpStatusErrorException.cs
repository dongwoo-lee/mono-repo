using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MAMBrowser.Foundation
{
    public class HttpStatusErrorException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpStatusErrorException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        
    }
}
