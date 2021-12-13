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
    }
}
