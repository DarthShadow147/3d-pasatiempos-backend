using _3d_pasatiempos_backend.Application.Exceptions.Base;

namespace _3d_pasatiempos_backend.Application.Exceptions.Common
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string pMessage) 
            : base(pMessage, "BAD_REQUEST", 400)
        {
        }
    }
}
