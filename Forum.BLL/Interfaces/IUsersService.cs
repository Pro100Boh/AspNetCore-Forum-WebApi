using Forum.BLL.DTO;
using Forum.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.Interfaces
{
    public interface IUsersService : IDisposable
    {
        UserDTO GetById(int id);

        UserDTO Authenticate(string username, string password);

        UserDTO Create(UserDTO user, string password);

        void Update(UserDTO user, string password, string newPassword = null);

        void Delete(int id);

        void SaveChanges();
    }
}
