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
            // First user is always admin:
            if (database.Users.Count() == 0)
            {
                user.UserRole = UserRoleType.Admin;
            }

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
            return database.Users.FirstOrDefault(u => u.UserId == id);
        }
        public User Get(string username, string password)
        {
            return database.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public IEnumerable<User> GetAll()
        {
            return from u in database.Users
                   orderby u.Username
                   select u;
        }

        public void Update(User user, bool updatepassword = false)
        {
            User existing = database.Users.Find(user.UserId);
            if (existing != null)
            {
                if (updatepassword)
                {
                    existing.Password = user.Password;
                }

                existing.Username = user.Username;
                existing.FirstName = user.FirstName;
                existing.LastName = user.LastName;
                existing.UserRole = user.UserRole;
                existing.TimeZonePreference = user.TimeZonePreference;
                existing.DiscordId = user.DiscordId;
                database.SaveChanges();
            }
        }

        public void UpdateLoginDate(User user, DateTime date)
        {
            User existing = database.Users.Find(user.UserId);
            if (existing != null)
            {
                existing.LastLogin = date;
                database.SaveChanges();
            }
        }
    }
}
