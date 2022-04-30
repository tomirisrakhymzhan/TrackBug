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
    public class ProjectMemberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectMemberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IActionResult AddMember(int? projectId)
        {
            IEnumerable<ProjectMember> projectMembers = _unitOfWork.ProjectMember.GetAll(u => u.ProjectId == projectId, includeProperties: "ApplicationUser");
            List<ApplicationUser> projectApplicationUsers = new();
            foreach (var member in projectMembers)
            {
                projectApplicationUsers.Add(member.ApplicationUser);
            }
            ProjectMemberAddVM projectMemberAddVM = new();
            projectMemberAddVM.Project = _unitOfWork.Project.GetFirstOrDefault(u => u.Id == projectId);
            projectMemberAddVM.ProjectMember = new() { ProjectId = (int)projectId };
            projectMemberAddVM.returnUrl = Request.Headers["Referer"].ToString();
            if (projectApplicationUsers != null)
            {
               
                projectMemberAddVM.ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Except(projectApplicationUsers).Select(
                u => new SelectListItem
                {
                    Text = u.Email,
                    Value = u.Id.ToString()
                });
            }
            return View(projectMemberAddVM);
            //return PartialView("_AddProjectMember.cshtml", projectMemberAddVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMember(ProjectMemberAddVM projectMemberAddVM)
        {

            if (ModelState.IsValid)
            {
                if (projectMemberAddVM.ProjectMember.ApplicationUserId != null)
                {
                    _unitOfWork.ProjectMember.Add(projectMemberAddVM.ProjectMember);
                    _unitOfWork.Save();
                    TempData["success"] = "Project member added successfully";
                    return Redirect(projectMemberAddVM.returnUrl);
                }
                TempData["error"] = "Error, project member was not selected...";
                return Redirect(projectMemberAddVM.returnUrl);

            }
            TempData["error"] = "Error, project member is invalid...";
            return Redirect(projectMemberAddVM.returnUrl);
        }



        #region API CALLS
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var obj = await _unitOfWork.ProjectMember.GetFirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.ProjectMember.Remove(obj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Project member delete was successful" });

        }

        //public JsonResult AddMember()
        //{
        //    string status = "success";
        //    try
        //    {
        //        //adding project member logic goes here
        //    }
        //    catch (Exception ex)
        //    {
        //        status = ex.Message;
        //    }
        //    return Json(status, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}
