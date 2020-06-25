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
    public class UserControllerTest
    {

        [TestMethod]
        public void CreateValidUserTest()
        {
          
            var modelIn = new UserModelIn() {Name="Juan",LastName=" Perez", Email="jaunp@gmail.com",UserName = "Alberto", Password = "pass", IsAdministrator=true };

            var fakeUser = new User("Juan","Perez","Alberto","pass","juanp@gmail.com",true);
            fakeUser.UserId = 1;

            var modelOut = new UserModelOut(fakeUser);

            var userServiceMock = new Mock<IUserService>();

   
            userServiceMock.Setup(userService => userService.CreateUser(fakeUser));

            var controller = new UsersController(userServiceMock.Object);
         
            var result = controller.Post(modelIn);

            var createdResult = result as CreatedAtRouteResult;
            var modelOut2 = createdResult.Value as UserModelOut;

            userServiceMock.VerifyAll();
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("Get", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
           

        }

        [TestMethod]

        public void CreateFailedUserTest()

        {

            var modelIn = new UserModelIn();

            var userService = new Mock<IUserService>();

            var controller = new UsersController(userService.Object);

            controller.ModelState.AddModelError("", "Error");

            var result = controller.Post(modelIn);

            var createdResult = result as BadRequestObjectResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);

        }


        [TestMethod]

        public void TestGetUsersShouldReturnOKReturnUsers()

        {

            var mock = new Mock<IUserService>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllUsers()).Returns(new List<User> { new User(), new User() });
            var sut = new UsersController(mock.Object);
            var response = sut.Get();
            mock.Verify(m => m.GetAllUsers(), Times.Once);
            var result = response as OkObjectResult;
            var model = result.Value as IEnumerable<UserModelOut>;
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(2, model.Count());

        }

        [TestMethod]
        public void GetUserTest()

        {
            var userModel =new User() {UserId=1, Name = "Juan", LastName = " Perez", Email = "jaunp@gmail.com", UserName = "Alberto", Password = "pass", IsAdministrator = true };
            var mock = new Mock<IUserService>(MockBehavior.Strict);
            mock.Setup(m => m.GetUserById(userModel.UserId)).Returns(userModel);
            var controller = new UsersController(mock.Object);
            var result = controller.Get(userModel.UserId);
            var createdResult = result as OkObjectResult;
            var model = createdResult.Value as UserModelOut;
            mock.VerifyAll();
            Assert.AreEqual(userModel.LastName, model.LastName);
            Assert.AreEqual(userModel.UserId, model.UserId);

        }

        [TestMethod]
        public void DeleteSportTest()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
           
            var newUser =new User("Juan", "Perez", "Alberto", "pass", "juanp@gmail.com", true);
            newUser.UserId = 1;
            var mockuserservice= new Mock<IUserService>();
            mockuserservice.Setup(b1 => b1.DeleteUser(newUser.UserId)).Returns(true);
            var controller = new UsersController(mockuserservice.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = UserAdmin() }
            }; 
            controller.Delete(newUser.UserId);
            mockuserservice.VerifyAll();
        }

        [TestMethod]
        public void GetUsersEmptyTest()
        {

            var mockDB = new Mock<IUnitOfWork>();
            var mockuserservice = new Mock<IUserService>();
            var controller = new UsersController(mockuserservice.Object);
           
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = UserAdmin() }
            };
            var obtainedResult = controller.Get() as OkObjectResult;

            mockuserservice.VerifyAll();
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

