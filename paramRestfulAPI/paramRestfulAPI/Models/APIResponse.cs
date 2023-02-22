using System.Net;

namespace paramRestfulAPI.Models
{
    public class APIResponse
    {

        public APIResponse() 
        {
            ErrorMessages = new List<string>();
        }
        public bool IsUsable { get; set; }
        public Object Result { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public List<string> ErrorMessages { get; set; }
    }
}
