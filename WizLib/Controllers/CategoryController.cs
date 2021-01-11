using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<Category> categories = _dbContext.Categories.AsNoTracking().ToList();
            return View(categories);
        }

        public IActionResult Upsert(int? id)
        {
            Category category = null;
            if (id == null)
                category = new Category();
            else
                category = _dbContext.Categories.FirstOrDefault(c => c.Category_Id == id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Category_Id == 0)
                {
                    // This is create
                    _dbContext.Categories.Add(category);
                }
                else
                {
                    // This is update
                    _dbContext.Categories.Update(category);
                }
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Category_Id == id);
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateMultiple2()
        {
            for (int i = 0; i < 2; i++)
            {
                _dbContext.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }

            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateMultiple5()
        {
            for (int i = 0; i < 5; i++)
            {
                _dbContext.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }

            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveMultiple2()
        {
            IEnumerable<Category> categories = _dbContext.Categories.
                OrderByDescending(c => c.Category_Id)
                .Take(2).ToList();

            _dbContext.RemoveRange(categories);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveMultiple5()
        {
            IEnumerable<Category> categories = _dbContext.Categories.
                OrderByDescending(c => c.Category_Id)
                .Take(5).ToList();

            _dbContext.RemoveRange(categories);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
