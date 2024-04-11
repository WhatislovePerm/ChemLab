using ChemLab.Data.DataContext;
using ChemLab.Data.Entity;
using ChemLab.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChemLab.Data.Repository
{
    public class SubGroupMoleculeRepository : BaseRepository<SubGroupMolecule>, ISubGroupMoleculeRepository
    {
        private readonly ApplicationDbContext _context;

        public SubGroupMoleculeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddForUser(SubGroupMolecule molecule, string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                molecule.UserId = userId;
            }

            await Add(molecule);
        }

        public async Task<IEnumerable<SubGroupMolecule>> GetAllForUser(string userId)
        {
            var molecules = await _context.SubGroupMolecule
                .Where(m => m.UserId == userId)
                .Select(m => new SubGroupMolecule
                {
                    Name = m.Name,
                    Abbreviation = m.Abbreviation,
                    FormulaText = m.FormulaText != null ? m.FormulaText : "",
                    InputTexts = m.InputTexts != null ? m.InputTexts : "",
                    StructData = m.StructData,
                    ImageData = m.ImageData != null ? m.ImageData : new byte[0]
                })
                .ToListAsync();

            return molecules;
        }


        public async Task DeleteByName(string name, string userId)
        {
            var molecule = await _context.SubGroupMolecule.FirstOrDefaultAsync(m => m.Name == name && m.UserId == userId);
            if (molecule != null)
            {

                _context.SubGroupMolecule.Remove(molecule);
                await _context.SaveChangesAsync();

            }
            else
            {
                Console.WriteLine("Molecule not found or user not authorized to delete it");
            }
        }

        public async Task AddOrUpdateImage(SubGroupMolecule molecule)
        {
            _context.Update(molecule);
            await _context.SaveChangesAsync();
        }

        public async Task<SubGroupMolecule> GetByNameAndUserId(string name, string userId)
        {
            return await _context.SubGroupMolecule.FirstOrDefaultAsync(m => m.Name == name && m.UserId == userId);
        }
    }
}
