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
    public class EmployeeTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.EmployeeType.GetAllAsync());
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            EmployeeType employeeType = new();


            if (id == null || id == 0)
            {
                return View(employeeType);
            }
            else
            {
                // update product
                employeeType = _unitOfWork.EmployeeType.GetFirstOrDefault(u => u.Id == id);
                return View(employeeType);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(EmployeeType obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.EmployeeType.Add(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Employee type created successfully";
                }
                else
                {
                    _unitOfWork.EmployeeType.Update(obj);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Employee type updated successfully";
                }

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employeeTypes = await _unitOfWork.EmployeeType.GetAllAsync();
            return Json(new { data = employeeTypes });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var obj = await _unitOfWork.EmployeeType.GetFirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _unitOfWork.EmployeeType.Remove(obj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Employee type delete was successful" });

        }
        #endregion


    }
}
