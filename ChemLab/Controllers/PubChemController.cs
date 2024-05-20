using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class PubChemController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PubChemController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetChemInfo(string smiles)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            var propertiesUrl = $"https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/smiles/{smiles}/property/MolecularWeight,ExactMass/JSON";
            var propertiesResponse = await httpClient.GetStringAsync(propertiesUrl);
            var propertiesData = JObject.Parse(propertiesResponse);

            var molecularWeight = propertiesData["PropertyTable"]["Properties"][0]["MolecularWeight"].ToString();
            var exactMass = propertiesData["PropertyTable"]["Properties"][0]["ExactMass"].ToString();

            var synonymsAndCasUrl = $"https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/smiles/{smiles}/synonyms/JSON";
            var synonymsAndCasResponse = await httpClient.GetStringAsync(synonymsAndCasUrl);
            var synonymsAndCasData = JObject.Parse(synonymsAndCasResponse);

            var casNumber = ExtractCasNumber(synonymsAndCasData);

            var firstSynonym = ExtractFirstSynonym(synonymsAndCasData);

            if (casNumber != null)
            {
                var imageRequestUrl = $"https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/smiles/{smiles}/PNG";
                var imageResponse = await httpClient.GetByteArrayAsync(imageRequestUrl);

                //return Ok(new { success = true, casNumber, firstSynonym, image = Convert.ToBase64String(imageResponse) });
                return Ok(new { success = true, casNumber, firstSynonym, molecularWeight, exactMass, image = Convert.ToBase64String(imageResponse) });
            }
            else
            {
                return Ok(new { success = false, message = "CAS number not found" });
            }
        }
        catch (Exception ex)
        {
            return Ok(new { success = false, errorMessage = ex.Message });
        }
    }
    private string ExtractCasNumber(JObject synonymsAndCasData)
    {
        var synonyms = synonymsAndCasData["InformationList"]["Information"][0]["Synonym"].ToObject<JArray>();

        foreach (var synonym in synonyms)
        {
            var casMatch = Regex.Match(synonym.ToString(), @"^\d{2,7}-\d{2}-\d$");
            if (casMatch.Success)
            {
                return casMatch.Value;
            }
        }

        return null;
    }

    private string ExtractFirstSynonym(JObject synonymsAndCasData)
    {
        var synonyms = synonymsAndCasData["InformationList"]["Information"][0]["Synonym"].ToObject<JArray>();

        if (synonyms.HasValues)
        {
            return synonyms[0].ToString();
        }

        return null;
    }

}
