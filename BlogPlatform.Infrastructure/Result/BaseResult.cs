using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogPlatform.Infrastructure.Result
{
    public class BaseResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
