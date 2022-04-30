using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrackBug.DataAccess.Repository.IRepository;
using TrackBug.Models;
using TrackBug.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrackBug.Utilities;

namespace TrackBug.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<Project> curUserProjectList = await _unitOfWork.Project.GetAllAsync(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");

            return View(curUserProjectList);
        }

        public IActionResult Upsert(int? id)
        {
            Project project = new();
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();//save return url to redirect to after upserting is finished

            if (id == null || id == 0)//create new project
            {
                return View(project);
            }
            else // update ticket
            {
                project = _unitOfWork.Project.GetFirstOrDefault(u => u.Id == id);
                return View(project);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Project obj, string returnUrl)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                obj.ApplicationUserId = claim.Value;
                obj.Description = HtmlUtilities.HtmlToPlainText(obj.Description);
                _unitOfWork.Project.Update(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Project created successfully";
                return Redirect(returnUrl);
            }
            return View(obj);
        }

       
        public async Task<IActionResult> Details(int? id)
        {
            ProjectVM projectVM = new()
            {
                Project = await _unitOfWork.Project.GetFirstOrDefaultAsync(u => u.Id == id, includeProperties: "ApplicationUser"),
                ProjectMembers = await _unitOfWork.ProjectMember.GetAllAsync(u=>u.ProjectId==id, includeProperties: "Project,ApplicationUser"),
                Tickets = await _unitOfWork.Ticket.GetAllAsync(u=>u.ProjectId==id, includeProperties: "Project,Priority,Status,ApplicationUser")
            };

            projectVM.Project.CreatedDateTimeAsString = projectVM.Project.CreatedDateTime.ToString("ddd, dd MMM yyyy, HH:mm tt");
            foreach (var ticket in projectVM.Tickets)
            {
                ticket.CreatedDateTimeAsString = ticket.CreatedDateTime.ToString("ddd, dd MMM yyyy, HH:mm tt");
                ticket.DueDateTimeAsString = ticket.DueDateTime.ToString("ddd, dd MMM yyyy, HH:mm tt");
                ticket.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == ticket.ApplicationUser.Id, includeProperties: "EmployeeType");
            }
            foreach (var member in projectVM.ProjectMembers)
            {
                member.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == member.ApplicationUserId, includeProperties: "EmployeeType");
            }

            return View(projectVM);
        }

        #region API CALLS

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var obj = await _unitOfWork.Project.GetFirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting project" });
            }


            _unitOfWork.Project.Remove(obj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Project member delete was successful" });

        }
        #endregion
    }
}
