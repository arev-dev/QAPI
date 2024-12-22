using Microsoft.AspNetCore.Mvc;
using QAPI.Models;
using QAPI.Models.DTO;
using QAPI.Services.Interfaces;

namespace QAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{id}")]
        public ActionResult<Post> GetPostById(int id)
        {
            Post post = _postService.GetPostById(id);
            if (post.Id == 0)
            {
                return NotFound(ResponseModel<Post>.ErrorResponse("Publicación no encontrada."));
            }
            return Ok(ResponseModel<Post>.SuccessResponse(post, "Publicación encontrada."));
        }

        [HttpPost]
        public ActionResult CreatePost([FromBody] Post post)
        {
            var responseModel = _postService.CreatePost(post);
            var responses = new Dictionary<int, Object>
            {
                { -1, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. Title inválido.")) },
                { -2, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. Content inválida.")) },
                { -3, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. UserId invalido.")) },
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return CreatedAtAction(nameof(GetPostById), new { id = responseModel.Post.Id }, ResponseModel<Post>.SuccessResponse(responseModel.Post, "Publicación creada."));
        }
    }
}