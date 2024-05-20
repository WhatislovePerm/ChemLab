using System;
using ChemLab.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChemLab.Data.Entity
{
    public class SubGroupMolecule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string FormulaText { get; set; }
        public string InputTexts { get; set; }
        public string StructData { get; set; }
        public string UserId { get; set; }
        public byte[] ImageData { get; set; }
    }
}
