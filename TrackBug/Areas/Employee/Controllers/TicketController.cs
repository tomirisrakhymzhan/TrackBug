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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackBug.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    IEnumerable<Project> curUserProjectList = await _unitOfWork.Project.GetAllAsync(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");

        //    return View(curUserProjectList);
        //}
        
        public IActionResult Upsert(int? id, int? projectId)
        {
            TicketUpsertVM ticketUpsertVM = new()
            {
                Ticket = new(),
                PriorityList = _unitOfWork.Priority.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Title,
                    Value = i.Id.ToString()
                }),
                StatusList = _unitOfWork.Status.GetAll().Select(
                       i => new SelectListItem
                       {
                           Text = i.Title,
                           Value = i.Id.ToString()
                       })
            };

            ViewBag.returnUrl = Request.Headers["Referer"].ToString();//save return url to redirect to after upserting is finished

            if (id == null || id == 0)//create new ticket
            {
                if (projectId != null || projectId != 0)
                {
                    ticketUpsertVM.Ticket.ProjectId = (int)projectId;
                }
                return View(ticketUpsertVM);
            }
            else // update ticket
            {
                ticketUpsertVM.Ticket = _unitOfWork.Ticket.GetFirstOrDefault(u => u.Id == id && u.ProjectId == projectId, includeProperties: "Project,ApplicationUser,Priority,Status");
                return View(ticketUpsertVM);
            }

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TicketUpsertVM obj, string returnUrl)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                if (obj.Ticket.Id == 0)
                {
                    obj.Ticket.ApplicationUserId = claim.Value;

                    var project = _unitOfWork.Project.GetFirstOrDefault(u => u.Id == obj.Ticket.ProjectId);
                    project.NumOfTickets++;
                    _unitOfWork.Project.Update(project);
                    await _unitOfWork.SaveAsync();

                    _unitOfWork.Ticket.Add(obj.Ticket);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Ticket created successfully";
                    return Redirect(returnUrl);
                    //return RedirectToAction("Details", "Project", obj.Ticket.ProjectId);
                }
                else
                {
                    obj.Ticket.ApplicationUserId = claim.Value;
                    var project = _unitOfWork.Project.GetFirstOrDefault(u => u.Id == obj.Ticket.ProjectId);
                    var statusTitle = _unitOfWork.Status.GetFirstOrDefault(u => u.Id == obj.Ticket.StatusID).Title;
                    if (statusTitle=="Closed")
                    {
                        project.NumOfClosedTickets++;
                    }
                    _unitOfWork.Project.Update(project);
                    await _unitOfWork.SaveAsync();

                    _unitOfWork.Ticket.Update(obj.Ticket);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Ticket updated successfully";
                    return Redirect(returnUrl);
                }

                
            }
            return View(obj);
        }
        
        // GET: Ticket/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _unitOfWork.Ticket
                .GetFirstOrDefaultAsync(u => u.Id == id,includeProperties: "Project,ApplicationUser,Priority,Status");
            if (ticket == null)
            {
                return NotFound();
            }
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int projectId, string returnUrl)
        {
            var ticket = await _unitOfWork.Ticket
                .GetFirstOrDefaultAsync(u => u.Id == id && u.ProjectId == projectId, includeProperties: "Project");
            ticket.Project.NumOfTickets--;
            _unitOfWork.Ticket.Remove(ticket);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Ticket deleted successfully";
            return Redirect(returnUrl);
            //return RedirectToAction("Details", "Project", projectId);
        }

        public async Task<IActionResult> Details(int? id, int? projectId)
        {
            TicketVM ticketVM = new()
            {
                Ticket = await _unitOfWork.Ticket.GetFirstOrDefaultAsync(u => u.Id == id, includeProperties: "Project,Priority,Status,ApplicationUser"),
                TicketMembers = await _unitOfWork.TicketMember.GetAllAsync(u => u.Ticket.ProjectId == projectId, includeProperties: "Ticket,ApplicationUser")
            };
            ticketVM.Ticket.CreatedDateTimeAsString = ticketVM.Ticket.CreatedDateTime.ToString("ddd, dd MMM yyyy, HH:mm tt");
            ticketVM.Ticket.DueDateTimeAsString = ticketVM.Ticket.DueDateTime.ToString("ddd, dd MMM yyyy, HH:mm tt");
            foreach (var member in ticketVM.TicketMembers)
            {
                member.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == member.ApplicationUserId, includeProperties: "EmployeeType");
            }

            return View(ticketVM);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ticketsList = await _unitOfWork.Ticket.GetAllAsync(includeProperties: "Priority,Status,ApplicationUser");
            return Json(new { data = ticketsList });
        }
        
        #endregion
    }
}
