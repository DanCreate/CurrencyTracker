using CurrencyTracker.Data;
using CurrencyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml.Linq;

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

            (decimal exchangeRateVenta, decimal exchangeRateCompra) = ApiHacienda();
            ViewBag.VentaToday = exchangeRateVenta;
            ViewBag.CompraToday = exchangeRateCompra;
            if (TempData.ContainsKey("AlertMessage"))
            {
                ViewBag.AlertMessage = TempData["AlertMessage"];
            }

            var arrayventa = _context.Rates
                 .Select(r => r.CrVenta)
                 .ToList();
            ViewBag.Array = arrayventa;

            var arrayday = _context.Rates
                 .Select(r => r.Date.DayOfWeek.ToString())
                 .ToList();
            ViewBag.DayOfWeek = arrayday;

            return View(await _context.Rates.ToListAsync());
        }




        public static (decimal, decimal) ApiHacienda()
        {
            HttpClient client = new HttpClient();
            string apiUrl = "https://api.hacienda.go.cr/indicadores/tc/dolar?fbclid=IwAR0zWDoToLXrNWsH0d7kG184c-hGT9FwpgWwzI0F22bpJbKzLSJ7KyqRu0c";
            var response = client.GetAsync(apiUrl).Result;
            var jsonData = response.Content.ReadAsStringAsync().Result;

            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData);
            var exchangeRateVenta = (decimal)jsonObject.venta.valor;
            var exchangeRateCompra = (decimal)jsonObject.compra.valor;

            return (exchangeRateVenta, exchangeRateCompra);
        }

        public IActionResult Create()
        {

            (decimal exchangeRateVenta, decimal exchangeRateCompra) = ApiHacienda();

            var targetDate = DateTime.Now.Date;

            var presentDate = _context.Rates
                .Where(r => r.Date == targetDate)
                .ToList();
            if (presentDate.Count == 1)

            {
                TempData["AlertMessage"] = "A rate for today already exists.";

                return RedirectToAction(nameof(Index));

            }
            else {
                var newRate = new Rates
                {
                    Date = DateTime.Now,
                    USD = 1,
                    CrVenta = exchangeRateVenta,
                    CrCompra = exchangeRateCompra


                };


                _context.Rates.Add(newRate);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }


        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}