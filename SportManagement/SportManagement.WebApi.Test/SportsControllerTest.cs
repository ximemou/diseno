using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportManagement.Data;
using SportManagement.Data.Repository;
using SportManagement.WebApi.Controllers;
using SportManagement.WebApi.Model;
using SportManagement.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SportManagement.WebApi.Test
{
    [TestClass]
    public class SportsControllerTest
    {


        [TestMethod]

        public void CreateFailedUserTest()

        {

            var modelIn = new SportModelIn();

            var userService = new Mock<ISportService>();

            var controller = new SportsController(userService.Object);

            controller.ModelState.AddModelError("", "Error");

            var result = controller.Post(modelIn);

            var createdResult = result as BadRequestObjectResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);

        }


        [TestMethod]

        public void TestGetSportsShouldReturnOKReturnSports()

        {

            var mock = new Mock<ISportService>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllSports()).Returns(new List<Sport> { new Sport("Basketball"), new Sport("Tennis") });
            var sut = new SportsController(mock.Object);
            var response = sut.Get();
            mock.Verify(m => m.GetAllSports(), Times.Once);
            var result = response as OkObjectResult;
            var model = result.Value as IEnumerable<SportModelOut>;
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(2, model.Count());

        }

        [TestMethod]
        public void GetSportTest()

        {
            var sportModel = new Sport("Basketball");
            sportModel.SportId = 1;
            var mock = new Mock<ISportService>(MockBehavior.Strict);
            mock.Setup(m => m.GetSportById(sportModel.SportId)).Returns(sportModel);
            var controller = new SportsController(mock.Object);
            var result = controller.Get(sportModel.SportId);
            var createdResult = result as OkObjectResult;
            var model = createdResult.Value as SportModelOut;
            mock.VerifyAll();
         //   Assert.AreEqual(sportModel.Name, model.Name);
            Assert.AreEqual(sportModel.SportId, model.SportId);

        }

        [TestMethod]
        public void DeleteSportTest()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var newSport = new Sport() { SportId = 1, Name = "Basketball" };         
            var mocksportservice = new Mock<ISportService>();
            mocksportservice.Setup(b1 => b1.DeleteSport(newSport.SportId)).Returns(true);
            var controller = new SportsController(mocksportservice.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = UserAdmin() }
            };
            controller.DeleteSport(newSport.SportId);
            mocksportservice.VerifyAll();
        }

        [TestMethod]
        public void GetSportsEmptyTest()
        {

            var mockDB = new Mock<IUnitOfWork>();
            var mocksportservice = new Mock<ISportService>();
            var controller = new SportsController(mocksportservice.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = UserAdmin() }
            };
            var obtainedResult = controller.Get() as OkObjectResult;

            mocksportservice.VerifyAll();
            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(200, obtainedResult.StatusCode);
        }




        private ClaimsPrincipal UserAdmin()
        {
            var genericIdentity = new GenericIdentity("Admin");

            genericIdentity.AddClaim(new Claim(ClaimTypes.Name, "Pedro"));

            genericIdentity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));
            return new GenericPrincipal(genericIdentity, null);
        }



    }
}
