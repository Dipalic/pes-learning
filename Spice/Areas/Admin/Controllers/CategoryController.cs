using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        //public JsonResult IsCategoryNameExist(string CategoryName, int? Id)
        //{
        //    var validateName = _db.Category.FirstOrDefault
        //                        (x => x.Name == CategoryName && x.Id != Id);
        //    if (validateName != null)
        //    {
        //        return Json(false);
        //    }
        //    else
        //    {
        //        return Json(true);
        //    }
        //}
        public async Task<IActionResult> Index()
        {
            return View(await _db.Category.OrderBy(m => m.Name).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if(ModelState.IsValid)
            {
                var doesCategoryExists = _db.Category.Any(x => x.Name == category.Name);
                if (doesCategoryExists)
                {
                    ViewBag.ErrorMessage="Category already exists";
                    //return View("Create");
                }
                else
                {
                    _db.Category.Add(category);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                
            }
            return View(category);
        }

         public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var category = await _db.Category.FindAsync(id);
            if(category==null)
            {
                return NotFound();
            }
            return View(category);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Update(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            
            return View(category);

        }

       

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return View();
            }
            _db.Category.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}