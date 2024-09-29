using CustomerOrder.API.Services.Authority;
using CustomerOrder.API.Services.Authority.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorityController : ControllerBase
    {
        private readonly IAuthorityService _authorityService;
        public AuthorityController(IAuthorityService authorityService)
        {
            _authorityService = authorityService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // hardcoded kullanıcı
            if (loginDto.Username == "test" && loginDto.Password == "123456")
            {
                var token = _authorityService.GenerateJwtToken("1"); // Kullanıcı ID'si
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
