using System;
using ChemLab.Data.Entity;

namespace ChemLab.Data.Repository.Interfaces
{
	public interface ILabPracticeRepository : IBaseRepository<LabPractice>
	{
        Task<List<LabPractice>> GetAllLabPractices(string userid);

    }
}

