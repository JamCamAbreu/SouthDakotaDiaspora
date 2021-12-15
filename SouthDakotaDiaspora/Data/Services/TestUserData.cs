using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Services
{
    public class TestUserData : IUserData
    {
        List<User> users;
        public TestUserData()
        {
            this.users = new List<User>()
            {
                new User() { Id = 1, FirstName = "Cam", LastName = "Abreu", Username = "Jerof", Password="test123" },
                new User() { Id = 2, FirstName = "Adam", LastName = "Rezich", Username = "Takua", Password="test123" },
                new User() { Id = 3, FirstName = "Carson", LastName = "Crawford", Username = "Mcbymef", Password="test123" },
                new User() { Id = 4, FirstName = "Niki", LastName = "Abreu", Username = "Muffins", Password="test123" }
            };
        }
        public IEnumerable<User> GetAll()
        {
            return this.users.OrderBy(u => u.LastName);
        }

        public User Get(int id)
        {
            return users.FirstOrDefault(u => u.Id == id);
        }
        public User Get(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public void Add(User user)
        {
            user.Id = this.users.Max(u => u.Id) + 1;
            this.users.Add(user);
        }

        public void Delete(int id)
        {
            var user = this.Get(id);
            if (user != null)
            {
                this.users.Remove(user);
            }
        }

        public void Update(User user)
        {
            User existing = this.Get(user.Id);
            if (existing != null)
            {
                existing.Username = user.Username;
                existing.Password = user.Password;
                existing.FirstName = user.FirstName;
                existing.LastName = user.LastName;
            }
        }
    }
}
