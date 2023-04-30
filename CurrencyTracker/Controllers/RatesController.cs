using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CurrencyTracker.Data;
using CurrencyTracker.Models;
using CurrencyTracker.Services;


namespace CurrencyTracker.Controllers
{
    public class RatesController : Controller
    {
        private readonly CurrencyTrackerContext _context;

        public RatesController(CurrencyTrackerContext context)
        {
            _context = context;
        }


        public IActionResult Create()
        {

            (decimal exchangeRateVenta, decimal exchangeRateCompra) = Hacienda.ApiHacienda();

            var targetDate = DateTime.Now.Date;

            var presentDate = _context.Rates
                .Where(r => r.Date == targetDate)
                .ToList();
            if (presentDate.Count == 1)

            {
                TempData["AlertMessage"] = "A rate for today already exists.";

                return RedirectToAction("Index", "Home");

            }
            else
            {
                var newRate = new Rates
                {
                    Date = DateTime.Now,
                    USD = 1,
                    CrVenta = exchangeRateVenta,
                    CrCompra = exchangeRateCompra


                };


                _context.Rates.Add(newRate);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");

            }


        }


    }
}
