using Forum.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.Interfaces
{
    public interface IPostsService : IDisposable
    {
        PostDTO GetPost(int postId);

        CommentDTO AddComment(CommentDTO commentDTO);

        void DeleteCommnet(int commentId);

        IEnumerable<CommentDTO> GetPostComments(int postId);

        IEnumerable<PostDTO> GetPostsAtPage(int page);

        PostDTO CreatePost(PostDTO postDTO);

        void EditPost(PostDTO postDTO);

        void DeletePost(int postId);

        void SaveChanges();
    }
}
