using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using YCNRefine.Controllers;
using YCNRefine.Core.Services;

namespace YCNRefine.UnitTests.Controllers
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public void CheckIsLoggedIn_IsLoggedIn_ReturnsTrueActionResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            identityService.Setup(x => x.IsAuthenticated()).Returns(true);

            IActionResult result = new AuthenticationController(identityService.Object)
                .CheckIsLoggedIn();

            var actionResult = Assert.IsType<OkObjectResult>(result);

            var returnedValue = Assert.IsType<bool>(actionResult.Value);

            Assert.True(returnedValue);
        }

        [Fact]
        public void CheckIsLoggedIn_NoLoggedIn_ReturnsFalseActionResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();
            identityService.Setup(x => x.IsAuthenticated()).Returns(false);

            IActionResult result = new AuthenticationController(identityService.Object)
                .CheckIsLoggedIn();

            var actionResult = Assert.IsType<OkObjectResult>(result);

            var returnedValue = Assert.IsType<bool>(actionResult.Value);

            Assert.False(returnedValue);
        }

        [Fact]
        public void Login_SuccessUserAgreedToTerms_ReturnsRedirectResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();

            IActionResult result = new AuthenticationController(identityService.Object)
                .Login();

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void Login_SuccessUserNotAgreedToTerms_ReturnsRedirectResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();


            IActionResult result = new AuthenticationController(identityService.Object)
                .Login();

            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void Login_UserAgreedToTerms_AddsClaim()
        {
            var identityService = new Mock<IIdentityService>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            IActionResult result = new AuthenticationController(identityService.Object)
                .Login();
        }


        [Fact]
        public void Login_UserHasntAgreedToTerms_DoesntAddClaim()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            IActionResult result = new AuthenticationController(identityService.Object)
                .Login();

            agreedToTermsClaimTranformation.Verify(x => x.TransformAsync(It.IsAny<ClaimsPrincipal>()), Times.Never);
        }

        [Fact]
        public void Logout_Success_ReturnsSignoutResult()
        {
            var identityService = new Mock<IIdentityService>();
            var agreedToTermsClaimTranformation = new Mock<IClaimsTransformation>();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            IActionResult result = new AuthenticationController(identityService.Object)
                .Logout();

            Assert.IsType<SignOutResult>(result);
        }
    }
}
