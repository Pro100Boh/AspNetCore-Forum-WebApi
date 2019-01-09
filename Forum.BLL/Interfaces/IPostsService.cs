using Forum.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.Interfaces
{
    public interface IPostsService : IDisposable
    {
        PostDTO GetPost(int postId);

        IEnumerable<PostDTO> GetPostsAtPage(int page);

        void CreatePost(PostDTO postDTO);

        void EditPost(PostDTO postDTO);

        void DeletePost(int postId);

        void SaveChanges();
    }
}
