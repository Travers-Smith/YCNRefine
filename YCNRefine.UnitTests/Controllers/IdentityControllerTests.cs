using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNBot.Controllers;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class IdentityControllerTests
    {
        [Fact]
        public void GetUser_SuccessAgreedToTerm_ReturnsUser()
        {
            var mockIdentityService = new Mock<IIdentityService>();

            Guid user = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(user);
            mockIdentityService.Setup(x => x.GetName()).Returns("Test username");

            IActionResult result = new IdentityController(mockIdentityService.Object).GetUser();

            Assert.Multiple(() =>
            {
                var actionResult = Assert.IsType<OkObjectResult>(result);

                Assert.IsType<UserModel>(actionResult.Value);
            });
        }

        [Fact]
        public void GetUser_UsernameNotFound_ReturnsNotFound()
        {
            var mockIdentityService = new Mock<IIdentityService>();

            Guid user = Guid.NewGuid();

            mockIdentityService.Setup(x => x.GetUserIdentifier()).Returns(user);


            IActionResult result = new IdentityController(mockIdentityService.Object).GetUser();

            Assert.IsType<NotFoundResult>(result);  
        }

        [Fact]
        public void GetUser_UserIdentifierNotFound_ReturnsUnauthorized()
        {
            var mockIdentityService = new Mock<IIdentityService>();

            IActionResult result = new IdentityController(mockIdentityService.Object).GetUser();

            Assert.IsType<UnauthorizedResult>(result);
        }
    }    
}
