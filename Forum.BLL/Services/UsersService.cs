using AutoMapper;
using Forum.BLL.DTO;
using Forum.BLL.Exceptions;
using Forum.BLL.Interfaces;
using Forum.DAL.Entities;
using Forum.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Forum.BLL.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IUnitOfWork uow;

        private readonly IMapper mapper;

        public UsersService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public UserDTO Authenticate(string username, string password)
        {
            // validation
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username required", nameof(username));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password required", nameof(password));

            var user = uow.Users.GetAll().SingleOrDefault(u => u.Username == username);

            // check if username exists
            if (user == null)
                throw new NotFoundInDbException("User not found");

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new ArgumentException("Wrong password", nameof(password));

            // authentication successful

            var userDTO = mapper.Map<User, UserDTO>(user);

            return userDTO;
        }

        public UserDTO GetById(int userId)
        {
            if (userId < 1)
                throw new ArgumentOutOfRangeException($"User Id cannot be zero or negative - {userId}");

            var user = uow.Users.Get(userId);

            // check if user exists
            if (user == null)
                throw new NotFoundInDbException("User not found");

            var userDTO = mapper.Map<User, UserDTO>(user);

            return userDTO;
        }

        public UserDTO Create(UserDTO userDTO, string password)
        {
            // validation
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO));
            if (!IsValidPassword(password))
                throw new ArgumentException("Invalid password", nameof(password));
            if (!IsValidUsername(userDTO.Username))
                throw new ArgumentException("Invalid username", nameof(userDTO.Username));
            if (!IsFreeUsername(userDTO.Username))
                throw new ArgumentException($"Username {userDTO.Username} is already taken");

            // UserId is database generated
            userDTO.UserId = null;

            // Set registration date as current date
            userDTO.RegistrationDate = DateTime.Now;

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = mapper.Map<UserDTO, User>(userDTO);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            uow.Users.Create(user);

            return userDTO;
        }

        public void Update(UserDTO userDTO, string password = null)
        {
            // validation
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO));

            if (userDTO.UserId == null)
                throw new ArgumentNullException(nameof(userDTO.UserId));

            if (userDTO.UserId < 1)
                throw new ArgumentException($"Id cannot be zero or negative: {userDTO.UserId}", nameof(userDTO.UserId));

            var originalUser = uow.Users.Get((int)userDTO.UserId);

            userDTO.RegistrationDate = originalUser.RegistrationDate;

            if (userDTO.Username == null)
                userDTO.Username = originalUser.Username;
            else if (!IsValidUsername(userDTO.Username))
                throw new ArgumentException("Invalid username", nameof(userDTO.Username));
            else if (!IsFreeUsername(userDTO.Username))
                throw new ArgumentException($"Username {userDTO.Username} is already taken");

            userDTO.UserId = null;

            var user = mapper.Map<UserDTO, User>(userDTO);

            // update password if it was entered
            if (password != null)
            {
                if (!IsValidPassword(password))
                    throw new ArgumentException("Invalid password");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            uow.Users.Update(user);
        }

        public void Delete(int userId)
        {
            if (userId < 1)
                throw new ArgumentOutOfRangeException($"Id cannot be zero or negative: {userId}", nameof(userId)); // id exc

            uow.Users.Delete(userId);

        }

        public void Dispose()
        {
            uow.Dispose();
        }

        public void SaveChanges()
        {
            uow.Save();
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        private static bool IsValidUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username is required", nameof(username));

            // Only alphabetic characters or digits, and from 3-50 characters in length.
            return Regex.IsMatch(username, @"^[a-zA-Z0-9]{3,50}$");
        }

        private static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required", nameof(password));

            // This requires at least one digit, at least one alphabetic character, no special characters, and from 6-15 characters in length.
            return Regex.IsMatch(password, @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$");
        }

        private bool IsFreeUsername(string username)
        {
            return uow.Users.GetAll().Any(u => u.Username == username);
        }

    }
}
