using _3d_pasatiempos_backend.Application.Exceptions.Base;

namespace _3d_pasatiempos_backend.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _Next;

        public ExceptionMiddleware(RequestDelegate Next)
        {
            _Next = Next;
        }

        /// <summary>
        /// Middleware encargado de capturar excepciones no controladas durante el pipeline HTTP
        /// </summary>
        /// <param name="pContext">Contexto HTTP de la solicitud actual</param>
        /// <returns>
        /// Tarea asincrónica que representa la ejecución del middleware
        /// </returns>
        /// <remarks>
        /// Este middleware intercepta cualquier excepción no manejada en la aplicación,
        /// registra el error utilizando el sistema de logging y retorna una respuesta
        /// estructurada en formato JSON con un código HTTP 500.
        ///
        /// Incluye un identificador único de traza (TraceId) que permite correlacionar
        /// errores en logs para facilitar el diagnóstico.
        /// </remarks>
        public async Task Invoke(HttpContext pContext)
        {
            try
            {
                await _Next(pContext);
            }
            catch (AppException lEx)
            {
                pContext.Response.ContentType = "application/json";
                pContext.Response.StatusCode = lEx.StatusCode;

                var lResponse = new
                {
                    Success = false,
                    lEx.Message,
                    lEx.Code,
                    Status = lEx.StatusCode,
                    TraceId = pContext.TraceIdentifier
                };

                await pContext.Response.WriteAsJsonAsync(lResponse);
            }
            catch (Exception lEx)
            {
                pContext.Response.ContentType = "application/json";
                pContext.Response.StatusCode = 500;

                var lResponse = new
                {
                    Success = false,
                    Message = "Internal server error",
                    Code = "INTERNAL_ERROR",
                    Status = 500,
                    Detail = lEx.Message,
                    TraceId = pContext.TraceIdentifier
                };

                await pContext.Response.WriteAsJsonAsync(lResponse);
            }
        }
    }
}
