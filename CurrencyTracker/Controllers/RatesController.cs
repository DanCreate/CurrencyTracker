using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CurrencyTracker.Data;
using CurrencyTracker.Models;

namespace CurrencyTracker.Controllers
{
    public class RatesController : Controller
    {
        private readonly CurrencyTrackerContext _context;

        public RatesController(CurrencyTrackerContext context)
        {
            _context = context;
        }

        // GET: Rates
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rates.ToListAsync());
        }

        // GET: Rates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rates = await _context.Rates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rates == null)
            {
                return NotFound();
            }

            return View(rates);
        }

        // GET: Rates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,USD,CrVenta,CrCompra")] Rates rates)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rates);
        }

        // GET: Rates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rates = await _context.Rates.FindAsync(id);
            if (rates == null)
            {
                return NotFound();
            }
            return View(rates);
        }

        // POST: Rates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,USD,CrVenta,CrCompra")] Rates rates)
        {
            if (id != rates.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatesExists(rates.Id))
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
            return View(rates);
        }

        // GET: Rates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rates = await _context.Rates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rates == null)
            {
                return NotFound();
            }

            return View(rates);
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rates == null)
            {
                return Problem("Entity set 'CurrencyTrackerContext.Rates'  is null.");
            }
            var rates = await _context.Rates.FindAsync(id);
            if (rates != null)
            {
                _context.Rates.Remove(rates);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatesExists(int id)
        {
          return _context.Rates.Any(e => e.Id == id);
        }
    }
}
