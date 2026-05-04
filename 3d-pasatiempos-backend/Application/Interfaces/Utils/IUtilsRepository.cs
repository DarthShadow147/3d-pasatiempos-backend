using _3d_pasatiempos_backend.Application.Dtos.CommonDto;

namespace _3d_pasatiempos_backend.Application.Interfaces.Utils
{
    public interface IUtilsRepository
    {
        IQueryable<T> GetFilteredQuery<T>(QueryParams pQuery) where T : class;
    }
}
