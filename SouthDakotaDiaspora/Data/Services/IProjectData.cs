using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IProjectData
    {
        IEnumerable<Project> GetAll();
        Project Get(int id);
        void Add(Project show);
        void Update(Project show);
        void Delete(int id);
    }
}
