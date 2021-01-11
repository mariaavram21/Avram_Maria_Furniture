using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Avram_Maria_Furniture.Data;
using Avram_Maria_Furniture.Models;
using Microsoft.AspNetCore.Authorization;

namespace Avram_Maria_Furniture.Controllers
{
    public class FurnituresController : Controller
    {
        private readonly FurnitureStoreContext _context;

        public FurnituresController(FurnitureStoreContext context)
        {
            _context = context;
        }

        // GET: Furnitures
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var furniture = from b in _context.Furnitures
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                furniture = furniture.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    furniture = furniture.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    furniture = furniture.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    furniture = furniture.OrderByDescending(b => b.Price);
                    break;
                default:
                    furniture = furniture.OrderBy(b => b.Title);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Furniture>.CreateAsync(furniture.AsNoTracking(), pageNumber ??
           1, pageSize));
        }

        // GET: Furnitures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniture = await _context.Furnitures
                 .Include(s => s.Orders)
                 .ThenInclude(e => e.Customer)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(m => m.ID == id);
            if (furniture == null)
            {
                return NotFound();
            }

            return View(furniture);
        }

        // GET: Furnitures/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Furnitures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Title,Description,Color,Price")] Furniture furniture)
        {
            try
            {
                if (ModelState.IsValid)
            {
                _context.Add(furniture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(furniture);
        }

        // GET: Furnitures/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniture = await _context.Furnitures.FindAsync(id);
            if (furniture == null)
            {
                return NotFound();
            }
            return View(furniture);
        }

        // POST: Furnitures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditFurniture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnitureToUpdate = await _context.Furnitures.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Furniture>(
            furnitureToUpdate,
            "",
            s => s.Title, s => s.Description, s => s.Color, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(furnitureToUpdate);
        }

        // GET: Furnitures/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniture = await _context.Furnitures
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (furniture == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(furniture);
        }

        // POST: Furnitures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var furniture = await _context.Furnitures.FindAsync(id);
            if (furniture == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Furnitures.Remove(furniture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool FurnitureExists(int id)
        {
            return _context.Furnitures.Any(e => e.ID == id);
        }
    }
}
