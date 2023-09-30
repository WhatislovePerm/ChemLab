using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChemLab.Models;
using ChemLab.Services;
using ChemLab.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChemLab.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        private readonly ILabPracticeService _labPracticeService;

        public HomeController(ILabPracticeService labPracticeService) =>
            _labPracticeService = labPracticeService;

        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated)
                return View(new List<LabPracticeViewModel>());

            var userId = User.Claims.Where(x => x.Issuer == "LOCAL AUTHORITY").Select(x => x.Value).First();
            var labPractice = await _labPracticeService.GetAllLabPractices(userId);
            return View(labPractice);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description")] CreateLabPracticeViewModel createLabPractice)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Claims.Where(x => x.Issuer == "LOCAL AUTHORITY").Select(x => x.Value).First();
                await _labPracticeService.CreateLabPractice(createLabPractice, userId);

                return RedirectToAction(nameof(Index));
            }
            return View(createLabPractice);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}

