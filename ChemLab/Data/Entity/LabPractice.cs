using System;
using ChemLab.Models;

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

        public LabPractice() { }

        public LabPractice(CreateLabPracticeViewModel createLabPracticeViewModel ){

            Description = createLabPracticeViewModel.Description;
            Name = createLabPracticeViewModel.Name;
        }


    }
}

