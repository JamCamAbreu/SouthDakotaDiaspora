using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IShowData
    {
        IEnumerable<Show> GetAll();
        Show Get(int id);
        void Add(Show show);
        void Update(Show show);
        void Delete(int id);
    }
}
