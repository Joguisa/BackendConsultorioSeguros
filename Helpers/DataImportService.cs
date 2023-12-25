using AutoMapper;
using BackendConsultorioSeguros.Models;

namespace BackendConsultorioSeguros.Helpers
{
    public class DataImportService<TDto, TEntity>
    where TDto : class
    where TEntity : class
    {
        private readonly DBSEGUROSCHUBBContext _context;
        private readonly IMapper _mapper;

        public DataImportService(DBSEGUROSCHUBBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ImportData(List<TDto> dtos)
        {
            var entities = _mapper.Map<List<TEntity>>(dtos);
            _context.Set<TEntity>().AddRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
