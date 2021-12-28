using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IActivityData
    {
        IEnumerable<Activity> GetAll();
        Activity Get(int id);
    }
}
