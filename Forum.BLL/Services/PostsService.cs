using AutoMapper;
using Forum.BLL.DTO;
using Forum.BLL.Interfaces;
using Forum.BLL.Infrastructure;
using Forum.DAL.Entities;
using Forum.DAL.Interfaces;
using Forum.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.BLL.Services
{
    internal class PostsService : IPostsService
    {
        private readonly IUnitOfWork uow;

        private readonly IMapper mapper;

        private const int postsPerPage = 10;  // Max count of posts at one page
        
        public PostsService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }      

        public PostDTO GetPost(int postId)
        {
            // validation
            if (postId < 1)
                throw new ArgumentOutOfRangeException($"Post Id cannot be zero or negative: {postId}");

            var post = uow.Posts.Get(postId);

            if (post == null)
                throw new NotFoundInDbException("Post not found");

            return mapper.Map<Post, PostDTO>(post);
        }

        public IEnumerable<PostDTO> GetPostsAtPage(int page)
        {
            // validation
            if (page < 1)
                throw new ArgumentOutOfRangeException($"Page number cannot be zero or negative: {page}");

            IEnumerable<Post> posts = uow.Posts.GetAll().
                                      OrderByDescending(p => p.Published).
                                      Skip(--page * postsPerPage).
                                      Take(postsPerPage).
                                      ToList();

            return mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
        }

        public IEnumerable<CommentDTO> GetPostComments(int postId)
        {
            GetPost(postId);

            IEnumerable<Comment> commnets = uow.Comments.GetAll().
                                            Where(c => c.PostId == postId).
                                            OrderByDescending(c => c.Published).
                                            ToList();

            return mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(commnets);
        }

        public CommentDTO AddComment(CommentDTO commentDTO)
        {
            // validation
            if (commentDTO == null)
                throw new ArgumentNullException(nameof(commentDTO));

            if (commentDTO.Content == null)
                throw new ArgumentNullException(nameof(commentDTO.Content));

            if (commentDTO.UserId == null)
                throw new ArgumentNullException(nameof(commentDTO.UserId));

            if (commentDTO.UserId < 1)
                throw new ArgumentOutOfRangeException($"User Id cannot be zero or negative: {commentDTO.UserId}");

            GetPost((int)commentDTO.PostId);

            // Set current time
            commentDTO.Published = DateTime.Now;

            // CommentId is database generated
            commentDTO.CommentId = 0;

            var comment = mapper.Map<CommentDTO, Comment>(commentDTO);

            uow.Comments.Create(comment);

            return mapper.Map<Comment, CommentDTO>(comment);
        }

        public void DeleteCommnet(int commentId)
        {
            // validation
            if (commentId < 1)
                throw new ArgumentOutOfRangeException($"Comment Id cannot be zero or negative: {commentId}");

            uow.Posts.Delete(commentId);
        }

        public PostDTO CreatePost(PostDTO postDTO)
        {
            // validation
            if (postDTO.Content == null)
                throw new ArgumentNullException(nameof(postDTO.Content));

            if (postDTO.Header == null)
                throw new ArgumentNullException(nameof(postDTO.Header));

            if (postDTO.UserId == null)
                throw new ArgumentNullException(nameof(postDTO.UserId));

            if (postDTO.UserId < 1)
                throw new ArgumentOutOfRangeException($"User Id cannot be zero or negative: {postDTO.UserId}");

            // PostId is database generated
            postDTO.PostId = 0;

            postDTO.Published = DateTime.Now;

            var post = mapper.Map<PostDTO, Post>(postDTO);

            uow.Posts.Create(post);

            return mapper.Map<Post, PostDTO>(post);
        }

        public void EditPost(PostDTO postDTO)
        {
            // validation
            if (postDTO == null)
                throw new ArgumentNullException(nameof(postDTO));

            if (postDTO.PostId < 1)
                throw new ArgumentOutOfRangeException($"Post Id cannot be zero or negative: {postDTO.PostId}");

            if (postDTO.UserId == null)
                throw new ArgumentNullException(nameof(postDTO.UserId));

            // get not modified post
            var originalPost = uow.Posts.Get((int)postDTO.PostId);

            // Check if user is author of curr post
            if (postDTO.UserId != originalPost.UserId)
                throw new ArgumentException("Only author of post can edit it");

            // publishing time does not changes
            postDTO.Published = originalPost.Published;

            if (postDTO.Header == null)
                postDTO.Header = originalPost.Header;

            if (postDTO.Content == null)
                postDTO.Content = originalPost.Header;

            var post = mapper.Map<PostDTO, Post>(postDTO);

            uow.Posts.Update(post);
        }

        public void DeletePost(int postId)
        {
            // validation
            if (postId < 1)
                throw new ArgumentOutOfRangeException($"Post Id cannot be zero or negative: {postId}");

            uow.Posts.Delete(postId);
        }

        public void SaveChanges()
        {
            uow.Save();
        }

        public void Dispose()
        {
            uow.Dispose();
        }

    }
}
