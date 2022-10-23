using LuSiBack.src.Context;
using LuSiBack.src.models;
using LuSiBack.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace LuSiBack.src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly LusiContext _context;
        
        public UserController(LusiContext context)
        {
            _context = context;
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser(User user)
        {
            try
            {
                if(user.Name == null ||user.Email == null || user.Password == null)
                {
                    return BadRequest(new 
                    {
                        msg = $"{user.Id} {user.Email} {user.Password}"
                    });
                }

                user.Id = 0;
                user.TaskUsers = null;

                var verifyEmail = _context.Users.Where(x => x.Email == user.Email);

                if(verifyEmail.Any())
                {
                    return BadRequest(new {
                        msg = "This user already exists"
                    });
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password); 

                user.Password = hashedPassword;

                _context.Users.Add(user);
                _context.SaveChanges();

                var token = TokenService.GenerateToken(user);

                return Ok(new {
                    token = token
                });
            }
            catch (System.Exception error)
            {
                return BadRequest(error);
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Authenticate(User user)
        {
            try
            {
                var userVerify = _context.Users.Where(x => x.Email == user.Email).FirstOrDefault();

                if(userVerify == null)
                {
                    return NotFound(new
                    {
                        msg = "User does not exist"
                    });
                }

                bool validPassword = BCrypt.Net.BCrypt.Verify(user.Password, userVerify.Password);

                if(!validPassword)
                {
                    return Unauthorized(new
                    {
                        msg = "Email or Password incorrect!"
                    });
                }

                var token = TokenService.GenerateToken(userVerify);

                return Ok(new
                {
                    token = token
                });
            }
            catch (System.Exception error)
            {
                return BadRequest(error);
            }
        }
    }
}