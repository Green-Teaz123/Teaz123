using Microsoft.AspNetCore.Mvc;
using SU_API.Models;

namespace API_SU.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<string>> GetUser()
        {
            var user = new List<string>();
            user.Add("User1");
            user.Add("User2");
            user.Add("User3");

            return Ok(user);
        }
        [HttpPost]
        [Route("create")]
        public ActionResult CreateUser([FromBody] UserViewModel userModel)
        {

            return Ok($"User {userModel.Username} created successfully");
        }

    }
}