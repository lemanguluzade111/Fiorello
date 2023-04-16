using Fiorello.DAL;
using Fiorello.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public CategoriesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _db.Categories.ToListAsync();
            return View(categories);
        }


        public async Task<IActionResult> Create()
        {
       
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Category category)
        {
           
            #region Exist Item
            bool isExist = await _db.Products.AnyAsync(x => x.Name == category.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This product is already exist !");
                return View();
            }
            #endregion

            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category dbCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCategory == null)
            {
                return BadRequest();
            }
            if (dbCategory.IsDeactive)
            {
                dbCategory.IsDeactive = false;
            }
            else
            {
                dbCategory.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
          
            if (id == null)
            {
                return NotFound();
            }
            Category dbCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCategory == null)
            {
                return BadRequest();
            }
            return View(dbCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Category category)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            Category dbCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCategory == null)
            {
                return BadRequest();
            }
            dbCategory.Name = category.Name;
         
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category dbCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCategory == null)
            {
                return BadRequest();
            }

            return View(dbCategory);
        }




    }
}
