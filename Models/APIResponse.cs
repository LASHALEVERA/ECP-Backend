    using System.Net;

    namespace ECPAPI.Models
    {
        public class APIResponse
        {
            public bool Status { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public dynamic Data { get; set; }
            public List<string> Errors { get; set; }
        public APIResponse()
        {
            Errors = new List<string>();
        }
    }
}
