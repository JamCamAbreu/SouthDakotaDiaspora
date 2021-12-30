using Data.Models;
using Data.Services;
using Site.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class ProjectsController : Controller
    {
        IProjectData db;
        public ProjectsController(IProjectData database)
        {
            this.db = database;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Project> rawProjects = db.GetAll();
            List<ProjectInfo> Projects = new List<ProjectInfo>();
            foreach (Project rawProject in rawProjects)
            {
                Projects.Add(new ProjectInfo(rawProject));
            }
            return View(Projects);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Project existing = db.Get(id);
            if (existing == null)
            {
                return View("NotFound");
            }
            return View(existing);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Create", rc = "Projects" });
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project Project)
        {
            if (ModelState.IsValid)
            {
                db.Add(Project);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Edit", rc = "Projects", rid = id });
            }

            var model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project Project)
        {
            var existing = db.Get(Project.ActivityId);
            if (existing == null)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                db.Update(Project);
                Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Successfully updated Project: {Project.Name}");
                return RedirectToAction("Index");
            }
            return View(Project);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!Helpers.GlobalMethods.IsLoggedIn(this.Session))
            {
                return RedirectToAction("Login", "Home", new { ra = "Index", rc = "Projects" });
            }

            Project model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form)
        {
            Project model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }

            db.Delete(id);
            Helpers.GlobalMethods.AddConfirmationMessage(this.Session, $"Project '{model.Name}' was successfully removed.");
            return RedirectToAction("Index");
        }
    }
}