using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPISecurity.Authentication
{
    public class ApiKeyRequirementHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        public const string API_KEY_HEADER_NAME = "X-API-KEY";

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            SucceedRequirementIfApiKeyPresentAndValid(context, requirement);
            return Task.CompletedTask;
        }

        private void SucceedRequirementIfApiKeyPresentAndValid(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                var apiKey = authorizationFilterContext.HttpContext.Request.Headers[API_KEY_HEADER_NAME].FirstOrDefault();
                if(apiKey == null)
                {
                    var filterContext = context.Resource as AuthorizationFilterContext;
                    var response = filterContext?.HttpContext.Response;
                    response?.OnStarting(async () =>
                    {
                        filterContext.Result = new CustomForbiddenResult("Access failed.");                   
                    });
                }
                else
                if (requirement.ApiKeys.Any(requiredApiKey => apiKey == requiredApiKey))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var filterContext = context.Resource as AuthorizationFilterContext;
                    var response = filterContext?.HttpContext.Response;
                    response?.OnStarting(async () =>
                    {
                        filterContext.Result = new CustomUnauthorizedResult("Authorization failed.");                  
                    });
                }
            }
        }
    }
}
