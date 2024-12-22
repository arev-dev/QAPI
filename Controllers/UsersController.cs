using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QAPI.Models;
using QAPI.Models.DTO;
using QAPI.Services.Interfaces;

namespace QAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            User user = _userService.GetUserById(id);
            if (user.Id == 0)
            {
                return NotFound(ResponseModel<User>.ErrorResponse("Usuario no encontrado."));
            }
            return Ok(ResponseModel<User>.SuccessResponse(user, "Usuario encontrado."));
        }

        [HttpGet("username/{username}")]
        public ActionResult<User> GetUserByUsername(string username)
        {
            User user = _userService.GetUserByUsername(username);
            if (user.Id == 0)
            {
                return NotFound(ResponseModel<User>.ErrorResponse("Usuario no encontrado."));
            }
            return Ok(ResponseModel<User>.SuccessResponse(user, "Usuario encontrado."));
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] User user)
        {
            var responseModel = _userService.CreateUser(user);
            var responses = new Dictionary<int, Object>
            {
                { -1, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el usuario. Username inválido.")) },
                { -2, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el usuario. Passwordd inválida.")) },
                { -3, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el usuario. Ese Username ya existe.")) },
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el usuario. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return CreatedAtAction(nameof(GetUserById), new { id = responseModel.User.Id }, ResponseModel<User>.SuccessResponse(responseModel.User, "Usuario creado."));
        }

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            List<User> users = _userService.GetAllUsers();
            return Ok(ResponseModel<List<User>>.SuccessResponse(users, "Usuarios encontrados."));
        }

        [HttpPatch]
        public ActionResult UpdateUser(int id, [FromBody] User user)
        {
            user.Id = id;
            var responseModel = _userService.UpdateUser(user);
            var responses = new Dictionary<int, Object>
            {
                { -1, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo actualizar el usuario. Username inválido.")) },
                { -2, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo actualizar el usuario. Passwordd inválida.")) },
                { -3, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo actualizar el usuario. Ese Username ya existe.")) },
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo actualizar el usuario. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return Ok(ResponseModel<User>.SuccessResponse(responseModel.User, "Usuario actualizado."));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var responseModel = _userService.DeleteUser(id);
            var responses = new Dictionary<int, Object>
            {
                { -1, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo eliminar el usuario. Usuario no encontrado.")) },
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo eliminar el usuario. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return Ok(ResponseModel<User>.SuccessResponse(responseModel.User, "Usuario eliminado."));
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] User user)
        {
            var responseModel = _userService.Login(user);
            var responses = new Dictionary<int, Object>
            {
                { -1, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo iniciar sesión. Credenciales inválidas")) }, //Campos vacios o nulos
                { -2, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo iniciar sesión. Credenciales inválidas")) }, //Campos vacios o nulos
                { -3, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo iniciar sesión. Credenciales inválidas")) }, //Password o usuario invalido
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo iniciar sesión. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return Ok(ResponseModel<string>.SuccessResponse(responseModel.Token, "Credenciales válidas."));
        }
    }
}