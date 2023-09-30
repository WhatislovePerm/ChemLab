using System;
using ChemLab.Data.Entity;

namespace ChemLab.Data.Repository.Interfaces
{
    public interface IAccountRepository : IBaseRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetById(string id);
    }
}

