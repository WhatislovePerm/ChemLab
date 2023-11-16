using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using ChemLab.Models;
using ChemLab.Data.Repository.Interfaces;
using ChemLab.Data.Entity;

namespace ChemLab.Controllers
{
    public class ChemEditorController : Controller
    {
        private readonly ILabPracticeRepository _labPracticeRepository; // Внедрите зависимость

        // Используйте конструктор для внедрения зависимостей
        public ChemEditorController(ILabPracticeRepository labPracticeRepository)
        {
            _labPracticeRepository = labPracticeRepository;
        }

        //[HttpGet("get_file/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetFile(int id)
        {
            try
            {
                // Получите данные химического редактора из базы данных по id
                var labPractice = await _labPracticeRepository.GetById(id);

                if (labPractice == null)
                {
                    return NotFound();
                }

                // Вернуть данные в формате JSON
                return Json(new
                {
                    ChemDocumentData = labPractice.ChemDocumentData,
                    ReactantData = labPractice.ReactantData,
                    ProductData = labPractice.ProductData
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        //[HttpPost("save_data/{id}")]
        [HttpPost]
        public async Task<IActionResult> SaveChemData(int id, [FromBody] DataContainer dataContainer)
        {
            try
            {
                // Получите сущность LabPractice из базы данных по id
                var labPractice = await _labPracticeRepository.GetById(id);

                if (labPractice == null)
                {
                    return NotFound();
                }

                // Обновите свойства LabPractice с использованием данных из dataContainer
                labPractice.ChemDocumentData = JsonConvert.SerializeObject(dataContainer.ChemDocumentData);
                labPractice.ReactantData = JsonConvert.SerializeObject(dataContainer.ReactantData);
                labPractice.ProductData = JsonConvert.SerializeObject(dataContainer.ProductData);


                //Console.WriteLine(JsonConvert.SerializeObject(dataContainer.ChemDocumentData));
                //Console.WriteLine(JsonConvert.SerializeObject(dataContainer.ReactantData));
                //Console.WriteLine(JsonConvert.SerializeObject(dataContainer.ProductData));
                // Сохранить обновленные данные в базе данных
                await _labPracticeRepository.Update(labPractice);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }





    }
}