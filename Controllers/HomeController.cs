using GodSimulator.Data;
using GodSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Diagnostics;

namespace GodSimulator.Controllers
{
    public class HomeController : Controller
    {
        #region движок. НЕ ТРОГАЙ.
        private readonly ILogger<HomeController> _logger;
        private readonly MyApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, MyApplicationContext context)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Index. Первая страница, привествие и экспозиция.

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Action. Тут основная логика работы

        [HttpGet]
        public IActionResult Action()
        {
            var updates = _context.Updates
                .OrderByDescending(x => x.Id)
                .ToList();
            return View(updates);
        }

        [HttpPost]
        public IActionResult Action(string version, string description)
        {
            var query = _context.Updates.AsQueryable();

            if (!string.IsNullOrEmpty(version))
            {
                query = query.Where(x => x.Version.Contains(version));
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(x => x.Description.Contains(description));
            }

            var updates = query.OrderByDescending(x => x.Id).ToList();

            return View(updates);
        }
        #endregion

        #region ADD.
        [HttpPost]
        public IActionResult Add(string version, string description)
        {
            if (string.IsNullOrWhiteSpace(version) || string.IsNullOrWhiteSpace(description))
            {
                return View();
            };
            var update = new Update
            {
                Version = version,
                Description = description
            };

            
            _context.Updates.Add(update);
            _context.SaveChanges();

            return RedirectToAction(nameof(Action));
        }

        [HttpGet]
        public IActionResult Add()
        {
            var lastVersion = _context.Updates
                .OrderByDescending(x => x.Version)
                .Select(x => x.Version)
                .FirstOrDefault();

            ViewBag.LastVersion = lastVersion ?? "";
            return View();
        }
        #endregion

        #region  REMOVE. Допустим, общественность не должна знать о каком-то острове...
        [HttpGet]
        public IActionResult Remove(int Id)
        {
            var update = _context.Updates.FirstOrDefault(x => x.Id == Id);

            if (update != null)
            {
                _context.Updates.Remove(update);
                _context.SaveChanges();

            }

            return RedirectToAction(nameof(Action));
        }
        #endregion

        #region EDIT.
        [HttpGet]
        public IActionResult Edit(int Id)
        {

            var update = _context.Updates.FirstOrDefault(x => x.Id == Id);

            return View(update);
        }

        [HttpPost]
        public IActionResult Edit(string version, string description, int Id)
        {
            if (string.IsNullOrWhiteSpace(version) || string.IsNullOrWhiteSpace(description))
            {
                return View();
            };

            var update = _context.Updates.FirstOrDefault(x => x.Id == Id);

            if (update != null)
            {
                update.Version = version;
                update.Description = description;

                _context.Updates.Update(update);
                _context.SaveChanges();
            };

            return RedirectToAction(nameof(Action));
        }

        #endregion

        #region Очень (!!!) важные мнения

        [HttpGet]
        public IActionResult InternetInsanity()
        {
            var post = MessagesData.GetRandom();
            return Json(new {post});
        }
        #endregion

        #region Перворождённые методы Privacy и Error.
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
