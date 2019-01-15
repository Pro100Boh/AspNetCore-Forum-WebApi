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
        private readonly IPostsService postsService;

        private readonly IMapper mapper;

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

                return Ok(posts);
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

        // GET api/posts/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var postDTO = postsService.GetPost(id);

                return Ok(postDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundInDbException ex)
            {
                return NotFound(ex.Message);
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
                var commnets = postsService.GetPostComments(id);

                return Ok(commnets);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundInDbException notFoundEx)
            {
                return BadRequest(notFoundEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // internal server error
            }
        }

        [Authorize]
        [HttpPost("{postId}/comments")]
        public IActionResult AddComment(int postId, [FromBody]CommentDTO commentDTO)
        {
            try
            {
                int currentUserId = int.Parse(User.Identity.Name);

                commentDTO.PostId = postId;

                commentDTO.UserId = currentUserId;

                var comment = postsService.AddComment(commentDTO);

                postsService.SaveChanges();

                return Created($"posts/{comment.PostId}/comments", comment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundInDbException notFoundEx)
            {
                return BadRequest(notFoundEx.Message);
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundInDbException notFoundEx)
            {
                return BadRequest(notFoundEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // internal server error
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreatePost([FromBody]PostDTO postDTO)
        {
            try
            {
                int currentUserId = int.Parse(User.Identity.Name);

                postDTO.UserId = currentUserId;

                var createdPost = postsService.CreatePost(postDTO);

                postsService.SaveChanges();

                return Created($"posts/{createdPost.PostId}", createdPost);
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

        // TODO by id put best
        [Authorize]
        [HttpPut]
        public IActionResult EditPost([FromBody]PostDTO postDTO)
        {
            try
            {
                int currentUserId = int.Parse(User.Identity.Name);

                postDTO.UserId = currentUserId;

                postsService.EditPost(postDTO);

                postsService.SaveChanges();

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundInDbException ex)
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundInDbException ex)
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
