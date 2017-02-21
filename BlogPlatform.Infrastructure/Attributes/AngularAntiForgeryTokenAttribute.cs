using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;
using System.Collections.Generic;

namespace BlogPlatform.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AngularAntiForgeryTokenAttribute : ActionFilterAttribute
    {
        private const string CookieName = "XSRF-TOKEN";
        private const string HeaderTokenName = "X-XSRF-Token";

        private readonly IAntiforgery antiforgery;

        public AngularAntiForgeryTokenAttribute(IAntiforgery antiforgery)
        {
            this.antiforgery = antiforgery;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if (!context.Cancel)
            {
                var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);

                context.HttpContext.Response.Cookies.Append(
                    CookieName,
                    tokens.RequestToken,
                    new CookieOptions { HttpOnly = false });
            }
        }
    }
}
