using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackBug.DataAccess;
using TrackBug.Models;
using TrackBug.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackBug.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class PriorityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriorityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Priority.GetAllAsync());
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Priority priority = new();


            if (id == null || id == 0)
            {
                return View(priority);
            }
            else
            {
                // update product
                priority = _unitOfWork.Priority.GetFirstOrDefault(u => u.Id == id);
                return View(priority);
            }

            //return View(productVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Priority obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Priority.Add(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Priority created successfully";
                }
                else
                {
                    _unitOfWork.Priority.Update(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Priority updated successfully";
                }

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var priorityList = await _unitOfWork.Priority.GetAllAsync();
            return Json(new { data = priorityList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var obj = await _unitOfWork.Priority.GetFirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.Priority.Remove(obj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Priority delete was successful" });

        }
        #endregion


    }
}
