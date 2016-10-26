using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AwesomeServer.Controllers.Attributes
{
    // Apply Https only for Production
    public class ProductionRequireHttpsAttribute : RequireHttpsAttribute
    {
        readonly IHostingEnvironment _env;

        public ProductionRequireHttpsAttribute(IHostingEnvironment env)
        {
            _env = env;
        }

        public override void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (_env.IsProduction())
            {
                base.OnAuthorization(filterContext);
            }
        }

        protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
        {
            if (_env.IsProduction())
            {
                base.HandleNonHttpsRequest(filterContext);
            }
        }
    }
}