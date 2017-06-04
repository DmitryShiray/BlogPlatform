using System.Threading.Tasks;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;

namespace BlogPlatform.Domain.Authentication.Requirements
{
    public class ArticleOwnerHandler : AuthorizationHandler<ArticleOwnerRequirement, Article>
    {
        private readonly IArticleManagingService articleManagingService;

        public ArticleOwnerHandler(IArticleManagingService articleManagingService)
        {
            this.articleManagingService = articleManagingService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ArticleOwnerRequirement requirement, Article article)
        {
            bool hasClaim = context.User.HasClaim(c => c.Value == Claims.ClaimsAuthorizedUserPolicyValue);
            string emailAddress = context.User.FindFirst(c => c.Value == Claims.ClaimsAuthorizedUserPolicyValue).Issuer;

            if (hasClaim && articleManagingService.IsArticleOwner(emailAddress, article.Id))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
