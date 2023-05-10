using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;
using YCNRefine.Services;

namespace YCNRefine.UnitTest.Services
{
    public class IdentityServiceTests
    {
        [Fact]
        public void GetUserIdentifier_Success_ReturnsGuid()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();
            Guid userGuid = Guid.NewGuid();

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userGuid.ToString())
            });

            Guid? userIdentifier = new IdentityService(mock.Object, configuration.Object).GetUserIdentifier();

            Assert.Equal(userGuid, userIdentifier);
        }

        [Fact]
        public void GetUserIdentifier_NoUserIdentifier_ReturnsNull()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>());

            Guid? userIdentifier = new IdentityService(mock.Object, configuration.Object).GetUserIdentifier();

            Assert.Null(userIdentifier);
        }

        [Fact]
        public void GetEmail_Success_ReturnsEmail()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();

            string userEmail = "test@email.com";

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>
            {
                new Claim("preferred_username", userEmail.ToString())
            });

            string? email = new IdentityService(mock.Object, configuration.Object).GetEmail();

            Assert.Equal(userEmail, email);
        }

        [Fact]
        public void GetEmail_NoEmail_ReturnsNull()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>());

            string? email = new IdentityService(mock.Object, configuration.Object).GetEmail();

            Assert.Null(email);
        }


        [Fact]
        public void IsAdmin_Success_ReturnsEmail()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();

            var securityGroupId = Guid.NewGuid().ToString();

            configuration
                .Setup(x => x["SecurityGroupId"])
                .Returns(securityGroupId);

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>
            {
                new Claim("groups", securityGroupId)
            });

            Assert.True(new IdentityService(mock.Object, configuration.Object).IsAdmin());
        }

        [Fact]
        public void IsAdmin_NoClaim_ReturnsFalse()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();

            var securityGroupId = Guid.NewGuid().ToString();

            configuration
                .Setup(x => x["SecurityGroupId"])
                .Returns(securityGroupId);

            mock.SetupGet(x => x.HttpContext.User.Claims).Returns(new List<Claim>());

            Assert.False(new IdentityService(mock.Object, configuration.Object).IsAdmin());
        }

        [Fact]
        public void IsAuthenticated_UserAuthenticated_ReturnsTrue()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();

            mock.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(true);

            Assert.True(new IdentityService(mock.Object, configuration.Object).IsAuthenticated());
        }

        [Fact]
        public void IsAuthenticated_NoUserAuthenticated_ReturnsFalse()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();

            mock.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(false);

            Assert.False(new IdentityService(mock.Object, configuration.Object).IsAuthenticated());
        }

        [Fact]
        public void IsAuthenticated_UserIsNull_ReturnsFalse()
        {
            var mock = new Mock<IHttpContextAccessor>();
            var configuration = new Mock<IConfiguration>();

            Assert.False(new IdentityService(mock.Object, configuration.Object).IsAuthenticated());
        }
    }
}
