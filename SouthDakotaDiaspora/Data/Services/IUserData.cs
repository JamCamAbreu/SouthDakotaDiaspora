using Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IUserData
    {
        IEnumerable<User> GetAll();
        User Get(string username, string password);
        User Get(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}
