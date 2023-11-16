using System;
using ChemLab.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ChemLab.Data.Entity
{
    public class LabPractice
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public string ChemDocumentData { get; set; }
        public string ReactantData { get; set; }
        public string ProductData { get; set; }

        public LabPractice()
        {

        }

        public LabPractice(CreateLabPracticeViewModel createLabPracticeViewModel)
        {
            Description = createLabPracticeViewModel.Description;
            Name = createLabPracticeViewModel.Name;
            ChemDocumentData = "";
            ReactantData = "";
            ProductData = "";
        }


    }
}

