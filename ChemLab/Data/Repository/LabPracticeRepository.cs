using System;
using ChemLab.Data.DataContext;
using ChemLab.Data.Entity;
using ChemLab.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ChemLab.Data.Repository
{
	public class LabPracticeRepository : BaseRepository<LabPractice>, ILabPracticeRepository
	{
		public LabPracticeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<LabPractice>> GetAllLabPractices(string userid)
        {
            return await db.LabPractices.Include(x => x.User).Where(u => u.UserId == userid).ToListAsync();
        }
    }
}

