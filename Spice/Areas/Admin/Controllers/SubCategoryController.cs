using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<IActionResult> Index()
        {
            //var url = Url.RequestContext.RouteData.Values["id"];
            int categoryId = int.Parse(this.RouteData.Values["id"].ToString());
            var subCategories = await _db.SubCategory.Include(s => categoryId).ToListAsync();

            return View(subCategories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategory.Any(x => x.Name == subcategory.Name);
                if (doesSubCategoryExists)
                {
                    ViewBag.ErrorMessage = "Sub-Category already exists";
                    //return View("Create");
                }
                else
                {
                    _db.SubCategory.Add(subcategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            }
            return View(subcategory);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subcategory = await _db.SubCategory.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            return View(subcategory);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                _db.Update(subcategory);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(subcategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subcategory = await _db.SubCategory.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }

            return View(subcategory);

        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var subcategory = await _db.SubCategory.FindAsync(id);

            if (subcategory == null)
            {
                return View();
            }
            _db.SubCategory.Remove(subcategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}