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
            // Используйте Select для преобразования IQueryable к IEnumerable
            var molecules = await _context.SubGroupMolecule
                .Where(m => m.UserId == userId)
                .Select(m => new SubGroupMolecule
                {
                    Name = m.Name,
                    Abbreviation = m.Abbreviation,
                    FormulaText = m.FormulaText != null ? m.FormulaText : "", // Проверка на NULL и замена на пустую строку
                    InputTexts = m.InputTexts != null ? m.InputTexts : "", // Проверка на NULL и замена на пустую строку
                    //StructData = m.StructData != null ? m.StructData : "", // Проверка на NULL и замена на пустую строку
                    StructData = m.StructData,
                    ImageData = m.ImageData != null ? m.ImageData : new byte[0] // Проверка на NULL и замена на пустой массив байт
                })
                .ToListAsync();

            return molecules;
        }


        public async Task DeleteByName(string name, string userId)
        {
            //Console.WriteLine($"Attempting to delete molecule with name: {name} and userId: {userId}");

            var molecule = await _context.SubGroupMolecule.FirstOrDefaultAsync(m => m.Name == name && m.UserId == userId);
            if (molecule != null)
            {
                //Console.WriteLine($"Molecule found. Deleting molecule with ID: {molecule.Id}");

                _context.SubGroupMolecule.Remove(molecule);
                await _context.SaveChangesAsync();

                //Console.WriteLine($"Molecule deleted successfully");
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
