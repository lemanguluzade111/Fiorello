
using Fiorello.DAL;
using Fiorello.Helper;
using Fiorello.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products.Include(x=>x.Category).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
           ViewBag.Categories= await _db.Categories.ToListAsync();
            return View(); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product,int categoryId)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();

            #region Exist Item
            bool isExist = await _db.Products.AnyAsync(x => x.Name == product.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This product is already exist !");
                return View();
            }
            #endregion

            #region SAVE IMAGE
            if (product.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image cannot be null");
                return View();
            }
            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image type");
                return View();
            }
            if (product.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "MAX 1 Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            product.Image = await product.Photo.SaveFileAsync(folder);
            #endregion

            product.CategoryId= categoryId;
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //////////TASK - Update,Delete & Detail/////////////////////
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            if (dbProduct.IsDeactive)
            {
                dbProduct.IsDeactive = false;
            }
            else
            {
                dbProduct.IsDeactive = true;
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
            Product dbProduct = await _db.Products.Include(x => x.ProductDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(dbProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Product product,int categoryId)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.Include(x=>x.ProductDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            ViewBag.Categories = await _db.Categories.ToListAsync();
            #region Exist Item
            bool isExist = await _db.Products.AnyAsync(x => x.Name == product.Name && x.Id!=id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This product is already exist !");
                return View();
            }
            #endregion

            #region SAVE IMAGE
            if (product.Photo != null)
            {
                if (!product.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image type");
                    return View();
                }
                if (product.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "MAX 1 Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                dbProduct.Image = await product.Photo.SaveFileAsync(folder);
            }

            #endregion

            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.CategoryId = categoryId;
            dbProduct.ProductDetail.Description = product.ProductDetail.Description;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
    }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            

            return View(dbProduct);
        }


    }
    }
