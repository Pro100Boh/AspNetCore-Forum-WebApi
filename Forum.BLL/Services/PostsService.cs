using AutoMapper;
using Forum.BLL.DTO;
using Forum.BLL.Interfaces;
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

        private const int postsPerPage = 10;  // Max count of posts on 1 page
        
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



            return mapper.Map<Post, PostDTO>(post);
        }

        public IEnumerable<PostDTO> GetPostsAtPage(int page)
        {
            // validation
            if (page < 1)
                throw new ArgumentOutOfRangeException($"Page number cannot be zero or negative: {page}");

            IEnumerable<Post> posts = uow.Posts.GetAll().OrderByDescending(p => p.Published).
                                        Skip(--page * postsPerPage).Take(postsPerPage).ToList();

            return mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
        }

        public void CreatePost(PostDTO postDTO)
        {
            // validation
            if (postDTO.Content == null)
                throw new ArgumentNullException(nameof(postDTO.Content));

            if (postDTO.Header == null)
                throw new ArgumentNullException(nameof(postDTO.Header));

            postDTO.PostId = null;

            postDTO.Published = DateTime.Now;


            var post = mapper.Map<PostDTO, Post>(postDTO);

            uow.Posts.Create(post);
        }

        public void EditPost(PostDTO postDTO)
        {
            // validation
            if (postDTO.PostId == null)
                throw new ArgumentNullException(nameof(postDTO.PostId));

            if (postDTO.PostId < 1)
                throw new ArgumentOutOfRangeException($"Post Id cannot be zero or negative - {postDTO.PostId}");

            var originalPost = uow.Posts.Get((int)postDTO.PostId);


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
                throw new ArgumentOutOfRangeException($"Post Id cannot be zero or negative - {postId}");

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
