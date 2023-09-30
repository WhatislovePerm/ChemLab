using System;
namespace ChemLab.Models
{
	public class LabPracticeViewModel
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public DateTime DateOfCreate { get; set; }

		public string Description { get; set; }

		public LabPracticeViewModel()
		{
			
		}
	}
}

