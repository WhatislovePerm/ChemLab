using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using ChemLab.Models;
using ChemLab.Data.Repository.Interfaces;
using ChemLab.Data.Entity;
using ChemLab.Data.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ChemLab.Controllers
{
    [Authorize]
    public class ChemEditorController : Controller
    {
        private readonly ILabPracticeRepository _labPracticeRepository;
        private readonly ISubGroupMoleculeRepository _subGroupMoleculeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChemEditorController(ILabPracticeRepository labPracticeRepository, ISubGroupMoleculeRepository subGroupMoleculeRepository, UserManager<ApplicationUser> userManager)
        {
            _labPracticeRepository = labPracticeRepository;
            _subGroupMoleculeRepository = subGroupMoleculeRepository;
            _userManager = userManager;
        }



        [HttpPost]
        public async Task<IActionResult> SaveChemData(int id, [FromBody] ChemDataViewModel data)
        {
            try
            {
                if (data == null)
                {
                    return BadRequest(new { error = "DataContainer is null!!!" });
                }

                var labPractice = await _labPracticeRepository.GetById(id);

                if (labPractice == null)
                {
                    return NotFound();
                }

                // Проверяем ChemDocumentData на null
                if (data.ChemDocumentData != null)
                {
                    labPractice.ChemDocumentData = data.ChemDocumentData;
                }                

                // Проверяем ReactantData на null
                if (data.ReactantData != null)
                {
                    labPractice.ReactantData = JsonConvert.SerializeObject(data.ReactantData);
                }

                // Проверяем ProductData на null
                if (data.ProductData != null)
                {
                    labPractice.ProductData = JsonConvert.SerializeObject(data.ProductData);
                }

                // Сохраняем XCoordinateArrow, даже если он null
                labPractice.XCoordinateArrow = data.XCoordinateArrow;

                await _labPracticeRepository.Update(labPractice);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
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

                // Получаем данные из базы данных
                var reactantDataJson = labPractice.ReactantData;
                var productDataJson = labPractice.ProductData;

                // Преобразуем JSON в объекты
                var reactantData = JsonConvert.DeserializeObject<IEnumerable<ReactantDataViewModel>>(reactantDataJson);
                var productData = JsonConvert.DeserializeObject<IEnumerable<ProductDataViewModel>>(productDataJson);

                return Json(new
                {
                    ChemDocumentData = labPractice.ChemDocumentData,
                    ReactantData = reactantData,
                    ProductData = productData,
                    XCoordinateArrow = labPractice.XCoordinateArrow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadMolecule([FromBody] MoleculeViewModel molecule)
        {
            try
            {
                if (molecule == null)
                {
                    return BadRequest(new { error = "Molecule data is null" });
                }

                var currentUser = await _userManager.GetUserAsync(User);

                // Проверяем, что пользователь существует
                if (currentUser == null)
                {
                    return BadRequest(new { error = "User not found" });
                }

                // Создаем экземпляр Entity из ViewModel
                var moleculeEntity = new SubGroupMolecule
                {
                    Name = molecule.Name,
                    Abbreviation = molecule.Abbreviation,
                    FormulaText = molecule.FormulaText,
                    InputTexts = molecule.InputTexts,
                    //RepItemData = molecule.RepItemData,
                    StructData = JsonConvert.SerializeObject(molecule.StructData),
                    UserId = currentUser.Id
                };


            // Сохраняем молекулу в базе данных для текущего пользователя
            await _subGroupMoleculeRepository.AddForUser(moleculeEntity, currentUser.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving molecule to database: " + ex.Message);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMolecules()
        {
            try
            {
                // Получаем текущего пользователя
                var currentUser = await _userManager.GetUserAsync(User);

                // Проверяем, что пользователь существует
                if (currentUser == null)
                {
                    return BadRequest(new { error = "User not found" });
                }

                // Получаем все молекулы для текущего пользователя
                var molecules = await _subGroupMoleculeRepository.GetAllForUser(currentUser.Id);

                // Возвращаем список молекул на фронтенд
                return Json(molecules);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMolecule(string name)
        {
            try
            {

                Console.WriteLine("Attempting to delete molecule with name: " + name);
                // Получаем текущего пользователя
                var currentUser = await _userManager.GetUserAsync(User);

                // Проверяем, что пользователь существует
                if (currentUser == null)
                {
                    return BadRequest(new { error = "User not found" });
                }

                // Удаляем молекулу из базы данных
                await _subGroupMoleculeRepository.DeleteByName(name, currentUser.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
