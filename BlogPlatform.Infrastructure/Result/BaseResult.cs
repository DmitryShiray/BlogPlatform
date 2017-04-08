using Microsoft.AspNetCore.Mvc;

namespace BlogPlatform.Infrastructure.Result
{
    public class BaseResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
