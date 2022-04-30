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
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class StatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Status.GetAllAsync());
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Status status = new();


            if (id == null || id == 0)
            {
                return View(status);
            }
            else
            {
                // update product
                status = _unitOfWork.Status.GetFirstOrDefault(u => u.Id == id);
                return View(status);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Status obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Status.Add(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Status created successfully";
                }
                else
                {
                    _unitOfWork.Status.Update(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Status updated successfully";
                }

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statusList = await _unitOfWork.Status.GetAllAsync();
            return Json(new { data = statusList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var obj = await _unitOfWork.Status.GetFirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.Status.Remove(obj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Status delete was successful" });

        }
        #endregion


    }
}
