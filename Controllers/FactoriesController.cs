using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Avram_Maria_Furniture.Data;
using Avram_Maria_Furniture.Models;
using Avram_Maria_Furniture.Models.FurnitureStoreViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Avram_Maria_Furniture.Controllers
{
    public class FactoriesController : Controller
    {
        private readonly FurnitureStoreContext _context;

        public FactoriesController(FurnitureStoreContext context)
        {
            _context = context;
        }

        // GET: Factories
        public async Task<IActionResult> Index(int? id, int? furnitureID)
        {
            var viewModel = new FactoryIndexData();
            viewModel.Factories = await _context.Factories
            .Include(i => i.ManufacturedFurniture)
            .ThenInclude(i => i.Furniture)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.FactoryName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["FactoryID"] = id.Value;
                Factory factory = viewModel.Factories.Where(
                i => i.ID == id.Value).Single();
                viewModel.Furnitures = factory.ManufacturedFurniture.Select(s => s.Furniture);
            }
            if (furnitureID != null)
            {
                ViewData["FurnitureID"] = furnitureID.Value;
                viewModel.Orders = viewModel.Furnitures.Where(
                x => x.ID == furnitureID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Factories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (factory == null)
            {
                return NotFound();
            }

            return View(factory);
        }

        // GET: Factories/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Factories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,FactoryName,Address")] Factory factory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(factory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(factory);
        }

        // GET: Factories/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factories
                .Include(i => i.ManufacturedFurniture).ThenInclude(i => i.Furniture)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (factory == null)
            {
                return NotFound();
            }
            PopulateManufacturedFurnitureData(factory);
            return View(factory);
        }
        private void PopulateManufacturedFurnitureData(Factory factory)
        {
            var allFurnitures = _context.Furnitures;
            var manufacturedFurnitures = new HashSet<int>(factory.ManufacturedFurniture.Select(c => c.FurnitureID));
            var viewModel = new List<ManufacturedFurnitureData>();
            foreach (var furniture in allFurnitures)
            {
                viewModel.Add(new ManufacturedFurnitureData
                {
                    FurnitureID = furniture.ID,
                    Title = furniture.Title,
                    IsDone = manufacturedFurnitures.Contains(furniture.ID)
                });
            }
            ViewData["Furnitures"] = viewModel;
        }

        // POST: Factories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int? id, string[] selectedFurnitures)
        {
            if (id == null)
            {
                return NotFound();
            }
            var factoryToUpdate = await _context.Factories
            .Include(i => i.ManufacturedFurniture)
            .ThenInclude(i => i.Furniture)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Factory>(factoryToUpdate,"",i => i.FactoryName, i => i.Address))
            {
                UpdateManufacturedFurnitures(selectedFurnitures, factoryToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateManufacturedFurnitures(selectedFurnitures, factoryToUpdate);
            PopulateManufacturedFurnitureData(factoryToUpdate);
            return View(factoryToUpdate);
        }
        private void UpdateManufacturedFurnitures(string[] selectedFurnitures, Factory factoryToUpdate)
        {
            if (selectedFurnitures == null)
            {
                factoryToUpdate.ManufacturedFurniture = new List<ManufacturedFurniture>();
                return;
            }
            var selectedFurnituresHS = new HashSet<string>(selectedFurnitures);
            var producedFurniture = new HashSet<int>
            (factoryToUpdate.ManufacturedFurniture.Select(c => c.Furniture.ID));
            foreach (var furniture in _context.Furnitures)
            {
                if (selectedFurnituresHS.Contains(furniture.ID.ToString()))
                {
                    if (!producedFurniture.Contains(furniture.ID))
                    {
                        factoryToUpdate.ManufacturedFurniture.Add(new ManufacturedFurniture
                        {
                            FactoryID = factoryToUpdate.ID,
                            FurnitureID = furniture.ID
                        });
                    }
                }
                else
                {
                    if (producedFurniture.Contains(furniture.ID))
                    {
                        ManufacturedFurniture bookToRemove = factoryToUpdate.ManufacturedFurniture.FirstOrDefault(i
                       => i.FurnitureID == furniture.ID);
                        _context.Remove(bookToRemove);
                    }
                }
            }
        }

        // GET: Factories/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (factory == null)
            {
                return NotFound();
            }

            return View(factory);
        }

        // POST: Factories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factory = await _context.Factories.FindAsync(id);
            _context.Factories.Remove(factory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactoryExists(int id)
        {
            return _context.Factories.Any(e => e.ID == id);
        }
    }
}
