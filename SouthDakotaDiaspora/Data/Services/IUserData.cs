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
    }
}
