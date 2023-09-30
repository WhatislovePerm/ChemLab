using System;
using ChemLab.Models;

namespace ChemLab.Services.Interfaces
{
	public interface ILabPracticeService
	{
        Task<List<LabPracticeViewModel>> GetAllLabPractices(string userid);

        Task CreateLabPractice(CreateLabPracticeViewModel createLabPracticeViewModel, string userid);
    }
}

