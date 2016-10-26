using System;
using System.Threading.Tasks;
using AwesomeServer.Controllers.Attributes;
using AwesomeServer.ViewModels;
using AwesomeLib.Services.Interfaces;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AwesomeServer.Controllers
{
    [TypeFilter(typeof(ProductionRequireHttpsAttribute))]
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> _logger;
        readonly IExceptionResolver _exResolver;
        readonly IIdentityServerInteractionService _interaction;

        public HomeController(
            ILogger<HomeController> logger,
            IExceptionResolver exResolver,
            IIdentityServerInteractionService interaction)
        {
            _logger = logger;
            _exResolver = exResolver;
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Error(string errorId)
        {
            var error = await RetrieveErrorDetailsFromIdentityServer(errorId);
            if (error == null)
            {
                error = RetrieveErrorDetailsFromHttpContext();
            }

            return View(new ErrorViewModel
            {
                ErrorMessage = error
            });
        }

        async Task<string> RetrieveErrorDetailsFromIdentityServer(string errorId)
        {
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message?.Error != null)
            {
                _logger.LogError(LogEvents.HomeController, message.Error);
                return message.Error;
            }

            return null;
        }

        string RetrieveErrorDetailsFromHttpContext()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (feature?.Error != null)
            {
                var msg = _exResolver.GetInnerMessage(feature.Error);
                _logger.LogError(LogEvents.HomeController, feature.Error, msg);
                return msg;
            }

            return null;
        }

        public IActionResult Culture(string culture, string currentUrl)
        {
            // Add culture cookie and reload the same page so the UseRequestLocalization middleware
            // will use that cookie to select culture
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(3) }
            );

            return Redirect(currentUrl);
        }
    }
}