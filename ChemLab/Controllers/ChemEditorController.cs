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
                    ProductData = labPractice.ProductData,
                    XCoordinateArrow = labPractice.XCoordinateArrow
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
                if (dataContainer == null)
                {
                    return BadRequest(new { error = "DataContainer is null!!!" });
                }

                var labPractice = await _labPracticeRepository.GetById(id);

                if (labPractice == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(dataContainer.ChemDocumentData))
                {
                    labPractice.ChemDocumentData = dataContainer.ChemDocumentData;
                }

                if (!string.IsNullOrEmpty(dataContainer.ReactantData))
                {
                    labPractice.ReactantData = dataContainer.ReactantData;
                }

                if (!string.IsNullOrEmpty(dataContainer.ProductData))
                {
                    labPractice.ProductData = dataContainer.ProductData;
                }

                labPractice.XCoordinateArrow = dataContainer.XCoordinateArrow;

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