using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowlerLib
{
    public class ZeroLengthFileResponseException : Exception
    {
        public string Url { get; set; }
        public string FileName { get; set; }

        public ZeroLengthFileResponseException()
            : base()
        { }

        public ZeroLengthFileResponseException(string message)
            : base(message)
        { }

        public ZeroLengthFileResponseException(string message, string relatedUrl, string fileName)
            : base(message)
        {
            this.Url = relatedUrl;
            this.FileName = fileName;
        }
    }
}