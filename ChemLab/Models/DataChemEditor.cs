namespace ChemLab.Models
{
    public class ChemDataViewModel
    {
        public string ChemDocumentData { get; set; }
        public IEnumerable<ReactantDataViewModel> ReactantData { get; set; }
        public IEnumerable<ProductDataViewModel> ProductData { get; set; }
        public double XCoordinateArrow { get; set; }
    }

    public class ReactantDataViewModel
    {
        public string fragmentId { get; set; }
        public string formula { get; set; }
        public double molarMass { get; set; }
        public double m { get; set; }
        public string mUnit { get; set; }
        public double n { get; set; }
        public string nUnit { get; set; }
        public double equivalent { get; set; }
        public bool action { get; set; }
    }

    public class ProductDataViewModel
    {
        public string fragmentId { get; set; }
        public string formula { get; set; }
        public double molarMass { get; set; }
        public double m { get; set; }
        public string mUnit { get; set; }
        public double n { get; set; }
        public string nUnit { get; set; }
        public double equivalent { get; set; }
        public bool action { get; set; }
    }

    public class MoleculeViewModel
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string FormulaText { get; set; }
        public string InputTexts { get; set; }
        public string StructData { get; set; }
    }

    public class ContentModel
    {
        public string Content { get; set; }
    }
}