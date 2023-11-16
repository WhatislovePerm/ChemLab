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
        private readonly ILabPracticeRepository _labPracticeRepository;

        public ChemEditorController(ILabPracticeRepository labPracticeRepository)
        {
            _labPracticeRepository = labPracticeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(int id)
        {
            try
            {
                var labPractice = await _labPracticeRepository.GetById(id);

                if (labPractice == null)
                {
                    return NotFound();
                }

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


        [HttpPost]
        public async Task<IActionResult> SaveChemData(int id, [FromBody] DataContainer dataContainer)
        {
            try
            {
                // Проверить, что dataContainer не равен null
                if (dataContainer == null)
                {
                    return BadRequest(new { error = "DataContainer is null." });
                }

                var labPractice = await _labPracticeRepository.GetById(id);

                if (labPractice == null)
                {
                    return NotFound();
                }

                // Теперь можно проверять свойства внутри dataContainer
                if (dataContainer.ChemDocumentData != null)
                {
                    labPractice.ChemDocumentData = JsonConvert.SerializeObject(dataContainer.ChemDocumentData);
                }

                if (dataContainer.ReactantData != null)
                {
                    labPractice.ReactantData = JsonConvert.SerializeObject(dataContainer.ReactantData);
                }

                if (dataContainer.ProductData != null)
                {
                    labPractice.ProductData = JsonConvert.SerializeObject(dataContainer.ProductData);
                }

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