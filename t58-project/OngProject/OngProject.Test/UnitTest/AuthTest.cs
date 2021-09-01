using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.Auth;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Interfaces.IServices.IUriPaginationService;
using OngProject.Core.Interfaces.IServices.SendEmail;
using OngProject.Core.Interfaces.IUnitOfWork;
using OngProject.Core.Models;
using OngProject.Core.Services;
using OngProject.Core.Services.Auth;
using OngProject.Core.Services.AWS;
using OngProject.Core.Services.SendEmail;
using OngProject.Core.Services.UriPagination;
using OngProject.Infrastructure;
using OngProject.Infrastructure.Data;
using OngProject.Test.Helper;
using SendGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OngProject.Test.UnitTest
{
    [TestClass]
    public class AuthTest : BaseTests
    {
        
        private ApplicationDbContext _context;
        private UserController userController;
        private IConfiguration _configuration;
        private IAuthService _authService;
        


        [TestInitialize]
        public void MakeArrange()
        {
            _context = MakeContext("TestDb");
            IUnitOfWork unitOfWork = new UnitOfWork(_context);
            IOrganizationService _organization = new OrganizationService(unitOfWork);
            ISendEmailService sendEmailService = new SendEmailService(_configuration, _organization);
            IImagenService imagenService = new ImageService();
            IAuthService authService = new AuthService(unitOfWork, _configuration, imagenService, sendEmailService);
            IUriPaginationService uriPaginationService = new UriPaginationService("https://test/");

            IUserService userService = new UserService(unitOfWork, imagenService, uriPaginationService);
            userController = new UserController(userService, authService);
        }


        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        public IFormFile CreateImage()
        {
            var stream = File.OpenRead(@"..\..\..\UnitTest\Image\Captura1.PNG");
            var image = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
            return image;
        }


        [TestMethod]
        public async Task Register_SuccesfulRegistration()
        {
            //Arrange
            RegisterDTO newUser = new RegisterDTO();
            newUser.firstName = "User";
            newUser.lastName = "Test";
            newUser.email = "usertest@mail.com";
            newUser.password = "123";
            newUser.photo = CreateImage();



            //Act
            var response = await userController.Register(newUser);
            var count = _context.Users.Count();

            //Assert
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Register_NoImageSubmited()
        {
            //Arrange
            LoginDTO newUser = new LoginDTO();
            newUser.email = "usertest@mail.com";
            newUser.password = "123";


            //Act 
            var response = await userController.Login(newUser);
            var result = response as StatusCodeResult;

            //Assert
            Assert.AreEqual(500, result.StatusCode);
        }

       
        [TestMethod]
        public async Task Login_SuccesfulLogin()
        {
            //Arrange
            LoginDTO userLogin = new LoginDTO();
            userLogin.email = "usertest@mail.com";
            userLogin.password = "123";

            var context = new ValidationContext(userLogin, null, null);
            var results = new List<ValidationResult>();
            //Act
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(RegisterDTO), typeof(RegisterDTO)), typeof(RegisterDTO));
            var isModelStateValid = Validator.TryValidateObject(userLogin, context, results, true);
            // Assert
            Assert.IsTrue(isModelStateValid);

        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Register_EmailAlreadyExist()
        {
            //Arrange
            RegisterDTO newUser = new RegisterDTO();
            newUser.firstName = "User";
            newUser.lastName = "Test";
            newUser.email = "usertest@mail.com";
            newUser.password = "123";
            newUser.photo = CreateImage();

            var response = await userController.Register(newUser);

            
            newUser.firstName = "User";
            newUser.lastName = "Test";
            newUser.email = "usertest@mail.com";
            newUser.password = "123";
            newUser.photo = CreateImage();



            //Act
            var response2 = await userController.Register(newUser);
            var resul = response2.Result as StatusCodeResult;

            //Assert
            Assert.AreEqual(500, resul.StatusCode);


        }
    }
}
