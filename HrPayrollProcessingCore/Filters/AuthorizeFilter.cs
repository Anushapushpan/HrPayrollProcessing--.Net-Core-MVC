using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HrPayrollProcessingCore.Filters
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        private readonly ISession _session;
        public AuthorizeFilter(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            //List<string> lstKeys = context.RouteData.Values.Keys.ToList();
            if (string.IsNullOrEmpty(_session.GetString("User")))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
            {  {"Area","default"},
                {"Controller","Login" },
                {"Action","Login" }
            });
            }

        }
    }

    public sealed class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { };
        }

    }
}
    
