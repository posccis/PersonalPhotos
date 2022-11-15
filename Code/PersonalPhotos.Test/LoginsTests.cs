using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalPhotos.Controllers;
using PersonalPhotos.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersonalPhotos.Test
{
    public class LoginsTests
    {
        private readonly LoginsController _controller;
        private readonly Mock<ILogins> _logins;
        private readonly Mock<IHttpContextAccessor> _acessor;

        public LoginsTests()
        {
            _logins = new Mock<ILogins>();
            ISession session = Mock.Of<ISession>();
            HttpContext httpContext = Mock.Of<HttpContext>(x => x.Session == session);
            _acessor = new Mock<IHttpContextAccessor>();
            _acessor.Setup(x => x.HttpContext).Returns(httpContext);
            _controller = new LoginsController(_logins.Object, _acessor.Object);
        }

        [Fact]
        public void Index_GivenReturnUrl_ReturnLogInView() 
        {
            ViewResult result = (_controller.Index() as ViewResult);
            Assert.NotNull(result);
            Assert.Equal("Login", result.ViewName, ignoreCase: true); 
        }

        [Fact]
        public async Task Login_GivenModelStateInvalid_ReturnLoginView() 
        {
            _controller.ModelState.AddModelError("Test", "Test");
            ViewResult result = await _controller.Login(Mock.Of<LoginViewModel>()) as ViewResult;
            Assert.Equal("Login", result.ViewName, ignoreCase: true);
            
        }

        [Fact]
        public async Task Login_GivenCorrectPassword_RedirectToDisplayAction() 
        {
            const string password = "123";
            LoginViewModel modelView = Mock.Of<LoginViewModel>(x => x.Email == "a@b.com" && x.Password == password);
            User model = Mock.Of<User>(x => x.Password == password);
            _logins.Setup(x => x.GetUser(It.IsAny<String>())).ReturnsAsync(model);
            var result = await _controller.Login(modelView);
            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
