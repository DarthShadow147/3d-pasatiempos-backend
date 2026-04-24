namespace _3d_pasatiempos_backend.Application.Exceptions.Base
{
    public class AppException : Exception
    {
        public string Code { get; }
        public int StatusCode { get; }

        public AppException(string pMessage, string pCode, int pStatusCode) 
            : base(pMessage)
        {
            Code = pCode;
            StatusCode = pStatusCode;
        }
    }
}
