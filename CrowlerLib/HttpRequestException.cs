using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowlerLib
{
    public class HttpRequestException : Exception
    {
        public string Url { get; set; }

        public HttpRequestException()
            : base()
        { }

        public HttpRequestException(string message)
            : base(message)
        { }

        public HttpRequestException(string message, string relatedUrl)
            : base(message)
        {
            this.Url = relatedUrl;
        }

        public HttpRequestException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public HttpRequestException(string message, Exception innerException, string relatedUrl)
            : base(message, innerException)
        {
            this.Url = relatedUrl;
        }
    }
}