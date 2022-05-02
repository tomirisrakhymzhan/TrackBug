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
using Microsoft.AspNetCore.Authorization;

namespace TrackBug.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize]
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
            List<ProjectCardVM> userProjectCards = new();
            IEnumerable<Project> curUserProjectList = await _unitOfWork.Project.GetAllAsync(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            foreach(var project in curUserProjectList)
            {
                int numMembers = _unitOfWork.ProjectMember.GetAll(u=>u.ProjectId==project.Id).Count();

                
                int numOpenTickets = _unitOfWork.Ticket.GetAll(u => u.ProjectId == project.Id && u.Status.Title=="Open").Count();
                userProjectCards.Add(new()
                {
                    Project = project,
                    NumMembers = numMembers,
                    NumOpenTickets = numOpenTickets
                });
            }
            return View(userProjectCards);
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
                if (obj.Id == 0)//create new project
                {
                    obj.ApplicationUserId = claim.Value;
                    obj.Description = HtmlUtilities.HtmlToPlainText(obj.Description);
                    _unitOfWork.Project.Add(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Project created successfully";

                    ProjectMember projectMember = new()
                    {
                        ApplicationUserId = obj.ApplicationUserId,
                        ProjectId = obj.Id
                    };
                    _unitOfWork.ProjectMember.Add(projectMember);
                    await _unitOfWork.SaveAsync();
                }
                else //update existing project
                {
                    obj.ApplicationUserId = claim.Value;
                    obj.Description = HtmlUtilities.HtmlToPlainText(obj.Description);
                    _unitOfWork.Project.Update(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Project updated successfully";
                }

                //return Redirect(returnUrl);
                return RedirectToAction("Details", obj.Id);
            }
            return View(obj);
        }

       
        public async Task<IActionResult> Details(int? id, string searchString, int? ticketPriority, int? ticketStatus)
        {
            //get tickets of the current project
            var tickets = _unitOfWork.Ticket.GetAll(u => u.ProjectId == id, includeProperties: "Project,Priority,Status,ApplicationUser");
            //apply search
            if (!string.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(u=>u.Title.Contains(searchString));
            }
            //apply priority filter if any
            if (ticketPriority!=null)
            {
                tickets = tickets.Where(u=>u.Priority.Id==ticketPriority);
            }
            //apply status filter if any
            if (ticketStatus!=null)
            {
                tickets = tickets.Where(u => u.Status.Id == ticketStatus);
            }

            ProjectVM projectVM = new()
            {
                Project = await _unitOfWork.Project.GetFirstOrDefaultAsync(u => u.Id == id, includeProperties: "ApplicationUser"),
                ProjectMembers = await _unitOfWork.ProjectMember.GetAllAsync(u => u.ProjectId == id, includeProperties: "Project,ApplicationUser"),
                PriorityList = _unitOfWork.Priority.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Title,
                    Value = i.Id.ToString()
                }),
                StatusList = _unitOfWork.Status.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Title,
                    Value = i.Id.ToString()
                }),
                Tickets = tickets,
                NumOpenTickets = _unitOfWork.Ticket.GetAll(u => u.ProjectId == id && u.Status.Title == "Open").Count(),
                NumClosedTickets = _unitOfWork.Ticket.GetAll(u => u.ProjectId == id && u.Status.Title == "Closed").Count(),

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
