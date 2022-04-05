using AutoMapper;
using WorkerService;
using WorkerService.Models;
using WorkerService.Repository;
using Newtonsoft.Json;

namespace WorkerService.Repository
{
    public class VisitRepository : IVisitRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VisitRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ProcessFileAsync(string fileName)
        {
            Console.WriteLine(fileName);
#if DEBUG
            string filePath = @"..\..\..\..\Data\Data\" + fileName + ".json";
#else
            string filePath = @"../var/Data/" + fileName + ".json";
#endif

            var visitDto = JsonConvert.DeserializeObject<VisitCreateDto>(File.ReadAllText(filePath));
            var visit = _mapper.Map<Visit>(visitDto);
            await AddVisit(visit);
            DeleteFile(filePath);
        }

        private async Task AddVisit(Visit visit)
        {
            await _context.AddAsync(visit);
            await _context.SaveChangesAsync();
        }

        private void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
    }
}