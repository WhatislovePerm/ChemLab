﻿namespace ChemLab.Models
{
    public class DataContainer
    {
        public string ChemDocumentData { get; set; }
        public List<ReactantData> ReactantData { get; set; }
        public List<ProductData> ProductData { get; set; }
    }

    public class ReactantData
    {
        public string FragmentId { get; set; }
        public string Formula { get; set; }
        public double MolarMass { get; set; }
        public string M { get; set; }
        public string N { get; set; }
        public double Equivalent { get; set; }
        public int Action { get; set; }
    }

    public class ProductData
    {
        public string FragmentId { get; set; }
        public string Formula { get; set; }
        public double MolarMass { get; set; }
        public string M { get; set; }
        public string N { get; set; }
        public double Equivalent { get; set; }
        public int Action { get; set; }
    }
}