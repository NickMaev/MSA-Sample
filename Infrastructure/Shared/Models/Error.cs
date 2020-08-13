using System.Net;

namespace Shared.Models
{
    /// <summary>
    /// Represents API error (an inelegant solution due to HttpStatusCode).
    /// </summary>
    public class Error
    {
        public HttpStatusCode StatusCode { get; set; }
        public string[] Messages { get; set; }

        public Error(HttpStatusCode statusCode, params string[] messages)
        {
            StatusCode = statusCode;
            Messages = messages;
        }

        public Error()
        {

        }
    }
}