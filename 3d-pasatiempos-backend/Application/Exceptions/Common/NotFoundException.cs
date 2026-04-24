using _3d_pasatiempos_backend.Application.Exceptions.Base;

namespace _3d_pasatiempos_backend.Application.Exceptions.Common
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string pMessage) 
            : base(pMessage, "NOT_FOUND", 404)
        {
        }
    }
}
