using Core.Interfaces;
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
    public class PhotoTests
    {

        [Fact]
        public async Task Upload_GivenFileName_ReturnsDisplayAction() 
        {
            //arrange
            ISession session = Mock.Of<ISession>();
            session.Set("User", Encoding.UTF8.GetBytes("a@b.com"));
            HttpContext context = Mock.Of<HttpContext>(x => x.Session == session);
            IHttpContextAccessor accessor = Mock.Of<IHttpContextAccessor>(x => x.HttpContext == context);
            IFileStorage fileStorage = Mock.Of<IFileStorage>();
            IKeyGenerator keyGenerator = Mock.Of<IKeyGenerator>();
            IPhotoMetaData photoMetaData = Mock.Of<IPhotoMetaData>();
            IFormFile formFile = Mock.Of<IFormFile>();
            PhotoUploadViewModel model = Mock.Of<PhotoUploadViewModel>(x => x.File == formFile);
            PhotosController controller = new PhotosController(keyGenerator, accessor, photoMetaData, fileStorage);

            //act
            var result = await controller.Upload(model) as RedirectToActionResult;

            //assert
            Assert.Equal("Display", result.ActionName, ignoreCase: true);
        }
    }
}
