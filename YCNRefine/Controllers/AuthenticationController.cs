using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using YCNRefine.Core.Services;

namespace YCNRefine.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthenticationController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpGet("check-is-logged-in")]
        public IActionResult CheckIsLoggedIn()
        {
            return Ok(_identityService.IsAuthenticated());
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return Redirect("/chat-labeller");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return SignOut(
                 new AuthenticationProperties
                 {
                     RedirectUri = "/login",
                 },
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 OpenIdConnectDefaults.AuthenticationScheme
                );

        }
    }
}