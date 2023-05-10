using Microsoft.AspNetCore.Mvc;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet("get-user")]
        public IActionResult GetUser()
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if (userIdentifier == null)
            {
                return Unauthorized();
            }

            UserModel user = new()
            {
                Email = _identityService.GetEmail(),
                IsAdmin = _identityService.IsAdmin()
            };

            string? name = _identityService.GetName();

            if (name == null)
            {
                return NotFound();
            }

            string[] names = name.Split(",");

            user.LastName = names.FirstOrDefault()?.Trim();

            if (names.Length > 1)
            {
                user.FirstName = names[1].Trim();
            }

            return Ok(user);
        }
    }
}