using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllUsers")]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();              
        }

        [HttpPost("addNewUser")]
        public bool AddNewUser()
        {
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Emin",
                Surname = "Novruz",
                PhoneNumber = "1234567890",
                Email = "novruzemin693@gmail.com",
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return true;
        }
    }
}
