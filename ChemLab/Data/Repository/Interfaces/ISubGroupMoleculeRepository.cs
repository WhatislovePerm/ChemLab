using System.Threading.Tasks;
using ChemLab.Data.Entity;

namespace ChemLab.Data.Repository.Interfaces
{
    public interface ISubGroupMoleculeRepository : IBaseRepository<SubGroupMolecule>
    {
        Task AddForUser(SubGroupMolecule molecule, string userId);

        Task<IEnumerable<SubGroupMolecule>> GetAllForUser(string userId);

        Task DeleteByName(string name, string userId);

        Task AddOrUpdateImage(SubGroupMolecule molecule);
        Task<SubGroupMolecule> GetByNameAndUserId(string name, string userId);
    }

}
