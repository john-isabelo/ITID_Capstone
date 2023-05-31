using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using WeeklyTask.Models;

namespace WeeklyTask.Controllers
{
    
    public class FoodsController : Controller
    {
        private readonly FoodDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FoodsController(FoodDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Foods
        public async Task<IActionResult> Index(int id)
        {
            /*var food = _context.Foods.Find(id);
            return View(food);*/
            return View(await _context.Foods.ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Ingredients,Price,ImagePath")] Food food)
        {
            if (ModelState.IsValid)
            {

                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Food food)
        {
            if (ModelState.IsValid)
            {
                if (food.ImageFile != null)
                {
                    // Generate a unique file name for the image
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(food.ImageFile.FileName);

                    // Get the path to the wwwroot folder
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    // Combine the path to the wwwroot folder and the folder for storing images
                    string imageFolder = Path.Combine(wwwRootPath, "images");

                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    // Combine the path to the image folder and the file name
                    string filePath = Path.Combine(imageFolder, fileName);

                    // Copy the file to the image folder
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await food.ImageFile.CopyToAsync(stream);
                    }

                    // Save the path to the database
                    food.ImagePath = "/images/" + fileName;
                }

                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }





        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Ingredients,Price,ImageFile")] Food food)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // If the user selected a new image file, upload and set the ImagePath property


                    if (food.ImageFile != null)
                    {
                        // Delete the previous image file
                        string previousImagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", food.ImagePath);
                        if (System.IO.File.Exists(previousImagePath))
                        {
                            System.IO.File.Delete(previousImagePath);
                        }

                        // Upload the new image file
                        string uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "images");
                        string imageName = Guid.NewGuid().ToString() + "_" + food.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await food.ImageFile.CopyToAsync(fs);
                        fs.Close();

                        food.ImagePath = imageName;
                    }
                    else
                    {
                        // Preserve the previous image file path
                        food.ImagePath = _context.Foods.AsNoTracking().Where(f => f.Id == food.Id).Select(f => f.ImagePath).FirstOrDefault();
                    }

                    _context.Update(food);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }

        


        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Foods == null)
            {
                return Problem("Entity set 'FoodDbContext.Foods'  is null.");
            }
            var food = await _context.Foods.FindAsync(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
          return _context.Foods.Any(e => e.Id == id);
        }
    }
}
