﻿using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM=new HomeVM 
            {
                Products= await _db.Products.Where(x => !x.IsDeactive).ToListAsync(),
             
                Categories = await _db.Categories.Where(x => !x.IsDeactive).ToListAsync(),
                Sliders = await _db.Sliders.ToListAsync(),
               SliderInfo = await _db.SliderInfo.FirstOrDefaultAsync(),

            };
            return View(homeVM);
        }

        

       
        public IActionResult Error()
        {
            return View();
        }
    }
}
