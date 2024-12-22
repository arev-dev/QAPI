using Microsoft.AspNetCore.Mvc;
using QAPI.Models;
using QAPI.Models.DTO;
using QAPI.Services.Interfaces;

namespace QAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{id}")]
        public ActionResult<Comment> GetCommentById(int id)
        {
            Comment comment = _commentService.GetCommentById(id);
            if (comment.Id == 0)
            {
                return NotFound(ResponseModel<Comment>.ErrorResponse("Comentario no encontrado."));
            }
            return Ok(ResponseModel<Comment>.SuccessResponse(comment, "Comentario encontrado."));
        }

        [HttpPost]
        public ActionResult CreateComment([FromBody] Comment comment)
        {
            var responseModel = _commentService.CreateComment(comment);
            var responses = new Dictionary<int, Object>
            {
                { -1, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. Content inválido.")) },
                { -2, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. UserId inválido.")) },
                { -3, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. PostId invalido.")) },
                { -4, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. El usuario con ese UserId no existe.")) },
                { -5, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. El post con ese PostId no existe.")) },
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return Ok(ResponseModel<Comment>.SuccessResponse(responseModel.Comment, "Publicación creada."));
        }
    }
}