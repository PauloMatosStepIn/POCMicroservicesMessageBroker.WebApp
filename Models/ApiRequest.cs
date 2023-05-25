using Microsoft.AspNetCore.Mvc;
using static StepIn.Web.SD;

namespace StepIn.Web.Models
{
    public class ApiRequest
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }

        public object Data { get; set; }

        public string AccessToken { get; set; }

    }
}
