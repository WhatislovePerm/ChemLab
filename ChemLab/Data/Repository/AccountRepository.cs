using System;
using ChemLab.Data.DataContext;
using ChemLab.Data.Entity;

namespace ChemLab.Data.Repository.Interfaces
{
    public class AccountRepository : BaseRepository<ApplicationUser>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        { }

        public async Task<ApplicationUser?> GetById(string id)
        {
            return await db.Set<ApplicationUser>().FindAsync(id);
        }
    }
}

