using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class AuhtorController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AuhtorController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<Author> authors = _dbContext.Authors.ToList();
            return View(authors);
        }

        public IActionResult Upsert(int? id)
        {
            Author author = null;
            if (id == null)
                author = new Author();
            else
                author = _dbContext.Authors.FirstOrDefault(c => c.Author_Id == id);
            return View(author);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Author author)
        {
            if (ModelState.IsValid)
            {
                if (author.Author_Id == 0)
                {
                    // This is create
                    _dbContext.Authors.Add(author);
                }
                else
                {
                    // This is update
                    _dbContext.Authors.Update(author);
                }
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }

        public IActionResult Delete(int id)
        {
            var author = _dbContext.Authors.FirstOrDefault(p => p.Author_Id == id);
            _dbContext.Authors.Remove(author);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
