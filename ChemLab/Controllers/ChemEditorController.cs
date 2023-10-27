using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using ChemLab.Models;

namespace ChemLab.Controllers
{
    public class ChemEditorController : Controller
    {

        [HttpPost]
        public async Task<IActionResult> GetFile(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return Ok();
            string file = await System.IO.File.ReadAllTextAsync($"{id}.txt");
            return Ok(file);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChemData()
        {
            using StreamReader sr = new StreamReader(HttpContext.Request.Body);
            try
            {
                string jsonData = await sr.ReadToEndAsync();
                var dataContainer = JsonConvert.DeserializeObject<DataContainer>(jsonData);

                string combinedData = JsonConvert.SerializeObject(dataContainer);
                await System.IO.File.WriteAllTextAsync($"{Regex.Replace(DateTime.Now.ToString("G"), "(\\.|:)", "_")}_request.txt", combinedData);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
    }
}