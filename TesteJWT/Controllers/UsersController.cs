using Microsoft.AspNetCore.Mvc;
using TesteJWT.Models;
using TesteJWT.Services;
using TesteJWT.Data;
using Microsoft.AspNetCore.Authorization;
using System;

namespace TesteJWT.Controllers
{
    [Route("v1/users")]
    public class UsersController : ControllerBase
    {
        //https://localhost:5001/v1/Users/login
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody]User model)
        {
            try
            {
                var user = UserRepository.GetUser(model.Username, model.Password);
                if (user == null)
                    return NotFound(new { message = "Usuário ou senha inválidos!" });
                var token = TokenService.GenarateToken(user);
                user.Password = "";
                return Ok(new { user = user, token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => string.Format("Autenticado -> {0}", User.Identity.Name);


        [HttpGet]
        [Route("user")]
        [Authorize(Roles="Admin,User")]
        public string Users() => "É User";

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles="Admin")]
        public string Admins() => "É Admin";
    }
}