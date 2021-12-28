using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SqlActivityData : IActivityData
    {
        private readonly SouthDakotaDiasporaDbContext database;
        public SqlActivityData(SouthDakotaDiasporaDbContext db)
        {
            this.database = db;
        }

        public Activity Get(int id)
        {
            return this.database.Activities.Where(a => a.ActivityId == id).FirstOrDefault();
        }

        public IEnumerable<Activity> GetAll()
        {
            return this.database.Activities.OrderBy(a => a.Name);
        }
    }
}
