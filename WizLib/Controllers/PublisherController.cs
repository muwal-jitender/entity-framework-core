using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PublisherController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<Publisher> publihser = _dbContext.Publishers.ToList();
            return View(publihser);
        }

        public IActionResult Upsert(int? id)
        {
            Publisher publisher = null;
            if (id == null)
                publisher = new Publisher();
            else
                publisher = _dbContext.Publishers.FirstOrDefault(c => c.Publisher_Id == id);
            return View(publisher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                if (publisher.Publisher_Id == 0)
                {
                    // This is create
                    _dbContext.Publishers.Add(publisher);
                }
                else
                {
                    // This is update
                    _dbContext.Publishers.Update(publisher);
                }
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(publisher);
        }

        public IActionResult Delete(int id)
        {
            var publisher = _dbContext.Publishers.FirstOrDefault(p => p.Publisher_Id == id);
            _dbContext.Publishers.Remove(publisher);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }       
    }
}
