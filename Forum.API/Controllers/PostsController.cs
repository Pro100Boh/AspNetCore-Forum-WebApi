using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forum.API.Models;
using Forum.BLL.DTO;
using Forum.BLL.Infrastructure;
using Forum.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Forum.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostsService postsService;

        private IMapper mapper;

        public PostsController(IPostsService service, IMapper mapper)
        {
            postsService = service;
            this.mapper = mapper;
        }

        [HttpGet("page/{page}")]
        public IActionResult GetPostsAtPage(int page)
        {
            try
            {
                var posts = postsService.GetPostsAtPage(page);

                var postsView = mapper.Map<IEnumerable<PostDTO>, IEnumerable<PostView>>(posts);

                return Ok(postsView);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // internal server error
            }

        }

        // GET api/posts/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var post = postsService.GetPost(id);

                if (post == null)
                {
                    return NotFound();
                }
                var postView = mapper.Map<PostDTO, PostView>(post);

                return Ok(postView);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // internal server error
            }
        }

        // GET api/posts/5/comments
        [HttpGet("{id}/comments")]
        public IActionResult GetComments(int id)
        {
            try
            {
                var post = postsService.GetPost(id);

                if (post == null)
                {
                    return NotFound();
                }
                var postView = mapper.Map<PostDTO, PostView>(post);

                return Ok(postView);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // internal server error
            }
        }

        [Authorize]
        [HttpPost("{postId}/comments")]
        public IActionResult AddComment(CommentView commentView)
        {
            try
            {
                var commentDTO = mapper.Map<CommentView, CommentDTO>(commentView);

                postsService.AddComment(commentDTO);

                postsService.SaveChanges();

                return Ok();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // internal server error
            }
        }

        [Authorize(Roles = Role.AdminOrModer)]
        [HttpDelete("comments/{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            try
            {
                postsService.DeleteCommnet(commentId);

                postsService.SaveChanges();

                return Ok();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // internal server error
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreatePost([FromBody]PostView post)
        {
            try
            {
                var productDTO = mapper.Map<PostView, PostDTO>(post);

                postsService.CreatePost(productDTO);

                postsService.SaveChanges();

                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Internal Server Error
            }

        }

        // TODO by id put best
        [Authorize]
        [HttpPut]
        public IActionResult EditPost([FromBody]PostView post)
        {
            try
            {
                var postDTO = mapper.Map<PostView, PostDTO>(post);

                postsService.EditPost(postDTO);

                postsService.SaveChanges();

                return Ok();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Internal Server Error
            }
        }

        // DELETE api/values/5
        [Authorize(Roles = Role.AdminOrModer)]
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            try
            {
                postsService.DeletePost(id);

                postsService.SaveChanges();

                return Ok();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Internal Server Error
            }
        }
    }
}
