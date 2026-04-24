using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.PaymentDto;

namespace _3d_pasatiempos_backend.Application.Interfaces.PaymentInterfaces
{
    public interface IPaymentService
    {
        Task<int> CreatePaymentAsync(CreatePaymentDto pRequest);
        Task<PaymentDetailDto> GetPaymentDetailAsync(int pPaymentId);
        Task<PagedResult<PaymentListDto>> GetPagedPaymentAsync(QueryParams pQuery);
    }
}
