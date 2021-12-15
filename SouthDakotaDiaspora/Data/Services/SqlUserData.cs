using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SqlUserData : IUserData
    {
        private readonly SouthDakotaDiasporaDbContext database;
        public SqlUserData(SouthDakotaDiasporaDbContext db)
        {
            this.database = db;
        }

        public void Add(User user)
        {
            // Must be updated explicitely in SQL for admin access
            user.UserRole = UserRoleType.Contributor; 

            database.Users.Add(user);
            database.SaveChanges();
        }

        public void Delete(int id)
        {
            User user = database.Users.Find(id);
            if (user != null)
            {
                database.Users.Remove(user);
                database.SaveChanges();
            }
        }

        public User Get(int id)
        {
            return database.Users.FirstOrDefault(u => u.Id == id);
        }
        public User Get(string username, string password)
        {
            return database.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public IEnumerable<User> GetAll()
        {
            return from u in database.Users
                   orderby u.LastName
                   select u;
        }

        public void Update(User user)
        {
            User existing = database.Users.Find(user.Id);
            if (existing != null)
            {
                existing.Username = user.Username;
                existing.Password = user.Password;
                existing.FirstName = user.FirstName;
                existing.LastName = user.LastName;
                existing.UserRole = user.UserRole;
                database.SaveChanges();
            }
        }
    }
}
