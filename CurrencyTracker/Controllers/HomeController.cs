using CurrencyTracker.Data;
using CurrencyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Xml.Linq;
using CurrencyTracker.Services;

namespace CurrencyTracker.Controllers
{
    public class HomeController : Controller
    {

        private readonly CurrencyTrackerContext _context;
        public HomeController(CurrencyTrackerContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            (decimal exchangeRateVenta, decimal exchangeRateCompra) = Hacienda.ApiHacienda();
            ViewBag.VentaToday = exchangeRateVenta;
            ViewBag.CompraToday = exchangeRateCompra;


            if (TempData.ContainsKey("AlertMessage"))
            {
                ViewBag.AlertMessage = TempData["AlertMessage"];
            }

            var arrayventa = _context.Rates
                 .Select(r => r.CrVenta)
                 .Skip(Math.Max(0, _context.Rates.Count() % 7))
                 .ToList();
            ViewBag.ArrayVenta = arrayventa;
            var arraycompra = _context.Rates
                 .Select(r => r.CrCompra)
                 .Skip(Math.Max(0, _context.Rates.Count() % 7))
                 .ToList();
            ViewBag.ArrayCompra = arraycompra;

            var arrayday = _context.Rates
                 .Select(r => r.Date.DayOfWeek.ToString())
                 .Skip(Math.Max(0, _context.Rates.Count() % 7))
                 .ToList();
            ViewBag.DayOfWeek = arrayday;

            Variance();

            return View(await _context.Rates.ToListAsync());
        }


        public IActionResult Variance()
        {
            var yesterdaysDate = DateTime.Now.Date.AddDays(-1);
            var todaysDate = DateTime.Now.Date;


            var yesterdaysCompra = _context.Rates
                             .Where(r => r.Date == yesterdaysDate)
                             .Select(r => new { r.CrCompra });

            decimal yesterdaysBuy = yesterdaysCompra.FirstOrDefault()?.CrCompra ?? 0;


            var todaysCompra = _context.Rates
                           .Where(r => r.Date == todaysDate)
                           .Select(r => new { r.CrCompra });
            decimal todaysBuy = todaysCompra.FirstOrDefault()?.CrCompra ?? 0;

            if (todaysBuy == 0)

            {
                ViewBag.BuyPercentage = 'a';

            }
            else
            {


                try
                {
                    var percentageCompra = ((todaysBuy / yesterdaysBuy) * 100) - 100;
                    var percentagefinalCompra = (decimal)Math.Round(percentageCompra, 2);
                    ViewBag.BuyPercentage = percentagefinalCompra;
                }
                catch (DivideByZeroException ex)
                {

                    var percentageCompra = 0;
                    ViewBag.BuyPercentage = 0;
                }

            }

            var yesterdaysVenta = _context.Rates
                                 .Where(r => r.Date == yesterdaysDate)
                                 .Select(r => new { r.CrVenta });

            decimal yesterdaysSell = yesterdaysVenta.FirstOrDefault()?.CrVenta ?? 0;


            var todaysVenta = _context.Rates
                           .Where(r => r.Date == todaysDate)
                           .Select(r => new { r.CrVenta });

            decimal todaysSell = todaysVenta.FirstOrDefault()?.CrVenta ?? 0;

            if (todaysSell == 0)

            {
                ViewBag.SellPercentage = 'a';

            }
            else
            {

                try
                {
                    var percentageVenta = ((todaysSell / yesterdaysSell) * 100) - 100;

                    var percentagefinalVenta = (decimal)Math.Round(percentageVenta, 2);
                    ViewBag.SellPercentage = percentagefinalVenta;
                }
                catch (DivideByZeroException ex)
                {

                    var percentageCompra = 0;
                    ViewBag.SellPercentage = 0;
                }
            }


            return RedirectToAction(nameof(Index));

        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}