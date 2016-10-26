using System.Threading.Tasks;
using AwesomeServer.Controllers.Attributes;
using AwesomeServer.Services;
using AwesomeServer.Services.Interfaces;
using AwesomeServer.ViewModels;
using AwesomeLib;
using AwesomeLib.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AwesomeServer.Controllers
{
    [Authorize]
    [TypeFilter(typeof(ProductionRequireHttpsAttribute))]
    public class AccountController : Controller
    {
        readonly IOptions<ServerOptions> _serverOptions;
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;
        readonly IEmailSender _emailSender;
        readonly IStringLocalizer<AccountController> _localizer;
        readonly IStringLocalizer<SharedResources> _sharedLocalizer;
        readonly ILogger<AccountController> _logger;
        readonly IConverter _converter;

        public AccountController(
            IOptions<ServerOptions> serverOptions,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            IStringLocalizer<AccountController> localizer,
            IStringLocalizer<SharedResources> sharedLocalizer,
            ILogger<AccountController> logger,
            IConverter converter)
        {
            _serverOptions = serverOptions;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
            _logger = logger;
            _converter = converter;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            // If user is already authenticated redirect
            if (User.Identity.IsAuthenticated)
            {
                return ResolveRedirect(returnUrl);
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogTrace(LogEvents.AccountController, AwesomeMethods.ConvertDataForLog(model, _converter));

            // If user is already authenticated redirect
            if (User.Identity.IsAuthenticated)
            {
                return ResolveRedirect(returnUrl);
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            // Create new user
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation(LogEvents.AccountController, $"USER {user.Email} CREATED");
                return await SendRegisterConfirmationEmail(user);
            }
            else
            {
                _logger.LogWarning(LogEvents.AccountController, $"USER {user.Email} CREATION FAILED");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation(string userId, string code)
        {
            if (userId == null || code == null)
            {
                ModelState.AddModelError("", _sharedLocalizer["InvalidOperation"]);
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning(LogEvents.AccountController, $"USER WITH ID {userId} NOT FOUND");
                ModelState.AddModelError("", _localizer["UserNotFound"]);
                return View();
            }

            // Try to confirm user
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                _logger.LogInformation(LogEvents.AccountController, $"EMAIL CONFIRMATION FOR USER WITH ID {userId} SUCCEEDED");
            }
            else
            {
                _logger.LogWarning(LogEvents.AccountController, $"EMAIL CONFIRMATION FOR USER WITH ID {userId} FAILED");
                ModelState.AddModelError("", _localizer["EmailConfirmationFailure"]);
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn(string returnUrl)
        {
            // If user is already authenticated redirect
            if (User.Identity.IsAuthenticated)
            {
                return ResolveRedirect(returnUrl);
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogTrace(LogEvents.AccountController, AwesomeMethods.ConvertDataForLog(model, _converter));

            // If user is already authenticated redirect
            if (User.Identity.IsAuthenticated)
            {
                return ResolveRedirect(returnUrl);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning(LogEvents.AccountController, $"USER WITH EMAIL {model.Email} NOT FOUND");
                ModelState.AddModelError("", _localizer["InvalidUsernameOrPassword"]);
                return View();
            }

            // Check if user is confirmed to send confirm email again
            if (!user.EmailConfirmed)
            {
                return await SendRegisterConfirmationEmail(user);
            }

            // Sign in
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return ResolveRedirect(returnUrl);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            // If user is authenticated sign out
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogTrace(LogEvents.AccountController, AwesomeMethods.ConvertDataForLog(model, _converter));

            // If user not found act like everything was ok. Do not reveal critical info like that
            // no user with that email exists
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ForgotPasswordCheckEmail");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                $"{_serverOptions.Value.ProjectName} - {_localizer["ResetPassword"]}",
                $"{_localizer["ResetPasswordDetail"]} <a href='{callbackUrl}'>{callbackUrl}</a>");

            _logger.LogInformation(LogEvents.AccountController, $"RESET PASSWORD EMAIL SENT FOR USER {user.Email}");

            return View("ForgotPasswordCheckEmail");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                ModelState.AddModelError("", _sharedLocalizer["InvalidOperation"]);
                return View();
            }

            // Add values to the model fields UserId and Code so the Post will have them filled
            return View(new ResetPasswordViewModel
            {
                UserId = userId,
                Code = code,
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // If user not found act like everything was ok. Do not reveal critical info like that
            // no user with that id exists
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                _logger.LogWarning(LogEvents.AccountController, $"USER WITH ID {model.UserId} NOT FOUND");
                return View("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }

            return View();
        }

        IActionResult ResolveRedirect(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                // If no return Url redirect to Home
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        async Task<IActionResult> SendRegisterConfirmationEmail(IdentityUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("RegisterConfirmation", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                $"{_serverOptions.Value.ProjectName} - {_localizer["EmailConfirmation"]}",
                $"{_localizer["EmailConfirmationDetail"]} <a href='{callbackUrl}'>{callbackUrl}</a>");

            _logger.LogInformation(LogEvents.AccountController, $"REGISTER EMAIL CONFIRMATION SEND FOR USER {user.Email}");

            return View("RegisterCheckEmail");
        }
    }
}