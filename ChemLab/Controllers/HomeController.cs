﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChemLab.Data.Repository.Interfaces;
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
        private readonly ILabPracticeRepository _labPracticeRepository;

        public HomeController(ILabPracticeService labPracticeService, ILabPracticeRepository labPracticeRepository)
        {
            _labPracticeService = labPracticeService;
            _labPracticeRepository = labPracticeRepository;
        }

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

        //public IActionResult Details(int id)
        //{
        //    // Здесь вы можете добавить логику для отображения деталей лабораторной практики с определенным id.
        //    // Например, получите данные из базы данных и передайте их в представление.

        //    // Ваш код...

        //    return View();
        //}

        //public IActionResult Details(int id)
        //{
        //    var labPractice = _labPracticeRepository.GetById(id);
        //    return View(labPractice);
        //}

        public async Task<IActionResult> Details(int id)
        {
            var labPractice = await _labPracticeRepository.GetById(id);
            ViewBag.Id = id;
            return View(labPractice);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] LabPracticeViewModel editedLabPractice)
        {
            if (id != editedLabPractice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var labPracticeToUpdate = await _labPracticeRepository.GetById(id);

                if (labPracticeToUpdate == null)
                {
                    return NotFound();
                }

                // Обновляем данные в объекте LabPractice
                labPracticeToUpdate.Name = editedLabPractice.Name;
                labPracticeToUpdate.Description = editedLabPractice.Description;

                // Сохраняем изменения в базе данных
                await _labPracticeRepository.Update(labPracticeToUpdate);

                return RedirectToAction(nameof(Index));
            }

            // Если ModelState недействительна, возвращаем пользователя на форму редактирования
            return View("Edit", editedLabPractice);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var labPractice = await _labPracticeRepository.GetById(id);

            if (labPractice == null)
            {
                return NotFound();
            }

            // Преобразуйте объект LabPractice в LabPracticeViewModel
            var labPracticeViewModel = new LabPracticeViewModel
            {
                Id = labPractice.Id,
                Name = labPractice.Name,
                Description = labPractice.Description
            };

            // Передайте LabPracticeViewModel в представление для отображения данных при редактировании
            return View("Edit", labPracticeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Здесь вы можете добавить логику для удаления лабораторной практики с определенным id.
            // Например, найдите запись в базе данных и удалите ее.

            var labPractice = await _labPracticeRepository.GetById(id);

            if (labPractice == null)
            {
                return NotFound();
            }

            await _labPracticeRepository.Remove(labPractice);

            return RedirectToAction(nameof(Index));
        }

    }
}

