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
                { -4, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. El usuario con ese UserId no existe.")) },
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo crear el post. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return CreatedAtAction(nameof(GetPostById), new { id = responseModel.Post.Id }, ResponseModel<Post>.SuccessResponse(responseModel.Post, "Publicación creada."));
        }

        [HttpGet]
        public ActionResult<List<Post>> GetAllPosts()
        {
            List<Post> posts = _postService.GetAllPosts();
            return Ok(ResponseModel<List<Post>>.SuccessResponse(posts, "Publicaciones encontradas."));
        }

        [HttpGet("user/{id}")]
        public ActionResult<List<UserPostsResponseModel>> GetAllPosts(int id)
        {
            List<UserPostsResponseModel> posts = _postService.GetUserPosts(id);
            if (posts.Count == 0)
            {
                return NotFound(ResponseModel<List<UserPostsResponseModel>>.ErrorResponse("No se encontraron publicaciones para este usuario."));
            }
            return Ok(ResponseModel<List<UserPostsResponseModel>>.SuccessResponse(posts, "Publicaciones encontradas."));
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePost(int id)
        {
            var responseModel = _postService.DeletePost(id);
            var responses = new Dictionary<int, Object>
            {
                { -1, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo eliminar la publicación. No existe esa publicación.")) },
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo eliminar la publicación. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return Ok(ResponseModel<Post>.SuccessResponse(null, "Publicación eliminada."));
        }

        [HttpPatch]
        public ActionResult UpdatePost([FromBody] Post post)
        {
            var responseModel = _postService.UpdatePost(post);
            var responses = new Dictionary<int, Object>
            {
                { -1, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo actualizar la publicación. No existe esta publicación")) },
                { -2, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo actualizar la publicación. Title inválido.")) },
                { -3, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo actualizar la publicación. Content invalido.")) },
                { -99, BadRequest(ResponseModel<Object>.ErrorResponse("No se pudo actualizar la publicación. Error interno.")) }
            };
            if (responses.ContainsKey(responseModel.ResponseCode))
            {
                return (ActionResult)responses[responseModel.ResponseCode];
            }
            return Ok(ResponseModel<Post>.SuccessResponse(responseModel.Post, "Publicación actualizada."));
        }

        [HttpGet("comments/{id}")]
        public ActionResult<List<Comment>> GetCommentsByPostId(int id)
        {
            List<Comment> comments = _postService.GetPostComments(id);
            if (comments.Count == 0)
            {
                return NotFound(ResponseModel<List<Comment>>.ErrorResponse("No se encontraron comentarios para esta publicación."));
            }
            return Ok(ResponseModel<List<Comment>>.SuccessResponse(comments, "Comentarios encontrados."));
        }

        [HttpPost("close/{id}")]
        public ActionResult ClosePost(int id)
        {
            PostResponseModel responseModel = _postService.ClosePost(id);
            if (responseModel.ResponseCode == -1)
            {
                return NotFound(ResponseModel<Object>.ErrorResponse("No se pudo cerrar esta publicación. No existe esta publicación."));
            }
            return Ok(ResponseModel<Post>.SuccessResponse(responseModel.Post, "Se cerró la publicación."));
        }
    }
}