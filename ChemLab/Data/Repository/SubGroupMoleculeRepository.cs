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
            // Получаем пользователя по его идентификатору
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                // Привязываем молекулу к пользователю
                molecule.UserId = userId;
            }

            // Добавляем молекулу в базу данных
            await Add(molecule);
        }

        public async Task<IEnumerable<SubGroupMolecule>> GetAllForUser(string userId)
        {
            return await _context.SubGroupMolecule.Where(m => m.UserId == userId).ToListAsync();
        }

        public async Task DeleteByName(string name, string userId)
        {
            Console.WriteLine($"Attempting to delete molecule with name: {name} and userId: {userId}");

            var molecule = await _context.SubGroupMolecule.FirstOrDefaultAsync(m => m.Name == name && m.UserId == userId);
            if (molecule != null)
            {
                Console.WriteLine($"Molecule found. Deleting molecule with ID: {molecule.Id}");

                _context.SubGroupMolecule.Remove(molecule);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Molecule deleted successfully");
            }
            else
            {
                Console.WriteLine("Molecule not found or user not authorized to delete it");
            }
        }
    }
}
