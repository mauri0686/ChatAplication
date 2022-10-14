using ChatDomain.Models;
using ChatInfrastruncture;
using ChatInfrastruncture.Service;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ChatUnitTest;

public class UsertTest
{
 
        private Mock<IRepository<User>> _userRepository;
        private UserService _service;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IRepository<User>>();
            var userManager = new Mock<UserManager<IdentityUser>>();
            _service = new UserService(_userRepository.Object);
         
        }

        [Test]
        public async Task ShouldReturnUsers()
        {

            var users =  new List<User>()
            {
                new(new IdentityUser<Guid>("Test")) { Email = "Test@gmail.com" },
                new(new IdentityUser<Guid>("Test2")) { Email = "Test2@gmail.com" }
            };
            _userRepository.Setup(  p =>  p.GetAll()).ReturnsAsync(users);
            
             var result = await _service.Get();
            Assert.That(() => result, Is.TypeOf(typeof(List<User>)));
        }
    }
