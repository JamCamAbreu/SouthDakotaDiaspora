using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SqlProjectData : IProjectData
    {
        private readonly SouthDakotaDiasporaDbContext database;
        public SqlProjectData(SouthDakotaDiasporaDbContext db)
        {
            database = db;
        }

        public void Add(Project project)
        {
            database.Activities.Add(project);
            database.SaveChanges();
        }

        public Project Get(int id)
        {
            return database.Projects.FirstOrDefault(g => g.ActivityId == id);
        }

        public IEnumerable<Project> GetAll()
        {
            return database.Projects.OrderBy(g => g.Name);
        }

        public void Update(Project project)
        {
            Project existing = this.Get(project.ActivityId);
            if (existing != null)
            {
                existing.Name = project.Name;
                existing.Description = project.Description;
                existing.DateStarted = project.DateStarted;
                existing.ReleaseDate = project.ReleaseDate;
                existing.ProjectOwner = project.ProjectOwner;
                existing.ProjectDevelopers = project.ProjectDevelopers;
                existing.WebsiteUrl = project.WebsiteUrl;
                existing.Platform = project.Platform;
                database.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Project project = this.Get(id);
            if (project != null)
            {
                database.Activities.Remove(project);
                database.SaveChanges();
            }
        }
    }
}
