using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPISecurity.Authentication
{
    public class CustomForbiddenResult : JsonResult
    {
        public CustomForbiddenResult(string message)
            : base(new CustomError(message))
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
